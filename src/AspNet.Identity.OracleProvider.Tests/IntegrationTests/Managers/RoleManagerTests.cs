// Copyright (c) Timm Krause. All rights reserved. See LICENSE file in the project root for license information.

namespace AspNet.Identity.OracleProvider.Tests.IntegrationTests.Managers
{
    using System;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Oracle.ManagedDataAccess.Client;

    [TestClass]
    public class RoleManagerTests
    {
        private const string _password = "123456";

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
            var result = _roleManager.Create<IdentityRole>(new IdentityRole(GetNewRandomName()));

            Assert.IsTrue(result.Succeeded);
        }

        [TestMethod]
        public void UpdateShouldUpdateRoleName()
        {
            var role = new IdentityRole(GetNewRandomName());

            _roleManager.Create(role);

            var retrievedRole = _roleManager.FindById(role.Id);
            var newRoleName = GetNewRandomName();

            retrievedRole.Name = newRoleName;

            _roleManager.Update(retrievedRole);

            retrievedRole = _roleManager.FindById(role.Id);

            Assert.AreEqual(newRoleName, retrievedRole.Name);
        }

        [TestMethod]
        public void IsInRoleShouldReturnTrueIfUserIsInRole()
        {
            var user = new IdentityUser(GetNewRandomName());
            var role = new IdentityRole(GetNewRandomName());

            _userManager.Create(user, _password);
            _roleManager.Create<IdentityRole>(role);
            _userManager.AddToRole<IdentityUser>(user.Id, role.Name);

            Assert.IsTrue(_userManager.IsInRole<IdentityUser>(user.Id, role.Name));
        }

        [TestMethod]
        public void IsInRoleShouldReturnFalseIfUserIsNotInRole()
        {
            var user = new IdentityUser(GetNewRandomName());
            var roleName = GetNewRandomName();

            _userManager.Create(user, _password);

            Assert.IsFalse(_userManager.IsInRole<IdentityUser>(user.Id, roleName));
        }

        [TestMethod]
        public void GetRolesForUserShouldRetrieveCorrectRoles()
        {
            var role1 = new IdentityRole(GetNewRandomName());
            var role2 = new IdentityRole(GetNewRandomName());

            _roleManager.Create<IdentityRole>(role1);
            _roleManager.Create<IdentityRole>(role2);

            var user1 = new IdentityUser(GetNewRandomName());
            var user2 = new IdentityUser(GetNewRandomName());
            var user3 = new IdentityUser(GetNewRandomName());

            _userManager.Create(user1, _password);
            _userManager.Create(user2, _password);
            _userManager.Create(user3, _password);
            _userManager.AddToRole(user1.Id, role1.Name);
            _userManager.AddToRole(user1.Id, role2.Name);
            _userManager.AddToRole(user2.Id, role1.Name);
            _userManager.AddToRole(user3.Id, role2.Name);

            var user1RoleNames = _userManager.GetRoles(user1.Id);
            var user2RoleNames = _userManager.GetRoles(user2.Id);
            var user3RoleNames = _userManager.GetRoles(user3.Id);

            Assert.AreEqual(2, user1RoleNames.Count);
            Assert.AreEqual(1, user2RoleNames.Count);
            Assert.AreEqual(1, user3RoleNames.Count);

            Assert.IsTrue(user1RoleNames.Any(r => r == role1.Name));
            Assert.IsTrue(user1RoleNames.Any(r => r == role2.Name));
            Assert.IsTrue(user2RoleNames.Any(r => r == role1.Name));
            Assert.IsTrue(user3RoleNames.Any(r => r == role2.Name));
        }

        [TestMethod]
        public void GetRolesForUserShouldNotReturnNullIfUserHasNoRoles()
        {
            var user = new IdentityUser(GetNewRandomName());

            _userManager.Create(user, _password);

            var roleNames = _userManager.GetRoles(user.Id);

            Assert.AreEqual(0, roleNames.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetRolesForUserShouldThrowIfUserDoesNotExist()
        {
            _userManager.GetRoles(GetNewRandomName());
        }

        private string GetNewRandomName()
        {
            return Guid.NewGuid().ToString().Replace("-", string.Empty);
        }
    }
}
