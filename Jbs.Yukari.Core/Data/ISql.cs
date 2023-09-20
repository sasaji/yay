using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jbs.Yukari.Core.Models;

namespace Jbs.Yukari.Core.Data
{
    public interface ISql
    {
        Task<IEnumerable<BasicInfoOutline>> Search(SearchCriteria searchCriteria);
        Task<T> GetData<T>(Guid yid);
        Task<IEnumerable<Dictionary<string, Role>>> GetRoles(Guid yid);
        Task<Role> GetJobMode(Guid yid);
        Task<IEnumerable<T>> GetObjects<T>(Guid yid, string type);
        Task<IEnumerable<TreeNode>> GetHierarchy(string type);
        Task<TreeNode> GetTree(string type);
        Task<IEnumerable<Role>> GetJobModes();
        void Save(BasicInfo model);
        void Publish(Guid yid);
    }
}
