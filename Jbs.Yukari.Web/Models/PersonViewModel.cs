using System.ComponentModel;
using Jbs.Yukari.Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Jbs.Yukari.Web.Models
{
    public class PersonViewModel : Person, IEditViewModel
    {
        // ツリービューやドロップダウンリストをクライアント側で再現させるためのJSON文字列。
        [DisplayName("所属/役職")]
        public IList<SelectListItem> AffiliationsViewModel { get; set; } = [];
        public string TreeJson { get; set; } = string.Empty;
        public IList<SelectListItem> Titles { get; set; } = [];
        public IList<SelectListItem> EmploymentStatuses { get; set; } = [];
        public string TabIndex { get; set; } = string.Empty;
        public string ObjectTabIndex { get; set; } = string.Empty;
    }
}
