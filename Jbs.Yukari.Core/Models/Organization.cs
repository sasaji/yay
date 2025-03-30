using System.Xml.Linq;

namespace Jbs.Yukari.Core.Models
{
    public class Organization : BasicInfo
    {
        public override void DeserializeProperties()
        {
        }

        public override void SerializeProperties()
        {
            Properties = new XDocument(
                new XElement("properties",
                    new XElement("organization_name", Name)
                )
            );
        }
    }
}
