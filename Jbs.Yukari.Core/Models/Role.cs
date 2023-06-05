using System;

namespace Jbs.Yukari.Core.Models
{
    public class Role
    {
        public int Id { get; set; }
        public Guid OrganizationYid { get; set; }
        public string OrganizationName { get; set; }
        public Guid TitleYid { get; set; }
        public string TitleName { get; set; }
    }
}
