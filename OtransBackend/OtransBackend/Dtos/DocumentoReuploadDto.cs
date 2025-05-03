namespace OtransBackend.Dtos
{
    public class DocumentoReuploadDto
    {
        public string NombreDocumento { get; set; } = null!;
        public IFormFile Archivo { get; set; } = null!;
    }
}
