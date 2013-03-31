using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TransportationService.Utility;
using MongoDB.Bson;

namespace TransportationService.Models
{
   public class ViewBusModel
   {
      public string LicensePlate { get; set; }
      public string State { get; set; }
      public int Capacity { get; set; }
      public string Status { get; set; }
      public int BusId { get; set; }
      public string RouteName { get; set; }
   }
   public class ViewRouteModel
   {
      public List<Stop> Stops { get; set; }
      public string DriverName { get; set; }
      public string Name { get; set; }
      public string LicensePlate { get; set; }
      public string RouteId { get; set; }
   }
   public class ViewStopModel
   {
      public string StopLocation { get; set; }
      public int StopId { get; set; }
   }
   public class ViewDriverModel
   {
      public string State { get; set; }
      public string License { get; set; }
      public string Name { get; set; }
      public string DriverId { get; set; }
   }
   public class ViewEmployeeModel
   {
      public string Name { get; set; }
      public string Gender { get; set; }
      public long SSN { get; set; }
      public string Position { get; set; }
      public string Email { get; set; }
      public string Number { get; set; }
      public string Address { get; set; }
      public string City { get; set; }
      public string State { get; set; }
      public string RouteName { get; set; }
      public string Id { get; set; }
   }
}
