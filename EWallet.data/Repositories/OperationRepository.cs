using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EWallet.data
{
    public class OperationRepository : IRepository<Operation>
    {
        private EWalletContext db;

        public OperationRepository(EWalletContext context)
        {
            this.db = context;
        }

        public void Create(Operation item)
        {
            db.Operations.Add(item);
        }

        public void Delete(int id)
        {
            Operation del = db.Operations.Find(id);

            if (del != null)
                db.Operations.Remove(del);
        }

        public IEnumerable<Operation> Find(Func<Operation, bool> predicate)
        {
            return db.Operations.AsNoTracking().Where(predicate);
        }

        public Operation Get(int id)
        {
            return db.Operations.Find(id);
        }

        public IEnumerable<Operation> GetAll()
        {
            return db.Operations.AsNoTracking();
        }

        public void Update(Operation item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
