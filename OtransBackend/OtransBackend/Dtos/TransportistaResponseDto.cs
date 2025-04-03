namespace OtransBackend.Dtos
{
    public class TransportistaResponseDto
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Telefono { get; set; }
        public string? TelefonoSos { get; set; }
        public string Rol { get; set; } = "Transportista";
        public string Estado { get; set; } = "Pendiente";
    }
}
