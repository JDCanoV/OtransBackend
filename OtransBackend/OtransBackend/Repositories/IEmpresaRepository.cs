using Microsoft.Data.SqlClient;
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
        Task<Viaje> GetByIdAsync(int idViaje);
        Task DeleteAsync(int idViaje);
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
            var viajeResult = await _context.Viaje
    .FromSqlRaw(
        "EXEC sp_AddViaje @Destino, @Origen, @Distancia, @IdEstado, @IdEmpresa, @Peso, @TipoCarga, @TipoCarroceria, @TamanoVeh, @Descripcion, @IdCarga, @Precio",
        new SqlParameter("@Destino", viaje.Destino ?? (object)DBNull.Value),
        new SqlParameter("@Origen", viaje.Origen ?? (object)DBNull.Value),
        new SqlParameter("@Distancia", viaje.Distancia),
        new SqlParameter("@IdEstado", viaje.IdEstado),
        new SqlParameter("@IdEmpresa", viaje.IdEmpresa),
        new SqlParameter("@Peso", viaje.Peso),
        new SqlParameter("@TipoCarga", viaje.TipoCarga ?? (object)DBNull.Value),
        new SqlParameter("@TipoCarroceria", viaje.TipoCarroceria ?? (object)DBNull.Value),
        new SqlParameter("@TamanoVeh", viaje.TamanoVeh ?? (object)DBNull.Value),
        new SqlParameter("@Descripcion", viaje.Descripcion ?? (object)DBNull.Value),
        new SqlParameter("@IdCarga", viaje.IdCarga),
        new SqlParameter("@Precio", viaje.Precio)
    )
    .AsQueryable()
    .FirstOrDefaultAsync();


            return viajeResult;
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
        public async Task<Viaje> GetByIdAsync(int idViaje)
        {
            return await _context.Set<Viaje>().FirstOrDefaultAsync(v => v.IdViaje == idViaje);
        }

        public async Task DeleteAsync(int idViaje)
        {
            // Ejecuta el stored procedure con parámetro
            await _context.Database.ExecuteSqlRawAsync("EXEC sp_DeleteViaje @p0", idViaje);
        }

    }
}