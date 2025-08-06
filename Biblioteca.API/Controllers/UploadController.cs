using Biblioteca.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostEnvironment;

        public UploadController(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        [HttpPost("upload-imagem")]
        public async Task<IActionResult> UploadImagem([FromForm] IFormFile file, [FromForm] Guid livroId)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "Nenhum arquivo foi enviado." });
            }

            // O livroId será usado para criar um nome de arquivo único
            string uniqueFileName = livroId.ToString() + "_" + file.FileName;

            // ... (restante da lógica de salvamento) ...

            string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Retorna o caminho com o nome de arquivo único que criamos
            string fileUrl = $"/uploads/{uniqueFileName}";
            return Ok(new { caminho = fileUrl });
        }
    }
}
