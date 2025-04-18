﻿using OtransBackend.Dtos;
using OtransBackend.Repositories.Models;
using OtransBackend.Utilities;
using OtransBackend.Repositories;

namespace OtransBackend.Services
{
    public interface IUserService
    {
        Task<Usuario> RegisterTransportistaAsync(TransportistaDto dto);
        Task<Usuario> RegisterEmpresaAsync(empresaDto dto);
        Task<Vehiculo> AddVehiculoAsync(VehiculoDto dto);
        Task<ResponseLoginDto> Login(LoginDto loginDto);
        Task<string> recuperarContra(string correo);
        Task<IEnumerable<UsuarioRevisionDto>> ObtenerUsuariosPendientesValidacionAsync();
        Task<UsuarioDetalleDto?> ObtenerDetalleUsuarioAsync(int idUsuario);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly JwtSettingsDto _jwtSettings;
        private readonly EmailUtility _emailUtility;
        private readonly GoogleDriveService _googleDriveService;

        public UserService(GoogleDriveService googleDriveService, IUserRepository userRepository, IPasswordHasher passwordHasher, JwtSettingsDto jwtSettings, EmailUtility emailUtility)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtSettings = jwtSettings;
            _emailUtility = emailUtility;
            _googleDriveService = googleDriveService;
        }

        // ---------------------------- REGISTRO TRANSPORTISTA ----------------------------
        public async Task<Usuario> RegisterTransportistaAsync(TransportistaDto dto)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(dto.Correo);
            if (existingUser != null)
                throw new Exception("El correo ya está registrado.");

