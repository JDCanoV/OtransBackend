using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OtransBackend.Repositories.Models;

public partial class Viaje
{
    public int IdViaje { get; set; }

    public string Destino { get; set; } = null!;

    public string Origen { get; set; } = null!;

    public double Distancia { get; set; }

    public DateTime Fecha { get; set; } = DateTime.Now;
    public double Peso { get; set; }
    public string TipoCarroceria { get; set; }
    public string TipoCarga { get; set; }
    public string TamanoVeh { get; set; }
    public string Descripcion { get; set; }
    public string? Precio { get; set; }

    public int? IdEstado { get; set; }

    public int IdCarga { get; set; }

    public int? IdTransportista { get; set; }

    public int? IdEmpresa { get; set; }

    [ForeignKey("IdCarga")]
    public virtual Carga? IdCargaNavigation { get; set; }

    public virtual Usuario? IdEmpresaNavigation { get; set; }
    [ForeignKey("IdEstado")]

    public virtual Estado? IdEstadoNavigation { get; set; }

    public virtual Usuario? IdTransportistaNavigation { get; set; }
}
