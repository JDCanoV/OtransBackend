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
    [SwaggerTag("Operaciones relacionadas con administrador.")]
    public class AdministradorController : ControllerBase
    {
        private readonly IAdministradorService _administradorService;
        public AdministradorController(IAdministradorService administradorService)
        {
            _administradorService = administradorService;
        }

        [HttpGet("reporte")]
        public async Task<IActionResult> GetUserReport()
        {
            var pdfBytes = await _administradorService.GenerateUserReportAsync();
            return File(pdfBytes, "application/pdf", "UserReport.pdf");
        }


        [HttpGet("usuarios")]
        [SwaggerOperation(Summary = "Lista todos los usuarios para el reporte")]
        public async Task<IActionResult> GetUsersForReport()
        {
            var list = await _administradorService.GetAllUsersForReportAsync();
            return Ok(list);
        }
        [HttpGet("pendientes-validacion")]
        [SwaggerOperation(
            Summary = "Obtiene usuarios pendientes de validación.",
            Description = "Devuelve la lista de usuarios que aún no han sido validados en el sistema."
        )]
        public async Task<ActionResult<IEnumerable<UsuarioRevisionDto>>> GetUsuariosPendientesValidacion()
        {
            var usuarios = await _administradorService.ObtenerUsuariosPendientesValidacionAsync();
            return Ok(usuarios);
        }

        [HttpGet("detalle/{idUsuario}")]
        [SwaggerOperation(
            Summary = "Obtiene el detalle de un usuario.",
            Description = "Recupera la información detallada de un usuario por su identificador."
        )]
        public async Task<IActionResult> ObtenerDetalleUsuario(int idUsuario)
        {
            var detalle = await _administradorService.ObtenerDetalleUsuarioAsync(idUsuario);
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
                await _administradorService.ValidateUsuarioAsync(dto);
                return Ok(new { mensaje = "Validación procesada" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

    }
}

