using System;
using System.Linq;
using System.Xml.Linq;

namespace Jbs.Yukari.Core.Models
{
    public class Organization : BasicInfo
    {
        public IdNamePair Parent { get; set; }

        public override void DeserializeProperties()
        {
            Parent = Membership.Select(x => new IdNamePair { Id = x.ParentId, Name = x.Name }).FirstOrDefault();
        }

        public override void SerializeProperties()
        {
            Properties = new XDocument(
                new XElement("properties",
                    new XElement("organization_name", Name)
                )
            );

            Membership = [];
            if (Parent != null)
            {
                Membership = Membership.Append(new Membership
                {
                    Rank = 0,
                    ParentId = Parent.Id,
                    Name = Parent.Name,
                    Type = "organization"
                });
            }
        }
    }
}
