using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Jbs.Yukari.Core.Data
{
    public class Database : IDatabase
    {
        private Lazy<IDbConnection> _connection;
        private IDbTransaction _transaction;

        public IDbConnection Connection => _connection.Value;

        public Database()
        {
            _connection = new Lazy<IDbConnection>(() =>
            {
                // MultipleActiveResultSets=true前提
                var connection = new SqlConnection("Data Source=localhost;Initial Catalog=Yukari;Integrated Security=True;MultipleActiveResultSets=True");
                connection.Open();
                return connection;
            });
        }

        public void BeginTransaction()
        {
            if (_transaction == null)
                _transaction = _connection.Value.BeginTransaction();
        }

        public IDbTransaction GetCurrentTransaction()
        {
            return _transaction;
        }
    }
}
