using Jbs.Yukari.Core.Models;

namespace Jbs.Yukari.Web.Models
{
    public class PersonViewModel : Person
    {
        // ツリービューやドロップダウンリストをクライアント側で再現させるためのJSON文字列。
        public string RolesJson { get; set; } = string.Empty;
        public string OrganizationsJson { get; set; } = string.Empty;
        public string TitlesJson { get; set; } = string.Empty;
        public IList<Relation> Enrollments { get; set; } = new List<Relation>();
    }
}
