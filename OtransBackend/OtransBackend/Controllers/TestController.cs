using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OtransBackend.Repositories.Models;
using OtransBackend.Utilities;
using System.Threading.Tasks;

namespace OtransBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly Otrans _context;

        public TestController(Otrans context)
        {
            _context = context;
        }

        [HttpGet("check-connection")]
        public async Task<IActionResult> CheckConnection()
        {
            var canConnect = await _context.Database.CanConnectAsync();
            return canConnect ? Ok("✅ Conexión exitosa") : StatusCode(500, "❌ Error en la conexión");
        }
    }
}


