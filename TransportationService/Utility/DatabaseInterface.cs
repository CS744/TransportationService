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

      public List<Bus> GetAvailableBuses()
      {
         var coll = _database.GetCollection(_busCollectionName);
         //var query = Query.EQ("_id", id);
         //return coll.FindAll<Bus>();
         return new List<Bus>(); // TODO do correctly...
      }
      public List<Stop> GetAvailableStops()
      {
         var coll = _database.GetCollection(_stopCollectionName);
         return new List<Stop>(); // TODO do correctly...
      }
      public List<Employee> GetAvailableEmployees()
      {
         var coll = _database.GetCollection(_employeeCollectionName);
         return new List<Employee>(); // TODO do correctly...
      }

   }
}