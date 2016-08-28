using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Xero.Repository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();

        IEnumerable<T> GetBy(Expression<Func<T, bool>> predicate);

        T Add(T entity);
        
        T Edit(T entity);

        void Delete(T entity);

        T FindById(Guid id);
    }
}
