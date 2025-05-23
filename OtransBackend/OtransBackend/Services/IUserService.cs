using OtransBackend.Dtos;
using OtransBackend.Repositories.Models;
using OtransBackend.Utilities;
using OtransBackend.Repositories;
using SkiaSharp;
using iText.Kernel.Pdf;
using iText.Kernel.Geom;
using iText.IO.Image;
using iText.Layout;
// Para PageSize
// Alias para evitar ambigüedades
using Doc = iText.Layout.Document;
using ITextImage = iText.Layout.Element.Image;
using ITextParagraph = iText.Layout.Element.Paragraph;
using ITextTable = iText.Layout.Element.Table;
using UV = iText.Layout.Properties.UnitValue;
using iText.IO.Font.Constants;      // StandardFonts
using iText.Kernel.Font;            // PdfFont
using TextAlignment = iText.Layout.Properties.TextAlignment;
using iText.Commons.Actions;
using iText.Kernel.Colors;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Event;
using iText.Kernel.Pdf.Extgstate;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.Extensions.Logging;
using iText.Layout.Borders;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using SkiaSharp;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Extgstate;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;



using Microsoft.AspNetCore.Identity;
using Google.Apis.Drive.v3.Data;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;

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
        Task<byte[]> GenerateUserReportAsync();
       
        Task<IEnumerable<UsuarioReportDto>> GetAllUsersForReportAsync();


    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly JwtSettingsDto _jwtSettings;
        private readonly EmailUtility _emailUtility;
        private readonly GoogleDriveService _googleDriveService;
        private readonly IConfiguration _config;
        private readonly CloudinaryService _cloudinaryService;
        private readonly string _hfToken;    // <<– Aquí
        private readonly IMemoryCache _cache;




        public UserService(GoogleDriveService googleDriveService, IUserRepository userRepository, IPasswordHasher passwordHasher, JwtSettingsDto jwtSettings, EmailUtility emailUtility, IConfiguration config, CloudinaryService cloudinaryService, IMemoryCache cache)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtSettings = jwtSettings;
            _emailUtility = emailUtility;
            _googleDriveService = googleDriveService;
            _config = config;
            _cloudinaryService = cloudinaryService;
            
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
            // Subir archivos a Cloudinary
            string urlArchiDocu = await _googleDriveService.UploadFileAsync(dto.ArchiDocu, "CC_" + dto.NumIdentificacion);
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
                ArchiDocu = urlArchiDocu,
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
            ////← Desofuscar la contraseña enviada(reverso +Base64)
            string pwdPlain;
            try
            {
                pwdPlain = PasswordMasker.Unmask(loginDTO.Contrasena);
            }
            catch
            {
                // ← Si falla el Base64 o la estructura, devolvemos error de credenciales
                return new ResponseLoginDto
                {
                    Respuesta = 0,
                    Mensaje = "Formato de contraseña inválido"
                };
            }

            ResponseLoginDto responseLoginDto = new();
            UsuarioDto usuario = new();

            // ← Recuperar usuario por correo (password se valida después)
            var user = await _userRepository.Login(loginDTO);

            // ← Verificar hash de bcrypt con la contraseña desofuscada
            if (user != null && _passwordHasher.VerifyPassword(user.Contrasena, pwdPlain))
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

            string subject = "Recuperación de contraseña - Otrans";

            string body = $@"
