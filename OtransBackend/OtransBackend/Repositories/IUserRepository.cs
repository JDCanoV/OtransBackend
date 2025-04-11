using Google.Apis.Drive.v3.Data;
using Microsoft.EntityFrameworkCore;
using OtransBackend.Repositories.Models;
using OtransBackend.Utilities;
using System.IO;

namespace OtransBackend.Repositories
{
    public interface IUserRepository
    {
        Task<Vehiculo> AddVehiculoAsync(Vehiculo vehiculo, IFormFile Soat, IFormFile SoTecnicomecanicaat, IFormFile LicenciaTransito);
        Task<Usuario> AddTransportistaAsync(Usuario user, IFormFile licenciaFile, IFormFile ArchiDocu); // Método para agregar transportista
        Task<Usuario> AddEmpresaAsync(Usuario user, IFormFile nitFile, IFormFile ArchiDocu); // Método para agregar empresa
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
        public async Task<Usuario> AddTransportistaAsync(Usuario user, IFormFile licenciaFile, IFormFile ArchiDocu)
        {
            // Verificar si el archivo de licencia no es null y convertirlo
            if (licenciaFile != null)
            {
                byte[] licenciaFileBytes = ConvertFileToBytes(licenciaFile); // Convertimos la licencia a binario
                user.Licencia = licenciaFileBytes; // Asignamos el binario
            }
            if (ArchiDocu != null)
            {
                byte[] ArchiDocuBytes = ConvertFileToBytes(ArchiDocu); // Convertimos la licencia a binario
                user.ArchiDocu = ArchiDocuBytes; // Asignamos el binario
            }

            // Guardamos el usuario (transportista) con el archivo de licencia (si existe)
            _context.Usuario.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        // Método para agregar Empresa
        public async Task<Usuario> AddEmpresaAsync(Usuario user, IFormFile nitFile, IFormFile ArchiDocu)
        {
            // Verificar si el archivo de NIT no es null y convertirlo
            if (nitFile != null)
            {
                byte[] nitFileBytes = ConvertFileToBytes(nitFile); // Convertimos el NIT a binario
                user.Nit = nitFileBytes; // Asignamos el binario
            }
            if (ArchiDocu != null)
            {
                byte[] ArchiDocuBytes = ConvertFileToBytes(ArchiDocu); // Convertimos el NIT a binario
                user.ArchiDocu = ArchiDocuBytes; // Asignamos el binario
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



        public async Task<Vehiculo> AddVehiculoAsync(Vehiculo vehiculo, IFormFile Soat, IFormFile Tecnicomecanicaat, IFormFile LicenciaTransito)
        {
            // Verificar si los archivos son nulos y convertirlos
            if (Soat != null)
            {
                byte[] soatFileBytes = ConvertFileToBytes(Soat); // Convertimos SOAT a binario
                vehiculo.Soat = soatFileBytes; // Asignamos el binario
            }
            if (Tecnicomecanicaat != null)
            {
                byte[] TecnicomecanicaatFileBytes = ConvertFileToBytes(Tecnicomecanicaat); // Convertimos Tecnicomecanica a binario
                vehiculo.Tecnicomecanica = TecnicomecanicaatFileBytes; // Asignamos el binario
            }
            if (LicenciaTransito != null)
            {
                byte[] LicenciaTransitoFileBytes = ConvertFileToBytes(LicenciaTransito); // Convertimos Licencia de tránsito a binario
                vehiculo.LicenciaTransito = LicenciaTransitoFileBytes; // Asignamos el binario
            }

            // Guardamos el vehículo en la base de datos
            _context.Vehiculo.Add(vehiculo);
            await _context.SaveChangesAsync(); // Esperamos a que se guarde en la base de datos

            return vehiculo; // Retornamos el vehículo guardado
        }
    }
}
