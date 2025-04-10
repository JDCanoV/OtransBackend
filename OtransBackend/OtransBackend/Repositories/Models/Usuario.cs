using System;
using System.Collections.Generic;

namespace OtransBackend.Repositories.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public int NumIdentificacion { get; set; }
    public byte[]? ArchiDocu { get; set; }
    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string TelefonoSos { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public string? NombreEmpresa { get; set; }

    public int? NumCuenta { get; set; }

    public string? Direccion { get; set; }

    public byte[]? Licencia { get; set; }

    public byte[]? Nit { get; set; }

    public int? IdRol { get; set; }

    public int? IdEstado { get; set; }

    public virtual ICollection<Auditoria> Auditoria { get; set; } = new List<Auditoria>();

    public virtual ICollection<Calificacion> Calificacions { get; set; } = new List<Calificacion>();

    public virtual Estado? IdEstadoNavigation { get; set; }

    public virtual Rol? IdRolNavigation { get; set; }

    public virtual ICollection<Notificacion> Notificacions { get; set; } = new List<Notificacion>();

    public virtual ICollection<Pago> PagoIdEmpresaNavigations { get; set; } = new List<Pago>();

    public virtual ICollection<Pago> PagoIdTransportistaNavigations { get; set; } = new List<Pago>();

    public virtual ICollection<Vehiculo> Vehiculos { get; set; } = new List<Vehiculo>();

    public virtual ICollection<Viaje> ViajeIdEmpresaNavigations { get; set; } = new List<Viaje>();

    public virtual ICollection<Viaje> ViajeIdTransportistaNavigations { get; set; } = new List<Viaje>();
}
