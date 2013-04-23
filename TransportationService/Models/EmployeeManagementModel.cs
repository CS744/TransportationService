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

   public class ViewInstanceModel
   {
      public Route Route { get; set; }
      public List<InstanceRow> Rows { get; set; }

   }
   public class InstanceRow
   {
      public Bus Bus { get; set; }
      public DateTime Date { get; set; }
      public List<EmployeeCell> Employees { get; set; }
   }
   public class EmployeeCell
   {
      public Employee Employee { get; set; }
      public Stop Stop { get; set; }
      public DateTime Date { get; set; }
   }
}
