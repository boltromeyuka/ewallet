using EWallet.data;
using System;
using System.ComponentModel.DataAnnotations;

namespace EWallet.viewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Название категории")]
        [Required]
        public string Name { get; set; }
        public OperationType CategoryType { get; set; }
    }
}
