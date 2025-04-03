using System;
using System.Collections.Generic;

namespace OtransBackend.Repositories.Models;

public partial class Viaje
{
    public int IdViaje { get; set; }

    public string Destino { get; set; } = null!;

    public string Origen { get; set; } = null!;

    public double Distancia { get; set; }

    public DateOnly Fecha { get; set; }

    public int? IdEstado { get; set; }

    public int? IdCarga { get; set; }

    public int? IdTransportista { get; set; }

    public int? IdEmpresa { get; set; }

    public virtual Carga? IdCargaNavigation { get; set; }

    public virtual Usuario? IdEmpresaNavigation { get; set; }

    public virtual Estado? IdEstadoNavigation { get; set; }

    public virtual Usuario? IdTransportistaNavigation { get; set; }
}
