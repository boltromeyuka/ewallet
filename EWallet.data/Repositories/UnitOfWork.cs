using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWallet.data
{
    public class UnitOfWork : IUnitOfWork
    {
        private EWalletContext _db;
        private CategoryRepository _categoryRepository;
        private CurrencyRepository _currencyRepository;
        private OperationRepository _operationRepsitory;

        public UnitOfWork(string conectionString)
        {
            _db = new EWalletContext(conectionString);
        }

        public IRepository<Category> Categories
        {
            get
            {
                if (_categoryRepository == null)
                    _categoryRepository = new CategoryRepository(_db);

                return _categoryRepository;
            }
        }

        public IRepository<Operation> Operations
        {
            get
            {
                if (_operationRepsitory == null)
                    _operationRepsitory = new OperationRepository(_db);

                return _operationRepsitory;
            }
        }

        public IRepository<Currency> Currencies
        {
            get
            {
                if (_currencyRepository == null)
                    _currencyRepository = new CurrencyRepository(_db);

                return _currencyRepository;
            }
        }        

        public void Save()
        {
            _db.SaveChanges();
        }

        private bool disposed = false;

        public void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
