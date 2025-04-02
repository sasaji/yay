using System;

namespace Jbs.Yukari.Core.Models
{
    public class Membership
    {
        public int Rank { get; set; }
        public Guid ParentId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
