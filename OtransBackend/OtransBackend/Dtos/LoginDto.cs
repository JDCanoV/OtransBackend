namespace OtransBackend.Dtos
{
    public class LoginDto
    {
        public string Correo { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
        public int Respuesta { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public DateTime TiempoExpiracion { get; set; }
    }
}
