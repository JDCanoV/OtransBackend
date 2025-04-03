namespace OtransBackend.Dtos
{
    public class EmpresaResponseDto
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? NombreEmpresa { get; set; }
        public string? Direccion { get; set; }
        public int? NumCuenta { get; set; }
        public string Rol { get; set; } = "Empresa";
        public string Estado { get; set; } = "Pendiente";
    }
}
