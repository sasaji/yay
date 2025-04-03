using System;
using System.Linq;
using System.Xml.Linq;

namespace Jbs.Yukari.Core.Models
{
    public class Organization : BasicInfo
    {
        public Guid? ParentId { get; set; }

        public override void DeserializeProperties()
        {
            ParentId = Membership.FirstOrDefault()?.ParentId;
        }

        public override void SerializeProperties()
        {
            Properties = new XDocument(
                new XElement("properties",
                    new XElement("organization_name", Name)
                )
            );

            Membership = [];
            if (ParentId != null)
            {
                Membership = Membership.Append(new Membership
                {
                    Rank = 0,
                    ParentId = (Guid)ParentId,
                    Type = "organization"
                });
            }
        }
    }
}
