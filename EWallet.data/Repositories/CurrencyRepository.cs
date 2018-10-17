using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EWallet.data
{
    public class CurrencyRepository : IRepository<Currency>
    {
        private EWalletContext db;

        public CurrencyRepository(EWalletContext context)
        {
            this.db = context;
        }

        public void Create(Currency item)
        {
            db.Currencies.Add(item);
        }

        public void Delete(int id)
        {
            Currency del = db.Currencies.Find(id);

            if (del != null)
                db.Currencies.Remove(del);
        }

        public IEnumerable<Currency> Find(Func<Currency, bool> predicate)
        {
            return db.Currencies.AsNoTracking().Where(predicate);
        }

        public Currency Get(int id)
        {
            return db.Currencies.Find(id);
        }

        public IEnumerable<Currency> GetAll()
        {
            return db.Currencies.AsNoTracking();
        }

        public void Update(Currency item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
