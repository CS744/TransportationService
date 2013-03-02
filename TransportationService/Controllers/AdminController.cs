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
      [HttpGet]
      public ActionResult LoadView()
      {
         var user = sessionManager.User;
         if (user == null)
         {
            return PartialView("Login");
         }
         var model = new OutputViewModel()
         {
            Username = user.Username
         };
         return PartialView("AdminView", model);
      }
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
      public ActionResult AddBus()
      {
         return PartialView("AddBus");
      }
      public ActionResult AddNewBus(int capacity, string license, string state)
      {
         DatabaseInterface db = new DatabaseInterface();
         if (!db.IsLicenseUnique(license))
            return Json("false");

         Bus bus = new Bus()
         {
            Id = ObjectId.GenerateNewId(),
            LiscensePlate = license,
            BusId = db.GetNextBusId(),
            Status = BusStatus.Active,
            Capacity = capacity,
            State = state,
            AssignedTo = -1

         };
         db.SaveBus(bus);
         return Json("true");
      }
      public ActionResult AddNewStop(string location)
      {
         DatabaseInterface db = new DatabaseInterface();
         if (!db.IsStopLocationUnique(location))
            return Json("false");

         Stop stop = new Stop()
         {
            Id = ObjectId.GenerateNewId(),
            Location = location,
            StopId = db.GetNextStopId()
         };
         db.SaveStop(stop);
         return Json("true");
      }
      public ActionResult AddNewRoute(List<int> stopIds, string driverName, string routeName, int busId, bool startsAtWork)
      {
         DatabaseInterface db = new DatabaseInterface();
         if (!db.IsRouteNameUnique(routeName))
            return Json("false");
         int routeId;
         if (startsAtWork)
             routeId = db.GetNextLowRouteId();
         else
             routeId = db.GetNextHighRouteId();
         List<Stop> stops = new List<Stop>();
         foreach (int id in stopIds)
         {
            stops.Add(db.GetStopByStopId(id));
         }
         List<Route> routes = new List<Route>();
         routes = db.GetAvailableRoutes();
         db.AssignBusToRoute(busId, routeId);
         Route route = new Route()
         {
            Stops = stops,
            DriverName = driverName,
            Name = routeName,
            RouteId = routeId,
            Id = ObjectId.GenerateNewId(),
            Bus = db.GetBusByBusId(busId)
         };
         db.SaveRoute(route);
         return Json("true");
      }
      public ActionResult RefreshAdmin()
      {
         User user = sessionManager.User;
         if (user != null)
         {
            return Json(new
            {
               user = JsonUtility.ToUserJson(user),
               headerText = "Welcome, " + user.Username
            });
         }
         return Json(new { error = true });
      }
   }
}
