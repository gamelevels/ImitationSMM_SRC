using DataObjects.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicInterfaces
{
    public interface IAPIManager
    {
        bool AddAPI(API api);
        bool UpdateAPI(API api);
        bool RemoveAPI(API api);
        List<API> GetAPIs();
        Task<APIResponse> SendAPIRequest(APIRequest request);
        Task<int> GetAccountCount();
        Task<APIResponse> SendAPICancelRequest(APIRequest request);
        Task<decimal> GetAPIBalance();
    }
}
