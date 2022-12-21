using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Unleash;

namespace FeatureManagementDemos.Web.Framework.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnleash unleash;

        public HomeController()
        {
            unleash = MvcApplication.Unleash;
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Unleash";

            ViewBag.Feature = "Demo123";
            ViewBag.FeatureEnabled = unleash.IsEnabled(ViewBag.Feature);

            return View();
        }

        [HttpGet]
        public JsonResult GetValue()
        {
            return Json(new { message = unleash.IsEnabled("Demo123")}, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult getPhoneNumber()
        {
            if(unleash.IsEnabled("AppSettings"))
                return Json(new { message = unleash.GetVariant("AppSettings").Payload.Value }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { message = "Not Available" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult getPhoneNumberFromStrategy()
        {
            if (unleash.IsEnabled("AppSettings"))
                return Json(new { message = unleash.FeatureToggles.Where(x => x.Name == "AppSettings").SelectMany(x => x.Strategies.Where(y => y.Name == "appSetting_PhoneNumber")).FirstOrDefault().Parameters.FirstOrDefault(z => z.Key == "PhoneNumbers").IfNotNull(x => x.Value) }, JsonRequestBehavior.AllowGet); //error prone
            else
                return Json(new { message = "Not Available" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}