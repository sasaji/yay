using System;
using System.ComponentModel;

namespace Jbs.Yukari.Core.Models
{
    public interface IBasicInfo
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public int Phase { get; set; }

        [DisplayName("反映予定日")]
        public DateTime PublishDueDate { get; set; }
    }
}
