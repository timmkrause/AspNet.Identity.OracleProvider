// Copyright (c) Timm Krause. All rights reserved. See LICENSE file in the project root for license information.

namespace AspNet.Identity.OracleProvider.Tests
{
    using AspNet.Identity.OracleProvider;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class IdentityRoleTests
    {
        [TestMethod]
        public void ConstructorShouldSetProperties()
        {
            var role = new IdentityRole("RoleName", "RoleId");

            Assert.AreEqual("RoleId", role.Id);
            Assert.AreEqual("RoleName", role.Name);
        }
    }
}
