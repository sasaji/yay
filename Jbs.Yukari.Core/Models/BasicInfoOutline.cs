using System;
using System.ComponentModel;

namespace Jbs.Yukari.Core.Models
{
    public class BasicInfoOutline
    {
        public Guid Yid { get; set; }

        [DisplayName("ID")]
        public string Id { get; set; }

        public string Type { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public int Phase { get; set; }

        [DisplayName("When Changed")]
        public DateTime WhenChanged { get; set; }

        public string Objects { get; set; }
    }
}
