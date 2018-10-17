using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EWallet.viewModels
{
    public class DynamicsViewModel
    {
        [Required]
        [Display(Name = "С")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DateFrom { get; set; }

        [Required]
        [Display(Name = "По")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DateTo { get; set; }

        [Required]
        public int CurrencyId { get; set; }

        public IEnumerable<SelectListItem> CurrenciesList { get; set; }

        [Required]
        [Display(Name = "По категориям")]
        public bool ByCategories { get; set; }

        public DynamicsViewModel()
        {
            CurrenciesList = new List<SelectListItem>();
        }
    }
}
