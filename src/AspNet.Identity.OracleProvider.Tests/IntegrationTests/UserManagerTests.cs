// Copyright (c) Timm Krause. All rights reserved. See LICENSE file in the project root for license information.

namespace AspNet.Identity.OracleProvider.Tests
{
    using System;
    using Microsoft.AspNet.Identity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Oracle.ManagedDataAccess.Client;

    [TestClass]
    public class UserManagerTests
    {
        private const string _password = "123456";

        private UserStore _userStore;
        private UserManager<IdentityUser> _userManager;

        [TestInitialize]
        public void TestInitialize()
        {
            var oracleDataContext = new OracleDataContext(new OracleConnection(Configuration.ConnectionString));

            _userStore = new UserStore(oracleDataContext);
            _userManager = new UserManager<IdentityUser>(_userStore);
        }

        [TestMethod]
        public void CreateShouldReturnSucceededResult()
        {
            var result = _userManager.Create(new IdentityUser { UserName = GetNewUserName() }, _password);

            Assert.IsTrue(result.Succeeded);
        }

        private string GetNewUserName()
        {
            return Guid.NewGuid().ToString().Replace("-", string.Empty);
        }
    }
}
