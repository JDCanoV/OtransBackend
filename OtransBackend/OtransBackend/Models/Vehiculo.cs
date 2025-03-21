using System;
using System.Collections.Generic;

namespace OtransBackend.Models;

public partial class Vehiculo
{
    public int IdVehiculo { get; set; }

    public string Placa { get; set; } = null!;

    public string CapacidadCarga { get; set; } = null!;

    public byte[]? Soat { get; set; }

    public byte[]? Tecnicomecanica { get; set; }

    public byte[]? LicenciaTransito { get; set; }

    public string NombreDueño { get; set; } = null!;

    public int? NumIdentDueño { get; set; }

    public int? TelDueño { get; set; }

    public string Carroceria { get; set; } = null!;

    public int? IdTransportista { get; set; }

    public int? IdEstado { get; set; }

    public virtual Estado? IdEstadoNavigation { get; set; }

    public virtual Usuario? IdTransportistaNavigation { get; set; }
}
