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



      public DatabaseInterface()
		{
         string connectionString = "mongodb://localhost/TransportationService";
			_database = MongoDatabase.Create(connectionString);
		}

      public Employee GetEmployeeBySSN(long ssn)
      {
         var coll = _database.GetCollection(_employeeCollectionName);
         var query = Query.EQ("SocialSecurityNumber", ssn);
         return coll.FindOneAs<Employee>(query);
      }

      public void SaveBus(Bus bus)
      {
         var coll = _database.GetCollection(_busCollectionName);
         coll.Save(bus);
      }
      public void SaveUser(User user)
      {
         var coll = _database.GetCollection(_usersCollectionName);
         coll.Save(user);
      }

      public User getUser(string username, string password)
      {
         var coll = _database.GetCollection(_usersCollectionName);
         var query = Query.And(Query.EQ("Username", username), Query.EQ("Password", password));
         return coll.FindOneAs<User>(query);
      }
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

      public Bus GetBusByBusId(int id)
      {
          var coll = _database.GetCollection(_busCollectionName);
          var query = Query.EQ("BusId", id);
          return coll.FindOneAs<Bus>(query);
      }

      public void AssignBusToRoute(int busId, int routeId)
      {
          _database.GetCollection(_busCollectionName).Update(Query.EQ("BusId", busId), Update.Set("AssignedTo", routeId));
      }

      public List<Bus> GetAvailableBuses()
      {
         var coll = _database.GetCollection(_busCollectionName);
         //var query = Query.EQ("_id", id);
         //return coll.FindAll<Bus>();
         return coll.FindAllAs<Bus>().ToList();
      }
      public List<Stop> GetAvailableStops()//pass in a list of stops already in the route.
      {
         var coll = _database.GetCollection(_stopCollectionName);
         return coll.FindAllAs<Stop>().ToList();
      }
      public List<Route> GetAvailableRoutes()
      {
          var coll = _database.GetCollection(_routeCollectionName);
          return coll.FindAllAs<Route>().ToList();
      }
      public void SaveRoute(Route route)
      {
         var coll = _database.GetCollection(_routeCollectionName);
         coll.Save(route);
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
      public Boolean IsRouteNameUnique(string name)
      {
          List<Route> routes = GetAvailableRoutes();
          foreach (Route r in routes)
          {
              if (String.Equals(r.Name, name))
                  return false;
          }
          return true;
      }
      public Boolean IsLicenseUnique(string license)
      {
          List<Bus> buses = GetAvailableBuses();
          foreach (Bus b in buses)
          {
              if (String.Equals(b.LiscensePlate, license))
                  return false;
          }
          return true;
      }
      public int GetNextStopId()
      {
          List<Stop> stops = GetAvailableStops();
          if (stops.OrderBy(s => s.StopId).LastOrDefault() == null)
              return 1;
          return stops.OrderBy(s => s.StopId).LastOrDefault().StopId + 1;
      }
      public int GetNextRouteId()
      {
          List<Route> routes = GetAvailableRoutes();
          if (routes.OrderBy(r => r.RouteId).LastOrDefault() == null)
              return 1;
          return routes.OrderBy(r => r.RouteId).LastOrDefault().RouteId + 1;
      }
      public int GetNextBusId()
      {
          List<Bus> buses = GetAvailableBuses();
          if (buses.OrderBy(b => b.BusId).LastOrDefault() == null)
              return 1;
          return buses.OrderBy(b => b.BusId).LastOrDefault().BusId + 1;
      }
   }
}