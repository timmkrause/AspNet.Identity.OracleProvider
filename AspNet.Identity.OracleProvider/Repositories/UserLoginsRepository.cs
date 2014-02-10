// Copyright (c) Timm Krause. All rights reserved. See LICENSE file in the project root for license information.

namespace AspNet.Identity.OracleProvider.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Oracle.ManagedDataAccess.Client;

    internal class UserLoginsRepository
    {
        private readonly OracleDataContext _db;

        public UserLoginsRepository(OracleDataContext oracleContext)
        {
            _db = oracleContext;
        }

        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "login", Justification = "Needless.")]
        public int Insert(IdentityUser user, UserLoginInfo login)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (login == null)
            {
                throw new ArgumentNullException("login");
            }

            return _db.ExecuteNonQuery(
                "INSERT INTO userlogins (userid, loginprovider, providerkey) VALUES (:userid, :loginprovider, :providerkey)",
                new OracleParameter { ParameterName = ":userid", Value = user.Id, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input },
                new OracleParameter { ParameterName = ":loginprovider", Value = login.LoginProvider, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input },
                new OracleParameter { ParameterName = ":providerkey", Value = login.ProviderKey, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input });
        }

        ////public int Delete(string userId)
        ////{
        ////    return _db.ExecuteScalarQuery<int>(
        ////       "DELETE FROM userlogins WHERE userid = :userid",
        ////       new OracleParameter { ParameterName = ":userid", Value = userId, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input });
        ////}

        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "login", Justification = "Needless.")]
        public int Delete(IdentityUser user, UserLoginInfo login)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (login == null)
            {
                throw new ArgumentNullException("login");
            }

            return _db.ExecuteNonQuery(
                "DELETE FROM userlogins WHERE userid = :userid AND loginprovider = :loginprovider AND providerkey = :providerkey",
                new OracleParameter { ParameterName = ":userid", Value = user.Id, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input },
                new OracleParameter { ParameterName = ":loginprovider", Value = login.LoginProvider, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input },
                new OracleParameter { ParameterName = ":providerkey", Value = login.ProviderKey, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input });
        }

        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Login", Justification = "Needless.")]
        public string FindUserIdByLogin(UserLoginInfo userLogin)
        {
            if (userLogin == null)
            {
                throw new ArgumentNullException("userLogin");
            }

            return _db.ExecuteScalarQuery<string>(
               "SELECT userid FROM userlogins WHERE loginprovider = :loginprovider AND providerkey = :providerkey",
               new OracleParameter { ParameterName = ":loginprovider", Value = userLogin.LoginProvider, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input },
               new OracleParameter { ParameterName = ":providerkey", Value = userLogin.ProviderKey, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input });
        }

        public IList<UserLoginInfo> FindByUserId(string userId)
        {
            var result = _db.ExecuteQuery(
               "SELECT * FROM userlogins WHERE userid = :userid",
               new OracleParameter { ParameterName = ":userid", Value = userId, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input });

            return result.Rows.Cast<DataRow>().Select(row => new UserLoginInfo(row["loginprovider"].ToString(), row["providerkey"].ToString())).ToList();
        }
    }
}
