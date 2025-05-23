using static System.Net.WebRequestMethods;

namespace OtransBackend.Dtos
{
    public class UsuarioReportDto
    {
        public string NombreCompleto { get; set; } = null!;
        public string TipoUsuario { get; set; } = null!;
        public DateTime FechaRegistro { get; set; }
    }
}

