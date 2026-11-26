using CercaCafe.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CercaCafe.BLL.Services
{
    public class ProductoService
    {
        private readonly CercaCafeContext _context;

        public ProductoService(CercaCafeContext context)
        {
            _context = context;
        }

        public async Task<List<Producto>> ObtenerTodosAsync() => await _context.Productos.ToListAsync();

        public async Task<Producto?> ObtenerPorIdAsync(int id) => await _context.Productos.FindAsync(id);

        public async Task<Producto> CrearAsync(Producto producto)
        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
            return producto;
        }

        public async Task<bool> ActualizarAsync(int id, Producto producto)
        {
            if (id != producto.IdProducto) return false;
            _context.Entry(producto).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Productos.Any(e => e.IdProducto == id)) return false;
                throw;
            }
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var item = await _context.Productos.FindAsync(id);
            if (item == null) return false;
            _context.Productos.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
