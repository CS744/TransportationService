﻿using System;
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
        public string Name { get; set; }
        public List<Stop> Stops { get; set; }
        public string RouteId { get; set; }
        public bool IsActive { get; set; }
        public List<DriverBus> DriverBusList { get; set; }
        public Boolean IsToWork { get; set; }
    }

    public class BusModel
    {
        public List<string> StateNames { get; set; }
        public List<string> StateAbbreviations { get; set; }
        public bool UpdatingBus { get; set; }
        public string Capacity { get; set; }
        public string License { get; set; }
        public string State { get; set; }
        public bool MorningIsActive { get; set; }
        public bool EveningIsActive { get; set; }
        public string BusId { get; set; }
    }

    public class DriverModel
    {
        public List<string> StateNames { get; set; }
        public List<string> StateAbbreviations { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public string License { get; set; }
        public ObjectId Id { get; set; }
        public Boolean UpdatingDriver { get; set; }
        public int DriverId { get; set; }
        public bool MorningIsActive { get; set; }
        public bool EveningIsActive { get; set; }
    }

    public class EmployeeModel
    {
        public List<Route> AvailableRoutes { get; set; }
        public List<string> StateNames { get; set; }
        public List<string> StateAbbreviations { get; set; }
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
        public int MorningAssignedTo { get; set; }
        public int EveningAssignedTo { get; set; }
        public int Zip { get; set; }
    }

    public class DriverBus
    {
        public int BusId { get; set; }
        public int DriverId { get; set; }
        public string Hour { get; set; }
        public string Minute { get; set; }
        public string AMPM { get; set; }
        public bool IsActive { get; set; }
    }

    public class CustomRow
    {
       public List<string> Columns { get; set; }
       public string ModifyCall { get; set; }
       public string DeleteCall { get; set; }
       public string ObjectId { get; set; }
    }
    public class CustomTable
    {
       public List<string> Headers { get; set; }
       public List<CustomRow> Rows { get; set; }
    }
}
