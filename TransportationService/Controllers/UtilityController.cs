using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TransportationService.Utility;
using MongoDB.Bson;

namespace TransportationService.Controllers
{
   public class UtilityController : TransportationBaseController
   {
      //
      // GET: /Home/

      public ActionResult Index()
      {
         return View();
      }

      public ActionResult GetRegisterModal()
      {
         return View();
      }
      public ActionResult GetConfirmationHTML()
      {
         return PartialView("Confirmation");
      }
      public ActionResult IsValidDeleteRoute(string id)
      {
         return Json(new { isValid = "true" });
      }
      public ActionResult IsValidDeleteBus(string id)
      {
          return Json(new { isValid = "true" });
      }
      public ActionResult IsValidDeleteStop(string id)
      {
          return Json(new { isValid = "true" });         
      }
      public ActionResult IsValidDeleteDriver(string id)
      {
          return Json(new { isValid = "true" });
      }
      public ActionResult IsValidDeleteEmployee(string id)
      {
         return Json(new { isValid = "true" });
      }
   }
}
