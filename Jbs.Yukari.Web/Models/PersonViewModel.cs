using System.ComponentModel;
using Jbs.Yukari.Core.Models;

namespace Jbs.Yukari.Web.Models
{
    public class PersonViewModel : Person
    {
        // ツリービューやドロップダウンリストをクライアント側で再現させるためのJSON文字列。
        [DisplayName("所属/役職")]
        public string RolesViewModel { get; set; } = string.Empty;
        public string TreeJson { get; set; } = string.Empty;
        public string TitlesJson { get; set; } = string.Empty;
        public IList<Relation> Enrollments { get; set; } = [];
    }
}
