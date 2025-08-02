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
        public string Genero { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public string Editora { get; set; } = string.Empty;
        public string Sinopse { get; set; } = string.Empty;

        public string CaminhoFoto { get; set; } = string.Empty;

        [NotMapped]
        public byte FotoLivro { get; set; }

        public Livro()
        {
                
        }

        public Livro(string titulo, string isbn, string genero, string autor, string editora, string sinopse, string caminhoFoto)
        {
            Titulo = titulo;
            ISBN = isbn;
            Genero = genero;
            Autor = autor;
            Editora = editora;
            Sinopse = sinopse;
            CaminhoFoto = caminhoFoto;
        }
    }
}
