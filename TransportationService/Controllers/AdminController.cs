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
   public class AdminController : TransportationBaseController
   {
      public Random ran = new Random();
      public ActionResult AddRoute()
      {
         DatabaseInterface db = new DatabaseInterface();
         AddRouteModel model = new AddRouteModel()
         {
            AvailableBuses = db.GetAvailableBuses(),
            AvailableStops = db.GetAvailableStops()
         };
         return PartialView("AddRoute", model);
      }
      public ActionResult AddStop()
      {
         return PartialView("AddStop");
      }
      public ActionResult AddNewStop(string location)
      {
         Stop stop = new Stop()
         {
            Id = ObjectId.GenerateNewId(),
            Location = location,
            StopId = ran.Next(1000)
         };
         DatabaseInterface db = new DatabaseInterface();
         db.SaveStop(stop);
         return Json(new{});
      }
      public ActionResult AddNewRoute(List<int> stopIds, string driverName, string routeName)
      {
         DatabaseInterface db = new DatabaseInterface();
         List<Stop> stops = new List<Stop>();
         foreach(int id in stopIds){
            stops.Add(db.GetStopByStopId(id));
         }
         Route route = new Route()
         {
            Stops = stops,
            DriverName = driverName,
            Name = routeName,
            Id = ObjectId.GenerateNewId()
         };
         db.SaveRoute(route);
         return Json(new { });
      }
   }
}
