using System;
using System.Collections.Generic;

namespace OtransBackend.Models;

public partial class Carga
{
    public int IdCarga { get; set; }

    public double Peso { get; set; }

    public byte[]? Imagen { get; set; }

    public string Tipo { get; set; } = null!;

    public int? IdEstado { get; set; }

    public virtual Estado? IdEstadoNavigation { get; set; }

    public virtual ICollection<Viaje> Viajes { get; set; } = new List<Viaje>();
}
