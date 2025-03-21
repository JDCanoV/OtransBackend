using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OtransBackend.Models;
using OtransBackend.Utilities;
using System.Threading.Tasks;

namespace OtransBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TestController(ApplicationDbContext context)
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


