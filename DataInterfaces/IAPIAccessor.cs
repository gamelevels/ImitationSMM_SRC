using DataObjects.models;
using System.Collections.Generic;

namespace DataInterfaces
{
    public interface IAPIAccessor
    {
        bool AddAPI(API api);
        bool UpdateAPI(API api);
        bool RemoveAPI(API api);
        List<API> GetAllAPI();
    }
}
