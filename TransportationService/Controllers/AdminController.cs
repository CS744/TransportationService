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
        public ActionResult AddBus()
        {
            return PartialView("AddBus");
        }
        public ActionResult AddNewBus(int capacity, string license)
        {
            DatabaseInterface db = new DatabaseInterface();
            //if (!db.IsStopLocationUnique(location))
            //    return Json("false");

            Bus bus = new Bus()
            {
                Id = ObjectId.GenerateNewId(),
                LiscensePlate = license,
                BusId = db.GetNextBusId(),
                Status = BusStatus.Active,
                Capacity = capacity,
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
        public ActionResult AddNewRoute(List<int> stopIds, string driverName, string routeName, int BusId)
        {
            DatabaseInterface db = new DatabaseInterface();
            if (!db.IsRouteNameUnique(routeName))
                return Json("false");
            List<Stop> stops = new List<Stop>();
            foreach (int id in stopIds)
            {
                stops.Add(db.GetStopByStopId(id));
            }
            List<Route> routes = new List<Route>();
            routes = db.GetAvailableRoutes();
            Route route = new Route()
            {
                Stops = stops,
                DriverName = driverName,
                Name = routeName,
                RouteId = db.GetNextRouteId(),
                Id = ObjectId.GenerateNewId(),
                Bus = db.GetBusByBusId(BusId)
            };
            db.SaveRoute(route);
            return Json("true");
        }
    }
}
