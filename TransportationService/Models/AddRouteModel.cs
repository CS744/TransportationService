using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TransportationService.Utility;

namespace TransportationService.Models
{
   public class AddRouteModel
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
}