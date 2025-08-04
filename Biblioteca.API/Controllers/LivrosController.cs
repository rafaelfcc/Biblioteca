using Biblioteca.Data.Repositories;
using Biblioteca.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.API.Controllers
{
    [Authorize]
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

        [HttpPut]
        public IActionResult Put([FromBody] Livro livroAtualizado)
        {
            var existente = _repository.GetList(l => l.Id == livroAtualizado.Id).FirstOrDefault();

            if (existente == null)
                return NotFound();

            existente.Id = livroAtualizado.Id;
            existente.Titulo = livroAtualizado.Titulo;
            existente.ISBN = livroAtualizado.ISBN;
            existente.Autor = livroAtualizado.Autor;
            existente.Editora = livroAtualizado.Editora;
            existente.Sinopse = livroAtualizado.Sinopse;
            existente.CaminhoFoto = livroAtualizado.CaminhoFoto;

            _repository.Update(existente);
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

        [HttpPost]
        public IActionResult AtualizarImagemLivro([FromBody] ImagemLivro imagemLivro)
        {
            var existente = _repository.GetList(l => l.Id == imagemLivro.LivroId).FirstOrDefault();

            if (existente == null)
                return NotFound();

            existente.CaminhoFoto = imagemLivro.ImagemPath;

            _repository.Update(existente);
            return Ok("Livro atualizado com sucesso");
        }
    }
}
