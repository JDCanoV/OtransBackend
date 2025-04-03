using Microsoft.EntityFrameworkCore;
using OtransBackend.Repositories.Models;
using OtransBackend.Utilities;
using System.IO;

namespace OtransBackend.Repositories
{
    public interface IUserRepository
    {
        Task<Usuario> AddTransportistaAsync(Usuario user, IFormFile licenciaFile); // Método para agregar transportista
        Task<Usuario> AddEmpresaAsync(Usuario user, IFormFile nitFile); // Método para agregar empresa
        Task<Usuario> GetUserByEmailAsync(string email);
    }

    public class UserRepository : IUserRepository
    {
        private readonly Otrans _context;

        public UserRepository(Otrans context)
        {
            _context = context;
        }

        // Convertir archivo a binario
        private byte[] ConvertFileToBytes(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        // Método para agregar Transportista
        public async Task<Usuario> AddTransportistaAsync(Usuario user, IFormFile licenciaFile)
        {
            // Verificar si el archivo de licencia no es null y convertirlo
            if (licenciaFile != null)
            {
                byte[] licenciaFileBytes = ConvertFileToBytes(licenciaFile); // Convertimos la licencia a binario
                user.Licencia = licenciaFileBytes; // Asignamos el binario
            }

            // Guardamos el usuario (transportista) con el archivo de licencia (si existe)
            _context.Usuario.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        // Método para agregar Empresa
        public async Task<Usuario> AddEmpresaAsync(Usuario user, IFormFile nitFile)
        {
            // Verificar si el archivo de NIT no es null y convertirlo
            if (nitFile != null)
            {
                byte[] nitFileBytes = ConvertFileToBytes(nitFile); // Convertimos el NIT a binario
                user.Nit = nitFileBytes; // Asignamos el binario
            }

            // Guardamos el usuario (empresa) con el archivo de NIT (si existe)
            _context.Usuario.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<Usuario> GetUserByEmailAsync(string email)
        {
            return await _context.Usuario.FirstOrDefaultAsync(u => u.Correo == email);
        }
    }
}
