using CercaCafe.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CercaCafe.BLL.Services
{
        public class HorarioService
        {
            private readonly CercaCafeContext _context;

            public HorarioService(CercaCafeContext context)
            {
                _context = context;
            }

            public async Task<List<Horario>> ObtenerTodosAsync() => await _context.Horarios.ToListAsync();

            public async Task<Horario?> ObtenerPorIdAsync(int id) => await _context.Horarios.FindAsync(id);

            public async Task<Horario> CrearAsync(Horario horario)
            {
                _context.Horarios.Add(horario);
                await _context.SaveChangesAsync();
                return horario;
            }

            public async Task<bool> ActualizarAsync(int id, Horario horario)
            {
                if (id != horario.IdHorario) return false;
                _context.Entry(horario).State = EntityState.Modified;
                try
                {
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Horarios.Any(e => e.IdHorario == id)) return false;
                    throw;
                }
            }

            public async Task<bool> EliminarAsync(int id)
            {
                var item = await _context.Horarios.FindAsync(id);
                if (item == null) return false;
                _context.Horarios.Remove(item);
                await _context.SaveChangesAsync();
                return true;
            }
        }
}