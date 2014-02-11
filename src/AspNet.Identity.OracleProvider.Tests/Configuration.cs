// Copyright (c) Timm Krause. All rights reserved. See LICENSE file in the project root for license information.

namespace AspNet.Identity.OracleProvider.Tests
{
    public class Configuration
    {
        // NOTE: Be careful. NEVER EVER run the tests against a production database.
        //       The cleanup process purges the identity tables completely.
        public const string ConnectionString = "Data Source=TODO; User ID=TODO; Password=TODO";
    }
}
