using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TransportationService.Utility;
using MongoDB.Bson;
using TransportationService.Models;

namespace TransportationService.Controllers
{
   public class ViewInformationController : TransportationBaseController
   {

      [HttpPost]
      public ActionResult ViewBus(string id)
      {
         DatabaseInterface db = new DatabaseInterface();
         Bus bus = db.GetBusById(new ObjectId(id));
         if (bus == null)
         {
            return Json(new { error = "true" });
         }
         return PartialView("ViewBusView", new ViewBusModel()
         {
            LicensePlate = bus.LicensePlate,
            State = bus.State,
            Capacity = bus.Capacity,
            Status = bus.Status == BusStatus.Active ? "Active" : "Inactive",
            BusId = bus.BusId,
            RouteName = bus.AssignedTo == -1 ? "" : db.GetRouteByRouteId(bus.AssignedTo).Name
         });
      }

      [HttpPost]
      public ActionResult ViewRoute(string id)
      {
         DatabaseInterface db = new DatabaseInterface();
         ObjectId newId = new ObjectId(id);
         Route route = db.GetRouteById(newId);
         if (route == null)
         {
            return Json(new { error = "true" });
         }
         return Json(new
         {
            html = RenderPartialViewToString("ViewRouteView", new ViewRouteModel()
               {
                  Stops = route.Stops,
                  DriverName = route.Driver.Name,
                  Name = route.Name,
                  LicensePlate = route.Bus.LicensePlate
               })
         });
      }
   }
}
