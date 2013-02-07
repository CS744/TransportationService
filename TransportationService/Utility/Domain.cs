using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

// These are the classes we will store in the Database.

namespace TransportationService.Utility
{
   public enum UserType
   {
      Admin = 0,
      Driver
   }
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
      public UserType Type { get; set; }
   }

   public class Bus
   {
      [BsonId]
      public ObjectId Id { get; set; }
      public string LiscensePlate { get; set; }
      public int Capacity { get; set; }
      public BusStatus status { get; set; }
      Route Route { get; set; }
   }

   public class Employee
   {
      [BsonId]
      public ObjectId Id { get; set; }
      public long SocialSecurityNumber { get; set; }
      public string Position { get; set; }
      public ObjectId RouteId { get; set; }
   }
   public class Route : List<int>
   {
      [BsonId]
      public ObjectId Id { get; set; }
      List<ObjectId> Employees { get; set; }
      public Route(List<int> r)
         : base(r)
      {
      }
   }
}