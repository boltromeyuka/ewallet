using System;
using System.Collections.Generic;

namespace EWallet.data
{
    /// <summary>
    /// Interface for all repositories
    /// </summary>
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        IEnumerable<T> Find(Func<T, Boolean> predicate);
        void Create(T item);
        void Update(T item);
        void Delete(int id);
    }
}
