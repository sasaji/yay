using System;
using System.ComponentModel;

namespace Jbs.Yukari.Core.Models
{
    public class BasicInfoOutline : BasicInfoBase, IBasicInfoMeta
    {
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
    }
}
