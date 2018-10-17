using EWallet.data;
using EWallet.viewModels;
using System.Collections.Generic;
using System.Linq;

namespace EWallet.bl
{
    public class CurrencyService : ICurrencyService
    {
        IUnitOfWork Database { get; set; }

        public CurrencyService(IUnitOfWork uow)
        {
            Database = uow;
        }

        /// <summary>
        /// Get all currencies
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CurrencyViewModel> GetCurrencies()
        {
            return Database.Currencies.GetAll()
                                            .Select(x => new CurrencyViewModel {
                                                                            Id = x.Id,
                                                                            Name = x.Name
                                                                         });
        }

        /// <summary>
        /// Get currency info by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public CurrencyViewModel GetCurrency(int Id)
        {
            var current = Database.Categories.Get(Id);

            return new CurrencyViewModel
            {
                Id = current.Id,
                Name = current.Name
            };
        }
    }
}
