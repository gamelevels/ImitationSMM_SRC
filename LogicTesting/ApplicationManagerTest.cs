using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataObjects;
using DataFakes;
using LogicInterfaces;
using Logic;
using DataObjects.models;

namespace LogicTesting
{
    [TestClass]
    public class ApplicationManagerTest
    {
        IApplicationManager applicationManager = null;
        public ApplicationManagerTest()
        {
            applicationManager = new ApplicationManager(new ApplicationAccessFake());
        }

        [TestMethod]
        public void TestGetsCorrectApplicationInformation()
        {
            AppInformation appInfo = applicationManager.SetApplicationInformation();
            Assert.AreEqual("1.0", appInfo.AppVersion);
        }

        [TestMethod]
        public void TestGetsCorrectUserCount()
        {
            AppInformation appInfo = applicationManager.SetApplicationInformation();
            Assert.AreEqual(30, appInfo.UserCount);
        }
    }
}