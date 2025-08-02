using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Domain.Contracts
{
    public interface _IBaseRepository<T>
    {
        T? Get(Guid id);
        List<T> GetList(Expression<Func<T, bool>> filter);
        Tid Insert<Tid>(T item);
        bool Update(T item);
        bool Delete<Tid>(Tid id);
    }
}
