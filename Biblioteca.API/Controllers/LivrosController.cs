using Biblioteca.Data.Repositories;
using Biblioteca.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LivrosController : ControllerBase
    {
        private readonly LivroRepository _repository;

        public LivrosController(LivroRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{id:guid}")]
        public ActionResult<Livro> Get(Guid id)
        {
            var livro = _repository.Get(id);
            if (livro == null)
                return NotFound();

            return Ok(livro);
        }

        [HttpGet]
        public ActionResult<List<Livro>> GetAll()
        {
            var livros = _repository.GetList(l => true);
            return Ok(livros);
        }

        [HttpPost]
        public ActionResult<Guid> Post([FromBody] Livro novoLivro)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = _repository.Insert(novoLivro);
            return CreatedAtAction(nameof(Get), new { id = id }, novoLivro);
        }

        [HttpPut("{id:guid}")]
        public IActionResult Put(Guid id, [FromBody] Livro livroAtualizado)
        {
            if (id != livroAtualizado.Id)
                return BadRequest("ID inconsistente");

            var livroExistente = _repository.Get(id);
            if (livroExistente == null)
                return NotFound();

            _repository.Update(livroAtualizado);
            return Ok("Livro atualizado com sucesso");
        }

        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var sucesso = _repository.Delete(id);
            if (!sucesso)
                return NotFound();

            return Ok("Livro excluído com sucesso");
        }
    }
}
