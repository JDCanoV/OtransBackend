namespace OtransBackend.Dtos
{
    public class PagoDto
    {

        public int IdPago { get; set; }

        public string MetodoPago { get; set; } =  string.Empty;

        public DateOnly Fecha { get; set; }

        public double Valor { get; set; }

        public double? Propina { get; set; }

        public int? IdTransportista { get; set; }

        public int? IdEmpresa { get; set; }

        public int? IdEstado { get; set; }

    }
}
