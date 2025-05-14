using OtransBackend.Repositories.Models;

namespace OtransBackend.Dtos
{
    public class VerViajeDto
    {
        public int IdViaje { get; set; }
        public string Destino { get; set; } = string.Empty;
        public string Origen { get; set; } = string.Empty;
        public double Distancia { get; set; }
        public DateTime Fecha { get; set; }
        public int? IdEstado { get; set; }
        public int? IdCarga { get; set; }
        public int? IdTransportista { get; set; }
        public int? IdEmpresa { get; set; }
        public double Peso { get; set; }
        public string Precio { get; set; } = string.Empty;
        public string TipoCarroceria { get; set; } = string.Empty;
        public string TipoCarga { get; set; } = string.Empty;
        public string TamanoVeh { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public List<string> Imagenes { get; set; } = new List<string>();

        // Constructor para cuando tiene un viaje activo
        public VerViajeDto(Viaje viaje, Carga carga)
        {
            IdViaje = viaje.IdViaje;
            Destino = viaje.Destino;
            Origen = viaje.Origen;
            Distancia = viaje.Distancia;
            Fecha = viaje.Fecha;
            IdEstado = viaje.IdEstado;
            IdCarga = viaje.IdCarga;
            IdTransportista = viaje.IdTransportista;
            IdEmpresa = viaje.IdEmpresa;
            Peso = viaje.Peso;
            Precio = viaje.Precio;
            TipoCarroceria = viaje.TipoCarroceria;
            TipoCarga = viaje.TipoCarga;
            TamanoVeh = viaje.TamanoVeh;
            Descripcion = viaje.Descripcion;

            // Obtener las imágenes de carga
            if (carga != null)
            {
                // Aquí se maneja cómo obtener las imágenes asociadas a la carga
                Imagenes = new List<string> { carga.Imagen1, carga.Imagen2, carga.Imagen3, carga.Imagen4, carga.Imagen5, carga.Imagen6, carga.Imagen7, carga.Imagen8, carga.Imagen9, carga.Imagen10 }
                              .Where(img => !string.IsNullOrEmpty(img)) // Filtramos las imágenes no vacías
                              .ToList();
            }
        }
    }
}
