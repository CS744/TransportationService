using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TransportationService.Utility;

namespace TransportationService.Models
{
    public class AddBusModel
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
}