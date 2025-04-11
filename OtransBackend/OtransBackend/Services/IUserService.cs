using OtransBackend.Dtos;
using OtransBackend.Repositories.Models;
using OtransBackend.Utilities;
using OtransBackend.Repositories;

namespace OtransBackend.Services
{
    public interface IUserService
    {
        Task<Usuario> RegisterTransportistaAsync(TransportistaDto dto); // El archivo está en el DTO
        Task<Usuario> RegisterEmpresaAsync(empresaDto dto); // El archivo está en el DTO
        Task<Vehiculo> AddVehiculoAsync(VehiculoDto dto);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        // Registro de transportistas
        public async Task<Usuario> RegisterTransportistaAsync(TransportistaDto dto)
        {
            // Verificar si el correo ya existe
            var existingUser = await _userRepository.GetUserByEmailAsync(dto.Correo);
            if (existingUser != null)
            {
                throw new Exception("El correo ya está registrado.");
            }

            // Encriptar la contraseña
            var hashedPassword = _passwordHasher.HashPassword(dto.Contrasena);

            // Crear el transportista con valores predeterminados si no se proporcionan
            var user = new Usuario
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Correo = dto.Correo,
                Contrasena = hashedPassword,
                Telefono = dto.Telefono,
                TelefonoSos = dto.TelefonoSos,
                NumIdentificacion = dto.NumIdentificacion,
                IdRol = dto.IdRol ?? 1,  // Rol predeterminado
                IdEstado = dto.IdEstado ?? 1  // Estado predeterminado
            };

            // Asignación de Licencia y ArchiDocu si existen
            user.Licencia = dto.Licencia != null ? ConvertFileToBytes(dto.Licencia) : null;
            user.ArchiDocu = dto.ArchiDocu != null ? ConvertFileToBytes(dto.ArchiDocu) : null;

            // Guardar el transportista en la base de datos
            return await _userRepository.AddTransportistaAsync(user, dto.Licencia, dto.ArchiDocu);
        }


        // Registro de empresas
        public async Task<Usuario> RegisterEmpresaAsync(empresaDto dto)
        {
            // Verificar si el correo ya existe
            var existingUser = await _userRepository.GetUserByEmailAsync(dto.Correo);
            if (existingUser != null)
            {
                throw new Exception("El correo ya está registrado.");
            }

            // Encriptar la contraseña
            var hashedPassword = _passwordHasher.HashPassword(dto.Contrasena);

            // Crear la empresa
            var user = new Usuario
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Correo = dto.Correo,
                NumIdentificacion=dto.NumIdentificacion,
                Contrasena = hashedPassword,
                Telefono = dto.Telefono,
                TelefonoSos = dto.TelefonoSos,
                NombreEmpresa = dto.NombreEmpresa,
                NumCuenta = dto.NumCuenta,
                Direccion = dto.Direccion,
                Nit = null,  // Inicializamos como null
                ArchiDocu = null,
                IdRol = dto.IdRol ?? 2,
                IdEstado = dto.IdEstado ?? 1
            };

            // Si se ha enviado el NIT, lo convertimos y lo asignamos al usuario
            if (dto.NitFile != null)
            {
                byte[] nitFileBytes = ConvertFileToBytes(dto.NitFile);  // Convertimos el archivo a binario
                user.Nit = nitFileBytes;  // Asignamos el binario
            }
            if (dto.ArchiDocu != null)
            {
                byte[] ArchiDocuBytes = ConvertFileToBytes(dto.ArchiDocu);  // Convertimos el archivo a binario
                user.ArchiDocu = ArchiDocuBytes;  // Asignamos el binario
            }

            // Guardar la empresa en la base de datos
            return await _userRepository.AddEmpresaAsync(user, dto.NitFile,dto.ArchiDocu); // Usamos el método específico para empresa
        }

        // Convertir archivo a binario
        private byte[] ConvertFileToBytes(IFormFile file)
        {
            using (var memoryStream = new System.IO.MemoryStream())
            {
                file.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public async Task<Vehiculo> AddVehiculoAsync(VehiculoDto dto)
        {


            // Crear la vehiculo
            var vehiculo = new Vehiculo
            {
                Placa = dto.Placa,
                CapacidadCarga = dto.CapacidadCarga,
                Soat = null,
                Tecnicomecanica = null,
                LicenciaTransito = null,
                NombreDueño = dto.NombreDueño,
                NumIdentDueño = dto.NumIdentDueño,
                TelDueño = dto.TelDueño,
                Carroceria = dto.Carroceria,
                IdTransportista = dto.IdTransportista,
                IdEstado = dto.IdEstado ?? 1
            };

            // Si se ha enviado el SOAT, lo convertimos y lo asignamos al vehiculo
            if (dto.Soat != null)
            {
                byte[] soatFileBytes = ConvertFileToBytes(dto.Soat);  // Convertimos el archivo a binario
                vehiculo.Soat = soatFileBytes;  // Asignamos el binario
            }
            // Si se ha enviado el Tecnicomecanica, lo convertimos y lo asignamos al vehiculo
            if (dto.Tecnicomecanica != null)
            {
                byte[] TecnicomecanicaFileBytes = ConvertFileToBytes(dto.Tecnicomecanica);  // Convertimos el archivo a binario
                vehiculo.Soat = TecnicomecanicaFileBytes;  // Asignamos el binario
            }
            // Si se ha enviado el LicenciaTransito, lo convertimos y lo asignamos al vehiculo
            if (dto.LicenciaTransito != null)
            {
                byte[] LicenciaTransitoFileBytes = ConvertFileToBytes(dto.LicenciaTransito);  // Convertimos el archivo a binario
                vehiculo.Soat = LicenciaTransitoFileBytes;  // Asignamos el binario
            }

            // Guardar la empresa en la base de datos
            return await _userRepository.AddVehiculoAsync(vehiculo, dto.Soat, dto.Tecnicomecanica, dto.LicenciaTransito); // Usamos el método específico para empresa
        }
    }
}