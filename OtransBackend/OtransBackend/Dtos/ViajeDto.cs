﻿namespace OtransBackend.Dtos
{
    public class ViajeDto
    {
        public int IdViaje { get; set; }

        public string Destino { get; set; } = string.Empty;

        public string Origen { get; set; } = string.Empty;

        public double Distancia { get; set; }

        public DateOnly Fecha { get; set; }

        public int? IdEstado { get; set; }

        public int? IdCarga { get; set; }

        public int? IdTransportista { get; set; }

        public int? IdEmpresa { get; set; }
    }
}
