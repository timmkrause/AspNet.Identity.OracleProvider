// Copyright (c) Timm Krause. All rights reserved. See LICENSE file in the project root for license information.

namespace AspNet.Identity.OracleProvider.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Oracle.ManagedDataAccess.Client;

    internal class UserRolesRepository
    {
        private readonly OracleDataContext _db;

        public UserRolesRepository(OracleDataContext database)
        {
            _db = database;
        }

        public int Insert(IdentityUser user, string roleId)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return _db.ExecuteNonQuery(
                "INSERT INTO userroles (userid, roleid) values (:userid, :roleid)",
                new OracleParameter { ParameterName = ":userid", Value = user.Id, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input },
                new OracleParameter { ParameterName = ":roleid", Value = roleId, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input });
        }

        ////public int Delete(string userId)
        ////{
        ////    return _db.ExecuteScalarQuery<int>(
        ////       "DELETE FROM userroles WHERE userid = :userid",
        ////       new OracleParameter { ParameterName = ":userid", Value = userId, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input });
        ////}

        public int Delete(string userId, string roleId)
        {
            return _db.ExecuteNonQuery(
               "DELETE FROM userroles WHERE userid = :userid AND roleid = :roleid",
               new OracleParameter { ParameterName = ":userid", Value = userId, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input },
               new OracleParameter { ParameterName = ":roleid", Value = roleId, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input });
        }

        public int Delete(IdentityUser user, string roleId)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (roleId.HasNoValue())
            {
                throw new ArgumentNullException("roleId");
            }

            return Delete(user.Id, roleId);
        }

        public IList<string> FindByUserId(string userId)
        {
            var result = _db.ExecuteQuery(
                "SELECT roles.name FROM userroles, roles WHERE userroles.userid = :userid AND userroles.roleid = roles.id",
                new OracleParameter { ParameterName = ":userid", Value = userId, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input });

            return result.Rows.Cast<DataRow>().Select(row => row["name"].ToString()).ToList();
        }
    }
}
