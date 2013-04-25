using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using TransportationService.Models;

// These are the classes we will store in the Database.

namespace TransportationService.Utility
{
    
    public class User
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }

    public class Seed
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public int RouteIdHigh { get; set; }
        public int RouteIdLow { get; set; }
        public int BusId { get; set; }
        public int DriverId { get; set; }
        public int StopId { get; set; }
        public int EmployeeId { get; set; }
    }

    public class Bus
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string LicensePlate { get; set; }
        public string State { get; set; }
        public int Capacity { get; set; }
        public int BusId { get; set; }
        public bool MorningIsActive { get; set; }
        public bool EveningIsActive { get; set; }
        public int MorningAssignedTo { get; set; }
        public int EveningAssignedTo { get; set; }
        public bool HasBeenDeleted { get; set; }
    }

    public class Employee
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public long SocialSecurityNumber { get; set; }
        public string Position { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsMale { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int EmployeeId { get; set; }
        public int MorningAssignedTo { get; set; }
        public int EveningAssignedTo { get; set; }
        public int Zip { get; set; }
        public bool HasBeenDeleted { get; set; }
    }

    public class Driver
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string DriverLicense { get; set; }
        public string State { get; set; }
        public int DriverId { get; set; }
        public bool MorningIsActive { get; set; }
        public bool EveningIsActive { get; set; }
        public int MorningAssignedTo { get; set; }
        public int EveningAssignedTo { get; set; }
        public bool HasBeenDeleted { get; set; }
    }

    public class Route
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public List<Stop> Stops { get; set; }
        public string Name { get; set; }
        public int RouteId { get; set; }
        public bool IsActive { get; set; }
        public List<DriverBus> DriverBusList { get; set; }
        public bool HasBeenDeleted { get; set; }
    }

    public class Stop
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Location { get; set; }
        public int StopId { get; set; }
        public bool HasBeenDeleted { get; set; }
    }
    public class EmployeeInstance
    {
       [BsonId]
       public ObjectId Id { get; set; }
       public ObjectId RouteId { get; set; }
       public ObjectId EmployeeId { get; set; }
       public ObjectId StopId { get; set; }
       public ObjectId BusId { get; set; }
       public DateTime Date { get; set; }
    }

    public class EmployeeActivity
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public int RouteId { get; set; }
        public int EmployeeId { get; set; }
        public int StopId { get; set; }
        public int BusId { get; set; }
        public DateTime Date { get; set; }
        public int DriverId { get; set; }
        public string RouteName { get; set; }
        public string LicensePlate { get; set; }
        public string DriverName { get; set; }
        public string EmployeeName { get; set; }
        public string StopLocation { get; set; }
    }
}
