// Copyright (c) Timm Krause. All rights reserved. See LICENSE file in the project root for license information.

namespace AspNet.Identity.OracleProvider
{
    using System;
    using Microsoft.AspNet.Identity;

    public class IdentityRole : IRole
    {
        public IdentityRole()
        {
            Id = Guid.NewGuid().ToString();
        }

        public IdentityRole(string name)
            : this()
        {
            Name = name;
        }

        public IdentityRole(string name, string id)
        {
            Name = name;
            Id = id;
        }

        public string Id { get; set; }

        public string Name { get; set; }
    }
}
