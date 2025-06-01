using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataObjects;
using DataFakes;
using LogicInterfaces;
using Logic;
using DataObjects.models;


namespace LogicTesting
{
    [TestClass]
    public class APIManagerTest
    {
        IAPIManager apiManager = null;
        public APIManagerTest()
        {
            apiManager = new APIManager(new APIAccessFake());
            // not going to implement
        }
    }
}