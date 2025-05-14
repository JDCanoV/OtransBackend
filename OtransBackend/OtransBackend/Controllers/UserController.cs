using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OtransBackend.Dtos;
using OtransBackend.Repositories;
using OtransBackend.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace OtransBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Operaciones relacionadas con la gestión de usuarios, incluyendo autenticación, registro y validación.")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "Autentica un usuario en el sistema.",
            Description = "Este endpoint permite a los usuarios registrados iniciar sesión utilizando sus credenciales."
        )]
        public async Task<IActionResult> PostIniciarSesion([FromBody] LoginDto requestInicioSesionDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _userService.Login(requestInicioSesionDto));
        }

        [HttpPost("RecuperarContra")]
        [SwaggerOperation(
            Summary = "Envía un correo de recuperación de contraseña.",
            Description = "Este endpoint envía un correo electrónico con instrucciones para que el usuario recupere su contraseña."
        )]
        public async Task<IActionResult> EnviarContra([FromQuery] string correo)
        {
            // Verifica que 'correo' no sea null o vacío
            if (string.IsNullOrEmpty(correo))
            {
                return BadRequest(new { message = "Correo es requerido." });
            }

            var result = await _userService.recuperarContra(correo);
            if (result.Contains("Error"))
                return BadRequest(new { message = result });

            return Ok(new { message = result });
        }

        [HttpPost("registerTransportista")]
        [Consumes("multipart/form-data")]
        [SwaggerOperation(
            Summary = "Registra un nuevo transportista.",
            Description = "Registra un transportista con sus datos y licencia adjunta."
        )]
        public async Task<IActionResult> RegisterTransportista([FromForm] TransportistaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    message = "Datos inválidos",
                    errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                });

            if (dto.Licencia == null || dto.Licencia.Length == 0)
                return BadRequest(new { message = "La licencia es requerida" });

            var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(dto.Licencia.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
                return BadRequest(new { message = "Tipo de archivo no permitido", allowedExtensions });

            try
            {
                var responseDto = await _userService.RegisterTransportistaAsync(dto);
                return Ok(responseDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al registrar transportista", details = ex.Message });
            }
        }

        [HttpPost("registerEmpresa")]
        [Consumes("multipart/form-data")]
        [SwaggerOperation(
            Summary = "Registra una nueva empresa.",
            Description = "Registra una empresa con sus datos y archivo de NIT."
        )]
        public async Task<IActionResult> RegisterEmpresa([FromForm] empresaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    message = "Datos inválidos",
                    errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                });

            if (dto.NitFile == null || dto.NitFile.Length == 0)
                return BadRequest(new { message = "El archivo de NIT es requerido" });

            var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(dto.NitFile.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
                return BadRequest(new { message = "Tipo de archivo no permitido", allowedExtensions });

            try
            {
                var responseDto = await _userService.RegisterEmpresaAsync(dto);
                return Ok(responseDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al registrar empresa", details = ex.Message });
            }
        }

        [HttpPost("registerVehiculo")]
        [Consumes("multipart/form-data")]
        [SwaggerOperation(
            Summary = "Registra un vehículo para un usuario.",
            Description = "Agrega un vehículo al perfil del usuario con SOAT, Tecnicomecanica y licencia de tránsito."
        )]
        public async Task<IActionResult> AddVehiculoAsync([FromForm] VehiculoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    message = "Datos inválidos",
                    errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                });

            if (dto.Soat == null || dto.Soat.Length == 0)
                return BadRequest(new { message = "El Soat es requerido" });

            var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(dto.Soat.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
                return BadRequest(new { message = "Tipo de archivo no permitido", allowedExtensions });

            if (dto.Tecnicomecanica == null || dto.Tecnicomecanica.Length == 0)
                return BadRequest(new { message = "El Tecnicomecanica es requerido" });

            extension = Path.GetExtension(dto.Tecnicomecanica.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
                return BadRequest(new { message = "Tipo de archivo no permitido", allowedExtensions });

            if (dto.LicenciaTransito == null || dto.LicenciaTransito.Length == 0)
                return BadRequest(new { message = "La licencia de tránsito es requerida" });

            extension = Path.GetExtension(dto.LicenciaTransito.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
                return BadRequest(new { message = "Tipo de archivo no permitido", allowedExtensions });

            try
            {
                var responseDto = await _userService.AddVehiculoAsync(dto);
                return Ok(responseDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al registrar vehiculo", details = ex.Message });
            }
        }

        [HttpGet("pendientes-validacion")]
        [SwaggerOperation(
            Summary = "Obtiene usuarios pendientes de validación.",
            Description = "Devuelve la lista de usuarios que aún no han sido validados en el sistema."
        )]
        public async Task<ActionResult<IEnumerable<UsuarioRevisionDto>>> GetUsuariosPendientesValidacion()
        {
            var usuarios = await _userService.ObtenerUsuariosPendientesValidacionAsync();
            return Ok(usuarios);
        }

        [HttpGet("detalle/{idUsuario}")]
        [SwaggerOperation(
            Summary = "Obtiene el detalle de un usuario.",
            Description = "Recupera la información detallada de un usuario por su identificador."
        )]
        public async Task<IActionResult> ObtenerDetalleUsuario(int idUsuario)
        {
            var detalle = await _userService.ObtenerDetalleUsuarioAsync(idUsuario);
            if (detalle == null)
                return NotFound("Usuario no encontrado");

            return Ok(detalle);
        }

        [HttpPost("validar")]
        [SwaggerOperation(
            Summary = "Valida un usuario.",
            Description = "Procesa la validación de documentos o datos de un usuario."
        )]
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

        [HttpPost("reupload-documentos")]
        [Consumes("multipart/form-data")]
        [SwaggerOperation(
            Summary = "Re-sube los documentos de un usuario.",
            Description = "Permite al usuario volver a subir sus documentos para validación."
        )]
        public async Task<IActionResult> ReuploadDocumentos([FromForm] ReuploadDocumentosDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _userService.ReuploadDocumentosAsync(dto);
            return Ok(new { mensaje = "Documentos subidos correctamente" });
        }

        // Obtener el viaje para un transportista (solo si está activo con estado 5)
        [HttpGet("{idTransportista}/viaje")]
        public async Task<IActionResult> ObtenerViajeParaTransportista(int idTransportista)
        {
            // Obtener el viaje
            var viaje = await _userService.ObtenerViajePorTransportista(idTransportista);

            if (viaje == null)
            {
                // Si no tiene un viaje, devolver un mensaje adecuado
                return NoContent(); // Retorna 204 sin contenido
            }

            // Ya no necesitamos verificar el estado aquí, ya que ahora aceptamos 5, 6 o 7
            // Si el viaje tiene estado 5, 6 o 7, lo devolvemos.
            // El método ObtenerViajePorTransportista ya se encarga de devolver el viaje si está en uno de esos estados.

            // Obtener la carga y las imágenes asociadas al viaje
            var carga = await _userService.ObtenerCargaPorId(viaje.IdCarga);

            // Crear el DTO VerViajeDto y devolverlo
            var viajeDTO = new VerViajeDto(viaje, carga);
            return Ok(viajeDTO); // Devolver los detalles del viaje con las imágenes
        }

        [HttpGet("viajes-por-carroceria/{transportistaId}")]
        public async Task<IActionResult> ObtenerViajesPorCarroceria(int transportistaId)
        {
            var viajes = await _userService.ObtenerViajesPorCarroceriaAsync(transportistaId);

            if (viajes == null || !viajes.Any())
                return NotFound("No se encontraron viajes para esta carrocería con estado 4.");

            return Ok(viajes);
        }

    }
}