using System;
using System.Collections.Generic;

namespace OtransBackend.Repositories.Models;

public partial class Carga
{
    public int IdCarga { get; set; }

    public byte[]? Imagen1 { get; set; }
    public byte[]? Imagen2 { get; set; }
    public byte[]? Imagen3 { get; set; }
    public byte[]? Imagen4 { get; set; }
    public byte[]? Imagen5 { get; set; }
    public byte[]? Imagen6 { get; set; }
    public byte[]? Imagen7 { get; set; }
    public byte[]? Imagen8 { get; set; }
    public byte[]? Imagen9 { get; set; }
    public byte[]? Imagen10 { get; set; }

    public int? IdEstado { get; set; }

    public virtual Estado? IdEstadoNavigation { get; set; }

    public virtual ICollection<Viaje> Viajes { get; set; } = new List<Viaje>();
}
