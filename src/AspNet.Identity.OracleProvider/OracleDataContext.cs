// Copyright (c) Timm Krause. All rights reserved. See LICENSE file in the project root for license information.

namespace AspNet.Identity.OracleProvider
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Oracle.ManagedDataAccess.Client;

    public class OracleDataContext : IDisposable
    {
        public OracleDataContext()
            : this("DefaultConnection")
        {
        }

        public OracleDataContext(string connectionStringName)
        {
            var connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;

            Connection = new OracleConnection(connectionString);
        }

        public OracleDataContext(OracleConnection connection)
        {
            Connection = connection;
        }

        ~OracleDataContext()
        {
            Dispose(false);
        }

        public OracleConnection Connection { get; private set; }

        [SuppressMessage("Microsoft.Globalization", "CA1306:SetLocaleForDataTypes", Justification = "Review.")]
        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "It actually is a parameterized SQL query.")]
        public DataTable ExecuteQuery(string query, params OracleParameter[] parameters)
        {
            OpenClosedConnection();

            var resultTable = new DataTable();
            var transaction = Connection.BeginTransaction(IsolationLevel.ReadCommitted);
            var command = new OracleCommand(query, Connection) { CommandType = CommandType.Text };

            command.Parameters.AddRange(parameters);

            var dataAdapter = new OracleDataAdapter(command);

            try
            {
                dataAdapter.Fill(resultTable);
            }
            catch (OracleException ex)
            {
                // Repeating OracleCommand because the procedure has been invalidated.
                if (ex.Number == 4068)
                {
                    dataAdapter.Fill(resultTable);
                }
                else
                {
                    throw;
                }
            }

            transaction.Commit();

            return resultTable;
        }

        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "It actually is a parameterized SQL query.")]
        public object ExecuteScalarQuery(string query, params OracleParameter[] parameters)
        {
            OpenClosedConnection();

            object result;
            var transaction = Connection.BeginTransaction(IsolationLevel.ReadCommitted);
            var command = new OracleCommand(query, Connection) { CommandType = CommandType.Text };

            command.Parameters.AddRange(parameters);

            try
            {
                result = command.ExecuteScalar();
            }
            catch (OracleException ex)
            {
                // Repeating OracleCommand because the procedure has been invalidated.
                if (ex.Number == 4068)
                {
                    result = command.ExecuteScalar();
                }
                else
                {
                    throw;
                }
            }

            transaction.Commit();

            return result;
        }

        public T ExecuteScalarQuery<T>(string query, params OracleParameter[] parameters)
        {
            return (T)ExecuteScalarQuery(query, parameters);
        }

        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "It actually is a parameterized SQL query.")]
        public int ExecuteNonQuery(string query, params OracleParameter[] parameters)
        {
            OpenClosedConnection();

            int result;
            var transaction = Connection.BeginTransaction(IsolationLevel.ReadCommitted);
            var command = new OracleCommand(query, Connection) { CommandType = CommandType.Text };

            command.Parameters.AddRange(parameters);

            try
            {
                result = command.ExecuteNonQuery();
            }
            catch (OracleException ex)
            {
                // Repeating OracleCommand because the procedure has been invalidated.
                if (ex.Number == 4068)
                {
                    result = command.ExecuteNonQuery();
                }
                else
                {
                    throw;
                }
            }

            transaction.Commit();

            return result;
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
                if (Connection != null)
                {
                    Connection.Close();
                    Connection.Dispose();
                    Connection = null;
                }
            }
        }

        private void OpenClosedConnection()
        {
            if (Connection.State == ConnectionState.Closed)
            {
                Connection.Open();
            }
        }
    }
}
