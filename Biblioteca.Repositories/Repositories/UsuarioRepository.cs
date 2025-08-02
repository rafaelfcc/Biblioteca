using Biblioteca.Data.Context;
using Biblioteca.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Livraria.Data.Repositories
{
    public class UsuarioRepository : _BaseRepository<Usuario>
    {
        public UsuarioRepository(DataContext context) : base(context)
        {
        }
    }
}
