// Copyright (c) Timm Krause. All rights reserved. See LICENSE file in the project root for license information.

namespace AspNet.Identity.OracleProvider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Repositories;

    public class UserStore :
        IUserStore<IdentityUser>,
        IUserClaimStore<IdentityUser>,
        IUserLoginStore<IdentityUser>,
        IUserRoleStore<IdentityUser>,
        IUserPasswordStore<IdentityUser>
    {
        private readonly UserRepository _userRepository;
        private readonly UserClaimsRepository _userClaimsRepository;
        private readonly UserLoginsRepository _userLoginsRepository;
        private readonly RoleRepository _roleRepository;
        private readonly UserRolesRepository _userRolesRepository;

        public UserStore()
            : this(new OracleDataContext())
        {
        }

        public UserStore(OracleDataContext database)
        {
            // TODO: Compare with EntityFramework provider.
            Database = database;

            _userRepository = new UserRepository(database);
            _roleRepository = new RoleRepository(database);
            _userRolesRepository = new UserRolesRepository(database);
            _userClaimsRepository = new UserClaimsRepository(database);
            _userLoginsRepository = new UserLoginsRepository(database);
        }

        public OracleDataContext Database { get; private set; }

        public Task CreateAsync(IdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            _userRepository.Insert(user);

            return Task.FromResult<object>(null);
        }

        public Task DeleteAsync(IdentityUser user)
        {
            if (user != null)
            {
                _userRepository.Delete(user);
            }

            return Task.FromResult<object>(null);
        }

        public Task<IdentityUser> FindByIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("Null or empty argument: userId");
            }

            var result = _userRepository.GetUserById(userId);

            return Task.FromResult(result);
        }

        public Task<IdentityUser> FindByNameAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentException("Null or empty argument: userName");
            }

            var result = _userRepository.GetUserByName(userName).SingleOrDefault();

            // Should I throw if > 1 user?
            return Task.FromResult(result);
        }

        public Task UpdateAsync(IdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            _userRepository.Update(user);

            return Task.FromResult<object>(null);
        }

        public Task AddClaimAsync(IdentityUser user, Claim claim)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (claim == null)
            {
                throw new ArgumentNullException("claim");
            }

            _userClaimsRepository.Insert(claim, user.Id);

            return Task.FromResult<object>(null);
        }

        public Task<IList<Claim>> GetClaimsAsync(IdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            var claimsIdentity = _userClaimsRepository.FindByUserId(user.Id);

            return Task.FromResult<IList<Claim>>(claimsIdentity.Claims.ToList());
        }

        public Task RemoveClaimAsync(IdentityUser user, Claim claim)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (claim == null)
            {
                throw new ArgumentNullException("claim");
            }

            _userClaimsRepository.Delete(user, claim);

            return Task.FromResult<object>(null);
        }

        public Task AddLoginAsync(IdentityUser user, UserLoginInfo login)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (login == null)
            {
                throw new ArgumentNullException("login");
            }

            _userLoginsRepository.Insert(user, login);

            return Task.FromResult<object>(null);
        }

        public Task<IdentityUser> FindAsync(UserLoginInfo login)
        {
            if (login == null)
            {
                throw new ArgumentNullException("login");
            }

            var userId = _userLoginsRepository.FindUserIdByLogin(login);

            if (userId != null)
            {
                var user = _userRepository.GetUserById(userId);

                if (user != null)
                {
                    return Task.FromResult(user);
                }
            }

            return Task.FromResult<IdentityUser>(null);
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(IdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            var userLogins = _userLoginsRepository.FindByUserId(user.Id);

            return Task.FromResult(userLogins);
        }

        public Task RemoveLoginAsync(IdentityUser user, UserLoginInfo login)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (login == null)
            {
                throw new ArgumentNullException("login");
            }

            _userLoginsRepository.Delete(user, login);

            return Task.FromResult<object>(null);
        }

        public Task AddToRoleAsync(IdentityUser user, string role)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrEmpty(role))
            {
                throw new ArgumentNullException("role");
            }

            var roleId = _roleRepository.GetRoleId(role);

            if (!string.IsNullOrEmpty(roleId))
            {
                _userRolesRepository.Insert(user, roleId);
            }

            return Task.FromResult<object>(null);
        }

        public Task<IList<string>> GetRolesAsync(IdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            var roles = _userRolesRepository.FindByUserId(user.Id);

            return Task.FromResult(roles);
        }

        public Task<bool> IsInRoleAsync(IdentityUser user, string role)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrEmpty(role))
            {
                throw new ArgumentNullException("role");
            }

            var roles = _userRolesRepository.FindByUserId(user.Id);

            return Task.FromResult(roles != null && roles.Contains(role));
        }

        public Task RemoveFromRoleAsync(IdentityUser user, string role)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrEmpty(role))
            {
                throw new ArgumentNullException("role");
            }

            var roleId = _roleRepository.GetRoleId(role);

            if (!string.IsNullOrEmpty(roleId))
            {
                _userRolesRepository.Delete(user, roleId);
            }

            return Task.FromResult<object>(null);
        }

        public Task<string> GetPasswordHashAsync(IdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            var passwordHash = _userRepository.GetPasswordHash(user.Id);

            return Task.FromResult(passwordHash);
        }

        public Task<bool> HasPasswordAsync(IdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            var hasPassword = !string.IsNullOrEmpty(_userRepository.GetPasswordHash(user.Id));

            return Task.FromResult(hasPassword);
        }

        public Task SetPasswordHashAsync(IdentityUser user, string passwordHash)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.PasswordHash = passwordHash;

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
