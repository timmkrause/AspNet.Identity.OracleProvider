// Copyright (c) Timm Krause. All rights reserved. See LICENSE file in the project root for license information.

namespace AspNet.Identity.OracleProvider.Tests
{
    using AspNet.Identity.OracleProvider;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ExtensionsTests
    {
        [TestMethod]
        public void HasValueShouldReturnTrue()
        {
            var value = "Foo";

            Assert.IsTrue(value.HasValue());
        }

        [TestMethod]
        public void HasValueShouldReturnFalse()
        {
            var value = string.Empty;

            Assert.IsFalse(value.HasValue());
        }

        [TestMethod]
        public void HasNoValueShouldReturnTrue()
        {
            var value = string.Empty;

            Assert.IsTrue(value.HasNoValue());
        }

        [TestMethod]
        public void HasNoValueShouldReturnFalse()
        {
            var value = "Foo";

            Assert.IsFalse(value.HasNoValue());
        }
    }
}
