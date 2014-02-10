// Copyright (c) Timm Krause. All rights reserved. See LICENSE file in the project root for license information.

namespace AspNet.Identity.OracleProvider.Tests
{
    using Microsoft.AspNet.Identity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Oracle.ManagedDataAccess.Client;

    [TestClass]
    public class UserManagerTests
    {
        private const string _connectionString = "";

        [TestMethod]
        public void CreateShouldReturnSucceededResult()
        {
            var userStore = new UserStore(new OracleDataContext(new OracleConnection(_connectionString)));
            var userManager = new UserManager<IdentityUser>(userStore);

            var result = userManager.Create(new IdentityUser { UserName = "timmkrause" }, "123456");

            Assert.IsTrue(result.Succeeded);
        }
    }
}
