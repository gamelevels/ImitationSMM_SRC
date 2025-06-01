using DataInterfaces;
using DataObjects.models;
using System;
using System.Collections.Generic;

namespace DataFakes
{
    public class PlatformAccessFake : IPlatformAccessor
    {
        List<Platform> platforms = new List<Platform>();
        List<PlatformConstraint> constraints = new List<PlatformConstraint>();
        public PlatformAccessFake()
        {
            platforms.Add(new Platform() { PlatformName = "Test", MinimumLevel = 1, Enabled = true });
            platforms.Add(new Platform() { PlatformName = "Test1", MinimumLevel = 2, Enabled = true });
            platforms.Add(new Platform() { PlatformName = "Test2", MinimumLevel = 3, Enabled = true });
            constraints.Add(new PlatformConstraint() { PlatformName = "Test", RequestCount = 15, RequestType = "Follow" });
            constraints.Add(new PlatformConstraint() { PlatformName = "Test", RequestCount = 30, RequestType = "Like" });
            constraints.Add(new PlatformConstraint() { PlatformName = "Test1", RequestCount = 45, RequestType = "Comment" });
            constraints.Add(new PlatformConstraint() { PlatformName = "Test1", RequestCount = 60, RequestType = "Follow" });
        }
        public List<Platform> SelectAllPlatforms()
        {
            return platforms;
        }

        public List<PlatformConstraint> SelectPlatformConstraints()
        {
            return constraints;
        }

        public Dictionary<string, int> SelectPopularPlatform()
        {
            throw new NotImplementedException();
        }
    }
}
