namespace OtransBackend.Dtos
{
    public class CargaDto
    {
        public int IdCarga { get; set; }

        public double Peso { get; set; }

        public byte[]? Imagen { get; set; }

        public string Tipo { get; set; } = string.Empty;

        public int? IdEstado { get; set; }
    }
}
