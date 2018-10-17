using EWallet.data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace EWallet.viewModels
{
    public class OperationViewModel
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Тип")]
        public OperationType OperationType { get; set; }

        [Display(Name = "Название категории")]
        public int? CategoryId { get; set; }
        public  List<SelectListItem> Categories { get; set; }

        [Display(Name = "Название категории")]
        public string CategoryName { get; set; }

        [Display(Name = "Валюта")]
        [Required]
        public int CurrencyId { get; set; }
        public List<SelectListItem> Currencies { get; set; }

        [Display(Name = "Валюта")]
        public string CurrencyName { get; set; }

        [Display(Name = "Сумма")]
        [Range(typeof(decimal), "0,01", "10000000")]        
        public decimal Amount { get; set; }

        [Display(Name = "Комментарий")]
        public string Comment { get; set; }

        [Display(Name = "Дата")]
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreateDate { get; set; }
    }
}
