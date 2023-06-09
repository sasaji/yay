using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Jbs.Yukari.Core.Models
{
    public abstract class BasicInfo : BasicInfoOutline
    {
        public XDocument Properties { get; set; }
        public IEnumerable<Dictionary<string, Role>> Roles { get; set; }
        public IEnumerable<User> Users { get; set; }
        public IEnumerable<Group> Groups { get; set; }

        public string TabIndex { get; set; } = string.Empty;
        public string ObjectTabIndex { get; set; } = string.Empty;

        public string GetPropertyValue(string key)
        {
            if (string.IsNullOrEmpty(key)) return null;
            if (Properties == null) return null;
            if (Properties.Root == null) return null;
            if (!Properties.Root.Elements().Any(el => el.Name == key)) return null;
            return Properties.Root.Elements().FirstOrDefault(el => el.Name == key).Value;
        }

        public abstract void SerializeProperties();
        public abstract void DeserializeProperties();
    }
}
