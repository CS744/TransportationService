using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

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

        public DatabaseInterface()
        {
            string connectionString = "mongodb://localhost/TransportationService";
            _database = MongoDatabase.Create(connectionString);
        }

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

        #region ROUTE

        public void SaveRoute(Route route)
        {
           var coll = _database.GetCollection(_routeCollectionName);
           coll.Save(route);
        }
        public Route GetRouteByRouteId(int id)
        {
            var coll = _database.GetCollection(_routeCollectionName);
            var query = Query.EQ("RouteId", id);
            return coll.FindOneAs<Route>(query);
        }

        public Route GetRouteById(ObjectId id)
        {
            var coll = _database.GetCollection(_routeCollectionName);
            var query = Query.EQ("_id", id);
            return coll.FindOneAs<Route>(query);
        }

        public int GetNextLowRouteId()
        {
            List<Route> routes = GetAvailableRoutes();
            if (routes.OrderBy(r => r.RouteId).LastOrDefault() == null)
                return 1;
            return routes.OrderBy(r => r.RouteId).LastOrDefault().RouteId + 1;
        }

        public int GetNextHighRouteId()
        {
            List<Route> routes = GetAvailableRoutes();
            if (routes.OrderBy(r => r.RouteId).LastOrDefault() == null)
                return 999;
            return routes.OrderBy(r => r.RouteId).LastOrDefault().RouteId - 1;
        }

        public List<Route> GetAvailableRoutes()
        {
            var coll = _database.GetCollection(_routeCollectionName);
            return coll.FindAllAs<Route>().ToList();
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
            var query = Query.EQ("BusId", id);
            return coll.FindOneAs<Bus>(query);
        }

        public Bus GetBusById(ObjectId id)
        {
            var coll = _database.GetCollection(_busCollectionName);
            var query = Query.EQ("_id", id);
            return coll.FindOneAs<Bus>(query);
        }

        public List<Bus> GetAvailableBuses()
        {
            var coll = _database.GetCollection(_busCollectionName);
            //var query = Query.EQ("_id", id);
            //return coll.FindAll<Bus>();
            return coll.FindAllAs<Bus>().ToList();
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
            List<Bus> buses = GetAvailableBuses();
            if (buses.OrderBy(b => b.BusId).LastOrDefault() == null)
                return 1;
            return buses.OrderBy(b => b.BusId).LastOrDefault().BusId + 1;
        }

        public void DeleteBus(ObjectId id)
        {
            var coll = _database.GetCollection(_busCollectionName);
            var query = Query.EQ("_id", id);
            coll.Remove(query);
        }

        #endregion


        #region STOP

        public void SaveStop(Stop stop)
        {
            var coll = _database.GetCollection(_stopCollectionName);
            coll.Save(stop);
        }

        public Stop GetStop(ObjectId id)
        {
            var coll = _database.GetCollection(_stopCollectionName);
            var query = Query.EQ("_id", id);
            return coll.FindOneAs<Stop>(query);
        }

        public Stop GetStopByStopId(int id)
        {
            var coll = _database.GetCollection(_stopCollectionName);
            var query = Query.EQ("StopId", id);
            return coll.FindOneAs<Stop>(query);
        }
        public void DeleteStop(ObjectId id)
        {
           var coll = _database.GetCollection(_stopCollectionName);
           var query = Query.EQ("_id", id);
           coll.Remove(query);
        }

        public List<Stop> GetAvailableStops()//pass in a list of stops already in the route.
        {
            var coll = _database.GetCollection(_stopCollectionName);
            return coll.FindAllAs<Stop>().ToList();
        }

        public int GetNextStopId()
        {
            List<Stop> stops = GetAvailableStops();
            if (stops.OrderBy(s => s.StopId).LastOrDefault() == null)
                return 1;
            return stops.OrderBy(s => s.StopId).LastOrDefault().StopId + 1;
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
            var query = Query.EQ("DriverLicense", driverLicense);
            return coll.FindOneAs<Driver>(query);
        }

        public Driver GetDriverById(string driverId)
        {
            var coll = _database.GetCollection(_driverCollectionName);
            var query = Query.EQ("DriverId", driverId);
            return coll.FindOneAs<Driver>(query);
        }

        public Driver GetDriverByobjId(ObjectId id)
        {
            var coll = _database.GetCollection(_driverCollectionName);
            var query = Query.EQ("_id", id);
            return coll.FindOneAs<Driver>(query);
        }

        public void DeleteDriver(ObjectId id)
        {
            var coll = _database.GetCollection(_driverCollectionName);
            var query = Query.EQ("_id", id);
            coll.Remove(query);
        }

        public List<Driver> GetAvailableDrivers()
        {
            var coll = _database.GetCollection(_driverCollectionName);
            return coll.FindAllAs<Driver>().ToList();
        }

        public Boolean IsDriverLicenseUnique(string license, string state, string driverId = "")
        {
            List<Driver> drivers = GetAvailableDrivers();
            foreach (Driver d in drivers)
            {
                if (driverId.Equals("") || !(d.Id.Equals(driverId)))
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

        public string GetNextDriverId()
        {
            List<Driver> drivers = GetAvailableDrivers();
            if (drivers.OrderBy(s => s.DriverId).LastOrDefault() == null)
                return "1";
            int highest = int.Parse(drivers.OrderBy(s => s.DriverId).LastOrDefault().DriverId);
            return (highest + 1).ToString();
        }

        #endregion


        #region EMPLOYEE

        public Employee GetEmployeeBySSN(long ssn)
        {
           var coll = _database.GetCollection(_employeeCollectionName);
           var query = Query.EQ("SocialSecurityNumber", ssn);
           return coll.FindOneAs<Employee>(query);
        }

        public Employee GetEmployeeById(int id)
        {
           var coll = _database.GetCollection(_employeeCollectionName);
           var query = Query.EQ("EmployeeId", id);
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
            return coll.FindAllAs<Employee>().ToList();
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
            List<Employee> employees = GetAvailableEmployees();
            if (employees.OrderBy(e => e.EmployeeId).LastOrDefault() == null)
                return 1;
            return employees.OrderBy(e => e.EmployeeId).LastOrDefault().EmployeeId + 1;
        }

        public void DeleteEmployee(ObjectId id)
        {
            var coll = _database.GetCollection(_employeeCollectionName);
            var query = Query.EQ("_id", id);
            coll.Remove(query);
        }

        #endregion


        public void AssignBusToRoute(int busId, int routeId)
        {
            _database.GetCollection(_busCollectionName).Update(Query.EQ("BusId", busId), Update.Set("AssignedTo", routeId));
        }

        public void AssignDriverToRoute(string driverLicense, int routeId)
        {
            _database.GetCollection(_driverCollectionName).Update(Query.EQ("DriverLicense", driverLicense), Update.Set("AssignedTo", routeId));
        }

    }
}
