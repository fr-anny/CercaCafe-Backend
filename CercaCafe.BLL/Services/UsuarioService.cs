using BCrypt.Net;
using CercaCafe.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace CercaCafe.BLL.Services
{
    public class UsuarioService
    {
        private readonly CercaCafeContext _context;

        public UsuarioService(CercaCafeContext context)
        {
            _context = context;
        }

        public async Task<List<Usuario>> ObtenerTodosAsync() => await _context.Usuarios.ToListAsync();

        public async Task<Usuario?> ObtenerPorIdAsync(int id) => await _context.Usuarios.FindAsync(id);

        // Registro
        public async Task<Usuario> CrearAsync(Usuario usuario)
        {
            usuario.Contrasena = BCrypt.Net.BCrypt.HashPassword(usuario.Contrasena);
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        // Validación de login
        public async Task<Usuario?> ValidarLoginAsync(string correo, string contrasena)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Correo.ToLower() == correo.ToLower());

            if (usuario == null || string.IsNullOrEmpty(usuario.Contrasena)) return null;

            if (!usuario.Contrasena.StartsWith("$2")) return null;

            bool contrasenaValida = BCrypt.Net.BCrypt.Verify(contrasena, usuario.Contrasena);
            return contrasenaValida ? usuario : null;
        }

        // Verificación de correo
        public async Task<bool> ExisteCorreoAsync(string correo)
        {
            return await _context.Usuarios.AnyAsync(u => u.Correo.ToLower() == correo.ToLower());
        }

        // Actualización de perfil
        public async Task<bool> ActualizarAsync(int id, Usuario usuario)
        {
            if (id != usuario.IdUsuario) return false;

            var usuarioExistente = await _context.Usuarios.FindAsync(id);
            if (usuarioExistente == null) return false;

            usuarioExistente.Nombre = usuario.Nombre;
            usuarioExistente.Correo = usuario.Correo;
            usuarioExistente.Telefono = usuario.Telefono;

            if (!string.IsNullOrWhiteSpace(usuario.Contrasena))
            {
                usuarioExistente.Contrasena = usuario.Contrasena.StartsWith("$2")
                    ? usuario.Contrasena
                    : BCrypt.Net.BCrypt.HashPassword(usuario.Contrasena);
            }

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Existe(id)) return false;
                throw;
            }
        }

        // Eliminación
        public async Task<bool> EliminarAsync(int id)
        {
            var item = await _context.Usuarios.FindAsync(id);
            if (item == null) return false;
            _context.Usuarios.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        private bool Existe(int id) => _context.Usuarios.Any(e => e.IdUsuario == id);
    }
}