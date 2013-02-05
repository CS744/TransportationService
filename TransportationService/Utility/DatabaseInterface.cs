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


   }
}