namespace OtransBackend.Dtos
{
    public class CargaDto
    {

        public int IdCarga { get; set; }

        public IFormFile? Imagen1 { get; set; }
        public IFormFile? Imagen2 { get; set; }
        public IFormFile? Imagen3 { get; set; }
        public IFormFile? Imagen4 { get; set; }
        public IFormFile? Imagen5 { get; set; }
        public IFormFile? Imagen6 { get; set; }
        public IFormFile? Imagen7 { get; set; }
        public IFormFile? Imagen8 { get; set; }
        public IFormFile? Imagen9 { get; set; }
        public IFormFile? Imagen10 { get; set; }

        public int? IdEstado { get; set; }
    }
}
