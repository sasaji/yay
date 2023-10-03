using System;

namespace Jbs.Yukari.Core.Models
{
    public class BasicInfoOutline
    {
        public Guid Yid { get; set; }
        public string Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public int Phase { get; set; }
        public DateTime WhenChanged { get; set; }
        public string Objects { get; set; }
    }
}
