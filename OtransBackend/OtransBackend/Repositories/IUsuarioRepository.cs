using Google.Apis.Drive.v3.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OtransBackend.Dtos;
using OtransBackend.Repositories.Models;
using OtransBackend.Utilities;
using System.Data;
using System.IO;
using System.Linq;

namespace OtransBackend.Repositories
{
    public interface IUsuarioRepository
    {
        Task<Vehiculo> AddVehiculoAsync(Vehiculo vehiculo);
        Task<Usuario> AddTransportistaAsync(Usuario user); // Método para agregar transportista
        Task<Usuario> AddEmpresaAsync(Usuario user); // Método para agregar empresa
        Task<Usuario> GetUserByEmailAsync(string email);
        Task<Usuario> Login(LoginDto request);
        Task UpdateUserPasswordAsync(int idUsuario, string nuevaContrasena);
        Task<IEnumerable<UsuarioRevisionDto>> ObtenerUsuariosPendientesValidacionAsync();
        Task<Usuario?> ObtenerUsuarioConVehiculoPorIdAsync(int idUsuario);
        Task ValidarUsuarioAsync(UsuarioValidacionDto dto);
        Task ActualizarDocumentoAsync(int idUsuario, string nombreDocumento, string url);
        Task CambiarEstadoAsync(int idUsuario, string nombreEstado);

        Task<Viaje> ObtenerViajePorTransportista(int idTransportista);
        Task<Carga> ObtenerCargaPorId(int idCarga);
        Task<Carga> AddAsync(Carga carga);
        Task<Carga?> GetByIdAsync(int id);
        Task<IEnumerable<Viaje>> ObtenerViajesPorCarroceriaAsync(int transportistaId);
        Task<List<UserRegistrationReportItem>> GetAllUserRegistrationsAsync();
        Task<List<MonthlyRegistrations>> GetMonthlyRegistrationsAsync();
        Task<IEnumerable<UsuarioReportDto>> GetAllUsersForReportAsync();
    }

    public class UserRepository : IUsuarioRepository
    {
        private readonly Otrans _context;

        public UserRepository(Otrans context)
        {
            _context = context;
        }

