using System;
using System.Data;

namespace Jbs.Yukari.Core.Data
{
    public interface IDatabase : IDisposable
    {
        IDbConnection Connection { get; }
        void BeginTransaction();
        IDbTransaction GetOrBeginTransaction();
        IDbTransaction GetCurrentTransaction();
        int ExecuteInTransaction(string sql, object model, CommandType commandType = CommandType.Text);
        void Commit();
        void Rollback();
    }
}
