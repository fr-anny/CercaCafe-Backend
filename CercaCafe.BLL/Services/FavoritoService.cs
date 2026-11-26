using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CercaCafe.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace CercaCafe.BLL.Services
{
    public class FavoritoService
    {
        private readonly CercaCafeContext _context;

        public FavoritoService(CercaCafeContext context)
        {
            _context = context;
        }

        // Obtener todas las cafeterías guardadas en favoritos por un usuario
        public async Task<List<Favorito>> ObtenerMisFavoritosAsync(int idUsuario)
        {
            return await _context.Favoritos
                .AsNoTracking()
                .Where(f => f.IdUsuario == idUsuario)
                .Include(f => f.IdCafeteriaNavigation)
                .ToListAsync();
        }

        // Verificar si una cafetería específica ya está en favoritos
        public async Task<bool> EsFavoritoAsync(int idUsuario, int idCafeteria)
        {
            return await _context.Favoritos
                .AnyAsync(f => f.IdUsuario == idUsuario && f.IdCafeteria == idCafeteria);
        }

        // Agregar una cafetería a la lista de favoritos
        public async Task<bool> AgregarFavoritoAsync(int idUsuario, int idCafeteria)
        {
            var existe = await _context.Favoritos
                .AnyAsync(f => f.IdUsuario == idUsuario && f.IdCafeteria == idCafeteria);

            if (existe) return false;

            var nuevoFavorito = new Favorito
            {
                IdUsuario = idUsuario,
                IdCafeteria = idCafeteria,
                FechaGuardado = DateTime.Now
            };

            _context.Favoritos.Add(nuevoFavorito);
            await _context.SaveChangesAsync();
            return true;
        }

        // Eliminar una cafetería de favoritos
        public async Task<bool> EliminarFavoritoAsync(int idUsuario, int idCafeteria)
        {
            var favorito = await _context.Favoritos
                .FirstOrDefaultAsync(f => f.IdUsuario == idUsuario && f.IdCafeteria == idCafeteria);

            if (favorito == null) return false;

            _context.Favoritos.Remove(favorito);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}