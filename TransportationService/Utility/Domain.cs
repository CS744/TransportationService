using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

// These are the classes we will store in the Database.

namespace TransportationService.Utility
{
   public enum BusStatus
   {
      Active = 0,
      Inactive
   }

   public class User
   {
      [BsonId]
      public ObjectId Id { get; set; }
      public string Username { get; set; }
      public string Password { get; set; }
      public string Email { get; set; }
   }

   public class Bus
   {
      [BsonId]
      public ObjectId Id { get; set; }
      public string LiscensePlate { get; set; }
      public int Capacity { get; set; }
      public BusStatus Status { get; set; }
      public long BusId { get; set; }
      //public ObjectId RouteId { get; set; }
   }

   public class Employee
   {
      [BsonId]
      public ObjectId Id { get; set; }
      public long SocialSecurityNumber { get; set; }
      public string Position { get; set; }
      //public ObjectId RouteId { get; set; }
   }
   public class Route : List<Stop>
   {
      [BsonId]
      public ObjectId Id { get; set; }
      public List<Employee> Employees { get; set; }
      public Bus Bus { get; set; }
      public Route(List<Stop> r)
         : base(r)
      {
      }
   }
   public class Stop
   {
      [BsonId]
      public ObjectId Id { get; set; }
      public string StreetName { get; set; }
      public int StreetNumber { get; set; }
      public int StopId { get; set; }
   }
}