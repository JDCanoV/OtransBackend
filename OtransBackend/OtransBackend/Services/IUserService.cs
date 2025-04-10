using OtransBackend.Dtos;
using OtransBackend.Repositories.Models;
using OtransBackend.Utilities;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using OtransBackend.Repositories;

namespace OtransBackend.Services
{
    public interface IUserService
    {
        Task<Usuario> RegisterTransportistaAsync(TransportistaDto dto); // El archivo está en el DTO
        Task<Usuario> RegisterEmpresaAsync(empresaDto dto); // El archivo está en el DTO
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

            // Crear el transportista
            var user = new Usuario
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Correo = dto.Correo,
                Contrasena = hashedPassword,
                Telefono = dto.Telefono,
                TelefonoSos = dto.TelefonoSos,
                Licencia = null,  // Inicializamos como null
                IdRol = dto.IdRol ?? 1,  // Rol predeterminado
                IdEstado = dto.IdEstado ?? 1  // Estado predeterminado
            };

            // Si se ha enviado la licencia, la convertimos y la asignamos al usuario
            if (dto.Licencia != null)
            {
                byte[] licenciaFileBytes = ConvertFileToBytes(dto.Licencia);  // Convertimos el archivo a binario
                user.Licencia = licenciaFileBytes;  // Asignamos el binario
            }

            // Guardar el transportista en la base de datos
            return await _userRepository.AddTransportistaAsync(user, dto.Licencia); // Usamos el método específico para transportista
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
                Contrasena = hashedPassword,
                Telefono = dto.Telefono,
                TelefonoSos = dto.TelefonoSos,
                NombreEmpresa = dto.NombreEmpresa,
                NumCuenta = dto.NumCuenta,
                Direccion = dto.Direccion,
                Nit = null,  // Inicializamos como null
                IdRol = dto.IdRol ?? 2, 
                IdEstado = dto.IdEstado ?? 1  
            };

            // Si se ha enviado el NIT, lo convertimos y lo asignamos al usuario
            if (dto.NitFile != null)
            {
                byte[] nitFileBytes = ConvertFileToBytes(dto.NitFile);  // Convertimos el archivo a binario
                user.Nit = nitFileBytes;  // Asignamos el binario
            }

            // Guardar la empresa en la base de datos
            return await _userRepository.AddEmpresaAsync(user, dto.NitFile); // Usamos el método específico para empresa
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
