namespace OtransBackend.Dtos
{
    public class UsuarioValidacionDto
    {
        public int IdUsuario { get; set; }
        public List<DocumentoValidacionDto> Documentos { get; set; } = new();
        public string Observaciones { get; set; } = string.Empty;
    }
}
