using Jbs.Yukari.Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Jbs.Yukari.Web.Models
{
    public class PersonViewModel : Person
    {
        public string SelectedRoles { get; set; } = string.Empty;
        public string RoleList { get; set; } = string.Empty;
    }
}
