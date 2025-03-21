using System;
using System.Collections.Generic;
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
            var person = await GetData<T>(yid);
            if (person != null)
            {
                person.Roles = person.Membership
                    .Where(x => (sourceArray).Contains(x.Type))
                .GroupBy(x => x.Key)
                    .Select(x => x.ToDictionary(y => y.Type, a => new Relation { Yid = a.ParentYid, Name = a.Name }));

                person.EmploymentStatus = person.Membership
                    .Where(x => x.Type == "jobmode")
                    .FirstOrDefault()?.ParentYid;

                person.DeserializeProperties();
            }
            return person;
        }

        public void Save(Person person)
        {
            person.Name = $"{person.Surname} {person.GivenName}".Trim();
            person.SerializeProperties();
            person.Membership = new List<Membership>();
            int key = 0;
            foreach (var role in person.Roles)
            {
                foreach (var item in role)
                {
                    person.Membership = person.Membership.Append(new Membership
                    {
                        Key = key,
                        ParentYid = item.Value.Yid,
                        Name = item.Value.Name,
                        Type = item.Key
                    });
                }
                key++;
            }
            if (person.EmploymentStatus != Guid.Empty)
            {

                person.Membership = person.Membership.Append(new Membership
                {
                    Key = 0,
                    ParentYid = (Guid)person.EmploymentStatus
                });
            }
            base.Save(person);
        }
    }
}
