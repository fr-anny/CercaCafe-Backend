using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CercaCafe.DAL.Models;
using CercaCafe.BLL.Services;

namespace CercaCafe.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CafeteriasController : ControllerBase
    {
        private readonly CafeteriaService _cafeteriaService;

        public CafeteriasController(CafeteriaService cafeteriaService)
        {
            _cafeteriaService = cafeteriaService;
        }

        // 1. LEER TODAS (GET: api/Cafeterias) - Consulta pública
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cafeteria>>> GetCafeterias()
        {
            var cafeterias = await _cafeteriaService.ObtenerTodasAsync();
            return Ok(cafeterias);
        }

        // 2. LEER POR ID (GET: api/Cafeterias/5) - Consulta pública
        [HttpGet("{id}")]
        public async Task<ActionResult<Cafeteria>> GetCafeteria(int id)
        {
            var cafeteria = await _cafeteriaService.ObtenerPorIdAsync(id);
            if (cafeteria == null)
            {
                return NotFound();
            }

            return Ok(cafeteria);
        }

        // 3. ACTUALIZAR (PUT: api/Cafeterias/5) - Solo Administradores
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutCafeteria(int id, Cafeteria cafeteria)
        {
            if (id != cafeteria.IdCafeteria)
            {
                return BadRequest();
            }

            var actualizado = await _cafeteriaService.ActualizarAsync(id, cafeteria);
            if (!actualizado)
            {
                return NotFound();
            }

            return NoContent();
        }

        // 4. CREAR (POST: api/Cafeterias) - Solo Administradores
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Cafeteria>> PostCafeteria(Cafeteria cafeteria)
        {
            var nuevaCafeteria = await _cafeteriaService.CrearAsync(cafeteria);
            return CreatedAtAction("GetCafeteria", new { id = nuevaCafeteria.IdCafeteria }, nuevaCafeteria);
        }

        // 5. ELIMINAR (DELETE: api/Cafeterias/5) - Solo Administradores
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCafeteria(int id)
        {
            var eliminado = await _cafeteriaService.EliminarAsync(id);
            if (!eliminado)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}