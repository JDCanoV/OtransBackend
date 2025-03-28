using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OtransBackend.Dtos;
using OtransBackend.Services;
using Swashbuckle.AspNetCore.Annotations; // Importante para usar SwaggerOperation
using System.Linq;

namespace OtransBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("registerTransportista")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> RegisterTransportista([FromForm] TransportistaDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Datos inválidos",
                    errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList()
                });
            }

            if (dto.Licencia == null || dto.Licencia.Length == 0)
            {
                return BadRequest(new { message = "La licencia es requerida" });
            }

            var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(dto.Licencia.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
            {
                return BadRequest(new
                {
                    message = "Tipo de archivo no permitido",
                    allowedExtensions
                });
            }

            try
            {
                var user = await _userService.RegisterTransportistaAsync(dto); // Solo pasamos el DTO completo
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al registrar transportista",
                    details = ex.Message
                });
            }
        }

        [HttpPost("registerEmpresa")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> RegisterEmpresa([FromForm] empresaDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Datos inválidos",
                    errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList()
                });
            }

            if (dto.NitFile == null || dto.NitFile.Length == 0)
            {
                return BadRequest(new { message = "El archivo de NIT es requerido" });
            }

            var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(dto.NitFile.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
            {
                return BadRequest(new
                {
                    message = "Tipo de archivo no permitido",
                    allowedExtensions
                });
            }

            try
            {
                var user = await _userService.RegisterEmpresaAsync(dto); // Solo pasamos el DTO completo
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al registrar empresa",
                    details = ex.Message
                });
            }
        }
    }
}