<!DOCTYPE html>
<html>
<head>
<meta charset='UTF-8'>
<title>Recuperación de contraseña</title>
<style>
  body {{
    font-family: Arial, sans-serif;
    background-color: #f7f7f7;
    margin: 0; padding: 0;
    color: #000000; /* Negro para todo el texto */
  }}
  .header {{
    padding: 20px 30px;
    border-bottom: 1px solid #FF6600;
    display: flex;
    align-items: center;
  }}
  .header img {{
    width: 150px;
  }}
  .content {{
    max-width: 600px;
    margin: 40px auto 60px;
    padding: 0 30px;
    color: #000000; /* Asegura negro en el contenido */
  }}
  h2 {{
    color: #FF6600;
  }}
  p {{
    font-size: 16px;
    line-height: 1.5;
    color: #000000; /* Negro para los párrafos */
  }}
  strong {{
    color: #000000; /* Fuerza negrita negra */
  }}
  code {{
    background-color: #f0f0f0;
    padding: 3px 6px;
    border-radius: 4px;
    font-size: 1.1em;
    color: #000000; /* También el texto del código */
  }}
  a {{
    color: #000000; /* Enlace negro (puedes cambiar a azul si quieres) */
  }}
  .footer {{
    background-color: #FF6600;
    color: white;
    padding: 20px 30px;
    font-size: 14px;
    text-align: center;
  }}
  .footer a {{
    color: white;
    text-decoration: none;
    margin: 0 10px;
  }}
  .footer .icon {{
    vertical-align: middle;
    margin-right: 5px;
  }}
</style>
</head>
<body>
  <div class='header'>
    <img src='https://res.cloudinary.com/dxdkogbrb/image/upload/v1747805261/ff591636-25e0-4c38-a21d-ba2866856119_wy1iep.jpg' alt='Otrans Logo'/>
  </div>
  <div class='content'>
    <h2>Hola {user.Nombre},</h2>
    <p>Hemos generado una nueva contraseña para tu cuenta en <strong>Otrans</strong>.</p>
    <p><strong>Tu nueva contraseña es:</strong> <code>{newPassword}</code></p>
    <p>Por motivos de seguridad, te recomendamos cambiar esta contraseña lo antes posible ingresando a tu perfil en nuestra plataforma.</p>
    <p>Si tú no solicitaste este cambio, por favor contacta con nuestro soporte inmediatamente en <a href='mailto:soporte@otrans.com'>soporte@otrans.com</a>.</p>
    <p>Saludos,<br />El equipo de <strong>Otrans</strong></p>
  </div>
  <div class='footer'>
    <span>📧 contacto@otrans.com</span> |
    <span>🌐 <a href='https://www.otrans.com'>www.otrans.com</a></span> |
    <span>📞 3XX-XXX-XXXX</span><br/>
    &copy; {DateTime.Now.Year} Otrans - Todos los derechos reservados
  </div>
