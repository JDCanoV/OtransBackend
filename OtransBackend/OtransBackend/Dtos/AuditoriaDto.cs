namespace OtransBackend.Dtos
{
    public class AuditoriaDto
    {
        public int IdAuditoria { get; set; }

        public int IdUsuario { get; set; }

        public string Consulta { get; set; } = string.Empty;

        public string Accion { get; set; } = string.Empty;

        public DateOnly Fecha { get; set; }
    }
}
