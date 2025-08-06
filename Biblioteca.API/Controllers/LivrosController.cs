using Biblioteca.API.Models;
using Biblioteca.Data.Repositories;
using Biblioteca.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using Biblioteca.Domain.Contracts;

namespace Biblioteca.API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LivrosController : ControllerBase
    {
        private readonly LivroRepository _repository;
        private readonly IMapper _mapper;
        private readonly IRelatorioPdf _relatorioPdf;

        public LivrosController(LivroRepository repository, IMapper mapper, IRelatorioPdf relatorioPdf)
        {
            _repository = repository;
            _mapper = mapper;
            _relatorioPdf = relatorioPdf;
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
        public ActionResult<List<Livro>> GetAll([FromQuery] string? titulo, [FromQuery] string? isbn, [FromQuery(Name = "genero")] int? generoId,[FromQuery(Name = "editora")] int? editoraId)
        {
            Expression<Func<Livro, bool>> filter = l => l.Id != null;

            ParameterExpression parameter = filter.Parameters[0];

            Expression<Func<Livro, bool>> pTitulo = l => l.Titulo.Contains(titulo);
            Expression<Func<Livro, bool>> pIsbn = l => l.ISBN == isbn;
            Expression<Func<Livro, bool>> pGenero = l => l.GeneroId == generoId;
            Expression<Func<Livro, bool>> pEditora = l => l.EditoraId == editoraId;


            if (!string.IsNullOrWhiteSpace(titulo))
            {
                Expression body = Expression.AndAlso(filter.Body, Expression.Invoke(pTitulo, parameter));
                filter = Expression.Lambda<Func<Livro, bool>>(body, parameter);
            }

            if (!string.IsNullOrWhiteSpace(isbn))
            {
                Expression body = Expression.AndAlso( filter.Body, Expression.Invoke(pIsbn, parameter));
                filter = Expression.Lambda<Func<Livro, bool>>(body, parameter);
            }

            if (generoId.HasValue)
            {
                Expression body = Expression.AndAlso(filter.Body, Expression.Invoke(pGenero, parameter));
                filter = Expression.Lambda<Func<Livro, bool>>(body, parameter);
            }

            if (editoraId.HasValue)
            {
                Expression body = Expression.AndAlso(filter.Body, Expression.Invoke(pEditora, parameter));
                filter = Expression.Lambda<Func<Livro, bool>>(body, parameter);
            }

            var livros = _repository.GetAllLivros(filter);
            return Ok(livros);
        }

        [HttpPost]
        public ActionResult<Guid> Post([FromBody] LivroInputModel novoLivro)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Livro livro = new Livro();
            _mapper.Map(novoLivro, livro);

            var id = _repository.Insert(livro);
            return CreatedAtAction(nameof(Get), new { id = id }, novoLivro);
        }

        [HttpPut]
        public IActionResult Put([FromBody] LivroInputModel livroAtualizado)
        {
            var existente = _repository.GetList(l => l.Id == livroAtualizado.Id).FirstOrDefault();

            if (existente == null)
                return NotFound();

            _mapper.Map(livroAtualizado, existente);

            _repository.Update(existente);
            return Ok(new { message = "Livro atualizado com sucesso" });
        }

        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var sucesso = _repository.Delete(id);
            if (!sucesso)
                return NotFound();

            return Ok(new { message = "Livro excluído com sucesso" });
        }

        [HttpPost("atualizar-caminho-imagem")]
        public ActionResult<Guid> AtualizarImagem([FromBody] ImagemLivro imagemLivro)
        {
            try
            {
                var existente = _repository.GetList(l => l.Id == imagemLivro.Id).FirstOrDefault();

                if (existente == null)
                    return NotFound();

                existente.CaminhoFoto = imagemLivro.CaminhoFoto;

                _repository.Update(existente);
                return Ok(new { message = "Livro atualizado com sucesso" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
            
        }

        [HttpGet("relatorio-pdf")]
        public IActionResult GerarRelatorioPdf([FromQuery] string usuarioRegistro)
        {
            var livros = _repository.GetAllLivros(l => l.UsuarioRegistro == usuarioRegistro);
            var pdfBytes = _relatorioPdf.GerarRelatorioLivros(livros.ToList(), usuarioRegistro);
            return File(pdfBytes, "application/pdf", $"RelatorioLivros_{DateTime.Now:yyyyMMddHHmmss}.pdf");
        }
    }
}
