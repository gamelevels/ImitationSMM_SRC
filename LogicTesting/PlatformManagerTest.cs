using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataObjects;
using DataFakes;
using LogicInterfaces;
using Logic;
using DataObjects.models;
namespace LogicTesting
{
    [TestClass]
    public class PlatformManagerTest
    {
        IPlatformManager platformManager = null;
        public PlatformManagerTest()
        {
            platformManager = new PlatformManager(new PlatformAccessFake());
        }

    }
}