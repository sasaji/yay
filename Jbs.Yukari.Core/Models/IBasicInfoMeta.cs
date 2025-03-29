using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jbs.Yukari.Core.Models
{
    public interface IBasicInfoMeta
    {
        public int Phase { get; set; }

        [DisplayName("反映予定日")]
        public DateTime PublishDueDate { get; set; }
    }
}
