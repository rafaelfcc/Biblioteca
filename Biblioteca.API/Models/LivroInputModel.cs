using System;
using System.ComponentModel.DataAnnotations;

namespace Biblioteca.API.Models
{
    public class LivroInputModel
    {
        public Guid? Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string ISBN { get; set; } = string.Empty;

        [Required]
        public int GeneroId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Autor { get; set; } = string.Empty;

        [Required]
        public int EditoraId { get; set; }

        [MaxLength(5000)]
        public string Sinopse { get; set; } = string.Empty;

        [Required]
        public string UsuarioRegistro { get; set; } = string.Empty;

        public string CaminhoFoto { get; set; } = "0";
        public int FotoLivro { get; set; } = 0;
    }
}
