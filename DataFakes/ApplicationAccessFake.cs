using DataInterfaces;
using DataObjects.models;

namespace DataFakes
{
    public class ApplicationAccessFake : IApplicationAccessor
    {
        AppInformation appInformation = null;
        public ApplicationAccessFake()
        {
            appInformation = new AppInformation()
            {
                MOTD = "Testing motd",
                AppVersion = "1.0",
                TOS = "Terms of service",
                Help = "Help menu",
                UserCount = 30
            };
        }
        public AppInformation SelectApplicationInformation()
        {
            return appInformation;
        }

        public int SelectRegisteredUserCount()
        {
            return (int)appInformation.UserCount;
        }
    }
}
