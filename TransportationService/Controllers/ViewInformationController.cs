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
            MorningIsActive = bus.MorningIsActive,
            EveningIsActive = bus.EveningIsActive,
            BusId = bus.BusId,
            //RouteName = bus.AssignedTo == -1 ? "" : db.GetRouteByRouteId(bus.AssignedTo).Name, TODO this will need to handle morning and evening
            ObjectId = bus.Id.ToString()
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
                  Name = route.Name,
                  RouteId = route.RouteId.ToString(),
                  ObjectId = route.Id.ToString()
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
               ObjectId = stop.Id.ToString()
            })
         });
      }

      [HttpPost]
      public ActionResult ViewDriver(string driverId)
      {
         DatabaseInterface db = new DatabaseInterface();
         Driver driver = db.GetDriverById(driverId);
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
               DriverId = driver.DriverId,
               ObjectId = driver.Id.ToString()
            })
         });
      }

      public ActionResult ViewEmployee(int employeeId)
      {
         DatabaseInterface db = new DatabaseInterface();
         Employee employee = db.GetEmployeeById(employeeId);
         if (employee == null)
         {
            return Json(new { error = "true" });
         }
         return Json(new
         {
            html = RenderPartialViewToString("ViewEmployeeView", new ViewEmployeeModel()
            {
               Name = employee.Name,
               Gender = employee.IsMale ? "Male" : "Female",
               SSN = employee.SocialSecurityNumber,
               Position = employee.Position,
               Email = employee.Email,
               Number = employee.Phone,
               Address = employee.Address,
               City = employee.City,
               State = employee.State,
               MorningRouteName = employee.MorningAssignedTo == -1 ? "" : db.GetRouteByRouteId(employee.MorningAssignedTo).Name,
               EveningRouteName = employee.EveningAssignedTo == -1 ? "" : db.GetRouteByRouteId(employee.EveningAssignedTo).Name,
               Id = employee.EmployeeId.ToString(),
               Zip = employee.Zip,
               ObjectId = employee.Id.ToString()
            })
         });
      }
   }
}
