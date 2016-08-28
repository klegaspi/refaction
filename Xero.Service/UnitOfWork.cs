using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xero.Model;
using Xero.Repository;

namespace Xero.Service
{     
    public class UnitOfWork : IUnitOfWork
    {
        #region Member Variables

        private readonly DatabaseContext _databaseContext;
        private IRepository<Product> _productRepository;
        private IRepository<ProductOption> _productOptionRepository;
        private bool _isDisposed = false;

        #endregion

        #region Constructors

        public UnitOfWork()
        {
            _databaseContext = new DatabaseContext();
        }

        #endregion

        #region Properties

        public IRepository<Product> ProductRepository
        {
            get { return _productRepository ?? (_productRepository = new Repository<Product>(_databaseContext)); }
        }

        public IRepository<ProductOption> ProductOptionRepository
        {
            get { return _productOptionRepository ?? (_productOptionRepository = new Repository<ProductOption>(_databaseContext)); }
        }
        
        #endregion

        #region Methods

        public int Save()
        {
            return _databaseContext.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _databaseContext.Dispose();
                }
            }
            this._isDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);            
        }

        #endregion       
    }
}
