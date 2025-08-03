using Biblioteca.API.JwtAux;
using Biblioteca.Data.Context;
using Biblioteca.Data.Repositories;
using Biblioteca.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginRequestController : ControllerBase
    {
        private readonly UsuarioRepository _uarioRepository;
        private readonly JwtService _jwtService;

        public LoginRequestController(UsuarioRepository usuarioRepository, JwtService jwtService)
        {
            _uarioRepository = usuarioRepository;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var usuario = _uarioRepository.Get(request.Email);

            if (usuario == null)
                return Unauthorized("Usuário não encontrado.");

            if (usuario.Senha != request.Senha) // em produção: use hash seguro
                return Unauthorized("Senha inválida.");

            var token = _jwtService.GenerateToken(
                usuario.EmailLogin,
                usuario.Senha
            );

            return Ok(new { Token = token });
        }
    }
}
