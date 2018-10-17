using System;
using System.Collections.Generic;
using System.Linq;
using EWallet.viewModels;
using EWallet.data;
using System.Web.Mvc;
using System.Security.Principal;
using Microsoft.AspNet.Identity;
using System.Net.Http;
using System.Web.Configuration;

namespace EWallet.bl
{
    /// <summary>
    /// Service for get chart data and other
    /// </summary>
    public class DynamicsService : IDynamicsService
    {
        private string _baseUrl = WebConfigurationManager.AppSettings["NBRB"];

        IUnitOfWork Database { get; set; }

        public DynamicsService(IUnitOfWork uow)
        {
            Database = uow;
        }


        /// <summary>
        /// Fill dictionaries for settings of chart
        /// </summary>
        /// <param name="model">model with settings</param>
        /// <returns></returns>
        public DynamicsViewModel FillDictionaries(DynamicsViewModel model)
        {
            model.DateFrom = DateTime.Now.AddDays(-30);
            model.DateTo = DateTime.Now;
            model.CurrenciesList = Database.Currencies.GetAll()
                                    .Select(x => new SelectListItem { Value = x.Id.ToString(),
                                                                      Text = x.Name });

            return model;          
        }

        /// <summary>
        /// Get data for chart by operations types
        /// </summary>
        /// <param name="model">model with settings</param>
        /// <param name="user">user</param>
        /// <returns></returns>
        public IEnumerable<IEnumerable<object>> GetChartDataByTypes(DynamicsViewModel model, IPrincipal user)
        {
            var currancyName = Database.Currencies.Get(model.CurrencyId).Name;

            var operations = Database.Operations.Find(x => x.UserId == user.Identity.GetUserId() &&
                                                                            model.DateFrom.Date <= x.CreateDate.Date &&
                                                                                                    x.CreateDate.Date <= model.DateTo.Date).ToList(); //operations by date range

            var incomeOperations = operations.Where(x => x.OperationType == OperationType.Income)
                                                                        .GroupBy(x => x.CreateDate.Date)
                                                                                        .Select(g => new {
                                                                                            Date = g.First().CreateDate.Date,
                                                                                            Sum = GetAmountInCurrency(currancyName, g.Sum(c => c.AmountInBYN), g.First().CreateDate) //chart data of income operations
                                                                                        });

            var spendingOperations = operations.Where(x => x.OperationType == OperationType.Spending)
                                                                        .GroupBy(x => x.CreateDate.Date)
                                                                                        .Select(g => new {
                                                                                            Date = g.First().CreateDate.Date,
                                                                                            Sum = GetAmountInCurrency(currancyName, g.Sum(c => c.AmountInBYN), g.First().CreateDate) //data of spending operations
                                                                                        });
            var data = new List<IEnumerable<object>>();
            data.Add(new List<object> { "", "Доходы", "Расходы" });             //adding legend info

            for (var i = model.DateFrom.Date; i<=model.DateTo.Date; i = i.AddDays(1))           //date range
            {
                var currIncome = incomeOperations.FirstOrDefault(x => x.Date == i)?.Sum ?? 0;           //if has't operation in date - sum=0
                var currSpending = spendingOperations.FirstOrDefault(x => x.Date == i)?.Sum ?? 0;       // 

                data.Add(new List<object> { i.ToShortDateString(), currIncome, currSpending });             
            }

            return data;
        }

        /// <summary>
        /// Get chart data by categories
        /// </summary>
        /// <param name="model">model with settins</param>
        /// <param name="user">user</param>
        /// <returns></returns>
        public IEnumerable<IEnumerable<object>> GetChartDataByCategories(DynamicsViewModel model, IPrincipal user)
        {
            var currancyName = Database.Currencies.Get(model.CurrencyId).Name;

            var operations = Database.Operations.Find(x => x.UserId == user.Identity.GetUserId()).ToList();

            var categories = Database.Categories.Find(x=> x.UserId == user.Identity.GetUserId()).ToList();

            var data = new List<IEnumerable<object>>();

            var groups = operations.GroupBy(x => x.CreateDate, (date, items) => new
            {
                Date = date,
                Items = items.GroupBy(x => x.CategoryId)
                                                        .Select(g => new
                                                        {
                                                            CategoryId = g.First().CategoryId,
                                                            Date = g.First().CreateDate.Date,
                                                            Sum = GetAmountInCurrency(currancyName, g.Sum(c => c.AmountInBYN), g.First().CreateDate)
                                                        })
            });

            var legendItem = new List<object> { "" };           
            categories.ForEach(x => legendItem.Add(x.Name));
            data.Add(legendItem);

            for (var i = model.DateFrom.Date; i <= model.DateTo.Date; i = i.AddDays(1))
            {
                var item = new List<object> { i.ToShortDateString() };

                var group = groups.FirstOrDefault(x => x.Date.Date == i);

                foreach(var cat in categories)
                {
                    if (group == null)
                    {
                        item.Add(0);
                    }                   
                    else
                    {
                        item.Add(group.Items.FirstOrDefault(x => x.CategoryId == cat.Id)?.Sum ?? 0);
                    }                    
                }
                data.Add(item);
            }               

            return data;
        }

        /// <summary>
        /// Get amount in select currency
        /// </summary>
        /// <param name="currencyId"></param>
        /// <param name="amount">amount of operation</param>
        /// <param name="date">date of operation</param>
        /// <returns></returns>
        private decimal GetAmountInCurrency(string currancyName, decimal amountInBYN, DateTime date)
        {
            try
            {                
                var formattingDate = date.ToString("yyyy-MM-dd");

                if (currancyName == "BYN" || amountInBYN == 0)
                    return amountInBYN;

                using (var http = new HttpClient())
                {
                    var result = http.GetAsync(new Uri(_baseUrl + currancyName + "?onDate=" + formattingDate + "&ParamMode=2")).Result;
                    result.EnsureSuccessStatusCode();

                    var rate = result.Content.ReadAsAsync<RateDTO>().Result;

                    return amountInBYN * rate.Cur_Scale / rate.Cur_OfficialRate.Value;
                }
            }
            catch (Exception e)
            {
                throw new Exception(String.Format("При пересчете суммы возникла ошибка: {0}", e.InnerException != null ? e.Message + " " + e.InnerException : e.Message));
            }
            
        }
    }
}
