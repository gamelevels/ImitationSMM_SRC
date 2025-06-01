using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public static class Validator
    {
        public static bool ValidString(this string input)
        {
            return input.Length >= 4 ? true : false;
        }
        public static bool ValidGuid(this string input)
        {
            return Guid.TryParse(input, out _);            
        }
    }
}
