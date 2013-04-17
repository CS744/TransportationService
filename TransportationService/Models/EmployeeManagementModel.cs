using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Bson;
using TransportationService.Utility;

namespace TransportationService.Models
{
   public class ManagementModel
   {
      public IEnumerable<Employee> Employees { get; set; }
      public IEnumerable<Stop> Stops { get; set; }
      public IEnumerable<Bus> Buses { get; set; }
   }
}
