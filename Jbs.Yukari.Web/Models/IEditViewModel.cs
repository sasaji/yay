using System.ComponentModel;
using Jbs.Yukari.Core.Models;

namespace Jbs.Yukari.Web.Models
{
    public interface IEditViewModel : IBasicInfoBase, IBasicInfoMeta
    {
        public string TabIndex { get; set; }
        public string ObjectTabIndex { get; set; }
    }
}
