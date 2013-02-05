using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TransportationService.Utility
{
   public class Bus
   {
      [BsonId]
      public ObjectId Id;
      public string Name { get; set; }
      public string LiscensePlate { get; set; }
      public int Capacity { get; set; }
      List<Tuple<int, int>> Route { get; set; } // stop number, amount of employees to pick up.
   }

   public class Driver
   {
      [BsonId]
      public ObjectId Id;
      public Bus Bus { get; set; }
   }

   public class Employee
   {
      [BsonId]
      public ObjectId Id;
      public long SocialSecurityNumber{ get; set; }
      public ObjectId BusId { get; set; }
   }
}