using System.Collections.Generic;

namespace EWallet.data
{
    public class Currency : Named
    {
        public virtual ICollection<Operation> Operations { get; set; }
        
        public Currency()
        {
            Operations = new List<Operation>();
        }
    }
}
