using System;
using System.Data;
using System.Data.SqlClient;
using Dapper;

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
            if (_transaction != null)
            {
                throw new ApplicationException("トランザクションはすでに開始しています。");
            }
            _transaction = _connection.Value.BeginTransaction();
        }

        public IDbTransaction GetOrBeginTransaction()
        {
            if (_transaction == null)
            {
                BeginTransaction();
            }
            return _transaction;
        }

        public IDbTransaction GetCurrentTransaction()
        {
            return _transaction;
        }

        public int ExecuteInTransaction(string sql, object model, CommandType commandType = CommandType.Text)
        {
            var transaction = EnsureInAndGetTransaction();
            return Connection.Execute(
                sql,
                param: model,
                transaction: transaction,
                commandType: commandType);
        }

        public void Commit()
        {
            if (this.IsTransactiionActive())
            {
                _transaction.Commit();
                _transaction.Dispose();
                _transaction = null;
            }
        }

        public void Rollback()
        {
            if (this.IsTransactiionActive())
            {
                _transaction.Rollback();
                _transaction.Dispose();
                _transaction = null;
            }
        }

        private IDbTransaction EnsureInAndGetTransaction()
        {
            var transaction = GetCurrentTransaction();
            return transaction ?? throw new InvalidOperationException("トランザクションが開始されていません。先にトランザクションを開始してください。");
        }

        private bool IsTransactiionActive()
        {
            // zombie-transactionの場合connectionがnullになることがある。
            return _transaction?.Connection != null;
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (this.IsTransactiionActive())
                    {
                        // WARN
                        this.Rollback();
                    }

                    _transaction?.Dispose();
                    if (_connection != null && _connection.IsValueCreated)
                    {
                        var conn = _connection.Value;
                        conn.Close();
                        conn.Dispose();
                    }
                }
                disposedValue = true;
            }
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
