using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OtransBackend.Dtos;
using OtransBackend.Repositories;
using OtransBackend.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;

namespace OtransBackend.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> PostIniciarSesion([FromBody] LoginDto requestInicioSesionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await _userService.Login(requestInicioSesionDto));
        }
        [HttpPost]
        [Route("RecuperarContra")]
        public async Task<IActionResult> EnviarContra(string correo)
        {
            var result = await _userService.recuperarContra(correo);
            if (result.Contains("Error"))
            {
                return BadRequest(new { message = result });
            }

            return Ok(new { message = result });
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
                var responseDto = await _userService.RegisterTransportistaAsync(dto);
                return Ok(responseDto);
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
                var responseDto = await _userService.RegisterEmpresaAsync(dto);
                return Ok(responseDto);
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

        [HttpPost("registerVehiculo")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddVehiculoAsync([FromForm] VehiculoDto dto)
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

            if (dto.Soat == null || dto.Soat.Length == 0)
            {
                return BadRequest(new { message = "El Soat es requerido" });
            }

            var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(dto.Soat.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
            {
                return BadRequest(new
                {
                    message = "Tipo de archivo no permitido",
                    allowedExtensions
                });
            }
            if (dto.Tecnicomecanica == null || dto.Tecnicomecanica.Length == 0)
            {
                return BadRequest(new { message = "El Tecnicomecanica es requerido" });
            }

             allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png" };
             extension = Path.GetExtension(dto.Tecnicomecanica.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
            {
                return BadRequest(new
                {
                    message = "Tipo de archivo no permitido",
                    allowedExtensions
                });
            }
            if (dto.LicenciaTransito == null || dto.LicenciaTransito.Length == 0)
            {
                return BadRequest(new { message = "El LicenciaTransito es requerido" });
            }

             allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png" };
             extension = Path.GetExtension(dto.LicenciaTransito.FileName).ToLowerInvariant();

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
                var responseDto = await _userService.AddVehiculoAsync(dto);
                return Ok(responseDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al registrar vehiculo",
                    details = ex.Message
                });
            }
        }

        
            [HttpGet("pendientes-validacion")]
            public async Task<ActionResult<IEnumerable<UsuarioRevisionDto>>> GetUsuariosPendientesValidacion()
            {
                var usuarios = await _userService.ObtenerUsuariosPendientesValidacionAsync();
                return Ok(usuarios);
            }


        [HttpGet("detalle/{idUsuario}")]
        public async Task<IActionResult> ObtenerDetalleUsuario(int idUsuario)
        {
            var detalle = await _userService.ObtenerDetalleUsuarioAsync(idUsuario);
            if (detalle == null)
                return NotFound("Usuario no encontrado");

            return Ok(detalle);
        }

        [HttpPost("validar")]
        public async Task<IActionResult> ValidarUsuario([FromBody] UsuarioValidacionDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _userService.ValidateUsuarioAsync(dto);
                return Ok(new { mensaje = "Validación procesada" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }



    }
}
