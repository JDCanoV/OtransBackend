using System.Text;

namespace OtransBackend.Utilities
{
    public static class PasswordMasker
    {
        public static string Unmask(string masked)
        {
            // 1) Base64 → bytes → texto invertido
            var bytes = Convert.FromBase64String(masked);
            var rev = Encoding.UTF8.GetString(bytes);
            // 2) Voltear de nuevo
            return new string(rev.Reverse().ToArray());
        }
    }
}
