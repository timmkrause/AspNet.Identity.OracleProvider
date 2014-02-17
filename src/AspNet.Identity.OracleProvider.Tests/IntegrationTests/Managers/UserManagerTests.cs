// Copyright (c) Timm Krause. All rights reserved. See LICENSE file in the project root for license information.

namespace AspNet.Identity.OracleProvider.Tests.Managers
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using Microsoft.AspNet.Identity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Oracle.ManagedDataAccess.Client;

    [TestClass]
    public class UserManagerTests
    {
        private const string _password = "123456";

        private UserStore _userStore;
        private UserManager<IdentityUser> _userManager;

        [TestInitialize]
        public void TestInitialize()
        {
            var oracleDataContext = new OracleDataContext(new OracleConnection(Configuration.ConnectionString));

            _userStore = new UserStore(oracleDataContext);
            _userManager = new UserManager<IdentityUser>(_userStore);
        }

        [TestMethod]
        public void CreateShouldReturnSucceededResult()
        {
            var result = _userManager.Create(new IdentityUser(GetNewRandomName()), _password);

            Assert.IsTrue(result.Succeeded);
        }

        [TestMethod]
        public void FindWithCorrectPasswordShouldReturnExpectedUser()
        {
            var user = new IdentityUser(GetNewRandomName());

            _userManager.Create(user, _password);

            var foundUser = _userManager.Find(user.UserName, _password);

            Assert.IsNotNull(foundUser);
            Assert.AreEqual(user.Id, foundUser.Id);
            Assert.AreEqual(user.UserName, foundUser.UserName);
        }

        [TestMethod]
        public void FindWithIncorrectPasswordShouldReturnNull()
        {
            var user = new IdentityUser(GetNewRandomName());

            _userManager.Create(user, _password);

            var foundUser = _userManager.Find(user.UserName, "wrongPassword");

            Assert.IsNull(foundUser);
        }

        [TestMethod]
        public void AddLoginVerificationTests()
        {
            var newUser = new IdentityUser(GetNewRandomName());
            var googleLoginProvider = "Google";
            var googleLoginProviderKey = "http://www.google.com/fake/user/identifier";
            var yahooLoginProvider = "Yahoo";
            var yahooLoginProviderKey = "http://www.yahoo.com/fake/user/identifier";

            var userCreationResult = _userManager.Create(newUser, _password);

            Assert.IsTrue(userCreationResult.Succeeded);
            Assert.IsNotNull(newUser.Id);

            var googleLoginResult = _userManager.AddLogin(newUser.Id, new UserLoginInfo(googleLoginProvider, googleLoginProviderKey));
            var yahooLoginResult = _userManager.AddLogin(newUser.Id, new UserLoginInfo(yahooLoginProvider, yahooLoginProviderKey));

            Assert.IsTrue(googleLoginResult.Succeeded);
            Assert.IsTrue(yahooLoginResult.Succeeded);

            var newUserLogins = _userManager.GetLogins(newUser.Id);

            Assert.AreEqual(newUserLogins.Count, 2);
            Assert.IsTrue(newUserLogins.Any(l => l.LoginProvider == googleLoginProvider && l.ProviderKey == googleLoginProviderKey));
            Assert.IsTrue(newUserLogins.Any(l => l.LoginProvider == yahooLoginProvider && l.ProviderKey == yahooLoginProviderKey));

            var userByName = _userManager.Find(newUser.UserName, _password);
            var userByGoogle = _userManager.Find(new UserLoginInfo(googleLoginProvider, googleLoginProviderKey));
            var userByYahoo = _userManager.Find(new UserLoginInfo(yahooLoginProvider, yahooLoginProviderKey));

            Assert.IsNotNull(userByName);
            Assert.IsNotNull(userByGoogle);
            Assert.IsNotNull(userByYahoo);
            Assert.AreEqual(userByName.UserName, newUser.UserName);
        }

        [TestMethod]
        public void FindShouldReturnUserAfterChangePassword()
        {
            var user = new IdentityUser(GetNewRandomName());

            _userManager.Create(user, _password);

            Assert.IsNotNull(_userManager.Find(user.UserName, _password));

            var newPassword = GetNewRandomName();

            _userManager.ChangePassword(user.Id, _password, newPassword);

            Assert.IsNull(_userManager.Find(user.UserName, _password));
            Assert.IsNotNull(_userManager.Find(user.UserName, newPassword));
        }

        [TestMethod]
        public void GetUserClaimsShouldReturnCorrectClaimsForUser()
        {
            var user = new IdentityUser(GetNewRandomName());

            _userManager.Create(user);
            _userManager.AddClaim(user.Id, new Claim("Scope", "Read"));
            _userManager.AddClaim(user.Id, new Claim("Scope", "Write"));

            var userClaims = _userManager.GetClaims(user.Id);

            Assert.AreEqual(2, userClaims.Count);
            Assert.AreEqual("Read", userClaims.ElementAt(0).Value);
            Assert.AreEqual("Write", userClaims.ElementAt(1).Value);
        }

        [TestMethod]
        public void GetUserClaimsShouldNotReturnNullIfUserHasNoClaims()
        {
            var user = new IdentityUser(GetNewRandomName());

            _userManager.Create(user);

            var userClaims = _userManager.GetClaims(user.Id);

            Assert.AreEqual(0, userClaims.Count);
        }

        [TestMethod]
        public void AddClaimShouldAddTheClaimIntoTheUserClaimsCollection()
        {
            var user = new IdentityUser(GetNewRandomName());

            _userManager.Create(user);

            var claimToAdd = new Claim(ClaimTypes.Role, "Customer");

            _userManager.AddClaim(user.Id, claimToAdd);

            var userClaims = _userManager.GetClaims(user.Id);

            Assert.AreEqual(1, userClaims.Count);
            Assert.AreEqual(claimToAdd.Value, userClaims.Single().Value);
            Assert.AreEqual(claimToAdd.Type, userClaims.Single().Type);
        }

        [TestMethod]
        public void RemoveClaimShouldRemoveClaimFromTheUserClaimsCollection()
        {
            var user = new IdentityUser(GetNewRandomName());

            _userManager.Create(user);

            var claimToAddAndRemove = new Claim(ClaimTypes.Role, "Customer");

            _userManager.AddClaim(user.Id, claimToAddAndRemove);
            _userManager.RemoveClaim(user.Id, claimToAddAndRemove);

            var userClaims = _userManager.GetClaims(user.Id);

            Assert.AreEqual(0, userClaims.Count);
        }

        private string GetNewRandomName()
        {
            return Guid.NewGuid().ToString().Replace("-", string.Empty);
        }
    }
}
