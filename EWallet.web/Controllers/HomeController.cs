using EWallet.bl;
using EWallet.viewModels;
using System;
using System.Web.Mvc;

namespace EWallet.web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private IOperationService _operationService;
        private ICurrencyService _currencyService;
        private ICategoryService _categoryService;

        public HomeController(IOperationService operationService, ICurrencyService currencyService,
                              ICategoryService categoryService)
        {
            _operationService = operationService;
            _categoryService = categoryService;
            _currencyService = currencyService;
        }

        public ActionResult Index()
        {
            return View(_operationService.GetOperations(User));
        }

        public ActionResult DataTable(SortOptionsViewModel sort)
        {
            return View(_operationService.GetSortingData(sort, User));
        }

        [HttpGet]
        public ActionResult Search()
        {
            return View(_operationService.FillDictionariesSearch(new SearchOptionsViewModel(), User));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(SearchOptionsViewModel model)
        {
            return View("DataTable", _operationService.GetSearchingData(model, User));
        }

        [HttpGet]
        public ActionResult Create(int type)
        {
            return View(_operationService.FillDictionaries(type, new OperationViewModel(), User));
        }        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OperationViewModel create)
        {
            if (ModelState.IsValid)
            {
                _operationService.CreateOpeation(create, User);

                return RedirectToAction("Index");
            }            

            return View(create);
        }        
    }
}