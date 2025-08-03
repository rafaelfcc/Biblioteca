using Biblioteca.Data.Repositories;
using Biblioteca.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioRepository _repository;

        public UsuarioController(UsuarioRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{email}")]
        public ActionResult<Usuario> Get(string email)
        {
            var usuario = _repository.GetList(u => u.EmailLogin == email).FirstOrDefault();

            if (usuario == null)
                return NotFound();

            return Ok(usuario);
        }

        [HttpGet]
        public ActionResult<List<Usuario>> GetAll()
        {
            var usuarios = _repository.GetList(u => true);
            return Ok(usuarios);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Usuario novoUsuario)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existente = _repository.GetList(u => u.EmailLogin == novoUsuario.EmailLogin).FirstOrDefault();
            if (existente != null)
                return Conflict("Já existe um usuário com esse e-mail.");

            _repository.Insert(novoUsuario);
            return CreatedAtAction(nameof(Get), new { email = novoUsuario.EmailLogin }, novoUsuario);
        }

        [HttpPut("{email}")]
        public IActionResult Put(string email, [FromBody] Usuario usuarioAtualizado)
        {
            if (email != usuarioAtualizado.EmailLogin)
                return BadRequest("E-mail inconsistente");

            var existente = _repository.GetList(u => u.EmailLogin == email).FirstOrDefault();
            if (existente == null)
                return NotFound();

            _repository.Update(usuarioAtualizado);
            return Ok("Usuário atualizado com sucesso");
        }

        [HttpDelete("{email}")]
        public IActionResult Delete(string email)
        {
            var existente = _repository.GetList(u => u.EmailLogin == email).FirstOrDefault();
            if (existente == null)
                return NotFound();

            return _repository.Delete<string>(email) ? Ok("Usuário excluído com sucesso") : StatusCode(500, "Erro ao excluir");
        }
    }
}
