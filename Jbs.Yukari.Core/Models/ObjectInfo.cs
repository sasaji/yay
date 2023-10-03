using System;
using System.Xml.Linq;

namespace Jbs.Yukari.Core.Models
{
    public abstract class ObjectInfo
    {
        public Guid Yid { get; set; }
        public string Type { get; set; }
        public string TypeName { get; set; }
        public string SamAccountName { get; set; }
        public string DisplayName { get; set; }
        public XDocument Properties { get; set; }
    }
}
