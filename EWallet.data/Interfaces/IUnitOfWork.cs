using System;

namespace EWallet.data
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Operation> Operations { get; }
        IRepository<Category> Categories { get; }
        IRepository<Currency> Currencies { get; }
        void Save();
    }
}
