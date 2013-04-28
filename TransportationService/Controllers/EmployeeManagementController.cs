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
            IEnumerable<EmployeeActivity> instances = db.GetEmployeeActivityByRoute(rId);
            ViewInstanceModel model = new ViewInstanceModel()
            {
                Route = db.GetRouteById(rId),
                Rows = new List<InstanceRow>()
            };
            instances.Aggregate<EmployeeActivity, ViewInstanceModel>(model, (vi, instance) =>
            {
                instance.Date = TimeZoneInfo.ConvertTimeFromUtc(instance.Date, TimeZoneInfo.Local);
                InstanceRow row = vi.Rows.FirstOrDefault(r => r.Bus.Id == instance.Bus.Id && r.Date.Date == instance.Date.Date);
                if (row == null)
                {
                    row = new InstanceRow()
                    {
                        Bus = instance.Bus,
                        Date = instance.Date.Date,
                        Employees = new List<EmployeeCell>()
                    };
                    model.Rows.Add(row);
                }
                row.Employees.Add(new EmployeeCell()
                {
                    Employee = instance.Employee,
                    Stop = instance.Stop,
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
            Route route = db.GetRouteById(new ObjectId(routeId));
            foreach (string employeeId in employeeDict.Keys)
            {
                Dictionary<string, object> inner = employeeDict[employeeId];
                Bus bus = db.GetBusById(new ObjectId((string)inner["bus"]));
                Driver driver = db.GetDriverById(route.DriverBusList.First(bd => bd.BusId == bus.BusId).DriverId);
                EmployeeActivity instance = new EmployeeActivity()
                {
                    Id = ObjectId.GenerateNewId(),
                    Route = route,
                    Employee = db.GetEmployeeById(new ObjectId(employeeId)),
                    Stop = db.GetStop(new ObjectId((string)inner["stop"])),
                    Bus = bus,
                    Driver = driver,
                    Date = (DateTime)inner["date"]
                };
                db.SaveEmployeeActivity(instance);
            }
            return null;
        }

    }
}
