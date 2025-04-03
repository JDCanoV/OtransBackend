namespace OtransBackend.Dtos
{
    public class CalificacionDto
    {
        public int IdCalificacion { get; set; }

        public int Puntuaje { get; set; }

        public string Comentario { get; set; } = string.Empty;

        public int IdEstado { get; set; }

        public int IdUsuario { get; set; }
    }
}
