using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jbs.Yukari.Core.Models;

namespace Jbs.Yukari.Core.Data
{
    public interface IQuery
    {
        Task<IEnumerable<BasicInfoOutline>> Search(SearchCriteria searchCriteria);
        Task<T> GetData<T>(Guid yid) where T : BasicInfo;
        Task<IEnumerable<T>> GetObjects<T>(Guid yid, string type);
        Task<IEnumerable<TreeNode>> GetHierarchy(string type, string rootId = null);
        Task<TreeNode> GetTree(string type);
        Task<IEnumerable<Relation>> GetList(string type, bool prependBlank);
        void Save(BasicInfo info);
        Task<IEnumerable<Membership>> GetMembership(Guid yid);
        void Publish(Guid yid);
    }
}
