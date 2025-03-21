using System;
using System.Collections.Generic;

namespace OtransBackend.Models;

public partial class Auditoria
{
    public int IdAuditoria { get; set; }

    public int? IdUsuario { get; set; }

    public string Consulta { get; set; } = null!;

    public string Accion { get; set; } = null!;

    public DateOnly Fecha { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
