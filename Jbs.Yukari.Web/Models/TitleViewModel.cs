using Jbs.Yukari.Core.Models;

namespace Jbs.Yukari.Web.Models
{
    public class TitleViewModel : Title, IEditViewModel
    {
        public string TabIndex { get; set; } = string.Empty;
        public string ObjectTabIndex { get; set; } = string.Empty;
    }
}
