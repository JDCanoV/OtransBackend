using Microsoft.EntityFrameworkCore;
using OtransBackend.Dtos;
using OtransBackend.Repositories.Models;
using OtransBackend.Utilities;

namespace OtransBackend.Repositories
{
    public interface IEmpresaRepository
    {
        Task<Viaje> AddViajeAsync(Viaje viaje);
        Task<List<Viaje>> GetViajesByEmpresaAsync(int idEmpresa);
        Task<Carga> AddEvidenciaAsync(Carga carga);
        Task<Carga?> GetEvidenciaByIdAsync(int id);
    }
    public class EmpresaRepository : IEmpresaRepository
    {
        private readonly Otrans _context;
        public EmpresaRepository(Otrans context)
        {
            _context = context;
        }
        public async Task<Viaje> AddViajeAsync(Viaje viaje)
        {
            _context.Viaje.Add(viaje);
            await _context.SaveChangesAsync();
            return viaje;
        }
        public async Task<List<Viaje>> GetViajesByEmpresaAsync(int idEmpresa)
        {
            return await _context.Viaje
                .Where(v => v.IdEmpresa == idEmpresa)
                .Include(v => v.IdTransportistaNavigation)
                .Include(v => v.IdCargaNavigation) // Aquí incluyes la carga relacionada
                .ToListAsync();
        }

        public async Task<Carga> AddEvidenciaAsync(Carga carga)
        {
            _context.Carga.Add(carga);
            await _context.SaveChangesAsync();
            return carga;
        }

        public async Task<Carga?> GetEvidenciaByIdAsync(int id)
        {
            return await _context.Carga.FindAsync(id);
        }
    }
}