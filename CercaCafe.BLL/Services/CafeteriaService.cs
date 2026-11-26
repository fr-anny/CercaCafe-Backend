using CercaCafe.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CercaCafe.BLL.Services
{
    public class CafeteriaService
    {
        private readonly CercaCafeContext _context;

        public CafeteriaService(CercaCafeContext context)
        {
            _context = context;
        }

        public async Task<List<Cafeteria>> ObtenerTodasAsync() => await _context.Cafeterias.ToListAsync();

        public async Task<Cafeteria?> ObtenerPorIdAsync(int id) => await _context.Cafeterias.FindAsync(id);

        public async Task<Cafeteria> CrearAsync(Cafeteria cafeteria)
        {
            _context.Cafeterias.Add(cafeteria);
            await _context.SaveChangesAsync();
            return cafeteria;
        }

        public async Task<bool> ActualizarAsync(int id, Cafeteria cafeteria)
        {
            if (id != cafeteria.IdCafeteria) return false;
            _context.Entry(cafeteria).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Cafeterias.Any(e => e.IdCafeteria == id)) return false;
                throw;
            }
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var item = await _context.Cafeterias.FindAsync(id);
            if (item == null) return false;
            _context.Cafeterias.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
