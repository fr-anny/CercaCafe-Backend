using CercaCafe.API.DTOs;
using CercaCafe.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CercaCafe.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class FavoritosController : ControllerBase
    {
        private readonly FavoritoService _favoritoService;

        public FavoritosController(FavoritoService favoritoService)
        {
            _favoritoService = favoritoService;
        }

        // Extrae automáticamente el ID del usuario autenticado desde el Token JWT
        private int ObtenerIdUsuarioDesdeToken()
        {
            var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                       ?? User.FindFirst("id_usuario")?.Value
                       ?? User.FindFirst("sub")?.Value;

            return int.TryParse(claimId, out int id) ? id : 0;
        }

        // GET: /api/Favoritos/mis-favoritos
        [HttpGet("mis-favoritos")]
        public async Task<IActionResult> ObtenerMisFavoritos()
        {
            int idUsuario = ObtenerIdUsuarioDesdeToken();
            if (idUsuario == 0) return Unauthorized("Token de usuario inválido.");

            var favoritos = await _favoritoService.ObtenerMisFavoritosAsync(idUsuario);

            // Mapeo dinámico hacia el DTO para responder al cliente (Angular / Expo Go)
            var listaDtos = favoritos.Select(f => new FavoritoDto
            {
                IdFavorito = f.IdFavorito,
                IdCafeteria = f.IdCafeteria,
                NombreCafeteria = f.IdCafeteriaNavigation?.Nombre ?? "",
                Direccion = f.IdCafeteriaNavigation?.Direccion ?? "",
                Latitud = f.IdCafeteriaNavigation?.Latitud ?? 0,
                Longitud = f.IdCafeteriaNavigation?.Longitud ?? 0,
                Calificacion = f.IdCafeteriaNavigation?.Calificacion,
                FechaGuardado = f.FechaGuardado
            });

            return Ok(listaDtos);
        }

        // GET: /api/Favoritos/check/{idCafeteria}
        [HttpGet("check/{idCafeteria}")]
        public async Task<IActionResult> CheckFavorito(int idCafeteria)
        {
            int idUsuario = ObtenerIdUsuarioDesdeToken();
            if (idUsuario == 0) return Unauthorized("Token de usuario inválido.");

            bool esFavorito = await _favoritoService.EsFavoritoAsync(idUsuario, idCafeteria);
            return Ok(new { isFavorite = esFavorito });
        }

        // POST: /api/Favoritos/{idCafeteria}
        [HttpPost("{idCafeteria}")]
        public async Task<IActionResult> AgregarFavorito(int idCafeteria)
        {
            int idUsuario = ObtenerIdUsuarioDesdeToken();
            if (idUsuario == 0) return Unauthorized("Token de usuario inválido.");

            bool agregado = await _favoritoService.AgregarFavoritoAsync(idUsuario, idCafeteria);
            if (!agregado) return BadRequest("La cafetería ya se encuentra en tus favoritos.");

            return Ok(new { mensaje = "Cafetería agregada a favoritos exitosamente." });
        }

        // DELETE: /api/Favoritos/{idCafeteria}
        [HttpDelete("{idCafeteria}")]
        public async Task<IActionResult> EliminarFavorito(int idCafeteria)
        {
            int idUsuario = ObtenerIdUsuarioDesdeToken();
            if (idUsuario == 0) return Unauthorized("Token de usuario inválido.");

            bool eliminado = await _favoritoService.EliminarFavoritoAsync(idUsuario, idCafeteria);
            if (!eliminado) return NotFound("No se encontró la cafetería en tu lista de favoritos.");

            return Ok(new { mensaje = "Cafetería eliminada de favoritos exitosamente." });
        }
    }
}