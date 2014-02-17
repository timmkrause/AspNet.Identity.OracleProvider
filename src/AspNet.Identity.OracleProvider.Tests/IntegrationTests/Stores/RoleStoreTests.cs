// Copyright (c) Timm Krause. All rights reserved. See LICENSE file in the project root for license information.

namespace AspNet.Identity.OracleProvider.Tests.IntegrationTests.Stores
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Oracle.ManagedDataAccess.Client;

    [TestClass]
    public class RoleStoreTests
    {
        private RoleStore _roleStore;

        [TestInitialize]
        public void TestInitialize()
        {
            var oracleDataContext = new OracleDataContext(new OracleConnection(Configuration.ConnectionString));

            _roleStore = new RoleStore(oracleDataContext);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task CreateWithNullRoleShouldThrow()
        {
            await _roleStore.CreateAsync(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task DeleteAsyncWithNullRoleShouldThrow()
        {
            await _roleStore.DeleteAsync(null);
        }

        [TestMethod]
        public async Task DeleteAsyncShouldDeleteRole()
        {
            var role = new IdentityRole(GetNewRandomName());

            await _roleStore.CreateAsync(role);
            await _roleStore.DeleteAsync(role);

            var retrievedRole = await _roleStore.FindByIdAsync(role.Id);

            Assert.IsNull(retrievedRole);
        }

        private string GetNewRandomName()
        {
            return Guid.NewGuid().ToString().Replace("-", string.Empty);
        }
    }
}
