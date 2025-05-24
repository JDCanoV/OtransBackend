using Microsoft.AspNetCore.Mvc;
using OtransBackend.Dtos;
using OtransBackend.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace OtransBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Operaciones relacionadas con la gestión de usuarios, incluyendo autenticación, registro y validación.")]
    public class TransportistaController : ControllerBase
    {
        private readonly ITransportistaService _transportistaService;

        public TransportistaController(ITransportistaService transportistaService)
        {
            _transportistaService = transportistaService;
        }

        [HttpGet("{idTransportista}/viaje")]
        public async Task<IActionResult> ObtenerViajeParaTransportista(int idTransportista)
        {
            // Obtener el viaje
            var viaje = await _transportistaService.ObtenerViajePorTransportista(idTransportista);

            if (viaje == null)
            {
                // Si no tiene un viaje, devolver un mensaje adecuado
                return NoContent(); // Retorna 204 sin contenido
            }

            // Ya no necesitamos verificar el estado aquí, ya que ahora aceptamos 5, 6 o 7
            // Si el viaje tiene estado 5, 6 o 7, lo devolvemos.
            // El método ObtenerViajePorTransportista ya se encarga de devolver el viaje si está en uno de esos estados.

            // Obtener la carga y las imágenes asociadas al viaje
            var carga = await _transportistaService.ObtenerCargaPorId(viaje.IdCarga);

            // Crear el DTO VerViajeDto y devolverlo
            var viajeDTO = new VerViajeDto(viaje, carga);
            return Ok(viajeDTO); // Devolver los detalles del viaje con las imágenes
        }

        [HttpGet("viajes-por-carroceria/{transportistaId}")]
        public async Task<IActionResult> ObtenerViajesPorCarroceria(int transportistaId)
        {
            var viajes = await _transportistaService.ObtenerViajesPorCarroceriaAsync(transportistaId);

            if (viajes == null || !viajes.Any())
                return NotFound("No se encontraron viajes para esta carrocería con estado 4.");

            return Ok(viajes);
        }

        [HttpPost("asignar-transportista")]
        public async Task<IActionResult> AsignarTransportista([FromBody] AsignarTransportistaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _transportistaService.AsignarTransportistaYActualizarEstadoAsync(dto.IdViaje, dto.IdTransportista);

            if (!result)
                return NotFound(new { message = "Viaje no encontrado" });

            return Ok(new { message = "Transportista asignado y estado actualizado" });
        }

    }
}
