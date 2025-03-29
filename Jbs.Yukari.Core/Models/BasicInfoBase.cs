using System;
using System.ComponentModel;

namespace Jbs.Yukari.Core.Models
{
    public class BasicInfoBase : IBasicInfoBase
    {
        public Guid Yid { get; set; }

        [DisplayName("ID")]
        public string Id { get; set; }

        [DisplayName("種別")]
        public string Type { get; set; }

        [DisplayName("名前")]
        public string Name { get; set; }
    }
}
