using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xero.Model;

namespace Xero.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        #region Member Variables

        private DatabaseContext _databaseContext;
        private readonly IDbSet<T> _dbSet;

        #endregion

        #region Constructors
        public Repository()
        {
            _databaseContext = new DatabaseContext();
        }

        public Repository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
            _dbSet = databaseContext.Set<T>();
        }
        #endregion

        #region Methods
        public IEnumerable<T> GetAll()
        {            
            return _dbSet.AsEnumerable<T>();
        }

        public IEnumerable<T> GetBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {            
            return _dbSet.Where(predicate).AsEnumerable<T>();
        }

        public T Add(T entity)
        {            
            return _dbSet.Add(entity);
        }

        public void Delete(T entity)
        {            
            _dbSet.Remove(entity);
        }

        public T Edit(T entity)
        {
            _databaseContext.Entry(entity).State = EntityState.Modified;            
            return entity;
        }

        public T FindById(Guid id)
        {
            return _databaseContext.Set<T>().Find(id);            
        }

        #endregion
    }
}
