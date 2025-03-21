namespace OtransBackend.Dtos
{
    public class NotificacionDto
    {
        public int IdNotificacion { get; set; }

        public int? IdUsuario { get; set; }

        public string Mensaje { get; set; } = string.Empty;

        public int? IdEstado { get; set; }
    }
}
