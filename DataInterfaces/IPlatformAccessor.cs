using DataObjects.models;
using System.Collections.Generic;

namespace DataInterfaces
{
    public interface IPlatformAccessor
    {
        List<Platform> SelectAllPlatforms();
        List<PlatformConstraint> SelectPlatformConstraints();
        Dictionary<string, int> SelectPopularPlatform();
    }
}
