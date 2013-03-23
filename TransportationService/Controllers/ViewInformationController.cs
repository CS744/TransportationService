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
                  LicensePlate = route.Bus.LicensePlate,
                  RouteId = route.RouteId.ToString()
               })
         });
      }


      [HttpPost]
      public ActionResult ViewStop(string id)
      {
         DatabaseInterface db = new DatabaseInterface();
         ObjectId newId = new ObjectId(id);
         Stop stop = db.GetStop(newId);
         if (stop == null)
         {
            return Json(new { error = "true" });
         }
         return Json(new
         {
            html = RenderPartialViewToString("ViewStopView", new ViewStopModel()
            {
               StopLocation = stop.Location,
               StopId = stop.StopId,
            })
         });
      }

      [HttpPost]
      public ActionResult ViewDriver(string id)
      {
         DatabaseInterface db = new DatabaseInterface();
         ObjectId newId = new ObjectId(id);
         Driver driver = db.GetDriverById(newId);
         if (driver == null)
         {
            return Json(new { error = "true" });
         }
         return Json(new
         {
            html = RenderPartialViewToString("ViewDriverView", new ViewDriverModel()
            {
               State = driver.State,
               License = driver.DriverLicense,
               Name = driver.Name,
               RouteName = driver.AssignedTo == -1 ? "" : db.GetRouteByRouteId(driver.AssignedTo).Name
            })
         });
      }
   }
}
