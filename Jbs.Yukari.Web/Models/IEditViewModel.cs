using System.ComponentModel;
using Jbs.Yukari.Core.Models;

namespace Jbs.Yukari.Web.Models
{
    public interface IEditViewModel : IBasicInfo
    {
        public string TabIndex { get; set; }
        public string ObjectTabIndex { get; set; }
    }
}
