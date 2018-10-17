using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EWallet.data
{
    public class Category : Named
    {
        [Required]
        public OperationType CategoryType { get; set; }
        public bool IsArchive { get; set; }

        [Required]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Operation> Operations { get; set; }

        public Category()
        {
            Operations = new List<Operation>();
        }
    }
}
