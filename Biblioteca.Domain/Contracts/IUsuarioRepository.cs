using Biblioteca.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Domain.Contracts
{
    public interface IUsuarioRepository : _IBaseRepository<Usuario>
    {
        Usuario Get(int id);
        string Insert(Usuario usuario);
        bool Delete(string id);
    }
}
