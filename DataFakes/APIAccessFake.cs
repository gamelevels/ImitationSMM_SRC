using DataInterfaces;
using DataObjects.models;
using System;
using System.Collections.Generic;

namespace DataFakes
{
    public class APIAccessFake : IAPIAccessor
    {
        public bool AddAPI(API api)
        {
            throw new NotImplementedException();
        }

        public List<API> GetAllAPI()
        {
            throw new NotImplementedException();
        }

        public bool RemoveAPI(API api)
        {
            throw new NotImplementedException();
        }

        public bool UpdateAPI(API api)
        {
            throw new NotImplementedException();
        }
    }
}
