using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jbs.Yukari.Core.Models;

namespace Jbs.Yukari.Core.Data
{
    public interface IPersonQuery : IQuery
    {
        Task<T> GetPerson<T>(Guid yid) where T : Person;
        void Save(Person person);
    }
}
