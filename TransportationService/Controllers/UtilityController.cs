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
      public ActionResult GetConfirmationHTML(){
         return PartialView("Confirmation");
      }
   }
}
