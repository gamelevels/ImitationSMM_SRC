using DataObjects.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicInterfaces
{
    public interface IApplicationManager
    {
        AppInformation SetApplicationInformation();
        int GetUserCount();
    }
}
