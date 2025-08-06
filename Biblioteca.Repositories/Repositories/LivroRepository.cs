using Biblioteca.Data.Context;
using Biblioteca.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Data.Repositories
{
    public class LivroRepository : _BaseRepository<Livro>
    {
        public LivroRepository(DataContext context) : base(context)
        {

        }

        public List<Livro> GetAllLivros(Expression<Func<Livro, bool>> filter)
        {
            return base._dbSet.Where(filter).Include(l => l.Editora).Include(l => l.GeneroLivro).ToList();
        }

        public Livro? Get(Guid id)
        {
            var livro = base.Get<Guid>(id);

            if (livro != null)
            {
                livro.Editora = base._context.Editoras.Where(e => e.Id == livro.EditoraId).First();
                livro.GeneroLivro = base._context.GeneroLivros.Where(g => g.Id == livro.GeneroId).First();
            }

            return livro;
        }

        public Guid? Insert(Livro livro)
        {
            livro.GeneroLivro = base._context.GeneroLivros.Where(g => g.Id == livro.GeneroId).First();
            livro.Editora = base._context.Editoras.Where(e => e.Id == livro.EditoraId).First();
            livro.Id = Guid.NewGuid();

            return base.Insert<Guid>(livro);
        }

        public bool Delete(Guid id)
        {
            return base.Delete<Guid>(id);
        }

        public override bool Update(Livro livro)
        {
            livro.GeneroLivro = base._context.GeneroLivros.Where(g => g.Id == livro.GeneroId).First();
            livro.Editora = base._context.Editoras.Where(e => e.Id == livro.EditoraId).First();

            return base.Update(livro);
        }
    }
}
