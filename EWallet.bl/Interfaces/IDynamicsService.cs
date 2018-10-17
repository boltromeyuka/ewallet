using EWallet.viewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace EWallet.bl
{
    public interface IDynamicsService
    {
        DynamicsViewModel FillDictionaries(DynamicsViewModel model);
        IEnumerable<IEnumerable<object>> GetChartDataByTypes(DynamicsViewModel model, IPrincipal user);
        IEnumerable<IEnumerable<object>> GetChartDataByCategories(DynamicsViewModel model, IPrincipal user);

    }
}
