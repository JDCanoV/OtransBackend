namespace OtransBackend.Dtos
{
    public class VehiculoDto
    {
        public int IdVehiculo { get; set; }

        public string Placa { get; set; } = string.Empty;

        public string CapacidadCarga { get; set; } = string.Empty;

        public IFormFile Soat { get; set; }

        public IFormFile Tecnicomecanica { get; set; }

        public IFormFile LicenciaTransito { get; set; }

        public string NombreDueño { get; set; } = string.Empty;

        public int? NumIdentDueño { get; set; }

        public int? TelDueño { get; set; }

        public string Carroceria { get; set; } = string.Empty;

        public int? IdTransportista { get; set; }

        public int? IdEstado { get; set; }
    }
}
