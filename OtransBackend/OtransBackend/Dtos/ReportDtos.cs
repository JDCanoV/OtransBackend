namespace OtransBackend.Dtos
{
    public class UserRegistrationReportItem
    {
        public string NombreCompleto { get; set; } = null!;
        public string TipoUsuario { get; set; } = null!;
        public DateTime FechaRegistro { get; set; }
    }

    public class MonthlyRegistrations
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Count { get; set; }
    }
}
