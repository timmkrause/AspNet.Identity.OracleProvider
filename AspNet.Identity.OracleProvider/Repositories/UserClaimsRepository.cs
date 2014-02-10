// Copyright (c) Timm Krause. All rights reserved. See LICENSE file in the project root for license information.

namespace AspNet.Identity.OracleProvider.Repositories
{
    using System;
    using System.Data;
    using System.Linq;
    using System.Security.Claims;
    using Oracle.ManagedDataAccess.Client;

    internal class UserClaimsRepository
    {
        private readonly OracleDataContext _db;

        public UserClaimsRepository(OracleDataContext oracleContext)
        {
            _db = oracleContext;
        }

        public int Insert(Claim userClaim, string userId)
        {
            if (userClaim == null)
            {
                throw new ArgumentNullException("userClaim");
            }

            return _db.ExecuteNonQuery(
                "INSERT INTO userclaims (userid, claimtype, claimvalue) VALUES (:userId, :type, :value)",
                new OracleParameter { ParameterName = ":userId", Value = userId, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input },
                new OracleParameter { ParameterName = ":type", Value = userClaim.Type, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input },
                new OracleParameter { ParameterName = ":value", Value = userClaim.Value, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input });
        }

        ////public int Delete(string userId)
        ////{
        ////    return _db.ExecuteScalarQuery<int>(
        ////        "DELETE FROM userclaims WHERE userid = :userid",
        ////        new OracleParameter { ParameterName = ":userid", Value = userId, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input });
        ////}

        public int Delete(IdentityUser user, Claim claim)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (claim == null)
            {
                throw new ArgumentNullException("claim");
            }

            return _db.ExecuteNonQuery(
                "DELETE FROM userclaims WHERE userid = :userid AND claimtype = :type AND claimvalue = :value",
                new OracleParameter { ParameterName = ":userid", Value = user.Id, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input },
                new OracleParameter { ParameterName = ":type", Value = claim.Type, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input },
                new OracleParameter { ParameterName = ":value", Value = claim.Value, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input });
        }

        public ClaimsIdentity FindByUserId(string userId)
        {
            var result = _db.ExecuteQuery(
                "SELECT * FROM userclaims WHERE userid = :userid",
                new OracleParameter { ParameterName = ":userid", Value = userId, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input });

            var claims = new ClaimsIdentity();

            foreach (var row in result.Rows.Cast<DataRow>())
            {
                claims.AddClaim(new Claim(row["claimtype"].ToString(), row["claimvalue"].ToString()));
            }

            return claims;
        }
    }
}
