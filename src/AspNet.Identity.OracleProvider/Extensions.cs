// Copyright (c) Timm Krause. All rights reserved. See LICENSE file in the project root for license information.

namespace AspNet.Identity.OracleProvider
{
    internal static class Extensions
    {
        public static bool HasValue(this string value)
        {
            return !value.HasNoValue();
        }

        public static bool HasNoValue(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }
    }
}
