using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jbs.Yukari.Core.Models;

namespace Jbs.Yukari.Core.Data
{
    public interface IQuery
    {
        Task<IEnumerable<BasicInfo>> Search(SearchCriteria searchCriteria);
        Task<T> GetData<T>(Guid id) where T : BasicInfo;
        Task<IEnumerable<T>> GetObjects<T>(Guid id, string type);
        Task<IEnumerable<TreeNode>> GetHierarchy(string type, string rootId = null);
        Task<TreeNode> GetOrganizationTree(string rootText);
        Task<IEnumerable<IdNamePair>> GetIdNamePairs(string type, bool prependBlank);
        void Save(BasicInfo info);
        Task<IEnumerable<Membership>> GetMembership(Guid id);
        void Publish(Guid id);
    }
}
