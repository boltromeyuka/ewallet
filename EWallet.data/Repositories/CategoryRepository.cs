using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EWallet.data
{
    public class CategoryRepository : IRepository<Category>
    {
        private EWalletContext db;

        public CategoryRepository(EWalletContext context)
        {
            this.db = context;
        }

        public void Create(Category item)
        {
            db.Categories.Add(item);
        }

        public void Delete(int id)
        {
            Category del = db.Categories.Find(id);

            if (del != null)
                db.Categories.Remove(del);
        }

        public IEnumerable<Category> Find(Func<Category, bool> predicate)
        {
            return db.Categories.AsNoTracking().Where(predicate);
        }

        public Category Get(int id)
        {
            return db.Categories.Find(id);
        }

        public IEnumerable<Category> GetAll()
        {
            return db.Categories.AsNoTracking();
        }

        public void Update(Category item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
