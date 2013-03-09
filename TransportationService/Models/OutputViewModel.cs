using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TransportationService.Utility;

namespace TransportationService.Models
{
   public class OutputViewModel
   {
      public string Username { get; set; }
      public IEnumerable<Bus> Buses { get; set; }
      public IEnumerable<Route> Routes { get; set; }
      public IEnumerable<Employee> Employees { get; set; }
      public IEnumerable<Driver> Drivers { get; set; }
      public IEnumerable<Stop> Stops { get; set; }
   }
}
