using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Domain.Entities
{
    public class ImagemLivro
    {
        public Guid LivroId { get; set; }
        public string ImagemPath { get; set; } = string.Empty;
    }
}
