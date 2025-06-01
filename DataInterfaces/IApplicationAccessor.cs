using DataObjects.models;

namespace DataInterfaces
{
    public interface IApplicationAccessor
    {
        AppInformation SelectApplicationInformation();
        int SelectRegisteredUserCount();
    }
}
