using EWallet.bl;
using EWallet.viewModels;
using System.Web.Mvc;

namespace EWallet.web.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private ICategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: Category
        public ActionResult Index(int type)
        {
            var result = _categoryService.GetCategories(type == 0 ? data.OperationType.Income : data.OperationType.Spending, User);
                                                                            
            ViewBag.Type = type;

            return View(result);
        }

        [HttpGet]
        public ActionResult Create(int type)
        {
            return View(new CategoryViewModel {
                                                CategoryType = type == 0 ? data.OperationType.Income : data.OperationType.Spending
                                            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoryViewModel create)
        {
            _categoryService.CreateCategory(create, User);

            return RedirectToAction("Index", new { type=(int)create.CategoryType});
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            return View(_categoryService.GetCategory(id, User));
        }

        [HttpPost]
        public ActionResult Edit(CategoryViewModel edit)
        {
            _categoryService.EditCategory(edit, User);

            return RedirectToAction("Index", new { type = (int)edit.CategoryType });
        }

        public ActionResult Delete(int id, int type)
        {
            var categoryType = type;
             
            _categoryService.DeleteCategory(id, User);

            return RedirectToAction("Index", new { type = categoryType });
        }
    }
}