using OtransBackend.Dtos;
using OtransBackend.Repositories.Models;
using OtransBackend.Utilities;
using OtransBackend.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Google.Apis.Drive.v3.Data;
using Microsoft.Extensions.Caching.Memory;

namespace OtransBackend.Services
{
    public interface IUserService
    {
        Task<Usuario> RegisterTransportistaAsync(TransportistaDto dto);
        Task<Usuario> RegisterEmpresaAsync(empresaDto dto);
        Task<Vehiculo> AddVehiculoAsync(VehiculoDto dto);
        Task<ResponseLoginDto> Login(LoginDto loginDto);
        Task<string> recuperarContra(string correo);
        Task<bool> CambiarContrasenaAsync(string correo, string nuevaContrasena);
        Task<IEnumerable<UsuarioRevisionDto>> ObtenerUsuariosPendientesValidacionAsync();
        Task<UsuarioDetalleDto?> ObtenerDetalleUsuarioAsync(int idUsuario);
        Task ValidateUsuarioAsync(UsuarioValidacionDto dto);
        Task ReuploadDocumentosAsync(ReuploadDocumentosDto dto);
        Task<Viaje> ObtenerViajePorTransportista(int idTransportista);
        Task<Carga> ObtenerCargaPorId(int idCarga);

        Task<IEnumerable<VerViajeDto>> ObtenerViajesPorCarroceriaAsync(int transportistaId);

    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly JwtSettingsDto _jwtSettings;
        private readonly EmailUtility _emailUtility;
        private readonly GoogleDriveService _googleDriveService;
        private readonly IConfiguration _config;
        private readonly IMemoryCache _cache;

        public UserService(GoogleDriveService googleDriveService, IUserRepository userRepository, IPasswordHasher passwordHasher, JwtSettingsDto jwtSettings, EmailUtility emailUtility, IConfiguration config, IMemoryCache cache)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtSettings = jwtSettings;
            _emailUtility = emailUtility;
            _googleDriveService = googleDriveService;
            _config = config;
            _cache = cache;
        }

        // ---------------------------- REGISTRO TRANSPORTISTA ----------------------------
        public async Task<Usuario> RegisterTransportistaAsync(TransportistaDto dto)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(dto.Correo);
            if (existingUser != null)
                throw new Exception("El correo ya está registrado.");

            var hashedPassword = _passwordHasher.HashPassword(dto.Contrasena);
            String UrlArchiDocu = await _googleDriveService.UploadFileAsync(dto.ArchiDocu, "CC_" + dto.NumIdentificacion);
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
        // Registro de empresas
        public async Task<Usuario> RegisterEmpresaAsync(empresaDto dto)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(dto.Correo);
            if (existingUser != null)
                throw new Exception("El correo ya está registrado.");
            var docsFolder = _config["GoogleDrive:DocsFolderId"];
            var hashedPassword = _passwordHasher.HashPassword(dto.Contrasena);
            String UrlArchiDocu = await _googleDriveService.UploadFileAsync(dto.ArchiDocu, "CC_" + dto.NumIdentificacion, docsFolder);
            string urlNit = await _googleDriveService.UploadFileAsync(dto.NitFile, "NIT_" + dto.NumIdentificacion, docsFolder);
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
            // ← Desofuscar la contraseña enviada (reverso + Base64)
            //string pwdPlain;
            //try
            //{
            //    pwdPlain = PasswordMasker.Unmask(loginDTO.Contrasena);
            //}
            //catch
            //{
            //    // ← Si falla el Base64 o la estructura, devolvemos error de credenciales
            //    return new ResponseLoginDto
            //    {
            //        Respuesta = 0,
            //        Mensaje = "Formato de contraseña inválido"
            //    };
            //}

            ResponseLoginDto responseLoginDto = new();
            UsuarioDto usuario = new();

            // ← Recuperar usuario por correo (password se valida después)
            var user = await _userRepository.Login(loginDTO);

