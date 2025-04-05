using System.Xml.Linq;

namespace Jbs.Yukari.Core.Models
{
    public class EmploymentStatus : BasicInfo
    {
        public override void DeserializeProperties()
        {
        }

        public override void SerializeProperties()
        {
            Properties = new XDocument(
                new XElement("properties",
                    new XElement("jobmode_name", Name)
                )
            );
        }
    }
}
