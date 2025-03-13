using System;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace Jbs.Yukari.Core.Data
{
    public class Database : IDatabase
    {
        private readonly Lazy<IDbConnection> connection;
        private IDbTransaction transaction;

        public IDbConnection Connection => connection.Value;

        public Database()
        {
            connection = new Lazy<IDbConnection>(() =>
            {
                // MultipleActiveResultSets=true前提
                var conn = new SqlConnection("Data Source=localhost;Initial Catalog=YUKARI_MAL;Integrated Security=True;MultipleActiveResultSets=True");
                conn.Open();
                return conn;
            });
        }

        public void BeginTransaction()
        {
            if (transaction != null)
            {
                throw new ApplicationException("トランザクションはすでに開始しています。");
            }
            transaction = connection.Value.BeginTransaction();
        }

        public IDbTransaction GetOrBeginTransaction()
        {
            if (transaction == null)
            {
                BeginTransaction();
            }
            return transaction;
        }

        public IDbTransaction GetCurrentTransaction()
        {
            return transaction;
        }

        public int ExecuteInTransaction(string sql, object model, CommandType commandType = CommandType.Text)
        {
            var tran = EnsureInAndGetTransaction();
            return Connection.Execute(
                sql,
                param: model,
                transaction: tran,
                commandType: commandType);
        }

        public void Commit()
        {
            if (IsTransactiionActive())
            {
                transaction.Commit();
                transaction.Dispose();
                transaction = null;
            }
        }

        public void Rollback()
        {
            if (IsTransactiionActive())
            {
                transaction.Rollback();
                transaction.Dispose();
                transaction = null;
            }
        }

        private IDbTransaction EnsureInAndGetTransaction()
        {
            var tran = GetCurrentTransaction();
            return tran ?? throw new InvalidOperationException("トランザクションが開始されていません。先にトランザクションを開始してください。");
        }

        private bool IsTransactiionActive()
        {
            // zombie-transactionの場合connectionがnullになることがある。
            return transaction?.Connection != null;
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (IsTransactiionActive())
                    {
                        // WARN
                        Rollback();
                    }

                    transaction?.Dispose();
                    if (connection != null && connection.IsValueCreated)
                    {
                        var conn = connection.Value;
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
