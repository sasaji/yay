using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Jbs.Yukari.Core.Models;

namespace Jbs.Yukari.Core.Data
{
    public class PersonQuery(IDatabase database) : Query(database), IPersonQuery
    {
        private static readonly string[] sourceArray = ["organization", "title"];

        public async Task<T> GetPerson<T>(Guid yid) where T : Person
        {
            var person = await base.GetData<T>(yid);
            person.Roles = person.Membership
                .Where(x => (sourceArray).Contains(x.Type))
            .GroupBy(x => x.Key)
                .Select(x => x.ToDictionary(y => y.Type, a => new Relation { Yid = person.Yid, Name = a.Name }));

            person.EmploymentStatus = person.Membership
                .Where(x => x.Type == "jobmode")
                .FirstOrDefault()?.ParentYid;
            person.DeserializeProperties();
            return person;
        }
    }
}
