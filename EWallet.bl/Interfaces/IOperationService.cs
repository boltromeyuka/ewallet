using EWallet.viewModels;
using System.Collections.Generic;
using System.Security.Principal;

namespace EWallet.bl
{
    public interface IOperationService
    {
        void CreateOpeation(OperationViewModel operation, IPrincipal user);
        IEnumerable<OperationViewModel> GetOperations(IPrincipal user);
        OperationViewModel FillDictionaries(int type, OperationViewModel model, IPrincipal user);
        SearchOptionsViewModel FillDictionariesSearch(SearchOptionsViewModel model, IPrincipal user);
        IEnumerable<OperationViewModel> GetSortingData(SortOptionsViewModel sort, IPrincipal user);
        IEnumerable<OperationViewModel> GetSearchingData(SearchOptionsViewModel model, IPrincipal user);
    }
}
