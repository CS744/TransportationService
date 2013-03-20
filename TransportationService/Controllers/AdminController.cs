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
               
        #region "state declarations"
        List<String> stateNames = new List<string>()
        {
            "Alaska",
                 "Alabama",
                 "Arkansas",
                 "Arizona",
                 "California",
                 "Colorado",
                 "Connecticut",
                 "District of Columbia",
                 "Delaware",
                 "Florida",
                 "Georgia",
                 "Hawaii",
                 "Iowa",
                 "Idaho",
                 "Illinois",
                 "Indiana",
                 "Kansas",
                 "Kentucky",
                 "Louisiana",
                 "Massachusetts",
                 "Maryland",
                 "Maine",
                 "Michigan",
                 "Minnesota",
                 "Missouri",
                 "Mississippi",
                 "Montana",
                 "North Carolina",
                 "North Dakota",
                 "Nebraska",
                 "New Hampshire",
                 "New Jersey",
                 "New Mexico",
                 "Nevada",
                 "New York",
                 "Ohio",
                 "Oklahoma",
                 "Oregon",
                 "Pennsylvania",
                 "Rhode Island",
                 "South Carolina",
                 "South Dakota",
                 "Tennessee",
                 "Texas",
                 "Utah",
                 "Virginia",
                 "Vermont",
                 "Washington",
                 "Wisconsin",
                 "West Virginia",
                 "Wyoming"
        };
        List<String> stateAbbreviations = new List<String>() {
            "AK",
                 "AL",
                 "AR",
                 "AZ",
                 "CA",
                 "CO",
                 "CT",
                 "DC",
                 "DE",
                 "FL",
                 "GA",
                 "HI",
                 "IA",
                 "ID",
                 "IL",
                 "IN",
                 "KS",
                 "KY",
                 "LA",
                 "MA",
                 "MD",
                 "ME",
                 "MI",
                 "MN",
                 "MO",
                 "MS",
                 "MT",
                 "NC",
                 "ND",
                 "NE",
                 "NH",
                 "NJ",
                 "NM",
                 "NV",
                 "NY",
                 "OH",
                 "OK",
                 "OR",
                 "PA",
                 "RI",
                 "SC",
                 "SD",
                 "TN",
                 "TX",
                 "UT",
                 "VA",
                 "VT",
                 "WA",
                 "WI",
                 "WV",
                 "WY"
        };