        // Convertir archivo a binario
        private byte[] ConvertFileToBytes(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        // Método para agregar Transportista
        public async Task<Usuario> AddTransportistaAsync(Usuario user)
        {
            var nuevoIdParam = new SqlParameter
            {
                ParameterName = "@NuevoId",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_AddTransportista @Nombre, @Apellido, @Correo, @Contrasena, @Telefono, @TelefonoSos, @NumIdentificacion, @IdRol, @IdEstado, @Licencia, @ArchiDocu, @NuevoId OUTPUT",
                new SqlParameter("@Nombre", user.Nombre ?? (object)DBNull.Value),
                new SqlParameter("@Apellido", user.Apellido ?? (object)DBNull.Value),
                new SqlParameter("@Correo", user.Correo ?? (object)DBNull.Value),
                new SqlParameter("@Contrasena", user.Contrasena ?? (object)DBNull.Value),
                new SqlParameter("@Telefono", user.Telefono ?? (object)DBNull.Value),
                new SqlParameter("@TelefonoSos", user.TelefonoSos ?? (object)DBNull.Value),
                new SqlParameter("@NumIdentificacion", user.NumIdentificacion),
                new SqlParameter("@IdRol", user.IdRol),
                new SqlParameter("@IdEstado", user.IdEstado),
                new SqlParameter("@Licencia", user.Licencia ?? (object)DBNull.Value),
                new SqlParameter("@ArchiDocu", user.ArchiDocu ?? (object)DBNull.Value),
                nuevoIdParam
            );

            user.IdUsuario = (int)nuevoIdParam.Value;

            return user;
        }





        // Método para agregar Empresa
        public async Task<Usuario> AddEmpresaAsync(Usuario user)
        {

            // Guardamos el usuario (empresa) con el archivo de NIT (si existe)
            _context.Usuario.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<Usuario> GetUserByEmailAsync(string email)
        {
            return await _context.Usuario.FirstOrDefaultAsync(u => u.Correo == email);
        }
        public async Task<Vehiculo> AddVehiculoAsync(Vehiculo vehiculo)
        {


            // Guardamos el vehículo en la base de datos
            _context.Vehiculo.Add(vehiculo);
            await _context.SaveChangesAsync(); // Esperamos a que se guarde en la base de datos

            return vehiculo; // Retornamos el vehículo guardado
        }
        public async Task<Usuario> Login(LoginDto request)
        {
            return await _context.Usuario.FirstOrDefaultAsync(u => u.Correo.Equals(request.Correo));
        }
        public async Task UpdateUserPasswordAsync(int idUsuario, string nuevaContrasena)
        {
            var sql = "EXEC ActualizarEstadoOContrasena @IdUsuario, @NuevaContrasena, @IdEstado = NULL";
            await _context.Database.ExecuteSqlRawAsync(sql,
                new SqlParameter("@IdUsuario", idUsuario),
                new SqlParameter("@NuevaContrasena", nuevaContrasena));
        }
        public async Task<IEnumerable<UsuarioRevisionDto>> ObtenerUsuariosPendientesValidacionAsync()
        {
            return await _context.Usuario
                .Where(u => u.IdEstadoNavigation != null && u.IdEstadoNavigation.Nombre == "PendienteValidacion")
                .Select(u => new UsuarioRevisionDto
                {
                    Id = u.IdUsuario,
                    Nombre = u.Nombre,
                    Correo = u.Correo,
                    TipoUsuario = u.NombreEmpresa != null ? "Empresa" : "Transportista",
                    FechaRegistro = u.FechaRegistro,
                    Estado = u.IdEstadoNavigation!.Nombre
                })
                .ToListAsync();
        }
        public async Task<Usuario?> ObtenerUsuarioConVehiculoPorIdAsync(int idUsuario)
        {
            return await _context.Usuario
                .Include(u => u.Vehiculos)
                .FirstOrDefaultAsync(u => u.IdUsuario == idUsuario);
        }

        // Implementación ajustada en UserRepository.cs
        public async Task ValidarUsuarioAsync(UsuarioValidacionDto dto)
        {
            // 1) Cargo usuario y sus vehículos
            var usuario = await _context.Usuario
                .Include(u => u.Vehiculos)
                .FirstOrDefaultAsync(u => u.IdUsuario == dto.IdUsuario);

            if (usuario == null)
                throw new KeyNotFoundException("Usuario no encontrado");

            // 2) ¿Algún documento inválido?
            bool tieneInvalido = dto.Documentos.Any(d => !d.EsValido);

            // 3) Por cada doc inválido, elimino la URL en la entidad
            foreach (var doc in dto.Documentos.Where(d => !d.EsValido))
            {
                switch (doc.NombreDocumento)
                {
                    case "Documento Identidad":
                        usuario.ArchiDocu = null;
                        break;

                    case "NIT":
                        usuario.Nit = null;
                        break;

                    case "Licencia Conducción":
                        usuario.Licencia = null;
                        break;

                    case "Soat":
                        var vehSoat = usuario.Vehiculos.FirstOrDefault();
                        if (vehSoat != null) vehSoat.Soat = null;
                        break;

                    case "Técnico Mecánica":
                        var vehTecno = usuario.Vehiculos.FirstOrDefault();
                        if (vehTecno != null) vehTecno.Tecnicomecanica = null;
                        break;

                    case "Licencia Tránsito":
                        var vehLicTra = usuario.Vehiculos.FirstOrDefault();
                        if (vehLicTra != null) vehLicTra.LicenciaTransito = null;
                        break;
                }
            }

            // 4) Actualizo el estado (Validado vs DocumentosRechazados)
            var estadoNombre = tieneInvalido ? "DocumentosRechazados" : "Validado";
            var nuevoEstado = await _context.Estado
                .FirstOrDefaultAsync(e => e.Nombre == estadoNombre)
                ?? throw new InvalidOperationException($"Estado '{estadoNombre}' no encontrado");

            usuario.IdEstado = nuevoEstado.IdEstado;

            // 5) Guardo cambios (sin observaciones en BD)
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarDocumentoAsync(int idUsuario, string nombreDocumento, string url)
        {
            var usuario = await _context.Usuario
                .Include(u => u.Vehiculos)
                .FirstOrDefaultAsync(u => u.IdUsuario == idUsuario);
            if (usuario == null) throw new KeyNotFoundException();

            switch (nombreDocumento)
            {
                case "Documento Identidad":
                    usuario.ArchiDocu = url;
                    break;
                case "NIT":
                    usuario.Nit = url;
                    break;
                case "Licencia Conducción":
                    usuario.Licencia = url;
                    break;
                case "Soat":
                    {
                        var veh = usuario.Vehiculos.FirstOrDefault();
                        if (veh != null) veh.Soat = url;
                    }
                    break;
                case "Técnico Mecánica":
                    {
                        var veh = usuario.Vehiculos.FirstOrDefault();
                        if (veh != null) veh.Tecnicomecanica = url;
                    }
                    break;
                case "Licencia Tránsito":
                    {
                        var veh = usuario.Vehiculos.FirstOrDefault();
                        if (veh != null) veh.LicenciaTransito = url;
                    }
                    break;
                default:
                    throw new InvalidOperationException("Documento no reconocido");
            }

            await _context.SaveChangesAsync();
        }
        public async Task CambiarEstadoAsync(int idUsuario, string nombreEstado)
        {
            var estado = await _context.Estado
                          .FirstOrDefaultAsync(e => e.Nombre == nombreEstado)
                          ?? throw new InvalidOperationException($"Estado '{nombreEstado}' no existe");

            var sql = "EXEC ActualizarEstadoOContrasena @IdUsuario, @NuevaContrasena = NULL, @IdEstado";
            await _context.Database.ExecuteSqlRawAsync(sql,
                new SqlParameter("@IdUsuario", idUsuario),
                new SqlParameter("@IdEstado", estado.IdEstado));
        }
        // Método para agregar imagenes a carga 
        public async Task<Carga> AddAsync(Carga carga)
        {
            _context.Carga.Add(carga);
            await _context.SaveChangesAsync();
            return carga;
        }

        public async Task<Carga?> GetByIdAsync(int id)
        {
            return await _context.Carga.FindAsync(id);
        }

        public async Task<Viaje> ObtenerViajePorTransportista(int idTransportista)
        {
            return await _context.Viaje
                .Where(v => v.IdTransportista == idTransportista && (v.IdEstado == 5 || v.IdEstado == 6 || v.IdEstado == 7)) // Aceptar estados 5, 6 o 7
                .FirstOrDefaultAsync();
        }
        public async Task<Carga> ObtenerCargaPorId(int idCarga)
        {
            return await _context.Carga
                .Where(c => c.IdCarga == idCarga)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Viaje>> ObtenerViajesPorCarroceriaAsync(int transportistaId)
        {
            var vehiculos = await _context.Vehiculo
                .Where(v => v.IdTransportista == transportistaId)
                .ToListAsync();

            if (vehiculos == null || !vehiculos.Any())
                return new List<Viaje>();  // Si no se encuentra el vehículo, retornar lista vacía

            var carroceria = vehiculos.FirstOrDefault()?.Carroceria;

            if (string.IsNullOrEmpty(carroceria))
                return new List<Viaje>();  // Si no tiene carrocería, retornar lista vacía

            var viajes = await _context.Viaje
                .Where(v => v.TipoCarroceria == carroceria && v.IdEstado == 4)
                .Include(v => v.IdCargaNavigation) // Incluir la carga para obtener las imágenes
                .ToListAsync();

            return viajes;
        }

        public async Task<List<UserRegistrationReportItem>> GetAllUserRegistrationsAsync()
        {
            return await _context.Usuario
                .Select(u => new UserRegistrationReportItem
                {
                    NombreCompleto = u.Nombre + " " + u.Apellido,
                    TipoUsuario = u.NombreEmpresa != null ? "Empresa" : "Transportista",
                    FechaRegistro = u.FechaRegistro
                })
                .ToListAsync();
        }

        public async Task<List<MonthlyRegistrations>> GetMonthlyRegistrationsAsync()
        {
            var users = await GetAllUserRegistrationsAsync();
            return users
                .GroupBy(u => new { u.FechaRegistro.Year, u.FechaRegistro.Month })
                .Select(g => new MonthlyRegistrations
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Count = g.Count()
                })
                .OrderBy(m => m.Year).ThenBy(m => m.Month)
                .ToList();
        }
        public async Task<IEnumerable<UsuarioReportDto>> GetAllUsersForReportAsync()
        {
            return await _context.Usuario
                .Select(u => new UsuarioReportDto
                {
                    NombreCompleto = u.Nombre + " " + u.Apellido,
                    TipoUsuario = u.NombreEmpresa != null ? "Empresa" : "Transportista",
                    FechaRegistro = u.FechaRegistro
                })
                .ToListAsync();
        }
    }
}
