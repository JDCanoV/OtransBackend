using System;
using System.Collections.Generic;

namespace OtransBackend.Repositories.Models;

public partial class Notificacion
{
    public int IdNotificacion { get; set; }

    public int? IdUsuario { get; set; }

    public string Mensaje { get; set; } = null!;

    public int? IdEstado { get; set; }

    public virtual Estado? IdEstadoNavigation { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
