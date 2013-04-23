using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TransportationService.Utility;
using MongoDB.Bson;
using TransportationService.Models;
using Newtonsoft.Json;

namespace TransportationService.Controllers
{

   public class EmployeeManagementController : TransportationBaseController
   {
      public ActionResult Index()
      {
         DatabaseInterface db = new DatabaseInterface();
         return View(db.GetAvailableRoutes());
      }
      public ActionResult GetEmployeeInfo(string id)
      {
         DatabaseInterface db = new DatabaseInterface();
         Route route = db.GetRouteById(new ObjectId(id));
         bool isToWork = route.RouteId < 500;
         ManagementModel model = new ManagementModel()
         {
            Buses = db.GetBusesAssignedToRoute(route.RouteId),
            Employees = db.GetEmployeesAssignedToRoute(route.RouteId),
            Stops = route.Stops
         };


         return Json(new
         {
            html = RenderPartialViewToString("RouteView", model),
            hour = isToWork ? 15 : 6
         });
      }

      public ActionResult RecordInstance(string employees, string routeId)
      {
         Dictionary<string, Dictionary<string, object>> employeeDict = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(employees);
         DatabaseInterface db = new DatabaseInterface();
         foreach (string employeeId in employeeDict.Keys)
         {
            Dictionary<string, object> inner = employeeDict[employeeId];
            EmployeeInstance instance = new EmployeeInstance()
            {
               Id = ObjectId.GenerateNewId(),
               RouteId = new ObjectId(routeId),
               EmployeeId = new ObjectId(employeeId),
               StopId = new ObjectId((string)inner["stop"]),
               BusId = new ObjectId((string)inner["bus"]),
               Date = (DateTime)inner["date"]
            };
            db.SaveEmployeeInstance(instance);
         }
         return null;
      }

   }
}
