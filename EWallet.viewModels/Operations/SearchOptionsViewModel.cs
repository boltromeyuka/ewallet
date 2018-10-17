using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace EWallet.viewModels
{
    public class SearchOptionsViewModel
    {
        public int? CategoryId { get; set; }
        public List<SelectListItem> Categories { get; set; }       

        public int? CurrencyId { get; set; }
        public List<SelectListItem> Currencies { get; set; }        
        public decimal? Amount { get; set; }       

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? CreateDate { get; set; }
    }
}
