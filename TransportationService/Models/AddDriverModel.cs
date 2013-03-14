using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TransportationService.Utility;

namespace TransportationService.Models
{
    public class AddDriverModel
    {
        public List<String> StateNames { get; set; }
        public List<String> StateAbbreviations { get; set; }
    }
}