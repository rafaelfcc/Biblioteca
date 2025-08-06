using Biblioteca.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Domain.Contracts
{
    public interface IRelatorioPdf
    {
        byte[] GerarRelatorioLivros(List<Livro> livros, string? usuarioEmail = null);
    }
}
