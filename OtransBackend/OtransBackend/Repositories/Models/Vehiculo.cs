using System.ComponentModel.DataAnnotations.Schema;

namespace OtransBackend.Repositories.Models;

public partial class Vehiculo
{
    public int IdVehiculo { get; set; }

    public string Placa { get; set; } = null!;

    public string CapacidadCarga { get; set; } = null!;

    public string? Soat { get; set; }

    public string? Tecnicomecanica { get; set; }

    public string? LicenciaTransito { get; set; }

    public string NombreDueño { get; set; } = null!;

    public int? NumIdentDueño { get; set; }

    public int? TelDueño { get; set; }

    public string Carroceria { get; set; } = null!;

    public int? IdTransportista { get; set; }

    public int? IdEstado { get; set; }

    [ForeignKey("IdEstado")]
    public virtual Estado? IdEstadoNavigation { get; set; }

    public virtual Usuario? IdTransportistaNavigation { get; set; }
}
