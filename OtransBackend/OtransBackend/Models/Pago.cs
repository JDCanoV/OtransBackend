using System;
using System.Collections.Generic;

namespace OtransBackend.Models;

public partial class Pago
{
    public int IdPago { get; set; }

    public string MetodoPago { get; set; } = null!;

    public DateOnly Fecha { get; set; }

    public double Valor { get; set; }

    public double? Propina { get; set; }

    public int? IdTransportista { get; set; }

    public int? IdEmpresa { get; set; }

    public int? IdEstado { get; set; }

    public virtual Usuario? IdEmpresaNavigation { get; set; }

    public virtual Estado? IdEstadoNavigation { get; set; }

    public virtual Usuario? IdTransportistaNavigation { get; set; }
}
