using System;
using System.Collections.Generic;

namespace OtransBackend.Repositories.Models;

public partial class Carga
{
    public int IdCarga { get; set; }

    public string? Imagen1 { get; set; }
    public string?  Imagen2 { get; set; }
    public string? Imagen3 { get; set; }
    public string? Imagen4 { get; set; }
    public string? Imagen5 { get; set; }
    public string? Imagen6 { get; set; }
    public string? Imagen7 { get; set; }
    public string? Imagen8 { get; set; }
    public string? Imagen9 { get; set; }
    public string? Imagen10 { get; set; }

    public int? IdEstado { get; set; }

    public virtual Estado? IdEstadoNavigation { get; set; }

    public virtual ICollection<Viaje> Viajes { get; set; } = new List<Viaje>();
}
