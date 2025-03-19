using System;

namespace Jbs.Yukari.Core.Models
{
    public class Membership
    {
        public int Key { get; set; }
        public Guid ParentYid { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
