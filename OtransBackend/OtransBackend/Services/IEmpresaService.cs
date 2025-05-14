using OtransBackend.Dtos;
using OtransBackend.Repositories;
using OtransBackend.Repositories.Models;

namespace OtransBackend.Services
{
    public interface IEmpresaService
    {
        Task<Viaje> AddViajeAsync(ViajeDto dto);
        Task<List<ViajeDto>> GetViajesByEmpresaAsync(int idEmpresa);
        Task<int> AddEvidenciaAsync(CargaDto dto);
        Task<Carga> GetEvidenciaByIdAsync(int id);
    }
    public class EmpresaService : IEmpresaService
    {
        private readonly IEmpresaRepository _empresaRepository;
        private readonly GoogleDriveService _googleDriveService;
        private readonly IConfiguration _config;

        public EmpresaService(GoogleDriveService googleDriveService, IEmpresaRepository empresaRepository, IConfiguration config)
        {
            _empresaRepository = empresaRepository;
            _googleDriveService = googleDriveService;
            _config = config;
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
                IdEmpresa = dto.IdEmpresa,
                Peso = dto.Peso,
                TipoCarga = dto.TipoCarga,
                TipoCarroceria = dto.TipoCarroceria,
                TamanoVeh = dto.TamanoVeh,
                Descripcion = dto.Descripcion,
                IdCarga = dto.IdCarga
            };

            return await _empresaRepository.AddViajeAsync(viaje);
        }
        public async Task<List<ViajeDto>> GetViajesByEmpresaAsync(int idEmpresa)
        {
            // Obtener los viajes de la empresa, incluyendo el nombre del transportista
            var viajes = await _empresaRepository.GetViajesByEmpresaAsync(idEmpresa);

            // Mapear los resultados a ViajeDto
            var viajesDto = viajes.Select(v => new ViajeDto
            {
                IdViaje = v.IdViaje,
                Origen = v.Origen,
                Destino = v.Destino,
                Distancia = v.Distancia,
                Fecha = v.Fecha,
                IdEstado = v.IdEstado,
                IdCarga = v.IdCarga,
                Peso = v.Peso,
                TipoCarroceria = v.TipoCarroceria,
                TipoCarga = v.TipoCarga,
                TamanoVeh = v.TamanoVeh,
                Descripcion = v.Descripcion,
                IdTransportista = v.IdTransportista,
                NombreTransportista = v.IdTransportistaNavigation?.Nombre + " " + v.IdTransportistaNavigation?.Apellido ?? "N/A"
                // Acceder al nombre del transportista
            }).ToList();

            return viajesDto;
        }
        public async Task<int> AddEvidenciaAsync(CargaDto dto)
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

                    var imgFolder = _config["GoogleDrive:ImgFolderId"];
                    var customName = $"Carga_{Guid.NewGuid():N}_Img{i + 1}";
                    urls[i] = await _googleDriveService.UploadFileAsync(file, customName, imgFolder);
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
            var saved = await _empresaRepository.AddEvidenciaAsync(entity);
            return saved.IdCarga;
        }
        public async Task<Carga> GetEvidenciaByIdAsync(int id)
        {
            var carga = await _empresaRepository.GetEvidenciaByIdAsync(id);
            if (carga == null)
                throw new KeyNotFoundException($"Carga con Id {id} no encontrada.");
            return carga;
        }
    }
}