</body>
</html>
";


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

        public async Task<int> RegisterAsync(CargaDto dto)
        {
            // Recolectar las IFormFile en array
            var archivos = new[]
            {
            dto.Imagen1, dto.Imagen2, dto.Imagen3, dto.Imagen4, dto.Imagen5,
            dto.Imagen6, dto.Imagen7, dto.Imagen8, dto.Imagen9, dto.Imagen10
        };

            // Array para URLs
            var urls = new string?[10];

            for (int i = 0; i < archivos.Length; i++)
            {
                var file = archivos[i];
                if (file is not null && file.Length > 0)
                {
                    // Nombre custom: e.g. “Carga_<GUID>_Img{i+1}”


                    var customName = $"Carga_{Guid.NewGuid():N}_Img{i + 1}";
                    urls[i] = await _cloudinaryService.UploadFileAsync(file, customName);
                }
            }

            // Crear entidad
            var entity = new Carga
            {
                Imagen1 = urls[0],
                Imagen2 = urls[1],
                Imagen3 = urls[2],
                Imagen4 = urls[3],
                Imagen5 = urls[4],
                Imagen6 = urls[5],
                Imagen7 = urls[6],
                Imagen8 = urls[7],
                Imagen9 = urls[8],
                Imagen10 = urls[9],
                IdEstado = dto.IdEstado
            };

            // Guardar en BD
            var saved = await _userRepository.AddAsync(entity);
            return saved.IdCarga;
        }
        public async Task<Carga> GetByIdAsync(int id)
        {
            var carga = await _userRepository.GetByIdAsync(id);
            if (carga == null)
                throw new KeyNotFoundException($"Carga con Id {id} no encontrada.");
            return carga;
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

        public async Task<byte[]> GenerateUserReportAsync()
        {
            // 1) Obtener datos
            var users = await _userRepository.GetAllUserRegistrationsAsync();
            var monthly = await _userRepository.GetMonthlyRegistrationsAsync();
            // 1.1) Generar análisis con Hugging Face
            //string analysis = await GetMonthlyAnalysisAsync(monthly);


            // 2) Gráfico de barras con SkiaSharp (ahora arrancan del suelo)
            byte[] barChartBytes;
            const int barW = 600, barH = 400;
            using (var bmp = new SKBitmap(barW, barH))
            using (var cv = new SKCanvas(bmp))
            {
                cv.Clear(SKColors.White);

                // Márgenes y baseline
                float sideMargin = 50f;
                float topMargin = 80f;   // espacio bajo el título
                float bottomMargin = 50f;   // espacio bajo el eje X
                float chartW = barW - sideMargin * 2;
                float chartH = barH - topMargin - bottomMargin;
                float baseline = topMargin + chartH; // “suelo” de la gráfica
                int pts = monthly.Count;

                // Título
                var titlePaint = new SKPaint { Color = SKColors.Black, TextSize = 22, IsAntialias = true };
                cv.DrawText("Usuarios registrados por mes",
                            sideMargin,
                            topMargin - 20,
                            titlePaint);

                // Ejes
                var axisPaint = new SKPaint { Color = SKColors.Black, StrokeWidth = 2 };
                // Y
                cv.DrawLine(sideMargin, topMargin,
                            sideMargin, baseline,
                            axisPaint);
                // X
                cv.DrawLine(sideMargin, baseline,
                            sideMargin + chartW, baseline,
                            axisPaint);

                if (pts > 0)
                {
                    float barWidth = chartW / pts * 0.8f;
                    int maxCount = monthly.Max(m => m.Count);

                    // Cuadrícula horizontal
                    var gridPaint = new SKPaint { Color = SKColors.LightGray, StrokeWidth = 1 };
                    for (int i = 1; i <= 5; i++)
                    {
                        float y = topMargin + chartH * i / 5;
                        cv.DrawLine(sideMargin, y,
                                    sideMargin + chartW, y,
                                    gridPaint);
                    }

                    var barPaint = new SKPaint { Color = SKColors.Orange, IsAntialias = true };
                    var textPaint = new SKPaint { Color = SKColors.Black, TextSize = 14, IsAntialias = true };

                    for (int i = 0; i < pts; i++)
                    {
                        var m = monthly[i];
                        float x = sideMargin + i * (chartW / pts) + ((chartW / pts) - barWidth) / 2;
                        float hgt = (m.Count / (float)maxCount) * chartH;

                        // Dibuja la barra desde baseline-hgt hasta baseline
                        var rect = new SKRect(
                            x,
                            baseline - hgt,
                            x + barWidth,
                            baseline
                        );
                        cv.DrawRect(rect, barPaint);

                        // Valor encima de la barra
                        string val = m.Count.ToString();
                        float vw = textPaint.MeasureText(val);
                        cv.DrawText(val,
                                    x + (barWidth - vw) / 2,
                                    baseline - hgt - 5,
                                    textPaint);

                        // Mes debajo del eje X
                        textPaint.TextSize = 12;
                        string lbl = new DateTime(m.Year, m.Month, 1)
                                           .ToString("MMM yyyy");
                        float lw = textPaint.MeasureText(lbl);
                        cv.DrawText(lbl,
                                    x + (barWidth - lw) / 2,
                                    baseline + 20,
                                    textPaint);
                        textPaint.TextSize = 14;
                    }
                }

                using var img = SKImage.FromBitmap(bmp);
                using var data = img.Encode(SKEncodedImageFormat.Png, 90);
                barChartBytes = data.ToArray();
            }

            // 3) Generar gráfico de pastel con SkiaSharp + leyenda colores
            byte[] pieChartBytes;
            const int pieSize = 300;
            SKColor[] palette = {
        SKColors.Orange,
        SKColors.DarkOrange,
        SKColors.Gold,
        SKColors.Gray,
        SKColors.LightGray
    };
            using (var bmp = new SKBitmap(pieSize, pieSize))
            using (var cv = new SKCanvas(bmp))
            {
                cv.Clear(SKColors.White);
                float cx = pieSize / 2f;
                float cy = pieSize / 2f;
                float radius = pieSize * 0.4f;
                float total = monthly.Sum(m => m.Count);
                float start = 0;

                var textPaint = new SKPaint { Color = SKColors.Black, TextSize = 14, IsAntialias = true };
                var titlePaint = new SKPaint { Color = SKColors.Black, TextSize = 18, IsAntialias = true };

                // Título del pastel
                string pieTitle = "Distribución mensual";
                cv.DrawText(pieTitle,
                    (pieSize - titlePaint.MeasureText(pieTitle)) / 2,
                    20,
                    titlePaint);

                for (int i = 0; i < monthly.Count; i++)
                {
                    var m = monthly[i];
                    float sweep = m.Count / total * 360f;

                    var paint = new SKPaint { Color = palette[i % palette.Length], IsAntialias = true };
                    var rect = new SKRect(cx - radius, cy - radius, cx + radius, cy + radius);
                    cv.DrawArc(rect, start, sweep, true, paint);

                    // Etiqueta %
                    float mid = (start + sweep / 2) * (float)Math.PI / 180f;
                    string pct = $"{m.Count / total * 100f:0}%";
                    float tx = cx + (radius * 0.6f) * (float)Math.Cos(mid) - textPaint.MeasureText(pct) / 2;
                    float ty = cy + (radius * 0.6f) * (float)Math.Sin(mid) + 5;
                    cv.DrawText(pct, tx, ty, textPaint);

                    start += sweep;
                }

                using var imgPie = SKImage.FromBitmap(bmp);
                using var data = imgPie.Encode(SKEncodedImageFormat.Png, 90);
                pieChartBytes = data.ToArray();
            }

            // 4) Generar PDF e insertar tabla, gráficos y leyenda de pastel
            using var ms = new MemoryStream();
            using var writer = new PdfWriter(ms);
            using var pdf = new PdfDocument(writer);
            var doc = new Document(pdf, PageSize.A4);
            var boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

            // Título principal
            doc.Add(new Paragraph("Reporte de Usuarios Registrados")
                .SetFont(boldFont)
                .SetFontSize(18)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetMarginBottom(10)
            );
            // 4.2 Análisis automático
            //doc.Add(new Paragraph("Análisis Automático")
            //    .SetFont(boldFont)
            //    .SetFontSize(14)
            //    .SetMarginTop(10)
            //    .SetMarginBottom(5)
            //);
            ////doc.Add(new Paragraph(analysis)
            ////    .SetFontSize(11)
            ////    .SetMarginBottom(20)
            ////);

            // Tabla estilizada (zebra + cabecera naranja)
            var table = new Table(UnitValue.CreatePercentArray(new[] { 4f, 3f, 3f }))
                .UseAllAvailableWidth()
                .SetFontSize(12);
            var hdrColor = new DeviceRgb(255, 165, 0);
            foreach (var h in new[] { "Nombre", "Tipo", "Fecha Registro" })
            {
                table.AddHeaderCell(new Cell()
                    .Add(new Paragraph(h))
                    .SetBackgroundColor(hdrColor)
                    .SetFontColor(ColorConstants.WHITE)
                    .SetPadding(5)
                    .SetTextAlignment(TextAlignment.CENTER)
                );
            }
            for (int i = 0; i < users.Count; i++)
            {
                var u = users[i];
                var rowBg = (i % 2 == 0)
                    ? ColorConstants.WHITE
                    : new DeviceRgb(245, 245, 245);
                table.AddCell(new Cell().Add(new Paragraph(u.NombreCompleto)).SetBackgroundColor(rowBg).SetPadding(5));
                table.AddCell(new Cell().Add(new Paragraph(u.TipoUsuario)).SetBackgroundColor(rowBg).SetTextAlignment(TextAlignment.CENTER).SetPadding(5));
                table.AddCell(new Cell().Add(new Paragraph(u.FechaRegistro.ToString("yyyy-MM-dd"))).SetBackgroundColor(rowBg).SetTextAlignment(TextAlignment.CENTER).SetPadding(5));
            }
            doc.Add(table);

            // Gráfico de barras
            doc.Add(new Paragraph("1. Usuarios por mes")
                .SetFont(boldFont)
                .SetFontSize(14)
                .SetMarginTop(20)
            );
            doc.Add(new Image(ImageDataFactory.Create(barChartBytes))
                .ScaleToFit(barW, barH)
                .SetHorizontalAlignment(HorizontalAlignment.CENTER)
            );

            // Gráfico de pastel
            doc.Add(new Paragraph("2. Distribución porcentual")
                .SetFont(boldFont)
                .SetFontSize(14)
                .SetMarginTop(20)
            );
            doc.Add(new Image(ImageDataFactory.Create(pieChartBytes))
                .ScaleToFit(pieSize, pieSize)
                .SetHorizontalAlignment(HorizontalAlignment.CENTER)
            );

            // Leyenda de pastel bajo el gráfico
            var legend = new Table(UnitValue.CreatePercentArray(new[] { 1f, 4f }))
                .UseAllAvailableWidth()
                .SetMarginTop(10);
            for (int i = 0; i < monthly.Count; i++)
            {
                var m = monthly[i];
                var color = palette[i % palette.Length];
                // Caja de color
                legend.AddCell(new Cell()
                    .SetBackgroundColor(new DeviceRgb(color.Red, color.Green, color.Blue))
                    .SetPadding(5)
                    .SetBorder(Border.NO_BORDER)
                    .SetWidth(20)
                );
                // Etiqueta mes
                legend.AddCell(new Cell()
                    .Add(new Paragraph(new DateTime(m.Year, m.Month, 1).ToString("MMM yyyy")))
                    .SetPadding(5)
                    .SetBorder(Border.NO_BORDER)
                );
            }
            doc.Add(legend);

            doc.Close();
            return ms.ToArray();
        }
        //private async Task<string> GetMonthlyAnalysisAsync(IEnumerable<MonthlyRegistrations> monthly)
        //{
        //    // Construye el prompt con tus datos
        //    var sb = new StringBuilder();
        //    sb.AppendLine("Datos de registros mensuales:");
        //    foreach (var m in monthly)
        //    {
        //        sb.AppendLine($"{new DateTime(m.Year, m.Month, 1):MMM yyyy}: {m.Count}");
        //    }
        //    sb.AppendLine();
        //    sb.AppendLine("Genera un breve análisis indicando tendencias y recomendaciones:");

        //    // Llama a Hugging Face Inference
        //    using var client = new HttpClient();
        //    client.DefaultRequestHeaders.Authorization =
        //        new AuthenticationHeaderValue("Bearer", _hfToken);

        //    var payload = new { inputs = sb.ToString() };
        //    var jsonBody = JsonSerializer.Serialize(payload);
        //    using var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        //    var resp = await client.PostAsync(
        //        "https://api-inference.huggingface.co/models/bigscience/bloomz-1b7",
        //        content
        //    );
        //    resp.EnsureSuccessStatusCode();

        //    // La respuesta suele ser un array de strings
        //    var resultJson = await resp.Content.ReadAsStringAsync();
        //    var results = JsonSerializer.Deserialize<string[]>(resultJson);
        //    return results?.FirstOrDefault()?.Trim()
        //           ?? "No se generó análisis.";
        //}



        // ----- Clase para la marca de agua -----

        public async Task<IEnumerable<UsuarioReportDto>> GetAllUsersForReportAsync()
        {
            return await _userRepository.GetAllUsersForReportAsync();
        }

    }

}
