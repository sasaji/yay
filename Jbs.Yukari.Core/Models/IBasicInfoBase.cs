using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jbs.Yukari.Core.Models
{
    public interface IBasicInfoBase
    {
        public Guid Yid { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
    }
}
