using Biblioteca.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Domain.Contracts
{
    public interface ILivroRepository : _IBaseRepository<Livro>
    {
        Guid? Insert(Livro livro);
        bool Delete(Guid id);
    }
}
