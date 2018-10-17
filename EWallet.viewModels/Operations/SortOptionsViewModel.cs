using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWallet.viewModels
{
    public class SortOptionsViewModel
    {
        public string PropertyName { get; set; }
        public SortType SortType { get; set; }
        public string SearchString { get; set; }
    }
}
