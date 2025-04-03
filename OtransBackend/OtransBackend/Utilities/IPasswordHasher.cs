namespace OtransBackend.Utilities
{
    public interface IPasswordHasher
    {
    
        string HashPassword(string password);
        bool VerifyPassword(string hashedPassword, string password);
    }

    public class PasswordHasher : IPasswordHasher
    {
        // Encriptar la contraseña
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);  // Llamando el método correcto de BCrypt
        }

        // Verificar si la contraseña es correcta comparando la encriptada
        public bool VerifyPassword(string hashedPassword, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);  // Método para verificar la contraseña
        }
    }
}
