using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TransportationService.Utility;
using MongoDB.Bson;

namespace TransportationService.Controllers
{
   public class HomeController : Controller
   {
      //
      // GET: /Home/

      public ActionResult Index()
      {
         Bus bus = new Bus()
         {
            Capacity = 5,
            Id = ObjectId.GenerateNewId(),
            LiscensePlate = "ABC 123",
            Name = "Best Bus Ever"
         };
         DatabaseInterface db = new DatabaseInterface();
         db.SaveBus(bus);

         return View(bus);
      }

   }
}
