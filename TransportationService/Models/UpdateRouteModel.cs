using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TransportationService.Utility;

namespace TransportationService.Models
{
    public class UpdateRouteModel
    {
        public List<Bus> AvailableBuses { get; set; }
        public List<Stop> AvailableStops { get; set; }
        public List<Driver> AvailableDrivers { get; set; }
    }
}