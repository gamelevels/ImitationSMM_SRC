using DataObjects.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Models
{
    public class SMMVM
    {
        public List<PlatformConstraint> constraints { get; set; }
        public List<Platform> platforms { get; set; }

        public string requestHistory { get; set; }
        public int requestMultiplier = 2500;
        public ApplicationUser loggedInUser { get; set; }
        public APIRequest requestData { get; set; }
    }
}