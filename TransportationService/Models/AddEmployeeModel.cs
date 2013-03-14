using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TransportationService.Utility;

namespace TransportationService.Models
{
    public class AddEmployeeModel
    {
        public List<Route> AvailableRoutes { get; set; }
        public List<String> StateNames { get; set; }
        public List<String> StateAbbreviations { get; set; }
    }

}
