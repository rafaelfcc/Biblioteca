using Biblioteca.Data.Context;
using Biblioteca.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Livraria.Data.Repositories
{
    public abstract class _BaseRepository<T> : _IBaseRepository<T> where T : class
    {
        protected readonly DataContext _context;
        protected readonly DbSet<T> _dbSet;

        public _BaseRepository(DataContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual T? Get(Guid id)
        {
            return _dbSet.Find(id);
        }

        public virtual List<T> GetList(Expression<Func<T, bool>> filter)
        {
            return _dbSet.Where(filter).ToList();
        }

        public virtual Guid Insert(T item)
        {
            _dbSet.Add(item);
            _context.SaveChanges();
            return Guid.Empty;
        }

        public virtual bool Update(T item)
        {
            _dbSet.Update(item);
            return _context.SaveChanges() > 0;
        }

        public virtual bool Delete(Guid id)
        {
            var item = _dbSet.Find(id);
            if (item == null)
            {
                return false;
            }

            _dbSet.Remove(item);
            return _context.SaveChanges() > 0;
        }
    }
}
