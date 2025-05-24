
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OtransBackend.Dtos;
using OtransBackend.Repositories.Models;
using OtransBackend.Utilities;
namespace OtransBackend.Repositories
{
    public interface ITransportistaRepository
    {
        Task<Viaje> ObtenerViajePorTransportista(int idTransportista);
        Task<Carga> ObtenerCargaPorId(int idCarga);
        Task<IEnumerable<Viaje>> ObtenerViajesPorCarroceriaAsync(int transportistaId);
        Task<bool> AsignarTransportistaYActualizarEstadoAsync(int idViaje, int idTransportista, int nuevoEstado);
    }
        public class TransportistaRepository : ITransportistaRepository
        {
            private readonly Otrans _context;

            public TransportistaRepository(Otrans context)
            {
                _context = context;
            }



            public async Task<Viaje> ObtenerViajePorTransportista(int idTransportista)
            {
                return await _context.Viaje
                    .Where(v => v.IdTransportista == idTransportista && (v.IdEstado == 5 || v.IdEstado == 6 || v.IdEstado == 7)) // Aceptar estados 5, 6 o 7
                    .FirstOrDefaultAsync();
            }
            public async Task<Carga> ObtenerCargaPorId(int idCarga)
            {
                return await _context.Carga
                    .Where(c => c.IdCarga == idCarga)
                    .FirstOrDefaultAsync();
            }
            public async Task<IEnumerable<Viaje>> ObtenerViajesPorCarroceriaAsync(int transportistaId)
            {
                var vehiculos = await _context.Vehiculo
                    .Where(v => v.IdTransportista == transportistaId)
                    .ToListAsync();

                if (vehiculos == null || !vehiculos.Any())
                    return new List<Viaje>();  // Si no se encuentra el vehículo, retornar lista vacía

                var carroceria = vehiculos.FirstOrDefault()?.Carroceria;

                if (string.IsNullOrEmpty(carroceria))
                    return new List<Viaje>();  // Si no tiene carrocería, retornar lista vacía

                var viajes = await _context.Viaje
                    .Where(v => v.TipoCarroceria == carroceria && v.IdEstado == 4)
                    .Include(v => v.IdCargaNavigation) // Incluir la carga para obtener las imágenes
                    .ToListAsync();

                return viajes;
            }

            public async Task<bool> AsignarTransportistaYActualizarEstadoAsync(int idViaje, int idTransportista, int nuevoEstado)
            {
                var viaje = await _context.Viaje.FindAsync(idViaje);
                if (viaje == null) return false;

                viaje.IdTransportista = idTransportista;
                viaje.IdEstado = nuevoEstado;

                await _context.SaveChangesAsync();
                return true;
            }

        }
    
}
