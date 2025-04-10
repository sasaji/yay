using System.Collections.Generic;
using Jbs.Yukari.Core.Models;

namespace Jbs.Yukari.Domain
{
    public class Types
    {
        private static readonly List<CodeNamePair> TypeNames =
        [
            new() { Code = "person", Name = "個人" },
            new() { Code = "organization", Name = "組織" },
            new() { Code = "title", Name = "役職" },
            new() { Code = "jobmode", Name = "雇用区分" },
            new() { Code = "user", Name = "ユーザー" }
        ];

        public static string GetTypeName(string code)
        {
            return TypeNames.Find(x => x.Code == code)?.Name ?? string.Empty;
        }
    }
}