            var hashedPassword = _passwordHasher.HashPassword(dto.Contrasena);
            String UrlArchiDocu= await _googleDriveService.UploadFileAsync(dto.ArchiDocu, "CC_" + dto.NumIdentificacion);
            string urlLicencia = await _googleDriveService.UploadFileAsync(dto.Licencia, "NIT_" + dto.NumIdentificacion);
            var user = new Usuario
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Correo = dto.Correo,
                Contrasena = hashedPassword,
                Telefono = dto.Telefono,
                TelefonoSos = dto.TelefonoSos,
                NumIdentificacion = dto.NumIdentificacion,
                IdRol = dto.IdRol ?? 1,
                IdEstado = dto.IdEstado ?? 1,
                Licencia = urlLicencia,
                ArchiDocu = UrlArchiDocu
            };

            return await _userRepository.AddTransportistaAsync(user);
        }

        // ---------------------------- REGISTRO EMPRESA ----------------------------
        public async Task<Usuario> RegisterEmpresaAsync(empresaDto dto)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(dto.Correo);
            if (existingUser != null)
                throw new Exception("El correo ya está registrado.");

            var hashedPassword = _passwordHasher.HashPassword(dto.Contrasena);
            String UrlArchiDocu = await _googleDriveService.UploadFileAsync(dto.ArchiDocu, "CC_" + dto.NumIdentificacion);
            string urlNit = await _googleDriveService.UploadFileAsync(dto.NitFile, "NIT_" + dto.NumIdentificacion);
            var user = new Usuario
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Correo = dto.Correo,
                NumIdentificacion = dto.NumIdentificacion,
                Contrasena = hashedPassword,
                Telefono = dto.Telefono,
                TelefonoSos = dto.TelefonoSos,
                NombreEmpresa = dto.NombreEmpresa,
                NumCuenta = dto.NumCuenta,
                Direccion = dto.Direccion,
                Nit = urlNit,
                ArchiDocu = UrlArchiDocu,
                IdRol = dto.IdRol ?? 2,
                IdEstado = dto.IdEstado ?? 1
            };

            return await _userRepository.AddEmpresaAsync(user);
        }

        // ---------------------------- REGISTRO VEHÍCULO ----------------------------
        public async Task<Vehiculo> AddVehiculoAsync(VehiculoDto dto)
        {
            String UrlSoat = await _googleDriveService.UploadFileAsync(dto.Soat, "SOAT_" + dto.NumIdentDueño);
            string urlLicenciaTransito = await _googleDriveService.UploadFileAsync(dto.LicenciaTransito, "LICENCIA_" + dto.NumIdentDueño);
            string urlTecnicomecanica = await _googleDriveService.UploadFileAsync(dto.Tecnicomecanica, "TECNO_" + dto.NumIdentDueño);
            var vehiculo = new Vehiculo
            {
                Placa = dto.Placa,
                CapacidadCarga = dto.CapacidadCarga,
                Soat = UrlSoat,
                Tecnicomecanica = urlLicenciaTransito,
                LicenciaTransito = urlTecnicomecanica,
                NombreDueño = dto.NombreDueño,
                NumIdentDueño = dto.NumIdentDueño,
                TelDueño = dto.TelDueño,
                Carroceria = dto.Carroceria,
                IdTransportista = dto.IdTransportista,
                IdEstado = dto.IdEstado ?? 1
            };

            return await _userRepository.AddVehiculoAsync(vehiculo);
        }

        // ---------------------------- LOGIN ----------------------------
        public async Task<ResponseLoginDto> Login(LoginDto loginDTO)
        {
            ResponseLoginDto responseLoginDto = new();
            UsuarioDto usuario = new();

            var user = await _userRepository.Login(loginDTO);

            if (user != null && _passwordHasher.VerifyPassword(user.Contrasena, loginDTO.Contrasena))
            {
                usuario = new UsuarioDto
                {
                    IdUsuario = user.IdUsuario,
                    NumIdentificacion = user.NumIdentificacion,
                    Nombre = user.Nombre,
                    Apellido = user.Apellido,
                    Telefono = user.Telefono,
                    Correo = user.Correo,
                    NombreEmpresa = user.NombreEmpresa,
                    NumCuenta = user.NumCuenta,
                    Direccion = user.Direccion,
                    IdRol = user.IdRol,
                    IdEstado = user.IdEstado
                };

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

        // ---------------------------- RECUPERACIÓN DE CONTRASEÑA ----------------------------
        public async Task<string> recuperarContra(string correo)
        {
            var user = await _userRepository.GetUserByEmailAsync(correo);
            if (user == null) return "Correo no encontrado";

            string newPassword = GenerateRandomPassword(8);
            string hashedPassword = _passwordHasher.HashPassword(newPassword);

            user.Contrasena = hashedPassword;
            await _userRepository.UpdateUserPasswordAsync(user);

            string subject = "Recuperación de Contraseña";
            string body = $"Hola {user.Nombre},<br/>Tu nueva contraseña es: <strong>{newPassword}</strong>. Cámbiala lo antes posible.";

            try
            {
                await _emailUtility.SendEmailAsync(user.Correo, subject, body);
                return "Correo enviado con la nueva contraseña";
            }
            catch
            {
                return "Error al enviar el correo";
            }
        }

        private string GenerateRandomPassword(int length)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            Random random = new();
            return new string(Enumerable.Repeat(validChars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // ---------------------------- USUARIOS PENDIENTES ----------------------------
        public async Task<IEnumerable<UsuarioRevisionDto>> ObtenerUsuariosPendientesValidacionAsync()
        {
            return await _userRepository.ObtenerUsuariosPendientesValidacionAsync();
        }

        // ---------------------------- UTILIDAD (archivo a bytes) ----------------------------
        private byte[] ConvertFileToBytes(IFormFile file)
        {
            using var memoryStream = new System.IO.MemoryStream();
            file.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
         //---------------------------- DETALLES ----------------------------
        public async Task<UsuarioDetalleDto?> ObtenerDetalleUsuarioAsync(int idUsuario)
        {
            var usuario = await _userRepository.ObtenerUsuarioConVehiculoPorIdAsync(idUsuario);
            if (usuario == null) return null;

            var detalle = new UsuarioDetalleDto
            {
                IdUsuario = usuario.IdUsuario,
                NombreCompleto = $"{usuario.Nombre} {usuario.Apellido}",
                Correo = usuario.Correo,
                Telefono = usuario.Telefono,
                TipoUsuario = usuario.NombreEmpresa != null ? "Empresa" : "Transportista",
                Observaciones = "Pendiente revisión",
                ArchiDocu = usuario.ArchiDocu
            };

            if (usuario.NombreEmpresa != null)
            {
                detalle.Nit = usuario.Nit;
                detalle.Documentos.Add(new DocumentoValidacionDto
                {
                    NombreDocumento = "NIT",
                    EsValido = false
                });
            }
            else
            {
                detalle.Licencia = usuario.Licencia;
                detalle.Documentos.Add(new DocumentoValidacionDto
                {
                    NombreDocumento = "Licencia Conducción",
                    EsValido = false
                });

                if (usuario.Vehiculos.Any())
                {
                    var vehiculo = usuario.Vehiculos.First();
                    detalle.Placa = vehiculo.Placa;
                    detalle.Soat = vehiculo.Soat;
                    detalle.Tecnomecanico = vehiculo.Tecnicomecanica;
                    detalle.LicenciaTransito = vehiculo.LicenciaTransito;

                    detalle.Documentos.AddRange(new List<DocumentoValidacionDto>
                {
                    new DocumentoValidacionDto { NombreDocumento = "Soat", EsValido = false },
                    new DocumentoValidacionDto { NombreDocumento = "Técnico Mecánica", EsValido = false },
                    new DocumentoValidacionDto { NombreDocumento = "Licencia Tránsito", EsValido = false }
                });
                }
            }

            return detalle;
        }
    }
}
