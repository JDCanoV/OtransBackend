using System;
using System.Collections.Generic;

namespace OtransBackend.Models;

public partial class Estado
{
    public int IdEstado { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Calificacion> Calificacions { get; set; } = new List<Calificacion>();

    public virtual ICollection<Carga> Cargas { get; set; } = new List<Carga>();

    public virtual ICollection<Notificacion> Notificacions { get; set; } = new List<Notificacion>();

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();

    public virtual ICollection<Vehiculo> Vehiculos { get; set; } = new List<Vehiculo>();

    public virtual ICollection<Viaje> Viajes { get; set; } = new List<Viaje>();
}
