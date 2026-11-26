using Microsoft.AspNetCore.Mvc;
using CercaCafe.DAL.Models;
using CercaCafe.BLL.Services;

namespace CercaCafe.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public UsuariosController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            var lista = await _usuarioService.ObtenerTodosAsync();
            return Ok(lista);
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _usuarioService.ObtenerPorIdAsync(id);
            if (usuario == null) return NotFound();
            return Ok(usuario);
        }

        // POST: api/Usuarios (REGISTRAR)
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            var nuevo = await _usuarioService.CrearAsync(usuario);
            return CreatedAtAction(nameof(GetUsuario), new { id = nuevo.IdUsuario }, nuevo);
        }

        // POST: api/Usuarios/login (LOGIN DESDE ANGULAR)
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            var usuario = await _usuarioService.ValidarLoginAsync(request.Correo, request.Contrasena);
            if (usuario == null)
            {
                return Unauthorized(new { mensaje = "Correo o contraseña incorrectos" });
            }
            return Ok(usuario);
        }

        // PUT: api/Usuarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            var exito = await _usuarioService.ActualizarAsync(id, usuario);
            if (!exito) return BadRequest();
            return NoContent();
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var exito = await _usuarioService.EliminarAsync(id);
            if (!exito) return NotFound();
            return NoContent();
        }
    }

    // DTO único para recibir las credenciales desde Angular
    public class LoginDto
    {
        public string Correo { get; set; } = null!;
        public string Contrasena { get; set; } = null!;
    }
}