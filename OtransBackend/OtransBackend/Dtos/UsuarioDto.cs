namespace OtransBackend.Dtos
{
    public class UsuarioDto
    {
        public int IdUsuario { get; set; }

        public int NumIdentificacion { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Apellido { get; set; } = string.Empty;

        public string Telefono { get; set; }

        public int? TelefonoSos { get; set; }

        public string Correo { get; set; } = string.Empty;

        public string Contrasena { get; set; } = string.Empty;

        public string? NombreEmpresa { get; set; }

        public int? NumCuenta { get; set; }

        public string? Direccion { get; set; }

        public byte[]? Licencia { get; set; }

        public byte[]? Nit { get; set; }

        public int? IdRol { get; set; }

        public int? IdEstado { get; set; }
    }
}
