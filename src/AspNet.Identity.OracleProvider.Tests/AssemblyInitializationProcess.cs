// Copyright (c) Timm Krause. All rights reserved. See LICENSE file in the project root for license information.

namespace AspNet.Identity.OracleProvider.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Oracle.ManagedDataAccess.Client;

    [TestClass]
    public class AssemblyInitializationProcess
    {
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext testContext)
        {
            var context = new OracleDataContext(new OracleConnection(Configuration.ConnectionString));

            AssemblyCleanupProcess.PurgeIdentityTables(context);
        }
    }
}
