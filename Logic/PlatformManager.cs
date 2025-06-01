using DataAccess;
using DataInterfaces;
using DataObjects.models;
using LogicInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logic
{
    public class PlatformManager : IPlatformManager
    {
        private IPlatformAccessor platformAccessor = null;

        public PlatformManager()
        {
            platformAccessor = new PlatformAccessor();
        }

        public PlatformManager(IPlatformAccessor access)
        {
            platformAccessor = access;
        }
        public List<Platform> GetAllPlatforms()
        {
            try
            {
                return platformAccessor.SelectAllPlatforms();
            }
            catch
            {
                throw new ApplicationException("Unable to find all platform information");
            }
        }

        public List<PlatformConstraint> GetPlatformConstraints()
        {
            try
            {
                return platformAccessor.SelectPlatformConstraints();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unable to find platform information", ex);
            }
        }

        public string GetPopularPlatform()
        {
            try
            {
                Dictionary<string, int> plats = platformAccessor.SelectPopularPlatform();
                return plats.OrderByDescending(kv => kv.Value).FirstOrDefault().Key;
            }
            catch
            {
                throw new ApplicationException("Unable to find popular platform information");
            }
        }

        public List<Platform> GetUserPlatforms(int level)
        {
            // Get all platforms, filter based on enabled and required level
            return GetAllPlatforms().FindAll(w => w.Enabled == true && w.MinimumLevel <= level);
        }
    }
}
