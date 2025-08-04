using Biblioteca.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.API.Controllers
{
    /*[Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostEnvironment;

        public UploadController(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        [HttpPost("upload-imagem")]
        public async Task<IActionResult> UploadImagem([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "Nenhum arquivo foi enviado." });
            }

            // Define o diretório para salvar o arquivo (por exemplo, na pasta wwwroot/uploads)
            string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Gera um nome de arquivo único para evitar conflitos
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Salva o arquivo no diretório
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Retorna o caminho relativo do arquivo salvo para o frontend
            string fileUrl = $"/uploads/{uniqueFileName}";
            return Ok(new { caminho = fileUrl });
        }
    }*/
}
