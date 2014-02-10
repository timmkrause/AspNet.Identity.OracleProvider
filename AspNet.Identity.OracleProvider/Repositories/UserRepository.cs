// Copyright (c) Timm Krause. All rights reserved. See LICENSE file in the project root for license information.

namespace AspNet.Identity.OracleProvider.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Oracle.ManagedDataAccess.Client;

    internal class UserRepository
    {
        private readonly OracleDataContext _db;

        public UserRepository(OracleDataContext oracleContext)
        {
            _db = oracleContext;
        }

        public int Insert(IdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return _db.ExecuteNonQuery(
                "INSERT INTO users (id, username, passwordhash, securitystamp) VALUES (:id, :name, :passwordhash, :securitystamp)",
                new OracleParameter { ParameterName = ":id", Value = user.Id, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input },
                new OracleParameter { ParameterName = ":name", Value = user.UserName, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input },
                new OracleParameter { ParameterName = ":passwordhash", Value = user.PasswordHash, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input },
                new OracleParameter { ParameterName = ":securitystamp", Value = user.SecurityStamp, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input });
        }

        public int Update(IdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return _db.ExecuteNonQuery(
                "UPDATE users SET username = :userName, passwordhash = :passwordhash, securitystamp = :securitystamp WHERE id = :userid",
                new OracleParameter { ParameterName = ":username", Value = user.UserName, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input },
                new OracleParameter { ParameterName = ":passwordhash", Value = user.PasswordHash, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input },
                new OracleParameter { ParameterName = ":securitystamp", Value = user.SecurityStamp, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input },
                new OracleParameter { ParameterName = ":userid", Value = user.Id, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input });
        }

        public int Delete(string userId)
        {
            return _db.ExecuteNonQuery(
                "DELETE FROM users WHERE id = :userid",
                new OracleParameter { ParameterName = ":userid", Value = userId, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input });
        }

        public int Delete(IdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Delete(user.Id);
        }

        ////public string GetUserName(string userId)
        ////{
        ////    return _db.ExecuteScalarQuery<string>(
        ////        "SELECT name FROM users WHERE id = :id",
        ////        new OracleParameter { ParameterName = ":id", Value = userId, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input });
        ////}

        ////public string GetUserId(string userName)
        ////{
        ////    return _db.ExecuteScalarQuery<string>(
        ////       "SELECT id FROM users WHERE username = :name",
        ////       new OracleParameter { ParameterName = ":name", Value = userName, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input });
        ////}

        public IdentityUser GetUserById(string userId)
        {
            var result = _db.ExecuteQuery(
              "SELECT * FROM users WHERE id = :id",
              new OracleParameter { ParameterName = ":id", Value = userId, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input });

            var row = result.Rows.Cast<DataRow>().SingleOrDefault();

            if (row != null)
            {
                return new IdentityUser
                {
                    Id = row["id"].ToString(),
                    UserName = row["username"].ToString(),
                    PasswordHash = string.IsNullOrEmpty(row["passwordhash"].ToString()) ? null : row["passwordhash"].ToString(),
                    SecurityStamp = string.IsNullOrEmpty(row["securitystamp"].ToString()) ? null : row["securitystamp"].ToString()
                };
            }

            return null;
        }

        public ICollection<IdentityUser> GetUserByName(string userName)
        {
            var result = _db.ExecuteQuery(
                "SELECT * FROM users WHERE username = :name",
                new OracleParameter { ParameterName = ":name", Value = userName, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input });

            return result.Rows.Cast<DataRow>().Select(
                r => new IdentityUser
                {
                    Id = r["id"].ToString(),
                    UserName = r["username"].ToString(),
                    PasswordHash = string.IsNullOrEmpty(r["passwordhash"].ToString()) ? null : r["passwordhash"].ToString(),
                    SecurityStamp = string.IsNullOrEmpty(r["securitystamp"].ToString()) ? null : r["securitystamp"].ToString()
                }).ToList();
        }

        public string GetPasswordHash(string userId)
        {
            var passwordHash = _db.ExecuteScalarQuery<string>(
                "SELECT passwordhash FROM users WHERE id = :id",
                new OracleParameter { ParameterName = ":id", Value = userId, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input });

            return string.IsNullOrEmpty(passwordHash) ? null : passwordHash;
        }

        ////public int SetPasswordHash(string userId, string passwordHash)
        ////{
        ////    return _db.ExecuteScalarQuery<int>(
        ////        "UPDATE users SET passwordhash = :passwordhash WHERE id = :id",
        ////        new OracleParameter { ParameterName = ":passwordhash", Value = passwordHash, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input },
        ////        new OracleParameter { ParameterName = ":id", Value = userId, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input });
        ////}

        ////public string GetSecurityStamp(string userId)
        ////{
        ////    return _db.ExecuteScalarQuery<string>(
        ////        "SELECT securitystamp FROM users WHERE id = :id",
        ////        new OracleParameter { ParameterName = ":id", Value = userId, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input });
        ////}
    }
}