            // ← Verificar hash de bcrypt con la contraseña desofuscada
            if (user != null && _passwordHasher.VerifyPassword(user.Contrasena, loginDTO.Contrasena))
            {
                // ← Mapear datos de usuario a DTO
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

                // ← Generar token JWT
                responseLoginDto = JWTUtility.GenTokenkey(responseLoginDto, _jwtSettings);
                responseLoginDto.Respuesta = 1;
                responseLoginDto.Mensaje = "Exitoso";
                responseLoginDto.Usuario = usuario;   // ← POPULATE
                responseLoginDto.idRol = user.IdRol;
            }
            else
            {
                // ← Credenciales inválidas
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


        public async Task<bool> CambiarContrasenaAsync(string correo, string nuevaContrasena)
        {
            var usuario = await _userRepository.GetUserByEmailAsync(correo);
            if (usuario == null)
                return false;

            // Hashear la nueva contraseña
            usuario.Contrasena = _passwordHasher.HashPassword(nuevaContrasena);

            await _userRepository.SaveChangesAsync();

            return true;
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

            };

            // Siempre agregamos ArchiDocu como primer documento
            detalle.ArchiDocu = usuario.ArchiDocu;
            detalle.Documentos.Add(new DocumentoValidacionDto
            {
                NombreDocumento = "Documento Identidad",
                EsValido = false
            });

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
         new DocumentoValidacionDto { NombreDocumento = "Soat",             EsValido = false },
         new DocumentoValidacionDto { NombreDocumento = "Técnico Mecánica", EsValido = false },
         new DocumentoValidacionDto { NombreDocumento = "Licencia Tránsito", EsValido = false }
     });
                }
            }

            return detalle;
        }

        //---------------------------- VALIDAR USUARIO  ----------------------------

        public async Task ValidateUsuarioAsync(UsuarioValidacionDto dto)
        {
            // 1) Limpia BD y estado
            await _userRepository.ValidarUsuarioAsync(dto);

            // 2) Prepara asunto y cuerpo
            bool fueRechazado = dto.Documentos.Any(d => !d.EsValido);
            string asunto = fueRechazado ? "Documentos Rechazados" : "Cuenta Validada";
            string cuerpo = fueRechazado
                ? $"Hola,<br/>Tus documentos fueron rechazados:<br/>{dto.Observaciones}"
                : "¡Tu cuenta ha sido validada correctamente!";

            // 3) Obtén el usuario completo por Id para leer el correo
            var usuario = await _userRepository.ObtenerUsuarioConVehiculoPorIdAsync(dto.IdUsuario);
            if (usuario == null)
                throw new KeyNotFoundException($"Usuario con Id {dto.IdUsuario} no encontrado");

            // 4) Envía el correo
            await _emailUtility.SendEmailAsync(usuario.Correo, asunto, cuerpo);
        }

        public async Task ReuploadDocumentosAsync(ReuploadDocumentosDto dto)
        {
            // 1) Sube cada archivo y actualiza la URL guardada en BD
            foreach (var doc in dto.Documentos)
            {
                // Ahora UploadFileAsync devuelve la URL directamente
                string url = await _googleDriveService.UploadFileAsync(
                    doc.Archivo,
                    $"{doc.NombreDocumento}_{dto.IdUsuario}"
                );

                // Guarda esa URL en la columna correspondiente
                await _userRepository.ActualizarDocumentoAsync(
                    dto.IdUsuario,
                    doc.NombreDocumento,
                    url
                );
            }

            // 2) Cambia de nuevo a PendienteValidacion usando el repo
            await _userRepository.CambiarEstadoAsync(dto.IdUsuario, "PendienteValidacion");
        }

        public async Task<Viaje> ObtenerViajePorTransportista(int idTransportista)
        {
            return await _userRepository.ObtenerViajePorTransportista(idTransportista); // Aquí corregimos el uso del repositorio
        }

        // Obtener la carga asociada al viaje
        public async Task<Carga> ObtenerCargaPorId(int idCarga)
        {
            return await _userRepository.ObtenerCargaPorId(idCarga); // Correcta llamada al repositorio

        }
        public async Task<IEnumerable<VerViajeDto>> ObtenerViajesPorCarroceriaAsync(int transportistaId)
        {
            var viajes = await _userRepository.ObtenerViajesPorCarroceriaAsync(transportistaId);

            // Convertir los viajes a VerViajeDto
            var viajeDtos = viajes.Select(v =>
            {
                var carga = v.IdCargaNavigation; // Obtener la carga asociada al viaje
                return new VerViajeDto(v, carga);  // Mapear el viaje y la carga al DTO
            });

            return viajeDtos;
        }
    }
}