#endregion

        [HttpGet]
        public ActionResult LoadView()
        {
            var user = sessionManager.User;
            if (user == null)
            {
                return PartialView("Login");
            }
            DatabaseInterface db = new DatabaseInterface();
            var model = new OutputViewModel()
            {
                Username = user.Username,
                Routes = db.GetAvailableRoutes(),
                Buses = db.GetAvailableBuses(),
                Employees = db.GetAvailableEmployees(),
                Stops = db.GetAvailableStops(),
                Drivers = db.GetAvailableDrivers()
            };
            return PartialView("AdminView", model);
        }

        public ActionResult AddRoute()
        {
            DatabaseInterface db = new DatabaseInterface();
            AddRouteModel model = new AddRouteModel()
            {
                AvailableBuses = db.GetAvailableBuses(),
                AvailableStops = db.GetAvailableStops(),
                AvailableDrivers = db.GetAvailableDrivers(),
                Bus = null,
                Driver = null,
                Name = "",
                RouteId = "",
                Stops = { },
                UpdatingRoute = false
            };
            return PartialView("AddRoute", model);
        }

        public ActionResult ModifyRoute(String routeId)
        {
            DatabaseInterface db = new DatabaseInterface();
            Route route = db.GetRouteByRouteId(int.Parse(routeId));
            AddRouteModel model = new AddRouteModel()
            {
                AvailableBuses = db.GetAvailableBuses(),
                AvailableStops = db.GetAvailableStops(),
                AvailableDrivers = db.GetAvailableDrivers(),
                Bus = route.Bus,
                Driver = route.Driver,
                Name = route.Name,
                RouteId = routeId,
                Stops = route.Stops,
                UpdatingRoute = true
            };
            return PartialView("AddRoute", model);
        }

        public ActionResult AddStop()
        {
            return PartialView("AddStop");
        }

        public ActionResult AddBus()
        {
            AddBusModel model = new AddBusModel
            {
                StateNames = stateNames,
                StateAbbreviations = stateAbbreviations,
                Capacity = "",
                License = "",
                UpdatingBus = false,
                BusId = ""
            };
            return PartialView("AddBus", model);
        }

        public ActionResult AddNewBus(int capacity, string license, string state)
        {
            DatabaseInterface db = new DatabaseInterface();
            if (!db.IsLicenseUnique(license))
                return Json("false");

            Bus bus = new Bus()
            {
                Id = ObjectId.GenerateNewId(),
                LicensePlate = license,
                BusId = db.GetNextBusId(),
                Status = BusStatus.Active,
                Capacity = capacity,
                State = state,
                AssignedTo = -1

            };
            db.AddBus(bus);
            return Json(new{
                success = "true",
                id = bus.Id.ToString()
            });

        }

        public ActionResult ModifyBus(int busId)
        {
            DatabaseInterface db = new DatabaseInterface();
            Bus bus = db.GetBusByBusId(busId);
            AddBusModel model = new AddBusModel
            {
                StateNames = stateNames,
                StateAbbreviations = stateAbbreviations,
                Capacity = bus.Capacity.ToString(),
                License = bus.LicensePlate,
                UpdatingBus = true,
                Status = bus.Status.ToString(),
                State = bus.State,
                BusId = busId.ToString()
            };
            return PartialView("AddBus", model);
        }

        public ActionResult UpdateBus(string busId, int capacity, string license, string state, string status)
        {
            DatabaseInterface db = new DatabaseInterface();
            if (!db.IsLicenseUnique(license, busId))
                return Json("false");
            Bus bus = db.GetBusByBusId(int.Parse(busId));
            bus.LicensePlate = license;
            bus.BusId = int.Parse(busId);
            BusStatus busStatus;
            if (status.Equals("0"))
            {
                busStatus = BusStatus.Active;
            }
            else
            {
                busStatus = BusStatus.Inactive;
            }
            bus.Status = busStatus;
            bus.Capacity = capacity;
            bus.State = state;
            db.UpdateBus(bus);
            return Json("true");
        }

        public ActionResult AddDriver()
        {
            AddDriverModel model = new AddDriverModel()
           {
               StateNames = stateNames,
               StateAbbreviations = stateAbbreviations
           };
            return PartialView("AddDriver", model);
        }

        public ActionResult AddEmployee()
        {
            DatabaseInterface db = new DatabaseInterface();
            AddEmployeeModel model = new AddEmployeeModel()
            {
                AvailableRoutes = db.GetAvailableRoutes(),
                StateNames = stateNames,
                StateAbbreviations = stateAbbreviations
            };
            return PartialView("AddEmployee", model);
        }

        public ActionResult AddNewDriver(string gender, string state, string name, string license)
        {
            DatabaseInterface db = new DatabaseInterface();
            if (!db.IsDriverLicenseUnique(license))
                return Json("false");

            Driver driver = new Driver()
            {
                Id = ObjectId.GenerateNewId(),
                DriverLicense = license,
                Name = name,
                AssignedTo = -1,
                Gender = gender,
                State = state

            };
            db.SaveDriver(driver);
            return Json("true");
        }

        public ActionResult AddNewEmployee(bool isMale, string email, string phone, string address, string city, string state, int routeId, long ssn, string position, string name)
        {
            DatabaseInterface db = new DatabaseInterface();
            if (!db.IsSocialSecurityNumberUnique(ssn))
                return Json("false");

            Employee employee = new Employee()
            {
                Id = ObjectId.GenerateNewId(),
                SocialSecurityNumber = ssn,
                Position = position,
                Name = name,
                IsMale = isMale,
                Email = email,
                Phone = phone,
                Address = address,
                City = city,
                State = state,
                route = db.GetRouteByRouteId(routeId),
                EmployeeId = db.GetNextEmployeeId()

            };
            db.SaveEmployee(employee);
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

        public ActionResult AddNewRoute(List<int> stopIds, string routeName, int busId, bool startsAtWork, string driverLicense)
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
            db.AssignDriverToRoute(driverLicense, routeId);
            Route route = new Route()
            {
                Stops = stops,
                Driver = db.GetDriverByDriverLicense(driverLicense),
                Name = routeName,
                RouteId = routeId,
                Id = ObjectId.GenerateNewId(),
                Bus = db.GetBusByBusId(busId)
            };
            db.AddRoute(route);
            return Json(new
            {
               success = "true",
               id = route.Id.ToString()
            });

        }

        public ActionResult UpdateRoute(int routeId, List<int> stopIds, string routeName, int busId, string driverLicense)
        {
            DatabaseInterface db = new DatabaseInterface();
            String sRouteId = routeId.ToString();
            if (!db.IsRouteNameUnique(routeName, sRouteId))
                return Json("false");
            List<Stop> stops = new List<Stop>();
            foreach (int id in stopIds)
            {
                stops.Add(db.GetStopByStopId(id));
            }
            db.AssignBusToRoute(busId, routeId);
            db.AssignDriverToRoute(driverLicense, routeId);
            Route route = new Route()
            {
                Stops = stops,
                Driver = db.GetDriverByDriverLicense(driverLicense),
                Name = routeName,
                RouteId = routeId,
                Bus = db.GetBusByBusId(busId)
            };
            db.UpdateRoute(route);
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
