using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Jbs.Yukari.Core.Data
{
    public interface IDatabase
    {
        IDbConnection Connection { get; }
        IDbTransaction GetCurrentTransaction();
    }
}
