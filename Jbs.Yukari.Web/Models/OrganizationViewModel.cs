﻿using Jbs.Yukari.Core.Models;

namespace Jbs.Yukari.Web.Models
{
    public class OrganizationViewModel : Organization
    {
        public string TabIndex { get; set; } = string.Empty;
        public string ObjectTabIndex { get; set; } = string.Empty;
    }
}
