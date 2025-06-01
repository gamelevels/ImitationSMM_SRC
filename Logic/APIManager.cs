using DataAccess;
using DataInterfaces;
using DataObjects;
using DataObjects.models;
using LogicInterfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class APIManager : IAPIManager
    {
        private IAPIAccessor apiAccessor = null;
        private ExtendedWebClient webClient = null; // could use httpclient, but this is easier to setup

        public APIManager()
        {
            apiAccessor = new APIAccessor();
            webClient = new ExtendedWebClient();
        }

        public APIManager(IAPIAccessor access)
        {
            apiAccessor = access;
        }

        public bool AddAPI(API api)
        {
            try
            {
                return apiAccessor.AddAPI(api);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unable to add api", ex);
            }
        }

        public List<API> GetAPIs()
        {
            try
            {
                return apiAccessor.GetAllAPI();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unable to find apis", ex);
            }
        }

        public bool RemoveAPI(API api)
        {
            throw new NotImplementedException();
        }

        public async Task<decimal> GetAPIBalance()
        {
            try
            {
                JObject json = JObject.Parse(await webClient.DownloadStringTaskAsync(AppVars.BalanceURL));
                return (decimal)json["balance"];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> GetAccountCount()
        {
            try
            {
                JObject json = JObject.Parse(await webClient.DownloadStringTaskAsync(AppVars.StatsURL));
                return (int)json["accounts"];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<APIResponse> SendAPIRequest(APIRequest request)
        {
            APIResponse apiResponse = null;
            try
            {
                string requestURL = request.URL + $"?handle={request.Handle}&fillType={request.FillType}&requests={request.Requests}";
                var resp = await webClient.DownloadStringTaskAsync(requestURL);
                if (resp != null)
                {
                    apiResponse = JsonConvert.DeserializeObject<APIResponse>(resp);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return apiResponse;
        }

        public async Task<APIResponse> SendAPICancelRequest(APIRequest request)
        {
            APIResponse apiResponse = null;
            try
            {
                string type = request.URL.Contains("/smm/v1/") ? "v1" : "v2";
                request.URL = request.URL.Replace("/smm/v1/", "/cancel/").Replace("/smm/v2/", "/cancel/");
                string requestURL = request.URL + $"?handle={request.Handle}&endpoint={type}";
                var resp = await webClient.DownloadStringTaskAsync(requestURL);
                if (resp != null)
                {
                    apiResponse = JsonConvert.DeserializeObject<APIResponse>(resp);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return apiResponse;
        }

        public bool UpdateAPI(API api)
        {
            try
            {
                return apiAccessor.UpdateAPI(api);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unable to update api", ex);
            }
        }
    }
}
