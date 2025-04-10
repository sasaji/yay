using System;
using System.ComponentModel;
using System.Xml.Linq;

namespace Jbs.Yukari.Core.Models
{
    public abstract class ObjectInfo
    {
        public Guid Yid { get; set; }
        public string Type { get; set; }
        public string SamAccountName { get; set; }

        [DisplayName("表示名")]
        public string DisplayName { get; set; }
        public XDocument Properties { get; set; }
    }
}
