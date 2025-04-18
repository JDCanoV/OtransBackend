using Google.Apis.Drive.v3.Data;
using Microsoft.EntityFrameworkCore;
﻿using Microsoft.EntityFrameworkCore;
using OtransBackend.Dtos;
using OtransBackend.Repositories.Models;
using OtransBackend.Utilities;
using System.IO;

namespace OtransBackend.Repositories
{
    public interface IUserRepository
    {
        Task<Vehiculo> AddVehiculoAsync(Vehiculo vehiculo);
        Task<Usuario> AddTransportistaAsync(Usuario user); // Método para agregar transportista
        Task<Usuario> AddEmpresaAsync(Usuario user); // Método para agregar empresa
        Task<Usuario> GetUserByEmailAsync(string email);
        Task<Usuario> Login(LoginDto request);
        Task UpdateUserPasswordAsync(Usuario user);
        Task<IEnumerable<UsuarioRevisionDto>> ObtenerUsuariosPendientesValidacionAsync();
        Task<Usuario?> ObtenerUsuarioConVehiculoPorIdAsync(int idUsuario);

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
        public async Task<Usuario> AddTransportistaAsync(Usuario user)
        {
            

            // Guardamos el usuario (transportista) con el archivo de licencia (si existe)
            _context.Usuario.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        // Método para agregar Empresa
        public async Task<Usuario> AddEmpresaAsync(Usuario user)
        {
           
            // Guardamos el usuario (empresa) con el archivo de NIT (si existe)
            _context.Usuario.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<Usuario> GetUserByEmailAsync(string email)
        {
            return await _context.Usuario.FirstOrDefaultAsync(u => u.Correo == email);
        }



        public async Task<Vehiculo> AddVehiculoAsync(Vehiculo vehiculo)
        {
           

            // Guardamos el vehículo en la base de datos
            _context.Vehiculo.Add(vehiculo);
            await _context.SaveChangesAsync(); // Esperamos a que se guarde en la base de datos

            return vehiculo; // Retornamos el vehículo guardado
        }
        public async Task<Usuario> Login(LoginDto request)
        {
            return await _context.Usuario.FirstOrDefaultAsync(u => u.Correo.Equals(request.Correo));
        }
        public async Task UpdateUserPasswordAsync(Usuario user)
        {
            _context.Usuario.Update(user);  // Actualiza el usuario con la nueva contraseña
            await _context.SaveChangesAsync();  // Guarda los cambios en la base de datos
        }
        public async Task<IEnumerable<UsuarioRevisionDto>> ObtenerUsuariosPendientesValidacionAsync()
        {
            return await _context.Usuario
                .Where(u => u.IdEstadoNavigation != null && u.IdEstadoNavigation.Nombre == "PendienteValidacion")
                .Select(u => new UsuarioRevisionDto
                {
                    Id = u.IdUsuario,
                    Nombre = u.Nombre,
                    Correo = u.Correo,
                    TipoUsuario = u.NombreEmpresa != null ? "Empresa" : "Transportista",
                    FechaRegistro = u.FechaRegistro,
                    Estado = u.IdEstadoNavigation!.Nombre
                })
                .ToListAsync();
        }
        public async Task<Usuario?> ObtenerUsuarioConVehiculoPorIdAsync(int idUsuario)
        {
            return await _context.Usuario
                .Include(u => u.Vehiculos)
                .FirstOrDefaultAsync(u => u.IdUsuario == idUsuario);
        }
    }

}

