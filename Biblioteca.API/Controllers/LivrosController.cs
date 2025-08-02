using Biblioteca.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LivrosController : ControllerBase
    {
        [HttpGet("{id}")]
        public ActionResult<Livro> Get(int id)
        {
            var livro = new Livro("Procurando novo Emprego em TI", "ISBN12345", "Aventura", "Rafael Caixeiro", "Editora Tô Ficando Maluco", "A história de um profissional demitido procurando novo emprego da área de desenvolvimento de Software", "");

            return Ok(livro);
        }

        [HttpPost]
        public ActionResult<Livro> Post([FromBody] Livro novoLivro)
        {
            return Ok();
        }

        [HttpPut]
        public IActionResult Put([FromBody] Livro livroAtualizado)
        {
            return Ok("Dado Atualizado");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok("Dado Excluído");
        }
    }
}
