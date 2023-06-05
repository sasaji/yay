using Jbs.Yukari.Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Jbs.Yukari.Web.Models
{
    public class PersonViewModel : Person
    {
        public IEnumerable<SelectListItem> SelectList
        {
            get {
                return Roles.Select(x => new SelectListItem { Text = x.OrganizationName, Value = x.OrganizationYid.ToString() });
            }
        }
    }
}
