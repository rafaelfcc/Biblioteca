using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Domain.Entities
{
    public class ImagemLivro
    {
        public Guid Id { get; set; }
        public string CaminhoFoto { get; set; } = string.Empty;
    }
}
