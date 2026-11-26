using CercaCafe.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CercaCafe.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResenasController : ControllerBase
    {
        private readonly CercaCafeContext _context;

        public ResenasController(CercaCafeContext context)
        {
            _context = context;
        }

        // GET: api/Resenas (Público)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Resena>>> GetResenas()
        {
            return await _context.Resenas.ToListAsync();
        }

        // GET: api/Resenas/cafeteria/1 (Público)
        [HttpGet("cafeteria/{idCafeteria}")]
        public async Task<ActionResult<IEnumerable<object>>> GetResenasPorCafeteria(int idCafeteria)
        {
            var resenas = await _context.Resenas
                .Where(r => r.IdCafeteria == idCafeteria)
                .Select(r => new
                {
                    r.IdResena,
                    r.IdCafeteria,
                    r.IdUsuario,
                    NombreUsuario = r.IdUsuarioNavigation != null ? r.IdUsuarioNavigation.Nombre : "Usuario",
                    r.Calificacion,
                    r.Comentario,
                    r.FechaResena
                })
                .ToListAsync();

            return Ok(resenas);
        }

        // GET: api/Resenas/5 (Público)
        [HttpGet("{id}")]
        public async Task<ActionResult<Resena>> GetResena(int id)
        {
            var resena = await _context.Resenas.FindAsync(id);

            if (resena == null)
            {
                return NotFound();
            }

            return resena;
        }

        // PUT: api/Resenas/5 (Requiere Autenticación)
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutResena(int id, Resena resena)
        {
            if (id != resena.IdResena)
            {
                return BadRequest();
            }

            _context.Entry(resena).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResenaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Resenas (Cualquier usuario autenticado puede publicar reseñas)
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Resena>> PostResena([FromBody] CrearResenaDto dto)
        {
            var nuevaResena = new Resena
            {
                IdCafeteria = dto.IdCafeteria,
                IdUsuario = dto.IdUsuario,
                Calificacion = dto.Calificacion,
                Comentario = dto.Comentario,
                FechaResena = DateOnly.FromDateTime(DateTime.Now)
            };

            _context.Resenas.Add(nuevaResena);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetResena", new { id = nuevaResena.IdResena }, nuevaResena);
        }

        // DELETE: api/Resenas/5 (Solo Administradores o autor)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteResena(int id)
        {
            var resena = await _context.Resenas.FindAsync(id);
            if (resena == null)
            {
                return NotFound();
            }

            _context.Resenas.Remove(resena);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ResenaExists(int id)
        {
            return _context.Resenas.Any(e => e.IdResena == id);
        }
    }

    // DTO para recibir únicamente los datos requeridos desde Angular
    public class CrearResenaDto
    {
        public int IdCafeteria { get; set; }
        public int IdUsuario { get; set; }
        public int Calificacion { get; set; }
        public string Comentario { get; set; } = null!;
    }
}