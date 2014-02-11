// Copyright (c) Timm Krause. All rights reserved. See LICENSE file in the project root for license information.

namespace AspNet.Identity.OracleProvider.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Oracle.ManagedDataAccess.Client;

    [TestClass]
    public class AssemblyCleanupProcess
    {
        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            var context = new OracleDataContext(new OracleConnection(Configuration.ConnectionString));

            PurgeIdentityTables(context);
        }

        public static void PurgeIdentityTables(OracleDataContext context)
        {
            context.ExecuteNonQuery("DELETE FROM userroles");
            context.ExecuteNonQuery("DELETE FROM userlogins");
            context.ExecuteNonQuery("DELETE FROM userclaims");
            context.ExecuteNonQuery("DELETE FROM roles");
            context.ExecuteNonQuery("DELETE FROM users");
        }
    }
}
