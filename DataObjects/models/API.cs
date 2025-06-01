using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.models
{
    // Would also have a parameters fill, but using static ones for current project state
    public class API
    {
        public string Link { get; set; }
        public string ParameterPlaceHolder { get; set; }
        public bool Enabled { get; set; }

        /// <summary>
        /// Added recently, program wasn't designed to store these
        /// Will be hard coding for display purposes
        /// </summary>
        public string Name { get; set; }
        public string Key { get; set; }
        public int ActiveRequests { get; set; }
        public int TotalRequests { get; set; }
        public decimal Balance { get; set; }
    }
}
