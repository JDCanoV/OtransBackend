using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace OtransBackend.Dtos
{
    public class empresaDto
    {
        public int IdUsuario { get; set; }
        public int NumIdentificacion { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? TelefonoSos { get; set; }
        public string? NombreEmpresa { get; set; } // Para empresa
        public int? NumCuenta { get; set; } // Para empresa
        public string? Direccion { get; set; } // Para empresa
        [Required(ErrorMessage = "El archivo de NIT es requerido")]
        public IFormFile? NitFile { get; set; }
        public IFormFile? ArchiDocu { get; set; }

        public int? IdRol { get; set; }
        public int? IdEstado { get; set; }
    }
}
