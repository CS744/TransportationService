using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TransportationService.Utility;

namespace TransportationService.Models
{
   public class ViewBusModel
   {
      public string LiscensePlate { get; set; }
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
      public string LiscensePlate { get; set; }
   }
}
