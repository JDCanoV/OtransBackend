using System;
using System.Collections.Generic;

namespace OtransBackend.Models;

public partial class Calificacion
{
    public int IdCalificacion { get; set; }

    public int Puntuaje { get; set; }

    public string? Comentario { get; set; }

    public int? IdEstado { get; set; }

    public int? IdUsuario { get; set; }

    public virtual Estado? IdEstadoNavigation { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
