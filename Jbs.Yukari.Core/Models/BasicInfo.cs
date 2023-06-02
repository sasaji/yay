using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Jbs.Yukari.Core.Models
{
    public abstract class BasicInfo
    {
        public Guid Yid { get; set; }
        public string Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public int Phase { get; set; }
        public string Status { get; set; }
        public DateTime WhenChanged { get; set; }
        public XDocument Properties { get; set; }
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

        public virtual void SerializeProperties() { }
        public virtual void DeserializeProperties() { }
    }
}
