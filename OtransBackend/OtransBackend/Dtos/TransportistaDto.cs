using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;


namespace OtransBackend.Dtos
{
  public class TransportistaDto
{
    [Required]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 50 caracteres")]
    public string Nombre { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 50 caracteres")]
    public string Apellido { get; set; }

    [Required]
    [EmailAddress(ErrorMessage = "Formato de correo electrónico inválido")]
    public string Correo { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 100 caracteres")]
    public string Contrasena { get; set; }

    [Required(ErrorMessage = "El número de identificación es requerido")]
    [Range(100000, 9999999999, ErrorMessage = "Número de identificación inválido")]
    public int NumIdentificacion { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 100 caracteres")]
    public string? Telefono { get; set; }

    [Phone(ErrorMessage = "Número de teléfono SOS inválido")]
    public string? TelefonoSos { get; set; }

    [Required(ErrorMessage = "El archivo de licencia es requerido")]
    public IFormFile? Licencia { get; set; }
    public IFormFile? ArchiDocu { get; set; }

    [Required(ErrorMessage = "El rol es requerido")]
        
    public int? IdRol { get; set; }

        public int? IdEstado { get; set; }
    }

}