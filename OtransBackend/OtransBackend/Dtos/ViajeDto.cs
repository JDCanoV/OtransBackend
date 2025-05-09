namespace OtransBackend.Dtos
{
    public class ViajeDto
    {
        public int IdViaje { get; set; }

        public string Destino { get; set; } = string.Empty;

        public string Origen { get; set; } = string.Empty;

        public double Distancia { get; set; }

        public DateTime Fecha { get; set; }

        public int? IdEstado { get; set; }

        public int IdCarga { get; set; }

        public int? IdTransportista { get; set; }

        public int? IdEmpresa { get; set; }
        public double Peso { get; set; }
        public string TipoCarroceria { get; set; }
        public string TipoCarga { get; set; }
        public string TamañoVeh { get; set; }
        public string Descripcion { get; set; }
    }
}
