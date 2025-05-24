namespace OtransBackend.Dtos
{
    public class UsuarioDetalleDto
    {
       
            public int IdUsuario { get; set; }
            public string NombreCompleto { get; set; } = null!;
            public string Correo { get; set; } = null!;
            public string Telefono { get; set; } = null!;
            public string TipoUsuario { get; set; } = null!;

            public string? Nit { get; set; }
            public string? Licencia { get; set; }

            public string? Placa { get; set; }
            public string? Soat { get; set; }
            public string? Tecnomecanico { get; set; }
            public string? LicenciaTransito { get; set; }
            public string? ArchiDocu { get; set; }

        public List<DocumentoValidacionDto> Documentos { get; set; } = new();
            public string Observaciones { get; set; } = "";
        }

    }

