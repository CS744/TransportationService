using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Bson;
using TransportationService.Utility;

namespace TransportationService.Models
{
    public class RouteModel
    {
        public List<Bus> AvailableBuses { get; set; }
        public List<Stop> AvailableStops { get; set; }
        public List<Driver> AvailableDrivers { get; set; }
        public Boolean UpdatingRoute { get; set; }
        public Driver Driver { get; set; }
        public String Name { get; set; }
        public Bus Bus { get; set; }
        public List<Stop> Stops { get; set; }
        public String RouteId { get; set; }
    }

    public class BusModel
    {
        public List<String> StateNames { get; set; }
        public List<String> StateAbbreviations { get; set; }
        public bool UpdatingBus { get; set; }
        public String Capacity { get; set; }
        public String License { get; set; }
        public String State { get; set; }
        public String Status { get; set; }
        public String BusId { get; set; }
    }

    public class DriverModel
    {
        public List<String> StateNames { get; set; }
        public List<String> StateAbbreviations { get; set; }
        public String Name { get; set; }
        public String State { get; set; }
        public String License { get; set; }
        public ObjectId Id { get; set; }
        public Boolean UpdatingDriver { get; set; }
        public String DriverId { get; set; }
    }

    public class EmployeeModel
    {
        public List<Route> AvailableRoutes { get; set; }
        public List<String> StateNames { get; set; }
        public List<String> StateAbbreviations { get; set; }
        public bool UpdatingEmployee { get; set; }
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
        public int AssignedTo { get; set; }
        public int Zip { get; set; }
    }
}
