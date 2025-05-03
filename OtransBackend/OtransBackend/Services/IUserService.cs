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
        Task<Viaje> AddViajeAsync(ViajeDto dto);
        Task<List<ViajeDto>> GetAllViajeAsync();
        Task<ResponseLoginDto> Login(LoginDto loginDto); 
        Task<string> recuperarContra(string correo); 
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly JwtSettingsDto _jwtSettings;
        private readonly EmailUtility _emailUtility;

        public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher, JwtSettingsDto jwtSettings, EmailUtility emailUtility)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtSettings = jwtSettings;
            _emailUtility = emailUtility;
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
        public async Task<Viaje> AddViajeAsync(ViajeDto dto)
        {
            var viaje = new Viaje
            {
                Destino = dto.Destino,
                Origen = dto.Origen,
                Distancia = dto.Distancia = 1,
                Fecha = dto.Fecha = DateTime.Now,
                IdEstado = dto.IdEstado ?? 1, // Default estado
               // IdTransportista = dto.IdTransportista,
               // IdEmpresa = dto.IdEmpresa,
                Peso = dto.Peso,
                TipoCarga = dto.Tipo
            };

            return await _userRepository.AddViajeAsync(viaje);
        }
        public async Task<List<ViajeDto>> GetAllViajeAsync()
        {
            var viajes = await _userRepository.GetAllViajeAsync();

            return viajes.Select(v => new ViajeDto
            {
                IdViaje = v.IdViaje,
                Origen = v.Origen,
                Destino = v.Destino,
                Distancia = v.Distancia,
                Fecha = v.Fecha,
                IdEstado = v.IdEstado,
                IdCarga = v.IdCarga,
                IdTransportista = v.IdTransportista,
                IdEmpresa = v.IdEmpresa
            }).ToList();
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
        public async Task<ResponseLoginDto> Login(LoginDto loginDTO)
        {
            ResponseLoginDto responseLoginDto = new();
            UsuarioDto usuario = new();

            var user = await _userRepository.Login(loginDTO);

            if (user != null && _passwordHasher.VerifyPassword(user.Contrasena, loginDTO.Contrasena)
)
            {
                usuario.IdUsuario = user.IdUsuario;
                usuario.NumIdentificacion = user.NumIdentificacion;
                usuario.Nombre = user.Nombre;
                usuario.Apellido = user.Apellido;
                usuario.Telefono = user.Telefono;
                // usuario.TelefonoSos = user.TelefonoSos;
                usuario.Correo = user.Correo;
                
                usuario.NombreEmpresa = user.NombreEmpresa;
                usuario.NumCuenta = user.NumCuenta; 
                usuario.Direccion = user.Direccion;
                usuario.Licencia = user.Licencia;
                usuario.Nit = user.Nit;
                usuario.IdRol = user.IdRol;
                usuario.IdEstado = user.IdEstado;

                responseLoginDto = JWTUtility.GenTokenkey(responseLoginDto, _jwtSettings);
                responseLoginDto.Respuesta = 1;
                responseLoginDto.Mensaje = "Exitoso";
                responseLoginDto.idRol = user.IdRol;
            }
            else
            {
                responseLoginDto.Respuesta = 0;
                responseLoginDto.Mensaje = "Correo o contraseña incorrecta";
            }

            return responseLoginDto;
        }
        public async Task<string> recuperarContra(string correo)
        {
            var user = await _userRepository.GetUserByEmailAsync(correo);

            if (user == null)
            {
                return "Correo no encontrado";
            }

            // Generar una nueva contraseña aleatoria
            string newPassword = GenerateRandomPassword(8);  // Genera una contraseña de 8 caracteres

            // Hashear la nueva contraseña
            string hashedPassword = _passwordHasher.HashPassword(newPassword);

            // Actualizar la contraseña en la base de datos
            user.Contrasena = hashedPassword;
            await _userRepository.UpdateUserPasswordAsync(user);  // Necesitas este método en tu repositorio

            // Enviar el correo con la nueva contraseña
            string subject = "Recuperación de Contraseña";
            string body = $"Hola {user.Nombre},<br/>Tu nueva contraseña es: <strong>{newPassword}</strong> cambiala lo antes posible";

            try
            {
                await _emailUtility.SendEmailAsync(user.Correo, subject, body);
                return "Correo enviado con la nueva contraseña";
            }
            catch (Exception ex)
            {
                return "Error al enviar el correo";
            }
        }

        // Método para generar una contraseña aleatoria
        private string GenerateRandomPassword(int length)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            Random random = new Random();
            char[] password = new char[length];

            for (int i = 0; i < length; i++)
            {
                password[i] = validChars[random.Next(validChars.Length)];
            }

            return new string(password);
        }

    }
}