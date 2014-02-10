// Copyright (c) Timm Krause. All rights reserved. See LICENSE file in the project root for license information.

namespace AspNet.Identity.OracleProvider
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Repositories;

    public class RoleStore : IRoleStore<IdentityRole>
    {
        private readonly RoleRepository _roleRepository;

        public RoleStore()
            : this(new OracleDataContext())
        {
        }

        public RoleStore(OracleDataContext oracleContext)
        {
            Database = oracleContext;

            _roleRepository = new RoleRepository(oracleContext);
        }

        public OracleDataContext Database { get; private set; }

        public Task CreateAsync(IdentityRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            _roleRepository.Insert(role);

            return Task.FromResult<object>(null);
        }

        public Task DeleteAsync(IdentityRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            _roleRepository.Delete(role.Id);

            return Task.FromResult<object>(null);
        }

        public Task<IdentityRole> FindByIdAsync(string roleId)
        {
            var result = _roleRepository.GetRoleById(roleId);

            return Task.FromResult(result);
        }

        public Task<IdentityRole> FindByNameAsync(string roleName)
        {
            var result = _roleRepository.GetRoleByName(roleName);

            return Task.FromResult(result);
        }

        public Task UpdateAsync(IdentityRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            _roleRepository.Update(role);

            return Task.FromResult<object>(null);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Database != null)
                {
                    Database.Dispose();
                    Database = null;
                }
            }
        }
    }
}
