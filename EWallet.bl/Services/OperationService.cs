using EWallet.data;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Microsoft.AspNet.Identity;
using System;
using System.Net.Http;
using EWallet.viewModels;
using System.Web.Mvc;
using EWallet.bl.Extensions;
using System.Web.Configuration;

namespace EWallet.bl
{
    public class OperationService : IOperationService
    {
        private ICurrencyService _currencyService;
        private ICategoryService _categoryService;

        private string _baseUrl = WebConfigurationManager.AppSettings["NBRB"];

        IUnitOfWork Database { get; set; }        

        public OperationService(IUnitOfWork uow,
                                ICurrencyService currencyService, ICategoryService categoryService) : base()
        {
            Database = uow;
            _currencyService = currencyService;
            _categoryService = categoryService;
        }

        public void CreateOpeation(OperationViewModel operation, IPrincipal user)
        {
            try
            {
                var create = new Operation
                {
                    CurrencyId = operation.CurrencyId,
                    CategoryId = operation.CategoryId,
                    CreateDate = operation.CreateDate,
                    Amount = operation.Amount,
                    AmountInBYN = GetAmountInBYN(operation.CurrencyId, operation.Amount, operation.CreateDate),
                    Comment = operation.Comment,
                    OperationType = operation.OperationType,
                    UserId = user.Identity.GetUserId()
                };

                Database.Operations.Create(create);
                Database.Save();
            }
            catch(Exception e)
            {
                throw new Exception(String.Format("При добавлении операции возникла ошибка: {0}", e.InnerException != null ? e.Message + " " + e.InnerException : e.Message));
            }
        }


        /// <summary>
        /// Get all operations of user
        /// </summary>
        /// <param name="user">user</param>
        /// <returns></returns>
        public IEnumerable<OperationViewModel> GetOperations(IPrincipal user)
        {
            return Database.Operations
                                .Find(x => x.UserId == user.Identity.GetUserId()).ToList()
                                                .Select(x => new OperationViewModel
                                                {
                                                    Id = x.Id,
                                                    Amount = x.Amount,
                                                    CategoryId = x.CategoryId,
                                                    CategoryName = x.Category?.Name,
                                                    CurrencyId = x.CurrencyId,
                                                    CurrencyName = x.Currency.Name,                                     
                                                    Comment = x.Comment,
                                                    CreateDate = x.CreateDate,
                                                    OperationType = x.OperationType
                                                });
        }


        /// <summary>
        /// Get sorting data
        /// </summary>
        /// <param name="sort">model with sort options</param>
        /// <param name="user">user</param>
        /// <returns></returns>
        public IEnumerable<OperationViewModel> GetSortingData(SortOptionsViewModel sort, IPrincipal user)
        {
            var sortParamString = string.Format("{0} {1}", sort.PropertyName, sort.SortType.ToString());    // string with property name and type order(ASC, DESC)
            return GetOperations(user).OrderBy(sortParamString);
        }

        /// <summary>
        /// Get searthing data
        /// </summary>
        /// <param name="model">model with search settings</param>
        /// <param name="user">user</param>
        /// <returns></returns>
        public IEnumerable<OperationViewModel> GetSearchingData(SearchOptionsViewModel model, IPrincipal user)
        {
            return Database.Operations.Find(x=>x.UserId==user.Identity.GetUserId()).ToList()                    // by user
                            .Where(x => (model.CategoryId.HasValue ? x.CategoryId == model.CategoryId : true))  // by category
                                .Where(x=>(model.CurrencyId.HasValue ? x.CurrencyId==model.CurrencyId : true))  // by currency
                                    .Where(x=>(model.Amount.HasValue ? x.Amount==model.Amount : true))          // by amount
                                        .Where(x=>(model.CreateDate.HasValue ? x.CreateDate.Date == model.CreateDate.Value.Date : true)) // by date
                            .Select(x => new OperationViewModel
            {
                CategoryName = x.Category?.Name,
                CurrencyName = x.Currency.Name,
                Comment = x.Comment,
                Amount = x.Amount,
                CreateDate = x.CreateDate,
                OperationType = x.OperationType
            });
        }

        /// <summary>
        /// Fill dictionaries for operation view
        /// </summary>
        /// <param name="type">type of operation</param>
        /// <param name="model"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public OperationViewModel FillDictionaries(int type, OperationViewModel model, IPrincipal user)
        {
            model.CreateDate = DateTime.Now;
            model.OperationType = type == 0 ? OperationType.Income : OperationType.Spending;

            model.Categories = new List<SelectListItem> { new SelectListItem { Value = "", Text = "Выберите категорю" } };

            model.Currencies = _currencyService.GetCurrencies()
                                            .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();

            model.Categories.AddRange(_categoryService.GetCategories(type == 0 ? OperationType.Income : OperationType.Spending, user)
                                                                                      .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }));

            return model;
        }


        /// <summary>
        /// Fill dictionaries for search settings
        /// </summary>
        /// <param name="model"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public SearchOptionsViewModel FillDictionariesSearch(SearchOptionsViewModel model, IPrincipal user)
        {
            model.Currencies = new List<SelectListItem> { new SelectListItem() };

            model.Categories = new List<SelectListItem> { new SelectListItem() };

            model.Currencies.AddRange(_currencyService.GetCurrencies()
                                            .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }));

            model.Categories.AddRange(_categoryService.GetAllCategories(user)
                                             .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }));

            return model;
        }

        /// <summary>
        /// Get amount in BYN
        /// </summary>
        /// <param name="currencyId"></param>
        /// <param name="amount">amount of operation</param>
        /// <param name="date">date of operation</param>
        /// <returns></returns>
        private decimal GetAmountInBYN(int currencyId, decimal amount, DateTime date)
        {
            try
            {
                var currancyName = Database.Currencies.Get(currencyId).Name;
                var formattingDate = date.ToString("yyyy-MM-dd");

                if (currancyName == "BYN")
                    return amount;

                using (var http = new HttpClient())
                {
                    var result = http.GetAsync(new Uri(_baseUrl + currancyName + "?onDate=" + formattingDate + "&ParamMode=2")).Result;
                    result.EnsureSuccessStatusCode();

                    var rate = result.Content.ReadAsAsync<RateDTO>().Result;

                    return amount * rate.Cur_OfficialRate.Value / rate.Cur_Scale;
                }
            }
            catch(Exception e)
            {
                throw new Exception(String.Format("При пересчете суммы возникла ошибка: {0}", e.InnerException != null ? e.Message + " " + e.InnerException : e.Message));
            }
            
        }
    }
}
