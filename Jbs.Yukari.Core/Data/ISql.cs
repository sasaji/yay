using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Jbs.Yukari.Core.Models;

namespace Jbs.Yukari.Core.Data
{
    public interface ISql
    {
        Task<IEnumerable<ListItem>> Search(SearchCriteria searchCriteria);
        Task<T> Get<T>(string yid);
        Task<IEnumerable<Role>> GetLink(string yid, string type);
        Task<IEnumerable<T>> GetObjects<T>(string yid, string type);
        Task<string> GetTree(string type);
        void Save(BasicInfo model);
        void Publish(Guid yid);
    }
}
