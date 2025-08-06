using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Domain.Entities
{
    public class Livro
    {
        public Guid Id { get; set; }

        public string Titulo { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public int GeneroId { get; set; } = 0;
        public string Autor { get; set; } = string.Empty;
        public int EditoraId { get; set; } = 0;
        public string Sinopse { get; set; } = string.Empty;
        public string UsuarioRegistro { get; set; } = string.Empty;

        public string CaminhoFoto { get; set; } = string.Empty;

        public GeneroLivro GeneroLivro { get; set; } = null!;
        public Editora Editora { get; set; } = null!;

        [NotMapped]
        public byte FotoLivro { get; set; }
    }
}
