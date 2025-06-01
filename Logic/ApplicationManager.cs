using DataAccess;
using DataInterfaces;
using DataObjects.models;
using LogicInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class ApplicationManager : IApplicationManager
    {
        private IApplicationAccessor applicationAccessor = null;

        public ApplicationManager()
        {
            applicationAccessor = new ApplicationAccessor();
        }

        public ApplicationManager(IApplicationAccessor access)
        {
            applicationAccessor = access;
        }

        public int GetUserCount()
        {
            try
            {
                return applicationAccessor.SelectRegisteredUserCount();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unable to find application information", ex);
            }
        }

        public AppInformation SetApplicationInformation()
        {
            try
            {
                return applicationAccessor.SelectApplicationInformation();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unable to find application information", ex);
            }
        }
    }
}
