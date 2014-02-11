// Copyright (c) Timm Krause. All rights reserved. See LICENSE file in the project root for license information.

namespace AspNet.Identity.OracleProvider.Tests.IntegrationTests
{
    using System;
    using Microsoft.AspNet.Identity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Oracle.ManagedDataAccess.Client;

    [TestClass]
    public class RoleManagerTests
    {
        private const string _password = "123456";
        private const string _roleName = "Administrators";

        private UserStore _userStore;
        private RoleStore _roleStore;
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        [TestInitialize]
        public void TestInitialize()
        {
            var oracleDataContext = new OracleDataContext(new OracleConnection(Configuration.ConnectionString));

            _userStore = new UserStore(oracleDataContext);
            _roleStore = new RoleStore(oracleDataContext);
            _userManager = new UserManager<IdentityUser>(_userStore);
            _roleManager = new RoleManager<IdentityRole>(_roleStore);
        }

        [TestMethod]
        public void CreateShouldReturnSucceededResult()
        {
            var result = _roleManager.Create<IdentityRole>(new IdentityRole("Foo"));

            Assert.IsTrue(result.Succeeded);
        }

        [TestMethod]
        public void AddToRoleWithValidUserShouldAddUserToRole()
        {
            var userName = GetNewUserName();
            var userResult = _userManager.Create(new IdentityUser { UserName = userName }, _password);
            var user = _userManager.FindByName(userName);
            var roleResult = _roleManager.Create<IdentityRole>(new IdentityRole(_roleName));

            var addUserToRoleResult = _userManager.AddToRole<IdentityUser>(user.Id, _roleName);

            Assert.IsTrue(addUserToRoleResult.Succeeded);
            Assert.IsTrue(_userManager.IsInRole<IdentityUser>(user.Id, _roleName));
        }

        private string GetNewUserName()
        {
            return Guid.NewGuid().ToString().Replace("-", string.Empty);
        }
    }
}
