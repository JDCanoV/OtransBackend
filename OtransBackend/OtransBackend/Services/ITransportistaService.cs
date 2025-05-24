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
    public interface ITransportistaService
    {
        Task<Viaje> ObtenerViajePorTransportista(int idTransportista);
        Task<Carga> ObtenerCargaPorId(int idCarga);
        Task<IEnumerable<VerViajeDto>> ObtenerViajesPorCarroceriaAsync(int transportistaId);
        Task<bool> AsignarTransportistaYActualizarEstadoAsync(int idViaje, int idTransportista);
    }
    public class TransportistaService : ITransportistaService
    {
        private readonly ITransportistaRepository _transportistaRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly JwtSettingsDto _jwtSettings;
        private readonly EmailUtility _emailUtility;
        private readonly GoogleDriveService _googleDriveService;
        private readonly IConfiguration _config;
        private readonly CloudinaryService _cloudinaryService;
        private readonly string _hfToken;    // <<– Aquí
        private readonly IMemoryCache _cache;

        public TransportistaService(GoogleDriveService googleDriveService, ITransportistaRepository transportistaRepository, IPasswordHasher passwordHasher, JwtSettingsDto jwtSettings, EmailUtility emailUtility, IConfiguration config, CloudinaryService cloudinaryService, IMemoryCache cache)
        {
            _transportistaRepository = transportistaRepository;
            _passwordHasher = passwordHasher;
            _jwtSettings = jwtSettings;
            _emailUtility = emailUtility;
            _googleDriveService = googleDriveService;
            _config = config;
            _cloudinaryService = cloudinaryService;

        }
        public async Task<Viaje> ObtenerViajePorTransportista(int idTransportista)
        {
            return await _transportistaRepository.ObtenerViajePorTransportista(idTransportista); // Aquí corregimos el uso del repositorio
        }
        public async Task<Carga> ObtenerCargaPorId(int idCarga)
        {
            return await _transportistaRepository.ObtenerCargaPorId(idCarga); // Correcta llamada al repositorio

        }

        public async Task<IEnumerable<VerViajeDto>> ObtenerViajesPorCarroceriaAsync(int transportistaId)
        {
            var viajes = await _transportistaRepository.ObtenerViajesPorCarroceriaAsync(transportistaId);


            // Convertir los viajes a VerViajeDto
            var viajeDtos = viajes.Select(v =>
            {
                var carga = v.IdCargaNavigation; // Obtener la carga asociada al viaje
                return new VerViajeDto(v, carga);  // Mapear el viaje y la carga al DTO
            });

            return viajeDtos;
        }

        public async Task<bool> AsignarTransportistaYActualizarEstadoAsync(int idViaje, int idTransportista)
        {
            int estadoAsignado = 5; // Estado al que quieres cambiar
            return await _transportistaRepository.AsignarTransportistaYActualizarEstadoAsync(idViaje, idTransportista, estadoAsignado);
        }

    }
}
