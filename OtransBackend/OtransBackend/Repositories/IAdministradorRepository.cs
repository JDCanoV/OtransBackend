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
    public interface IAdministradorRepository
    {
        Task<IEnumerable<UsuarioReportDto>> GetAllUsersForReportAsync();
        Task<List<UserRegistrationReportItem>> GetAllUserRegistrationsAsync();
        Task<List<MonthlyRegistrations>> GetMonthlyRegistrationsAsync();
        Task<IEnumerable<UsuarioRevisionDto>> ObtenerUsuariosPendientesValidacionAsync();
        Task<Usuario?> ObtenerUsuarioConVehiculoPorIdAsync(int idUsuario);
        Task ValidarUsuarioAsync(UsuarioValidacionDto dto);
    }
        public class AdministradorRepository : IAdministradorRepository
        {
            private readonly Otrans _context;

            public AdministradorRepository(Otrans context)
            {
                _context = context;
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
        }

    
}