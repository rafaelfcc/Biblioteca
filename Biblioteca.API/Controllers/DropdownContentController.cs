using Biblioteca.Data.Context;
using Biblioteca.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DropdownContentController : ControllerBase
    {
        private readonly DataContext _context;

        public DropdownContentController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("generos")]
        public ActionResult<IEnumerable<GeneroLivro>> GetGeneros()
        {
            return _context.GeneroLivros.ToList();
        }

        [HttpGet("editoras")]
        public ActionResult<IEnumerable<Editora>> GetEditoras()
        {
            return _context.Editoras.ToList();
        }
    }
}
