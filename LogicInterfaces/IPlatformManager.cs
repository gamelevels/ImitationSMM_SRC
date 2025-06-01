using DataObjects.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicInterfaces
{
    public interface IPlatformManager
    {
        List<Platform> GetAllPlatforms();
        List<Platform> GetUserPlatforms(int level);
        List<PlatformConstraint> GetPlatformConstraints();
        string GetPopularPlatform();
    }
}
