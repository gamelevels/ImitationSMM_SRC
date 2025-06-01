using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataObjects;
using DataFakes;
using LogicInterfaces;
using Logic;
using DataObjects.models;
using System;

namespace LogicTesting
{
    [TestClass]
    public class UserManagerTest
    {
        IUserManager userManager = null;
        public UserManagerTest()
        {
            userManager = new UserManager(new UserAccessFake());
        }

        [TestMethod]
        public void TestHash()
        {
            Assert.AreEqual(userManager.Hash256("mypassword"), "89e01536ac207279409d4de1e5253e01f4a1769e696db0d6062ca9b8f56767c8");
        }

        [TestMethod]
        public void TestAuthenticateUserWorksWithValidAccount()
        {
            var result = userManager.AuthenticateUser("Test2", "mypassword");

            Assert.AreEqual(true, result.Item1);
            Assert.AreEqual("Authenticated", result.Item2);
        }

        [TestMethod]
        public void TestAuthenticateUserFailsWithInvalidAccount()
        {
            var result = userManager.AuthenticateUser("username", "mypassword");
            Assert.AreEqual(false, result.Item1);
            Assert.AreEqual("Invalid username or password", result.Item2);
        }

        [TestMethod]
        public void TestAuthenticateUserFailsWithExpiredAccount()
        {
            var result = userManager.AuthenticateUser("Test1", "mypassword");
            Assert.AreEqual(false, result.Item1);
            Assert.AreEqual("Account is expired", result.Item2);
        }

        [TestMethod]
        public void TestAuthenticateUserFailsWithDisabledAccount()
        {
            var result = userManager.AuthenticateUser("Test", "mypassword");
            Assert.AreEqual(false, result.Item1);
            Assert.AreEqual("Account is disabled", result.Item2);
        }


        [TestMethod]
        public void TestGetCorrectRegisterToken()
        {
            RegisterToken result = userManager.GetToken(new Guid("333702f9-859a-4dac-92b0-638999a666a8"));

            Assert.AreEqual(new Guid("333702f9-859a-4dac-92b0-638999a666a8"), result.Token);
        }

        [TestMethod]
        public void TestUpdateUserSettings()
        {
            User user = new User()
            { 
                Username = "Test2",
                Settings = new UserSettings()
                {
                    EnableMOTD = false
                }
            };
            bool result = userManager.UpdateUserSettings(user);
            Assert.AreEqual(true, result);
        }
        [TestMethod]
        public void TestUpdateInvalidUserSettings()
        {
            User user = new User()
            {
                Username = "Test4",
                Settings = new UserSettings()
                {
                    EnableMOTD = false
                }
            };
            bool result = userManager.UpdateUserSettings(user);
            Assert.AreEqual(false, result);
        }
    }
}