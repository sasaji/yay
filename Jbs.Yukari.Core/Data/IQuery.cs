using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jbs.Yukari.Core.Models;

namespace Jbs.Yukari.Core.Data
{
    public interface IQuery
    {
        Task<IEnumerable<BasicInfoOutline>> Search(SearchCriteria searchCriteria);
        Task<T> GetData<T>(Guid yid);
        Task<IEnumerable<Dictionary<string, Relation>>> GetRoles(Guid yid);
        Task<Guid> GetEnrollment(Guid yid);
        Task<IEnumerable<T>> GetObjects<T>(Guid yid, string type);
        Task<IEnumerable<TreeNode>> GetHierarchy(string type);
        Task<TreeNode> GetTree(string type);
        Task<IEnumerable<Relation>> GetEnrollments();
        void Save(BasicInfo model);
        void Publish(Guid yid);
    }
}
