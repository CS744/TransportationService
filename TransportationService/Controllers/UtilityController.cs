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
      public ActionResult IsValidDeleteStop(string id)
      {
         return Json(new { isValid = "true" });
      }
      public ActionResult IsValidDeleteBus(string id)
      {
         DatabaseInterface db = new DatabaseInterface();
         Bus bus = db.GetBusById(new ObjectId(id));
         if (bus.AssignedTo < 0)
         {
            return Json(new { isValid = "true" });
         }
         return Json(new {/* isValid = undefined */ message = "The bus is actively involved with a route.  Unassign the bus before deleting." });
      }
      public ActionResult IsValidDeleteDriver(string id)
      {
         DatabaseInterface db = new DatabaseInterface();
         Driver driver = db.GetDriverByobjId(new ObjectId(id));
         if (driver.AssignedTo < 0)
         {
            return Json(new { isValid = "true" });
         }
         return Json(new {/* isValid = undefined */ message = "The driver is actively involved with a route.  Unassign the driver before deleting." });
      }
      public ActionResult IsValidDeleteEmployee(string id)
      {
         return Json(new { isValid = "true" });
      }
   }
}
