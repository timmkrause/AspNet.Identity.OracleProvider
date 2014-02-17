// Copyright (c) Timm Krause. All rights reserved. See LICENSE file in the project root for license information.

namespace AspNet.Identity.OracleProvider.Repositories
{
    using System;
    using System.Data;
    using Oracle.ManagedDataAccess.Client;

    internal class RoleRepository
    {
        private readonly OracleDataContext _db;

        public RoleRepository(OracleDataContext oracleContext)
        {
            _db = oracleContext;
        }

        public int Insert(IdentityRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            return _db.ExecuteNonQuery(
                "INSERT INTO roles (id, name) VALUES (:id, :name)",
                new OracleParameter { ParameterName = ":id", Value = role.Id, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input },
                new OracleParameter { ParameterName = ":name", Value = role.Name, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input });
        }

        public int Update(IdentityRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            return _db.ExecuteNonQuery(
                "UPDATE roles SET name = :name WHERE id = :id",
                new OracleParameter { ParameterName = ":name", Value = role.Name, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input },
                new OracleParameter { ParameterName = ":id", Value = role.Id, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input });
        }

        public int Delete(string roleId)
        {
            return _db.ExecuteNonQuery(
                "DELETE FROM roles WHERE id = :id",
                new OracleParameter { ParameterName = ":id", Value = roleId, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input });
        }

        public string GetRoleName(string roleId)
        {
            return _db.ExecuteScalarQuery<string>(
                "SELECT name FROM roles WHERE id = :id",
                new OracleParameter { ParameterName = ":id", Value = roleId, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input });
        }

        public string GetRoleId(string roleName)
        {
            return _db.ExecuteScalarQuery<string>(
                "SELECT id FROM roles WHERE name = :name",
                new OracleParameter { ParameterName = ":name", Value = roleName, OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input });
        }

        public IdentityRole GetRoleById(string roleId)
        {
            var roleName = GetRoleName(roleId);

            return roleName != null ? new IdentityRole(roleName, roleId) : null;
        }

        public IdentityRole GetRoleByName(string roleName)
        {
            var roleId = GetRoleId(roleName);

            return roleId != null ? new IdentityRole(roleName, roleId) : null;
        }
    }
}
