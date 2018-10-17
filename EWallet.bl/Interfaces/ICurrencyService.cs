using EWallet.viewModels;
using System.Collections.Generic;

namespace EWallet.bl
{
    public interface ICurrencyService
    {
        CurrencyViewModel GetCurrency(int Id);
        IEnumerable<CurrencyViewModel> GetCurrencies();
    }
}
