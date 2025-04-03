using System;
using System.ComponentModel;

namespace Jbs.Yukari.Core.Models
{
    public class IdNamePair
    {
        public Guid Id { get; set; }

        [DisplayName("名前")]
        public string Name { get; set; }
    }
}
