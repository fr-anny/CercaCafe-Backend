using CercaCafe.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CercaCafe.BLL.Services
{
    public class ResenaService
    {
        private readonly CercaCafeContext _context;

        public ResenaService(CercaCafeContext context)
        {
            _context = context;
        }

        public async Task<List<Resena>> ObtenerTodasAsync() => await _context.Resenas.ToListAsync();

        public async Task<Resena?> ObtenerPorIdAsync(int id) => await _context.Resenas.FindAsync(id);

        public async Task<Resena> CrearAsync(Resena resena)
        {
            _context.Resenas.Add(resena);
            await _context.SaveChangesAsync();
            return resena;
        }

        public async Task<bool> ActualizarAsync(int id, Resena resena)
        {
            if (id != resena.IdResena) return false;
            _context.Entry(resena).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Resenas.Any(e => e.IdResena == id)) return false;
                throw;
            }
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var item = await _context.Resenas.FindAsync(id);
            if (item == null) return false;
            _context.Resenas.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
