using System;
using System.ComponentModel;

namespace Jbs.Yukari.Core.Models
{
    public class BasicInfoOutline : BasicInfoBase
    {
        [DisplayName("有効")]
        public int Status { get; set; }

        [DisplayName("状態")]
        public int Phase { get; set; }

        [DisplayName("更新日時")]
        public DateTime WhenChanged { get; set; }

        [DisplayName("オブジェクト")]
        public string Objects { get; set; }
    }
}
