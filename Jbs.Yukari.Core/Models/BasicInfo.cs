using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;

namespace Jbs.Yukari.Core.Models
{
    public class BasicInfo : IdNamePair, IBasicInfo
    {
        [DisplayName("コード")]
        public string Code { get; set; }

        [DisplayName("種別")]
        public string Type { get; set; }

        public XDocument Properties { get; set; }
        public IEnumerable<Membership> Membership { get; set; }
        public IEnumerable<User> Users { get; set; }
        public IEnumerable<Group> Groups { get; set; }

        [DisplayName("有効")]
        public int Status { get; set; }

        [DisplayName("状態")]
        public int Phase { get; set; }

        [DisplayName("更新日時")]
        public DateTime WhenChanged { get; set; }

        [DisplayName("反映予定日")]
        public DateTime PublishDueDate { get; set; } = DateTime.Now;

        [DisplayName("オブジェクト")]
        public string Objects { get; set; }

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
