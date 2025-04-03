namespace OtransBackend.Repositories.Models
{
    public class UsuarioLite
    {
        public int IdUsuario { get; set; }

        public int NumIdentificacion { get; set; }

        public string Nombre { get; set; } = null!;

        public string Apellido { get; set; } = null!;

        public string? Telefono { get; set; }

        public string? TelefonoSos { get; set; }

        public string Correo { get; set; } = null!;

        public string Contrasena { get; set; } = null!;

        public string? NombreEmpresa { get; set; }

        public int? NumCuenta { get; set; }

        public string? Direccion { get; set; }

        public byte[]? Licencia { get; set; }

        public byte[]? Nit { get; set; }

        public int? IdRol { get; set; }

        public int? IdEstado { get; set; }
    }
}
