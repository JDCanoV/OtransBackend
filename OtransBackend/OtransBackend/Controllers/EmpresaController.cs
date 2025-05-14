using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OtransBackend.Dtos;
using OtransBackend.Services;
using Swashbuckle.AspNetCore.Annotations;


namespace OtransBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Operaciones relacionadas con la gestión de las Funcionalidades de la Empresa")]

    public class EmpresaController : ControllerBase
    {
        private readonly IEmpresaService _empresaService;
        public EmpresaController(IEmpresaService empresaService)
        {
            _empresaService = empresaService;

        }
        [HttpPost("registrarViaje")]
        public async Task<IActionResult> RegistrarViaje([FromBody] ViajeDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var viaje = await _empresaService.AddViajeAsync(dto);
            return Ok(viaje);
        }
        [Authorize]
        [HttpGet("listarViaje/{idEmpresa}")]
        public async Task<IActionResult> GetViajesByEmpresa(int idEmpresa)
        {
            var viajes = await _empresaService.GetViajesByEmpresaAsync(idEmpresa);
            return Ok(viajes);
        }
        [HttpPost("subir-imagenes-carga")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CargaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = await _empresaService.AddEvidenciaAsync(dto);

            // Devuelve 201 con solo el IdCarga
            return CreatedAtAction(
                nameof(GetEvidenciaById),
                new { id },
                new CargaResponseDto { IdCarga = id }
            );
        }
        [HttpGet("subir-imagenes-carga/{id}")]
        public async Task<IActionResult> GetEvidenciaById(int id)
        {
            // Aquí podrías implementar la obtención de una Carga por id
            // para devolver toda la info si la necesitas.
            return Ok(new { IdCarga = id });
        }
    }
}