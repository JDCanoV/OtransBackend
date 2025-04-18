namespace OtransBackend.Dtos
{
    public class UsuarioRevisionDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string TipoUsuario { get; set; } // Transportista o Empresa
        public DateTime FechaRegistro { get; set; }
        public string Estado { get; set; }
    }
}
