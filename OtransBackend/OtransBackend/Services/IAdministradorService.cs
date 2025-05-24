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

    public interface IAdministradorService
    {
       
        Task<byte[]> GenerateUserReportAsync();

        Task<IEnumerable<UsuarioReportDto>> GetAllUsersForReportAsync();
        Task<IEnumerable<UsuarioRevisionDto>> ObtenerUsuariosPendientesValidacionAsync();
        Task<UsuarioDetalleDto?> ObtenerDetalleUsuarioAsync(int idUsuario);
        Task ValidateUsuarioAsync(UsuarioValidacionDto dto);


    }

    public class AdministradorService : IAdministradorService
    {
        private readonly IAdministradorRepository _administradorRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly JwtSettingsDto _jwtSettings;
        private readonly EmailUtility _emailUtility;
        private readonly GoogleDriveService _googleDriveService;
        private readonly IConfiguration _config;
        private readonly CloudinaryService _cloudinaryService;
        private readonly string _hfToken;    // <<– Aquí
        private readonly IMemoryCache _cache;

        public AdministradorService(GoogleDriveService googleDriveService, IAdministradorRepository administradorRepository, IPasswordHasher passwordHasher, JwtSettingsDto jwtSettings, EmailUtility emailUtility, IConfiguration config, CloudinaryService cloudinaryService, IMemoryCache cache)
        {
            _administradorRepository = administradorRepository;
            _passwordHasher = passwordHasher;
            _jwtSettings = jwtSettings;
            _emailUtility = emailUtility;
            _googleDriveService = googleDriveService;
            _config = config;
            _cloudinaryService = cloudinaryService;

        }

        public async Task<byte[]> GenerateUserReportAsync()
        {
            // 1) Obtener datos
            var users = await _administradorRepository.GetAllUserRegistrationsAsync();
            var monthly = await _administradorRepository.GetMonthlyRegistrationsAsync();
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


        public async Task<IEnumerable<UsuarioReportDto>> GetAllUsersForReportAsync()
        {
            return await _administradorRepository.GetAllUsersForReportAsync();
        }

        public async Task<IEnumerable<UsuarioRevisionDto>> ObtenerUsuariosPendientesValidacionAsync()
        {
            return await _administradorRepository.ObtenerUsuariosPendientesValidacionAsync();
        }
        public async Task<UsuarioDetalleDto?> ObtenerDetalleUsuarioAsync(int idUsuario)
        {
            var usuario = await _administradorRepository.ObtenerUsuarioConVehiculoPorIdAsync(idUsuario);
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
        public async Task ValidateUsuarioAsync(UsuarioValidacionDto dto)
        {
            // 1) Limpia BD y estado
            await _administradorRepository.ValidarUsuarioAsync(dto);

            // 2) Prepara asunto y cuerpo
            bool fueRechazado = dto.Documentos.Any(d => !d.EsValido);
            string asunto = fueRechazado ? "Documentos Rechazados" : "Cuenta Validada";
            string cuerpo = fueRechazado
                ? $"Hola,<br/>Tus documentos fueron rechazados:<br/>{dto.Observaciones}"
                : "¡Tu cuenta ha sido validada correctamente!";

            // 3) Obtén el usuario completo por Id para leer el correo
            var usuario = await _administradorRepository.ObtenerUsuarioConVehiculoPorIdAsync(dto.IdUsuario);
            if (usuario == null)
                throw new KeyNotFoundException($"Usuario con Id {dto.IdUsuario} no encontrado");

            // 4) Envía el correo
            await _emailUtility.SendEmailAsync(usuario.Correo, asunto, cuerpo);
        }

    }
}
