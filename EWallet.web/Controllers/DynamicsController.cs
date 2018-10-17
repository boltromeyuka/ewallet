using EWallet.bl;
using EWallet.viewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EWallet.web.Controllers
{
    [Authorize]
    public class DynamicsController : Controller
    {
        private IDynamicsService _dynamicsService;

        public DynamicsController(IDynamicsService dynamicsService)
        {
            _dynamicsService = dynamicsService;
        }

        // GET: Dynamics
        public ActionResult Index()
        {
            return View(_dynamicsService.FillDictionaries(new DynamicsViewModel()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChartData(DynamicsViewModel model)
        {
            if (model.ByCategories)
            {
                return Json(_dynamicsService.GetChartDataByCategories(model, User), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(_dynamicsService.GetChartDataByTypes(model, User), JsonRequestBehavior.AllowGet);
            }            
        }
    }
}