using System;
using System.ComponentModel.DataAnnotations;

namespace EWallet.data
{
    public class Operation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public OperationType OperationType { get; set; }

        public int? CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public int CurrencyId { get; set; }
        public virtual Currency Currency { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public decimal AmountInBYN { get; set; }

        public string Comment { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [Required]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
