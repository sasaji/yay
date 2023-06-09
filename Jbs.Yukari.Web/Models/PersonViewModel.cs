using Jbs.Yukari.Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Jbs.Yukari.Web.Models
{
    public class PersonViewModel : Person
    {
        public string RoleList { get; set; } = string.Empty;
        public string OrganizationList { get; set; } = string.Empty;
        public string TitleList { get; set; } = string.Empty;
    }
}
