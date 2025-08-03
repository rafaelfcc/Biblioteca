using Biblioteca.Data.Context;
using Biblioteca.Domain.Entities;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Data.Repositories
{
    public class LivroRepository : _BaseRepository<Livro>
    {
        public LivroRepository(DataContext context) : base(context)
        {

        }

        public Livro? Get(Guid id)
        {
            return base.Get<Guid>(id);
        }

        public Guid? Insert(Livro livro)
        {
            return base.Insert<Guid>(livro);
        }

        public bool Delete(Guid id)
        {
            return base.Delete<Guid>(id);
        }
    }
}
