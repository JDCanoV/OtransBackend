namespace OtransBackend.Dtos
{
    public class ReuploadDocumentosDto
    {
        public int IdUsuario { get; set; }
        public List<DocumentoReuploadDto> Documentos { get; set; } = new();
    }
}
