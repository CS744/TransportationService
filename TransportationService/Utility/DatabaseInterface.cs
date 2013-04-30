using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace TransportationService.Utility
{
    public class DatabaseInterface
    {
        MongoDatabase _database;
        const string _usersCollectionName = "Users";
        const string _stopCollectionName = "Stops";
        const string _employeeCollectionName = "Employees";
        const string _busCollectionName = "Buses";
        const string _routeCollectionName = "Routes";
        const string _driverCollectionName = "Drivers";
        const string _seedCollectionName = "Seeds";
        const string _activityCollectionName = "EmployeeActivity";

        public DatabaseInterface()
        {
            string connectionString = "mongodb://localhost/TransportationService";
            _database = MongoDatabase.Create(connectionString);
        }

        public void CreateSeedTable()
        {
            var coll = _database.GetCollection(_seedCollectionName);
            coll.Save(new Seed()
            {
                RouteIdLow = 1,
                RouteIdHigh = 999,
                BusId = 1,
                DriverId = 1,
                StopId = 1,
                EmployeeId = 1,
                Id = ObjectId.GenerateNewId()
            });
        }

        public Seed GetSeed()
        {
            var coll = _database.GetCollection(_seedCollectionName);
            return coll.FindOneAs<Seed>();
        }

        public void SaveSeed(Seed seed)
        {
            var coll = _database.GetCollection(_seedCollectionName);
            coll.Save(seed);
        }

        #region USER
        public void SaveUser(User user)
        {
            var coll = _database.GetCollection(_usersCollectionName);
            coll.Save(user);
        }

        public User GetUser(string username, string password)
        {
            var coll = _database.GetCollection(_usersCollectionName);
            var query = Query.And(Query.EQ("Username", username), Query.EQ("Password", password));
            return coll.FindOneAs<User>(query);
        }

        public User GetUserById(ObjectId id)
        {
            var coll = _database.GetCollection(_usersCollectionName);
            var query = Query.EQ("_id", id);
            return coll.FindOneAs<User>(query);
        }
        #endregion


        #region ROUTE

        public void SaveRoute(Route route)
        {
            var coll = _database.GetCollection(_routeCollectionName);
            coll.Save(route);
        }

        public Route GetRouteByRouteId(int id)
        {
            var coll = _database.GetCollection(_routeCollectionName);
            var query = Query.And(Query.EQ("RouteId", id), Query.EQ("HasBeenDeleted", false));
            return coll.FindOneAs<Route>(query);
        }

        public Route GetRouteById(ObjectId id)
        {
            var coll = _database.GetCollection(_routeCollectionName);
            var query = Query.And(Query.EQ("_id", id), Query.EQ("HasBeenDeleted", false));
            return coll.FindOneAs<Route>(query);
        }

        public int GetNextLowRouteId()
        {
            Seed seed = GetSeed();
            int nextId = seed.RouteIdLow;
            seed.RouteIdLow = nextId + 1;
            SaveSeed(seed);
            return nextId;
        }

        public int GetNextHighRouteId()
        {
            Seed seed = GetSeed();
            int nextId = seed.RouteIdHigh;
            seed.RouteIdHigh = nextId - 1;
            SaveSeed(seed);
            return nextId;
        }

        public List<Route> GetAvailableRoutes()
        {
            var coll = _database.GetCollection(_routeCollectionName);
            var query = Query.EQ("HasBeenDeleted", false);
            return coll.FindAs<Route>(query).SetSortOrder(SortBy.Ascending("RouteId")).ToList();
        }

        public void AddRoute(Route route)
        {
            var coll = _database.GetCollection(_routeCollectionName);
            coll.Save(route);
        }

        public void UpdateRoute(Route route)
        {
            var coll = _database.GetCollection(_routeCollectionName);
            coll.Remove(Query.EQ("RouteId", route.RouteId));
            coll.Save(route);
        }

        public Boolean IsRouteNameUnique(string name, string routeId = "")
        {
            List<Route> routes = GetAvailableRoutes();
            foreach (Route r in routes)
            {
                if (routeId.Equals("") || !routeId.Equals(r.RouteId.ToString()))
                {
                    if (String.Equals(r.Name, name))
                        return false;
                }
            }
            return true;
        }

        public void RouteSetInactive(Route route)
        {
            route.IsActive = false;
        }

        public void DeleteRouteByObjId(ObjectId id)
        {
            var coll = _database.GetCollection(_routeCollectionName);
            var query = Query.EQ("_id", id);
            coll.Remove(query);
        }
        #endregion


        #region BUS

        public void AddBus(Bus bus)
        {
            var coll = _database.GetCollection(_busCollectionName);
            coll.Save(bus);
        }

        public void UpdateBus(Bus bus)
        {
            var coll = _database.GetCollection(_busCollectionName);
            coll.Remove(Query.EQ("BusId", bus.BusId));
            coll.Save(bus);
        }

        public Bus GetBusByBusId(int id)
        {
            var coll = _database.GetCollection(_busCollectionName);
            var query = Query.And(Query.EQ("BusId", id), Query.EQ("HasBeenDeleted", false));
            Bus bus = coll.FindOneAs<Bus>(query);
            if (bus == null)
            {
                bus = new Bus()
                {
                    BusId = -1
                };
            }
            return bus;
        }

        public Bus GetBusById(ObjectId id)
        {
            var coll = _database.GetCollection(_busCollectionName);
            var query = Query.And(Query.EQ("_id", id), Query.EQ("HasBeenDeleted", false));
            return coll.FindOneAs<Bus>(query);
        }

        public List<Bus> GetAvailableBuses()
        {
            var coll = _database.GetCollection(_busCollectionName);
            var query = Query.EQ("HasBeenDeleted", false);
            return coll.FindAs<Bus>(query).SetSortOrder(SortBy.Ascending("BusId")).ToList();
        }

        public Boolean IsLicenseUnique(string license, string busId = "")
        {
            List<Bus> buses = GetAvailableBuses();
            foreach (Bus b in buses)
            {
                if (busId.Equals("") || !busId.Equals(b.BusId.ToString()))
                {
                    if (license.Equals(b.LicensePlate))
                        return false;
                }
            }
            return true;
        }

        public int GetNextBusId()
        {
            Seed seed = GetSeed();
            int nextId = seed.BusId;
            seed.BusId = nextId + 1;
            SaveSeed(seed);
            return nextId;
        }

        public void DeleteBusByObjId(ObjectId id)
        {
            var coll = _database.GetCollection(_busCollectionName);
            var query = Query.EQ("_id", id);
            coll.Remove(query);
        }

        public void DeleteBusById(int bId)
        {
            var coll = _database.GetCollection(_busCollectionName);
            var query = Query.EQ("BusId", bId);
            coll.Remove(query);
        }

        public void BusSetActive(int busId, bool isActive, int routeId)
        {
            if (routeId < 500)
                _database.GetCollection(_busCollectionName).Update(Query.EQ("BusId", busId), Update.Set("MorningIsActive", isActive));
            else
                _database.GetCollection(_busCollectionName).Update(Query.EQ("BusId", busId), Update.Set("EveningIsActive", isActive));
        }

        #endregion


        #region STOP

        public void SaveStop(Stop stop)
        {
            var coll = _database.GetCollection(_stopCollectionName);
            coll.Save(stop);
        }

        public void UpdateStop(Stop stop)
        {
            var coll = _database.GetCollection(_stopCollectionName);
            coll.Remove(Query.EQ("StopId", stop.StopId));
            coll.Save(stop);
        }

        public Stop GetStop(ObjectId id)
        {
            var coll = _database.GetCollection(_stopCollectionName);
            var query = Query.And(Query.EQ("_id", id), Query.EQ("HasBeenDeleted", false));
            return coll.FindOneAs<Stop>(query);
        }

        public Stop GetStopByStopId(int id)
        {
            var coll = _database.GetCollection(_stopCollectionName);
            var query = Query.And(Query.EQ("StopId", id), Query.EQ("HasBeenDeleted", false));
            return coll.FindOneAs<Stop>(query);
        }

        public void DeleteStopByObjId(ObjectId id)
        {
            var coll = _database.GetCollection(_stopCollectionName);
            var query = Query.EQ("_id", id);
            coll.Remove(query);
        }

        public void DeleteStopById(int sId)
        {
            var coll = _database.GetCollection(_stopCollectionName);
            var query = Query.EQ("StopId", sId);
            coll.Remove(query);
        }

        public List<Stop> GetAvailableStops()
        {
            var coll = _database.GetCollection(_stopCollectionName);
            var query = Query.EQ("HasBeenDeleted", false);
            return coll.FindAs<Stop>(query).SetSortOrder(SortBy.Ascending("StopId")).ToList();
        }

        public int GetNextStopId()
        {
            Seed seed = GetSeed();
            int nextId = seed.StopId;
            seed.StopId = nextId + 1;
            SaveSeed(seed);
            return nextId;
        }

        public Boolean IsStopLocationUnique(string location)
        {
            List<Stop> stops = GetAvailableStops();
            foreach (Stop s in stops)
            {
                if (String.Equals(s.Location, location))
                    return false;
            }
            return true;
        }

        #endregion


        #region DRIVER

        public void SaveDriver(Driver driver)
        {
            var coll = _database.GetCollection(_driverCollectionName);
            coll.Save(driver);
        }

        public Driver GetDriverByDriverLicense(string driverLicense)
        {
            var coll = _database.GetCollection(_driverCollectionName);
            var query = Query.And(Query.EQ("DriverLicense", driverLicense), Query.EQ("HasBeenDeleted", false));
            return coll.FindOneAs<Driver>(query);
        }

        public Driver GetDriverById(int driverId)
        {
            var coll = _database.GetCollection(_driverCollectionName);
            var query = Query.And(Query.EQ("DriverId", driverId), Query.EQ("HasBeenDeleted", false));
            Driver driver = coll.FindOneAs<Driver>(query);
            if (driver == null)
            {
                driver = new Driver()
                {
                    DriverId = -1
                };
            }
            return driver;
        }

        public Driver GetDriverByobjId(ObjectId id)
        {
            var coll = _database.GetCollection(_driverCollectionName);
            var query = Query.And(Query.EQ("_id", id), Query.EQ("HasBeenDeleted", false));
            return coll.FindOneAs<Driver>(query);
        }

        public void DeleteDriverByObjId(ObjectId id)
        {
            var coll = _database.GetCollection(_driverCollectionName);
            var query = Query.EQ("_id", id);
            coll.Remove(query);
        }

        public void DeleteDriverById(string DId)
        {
            var coll = _database.GetCollection(_driverCollectionName);
            var query = Query.EQ("DriverId", DId);
            coll.Remove(query);
        }

        public List<Driver> GetAvailableDrivers()
        {
            var coll = _database.GetCollection(_driverCollectionName);
            var query = Query.EQ("HasBeenDeleted", false);
            return coll.FindAs<Driver>(query).SetSortOrder(SortBy.Ascending("DriverId")).ToList();
        }

        public Boolean IsDriverLicenseUnique(string license, string state, int driverId = -2)
        {
            List<Driver> drivers = GetAvailableDrivers();
            foreach (Driver d in drivers)
            {
                if (driverId == -2 || d.DriverId != driverId)
                {
                    if (license.Equals(d.DriverLicense) && state.Equals(d.State))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void UpdateDriver(Driver driver)
        {
            var coll = _database.GetCollection(_driverCollectionName);
            coll.Remove(Query.EQ("_id", driver.Id));
            coll.Save(driver);
        }

        public int GetNextDriverId()
        {
            Seed seed = GetSeed();
            int nextId = seed.DriverId;
            seed.DriverId = nextId + 1;
            SaveSeed(seed);
            return nextId;
        }

        public void DriverSetActive(int driverId, bool isActive, int routeId)
        {
            if (routeId < 500)
                _database.GetCollection(_driverCollectionName).Update(Query.EQ("DriverId", driverId), Update.Set("MorningIsActive", isActive));
            else
                _database.GetCollection(_driverCollectionName).Update(Query.EQ("DriverId", driverId), Update.Set("EveningIsActive", isActive));
        }

        #endregion


        #region EMPLOYEE

        public Employee GetEmployeeBySSN(long ssn)
        {
            var coll = _database.GetCollection(_employeeCollectionName);
            var query = Query.And(Query.EQ("SocialSecurityNumber", ssn), Query.EQ("HasBeenDeleted", false));
            return coll.FindOneAs<Employee>(query);
        }

        public Employee GetEmployeeById(int id)
        {
            var coll = _database.GetCollection(_employeeCollectionName);
            var query = Query.And(Query.EQ("EmployeeId", id), Query.EQ("HasBeenDeleted", false));
            return coll.FindOneAs<Employee>(query);
        }

        public Employee GetEmployeeById(ObjectId id)
        {
            var coll = _database.GetCollection(_employeeCollectionName);
            var query = Query.And(Query.EQ("_id", id), Query.EQ("HasBeenDeleted", false));
            return coll.FindOneAs<Employee>(query);
        }

        public void SaveEmployee(Employee employee)
        {
            var coll = _database.GetCollection(_employeeCollectionName);
            coll.Save(employee);
        }

        public List<Employee> GetAvailableEmployees()
        {
            var coll = _database.GetCollection(_employeeCollectionName);
            var query = Query.EQ("HasBeenDeleted", false);
            return coll.FindAs<Employee>(query).SetSortOrder(SortBy.Ascending("EmployeeId")).ToList();
        }

        public Boolean IsSocialSecurityNumberUnique(long ssn)
        {
            List<Employee> employees = GetAvailableEmployees();
            foreach (Employee e in employees)
            {
                if (String.Equals(e.SocialSecurityNumber, ssn))
                    return false;
            }
            return true;
        }

        public int GetNextEmployeeId()
        {
            Seed seed = GetSeed();
            int nextId = seed.EmployeeId;
            seed.EmployeeId = nextId + 1;
            SaveSeed(seed);
            return nextId;
        }

        public void UpdateEmployee(Employee e)
        {
            var coll = _database.GetCollection(_employeeCollectionName);
            coll.Remove(Query.EQ("EmployeeId", e.EmployeeId));
            coll.Save(e);
        }

        public void DeleteEmployeeByObjId(ObjectId id)
        {
            var coll = _database.GetCollection(_employeeCollectionName);
            var query = Query.EQ("_id", id);
            coll.Remove(query);
        }

        public void DeleteEmployeeById(int EId)
        {
            var coll = _database.GetCollection(_employeeCollectionName);
            var query = Query.EQ("EmployeeId", EId);
            coll.Remove(query);
        }

        #endregion


        public void AssignBusToRoute(int busId, int routeId)
        {
            if (routeId < 500)
                _database.GetCollection(_busCollectionName).Update(Query.EQ("BusId", busId), Update.Set("MorningAssignedTo", routeId));
            else
                _database.GetCollection(_busCollectionName).Update(Query.EQ("BusId", busId), Update.Set("EveningAssignedTo", routeId));
        }

        public void AssignDriverToRoute(int driverId, int routeId)
        {
            if (routeId < 500)
                _database.GetCollection(_driverCollectionName).Update(Query.EQ("DriverId", driverId), Update.Set("MorningAssignedTo", routeId));
            else
                _database.GetCollection(_driverCollectionName).Update(Query.EQ("DriverId", driverId), Update.Set("EveningAssignedTo", routeId));
        }

        public void UnassignBusesDriversFromRoute(int routeId)
        {
            if (routeId < 500)
            {
                _database.GetCollection(_busCollectionName).Update(Query.EQ("MorningAssignedTo", routeId), Update.Set("MorningAssignedTo", -1));
                _database.GetCollection(_driverCollectionName).Update(Query.EQ("MorningAssignedTo", routeId), Update.Set("MorningAssignedTo", -1));
            }
            else
            {
                _database.GetCollection(_busCollectionName).Update(Query.EQ("EveningAssignedTo", routeId), Update.Set("EveningAssignedTo", -1));
                _database.GetCollection(_driverCollectionName).Update(Query.EQ("EveningAssignedTo", routeId), Update.Set("EveningAssignedTo", -1));
            }
        }

        public void SetInactiveBusesDriversFromRoute(int routeId)
        {
            if (routeId < 500)
            {
                _database.GetCollection(_busCollectionName).Update(Query.EQ("MorningAssignedTo", routeId), Update.Set("MorningIsActive", false));
                _database.GetCollection(_driverCollectionName).Update(Query.EQ("MorningAssignedTo", routeId), Update.Set("MorningIsActive", false));
            }
            else
            {
                _database.GetCollection(_busCollectionName).Update(Query.EQ("EveningAssignedTo", routeId), Update.Set("EveningIsActive", false));
                _database.GetCollection(_driverCollectionName).Update(Query.EQ("EveningAssignedTo", routeId), Update.Set("EveningIsActive", false));
            }
        }

        public int GetTotalCapacity(int routeId)
        {
            int total = 0;
            if (GetRouteByRouteId(routeId).IsActive)//return 0 if the route is inactive
            {
                bool isToWork = routeId < 500;
                foreach (Bus bus in GetBusesAssignedToRoute(routeId))
                {
                    //only count active buses
                    if ((isToWork && bus.MorningIsActive) || (!isToWork && bus.EveningIsActive))
                        total += bus.Capacity;
                }
            }
            return total;
        }

        public IEnumerable<Employee> GetEmployeesAssignedToRoute(int routeId)
        {
            var coll = _database.GetCollection(_employeeCollectionName);
            string varName = routeId < 500 ? "MorningAssignedTo" : "EveningAssignedTo";
            var query = Query.EQ(varName, routeId);
            return coll.FindAs<Employee>(query);
        }

        public List<Stop> GetStopsAssignedToRoute(int routeId)
        {
            Route route = GetRouteByRouteId(routeId);
            List<Stop> stops = new List<Stop>();
            foreach (Stop s in route.Stops)
            {
                stops.Add(s);
            }
            return stops;
        }

        public IEnumerable<Bus> GetBusesAssignedToRoute(int routeId)
        {
            var coll = _database.GetCollection(_busCollectionName);
            string varName = routeId < 500 ? "MorningAssignedTo" : "EveningAssignedTo";
            var query = Query.EQ(varName, routeId);
            return coll.FindAs<Bus>(query);
        }

        public void SaveEmployeeActivity(EmployeeActivity activity)
        {
            var coll = _database.GetCollection(_activityCollectionName);
            coll.Save(activity);
        }

        public List<EmployeeActivity> GetEmployeeActivity()
        {
            var coll = _database.GetCollection(_activityCollectionName);
            return coll.FindAllAs<EmployeeActivity>().ToList();

        }

        public int GetDistinctEmployeActivityCount(string type)
        {
            var coll = _database.GetCollection(_activityCollectionName);
            return coll.Distinct(type).Count();
        }

        public IEnumerable<EmployeeActivity> GetEmployeeActivityByRoute(ObjectId routeId)
        {
           var coll = _database.GetCollection(_activityCollectionName);
           return coll.AsQueryable<EmployeeActivity>().Where(ei => ei.Route.Id == routeId).OrderBy(ei => ei.Date);
        }

        public Driver GetDriverAssignedToRouteBus(Route route, Bus bus)
        {
            bool isMorning = route.RouteId < 500;
            foreach (Models.DriverBus driverBus in route.DriverBusList)
            {
                if (bus.BusId == driverBus.BusId)
                    return GetDriverById(driverBus.DriverId);
            }

            return null;
        }
    }
}
