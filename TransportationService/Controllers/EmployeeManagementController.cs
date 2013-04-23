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

         ManagementModel model = new ManagementModel()
         {
            Buses = db.GetBusesAssignedToRoute(route.RouteId),
            Employees = db.GetEmployeesAssignedToRoute(route.RouteId),
            Stops = route.Stops
         };


         return Json(new
         {
            html = RenderPartialViewToString("RouteView", model),
            hour = route.RouteId < 500 ? 6 : 15
         });
      }
      public ActionResult GetRouteInformation(string routeId)
      {
         DatabaseInterface db = new DatabaseInterface();
         ObjectId rId = new ObjectId(routeId);
         IEnumerable<EmployeeInstance> instances = db.GetEmployeeInstanceByRoute(rId);
         ViewInstanceModel model = new ViewInstanceModel()
         {
            Route = db.GetRouteById(rId),
            Rows = new List<InstanceRow>()
         };
         instances.Aggregate<EmployeeInstance, ViewInstanceModel>(model, (vi, instance) =>
         {
            instance.Date = TimeZoneInfo.ConvertTimeFromUtc(instance.Date, TimeZoneInfo.Local);
            InstanceRow row = vi.Rows.FirstOrDefault(r => r.Bus.Id == instance.BusId && r.Date.Date == instance.Date.Date);
            if (row == null)
            {
               row = new InstanceRow()
               {
                  Bus = db.GetBusById(instance.BusId),
                  Date = instance.Date.Date,
                  Employees = new List<EmployeeCell>()
               };
               model.Rows.Add(row);
            }
            row.Employees.Add(new EmployeeCell()
            {
               Employee = db.GetEmployeeById(instance.EmployeeId),
               Stop = db.GetStop(instance.StopId),
               Date = instance.Date
            });
            return model;
         });

         return PartialView("RouteInformation", model);
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
