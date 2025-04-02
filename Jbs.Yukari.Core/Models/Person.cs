using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;

namespace Jbs.Yukari.Core.Models
{
    public class Person : BasicInfo
    {
        private static readonly string[] sourceArray = ["organization", "title"];

        public string Surname { get; set; }
        public string GivenName { get; set; }
        public string MiddleName { get; set; }
        public string KanaSurname { get; set; }
        public string KanaGivenName { get; set; }
        public string KanaMiddleName { get; set; }
        public string RomanSurname { get; set; }
        public string RomanGivenName { get; set; }
        public string RomanMiddleName { get; set; }

        [DisplayName("電話番号")]
        public string TelephoneNumber { get; set; }

        [DisplayName("所属 / 役職")]
        public IEnumerable<Dictionary<string, BasicInfo>> Roles { get; set; }

        [DisplayName("雇用区分")]
        public Guid? EmploymentStatus { get; set; }

        public override void DeserializeProperties()
        {
            Surname = GetPropertyValue("surname");
            GivenName = GetPropertyValue("given_name");
            MiddleName = GetPropertyValue("middle_name");
            KanaSurname = GetPropertyValue("surname_kana");
            KanaGivenName = GetPropertyValue("given_name_kana");
            KanaMiddleName = GetPropertyValue("middle_name_kana");
            RomanSurname = GetPropertyValue("surname_roman");
            RomanGivenName = GetPropertyValue("given_name_roman");
            RomanMiddleName = GetPropertyValue("middle_name_roman");
            TelephoneNumber = GetPropertyValue("phone_no");
            Roles = Membership
                .Where(x => (sourceArray).Contains(x.Type))
                .GroupBy(x => x.Rank)
                .Select(x => x.ToDictionary(y => y.Type, a => new BasicInfo { Id = a.ParentId, Name = a.Name }));
            EmploymentStatus = Membership
                .Where(x => x.Type == "jobmode")
                .FirstOrDefault()?.ParentId;
        }

        public override void SerializeProperties()
        {
            Name = $"{Surname} {GivenName}".Trim();
            Properties = new XDocument(
                new XElement("properties",
                    new XElement("surname", Surname),
                    new XElement("given_name", GivenName),
                    new XElement("middle_name", MiddleName),
                    new XElement("surname_kana", KanaSurname),
                    new XElement("given_name_kana", KanaGivenName),
                    new XElement("middle_name_kana", KanaMiddleName),
                    new XElement("surname_roman", RomanSurname),
                    new XElement("given_name_roman", RomanGivenName),
                    new XElement("middle_name_roman", RomanMiddleName),
                    new XElement("phone_no", TelephoneNumber)
                )
            );
            Membership = [];
            int key = 0;
            if (Roles != null)
            {
                foreach (var role in Roles)
                {
                    foreach (var item in role)
                    {
                        Membership = Membership.Append(new Membership
                        {
                            Rank = key,
                            ParentId = item.Value.Id,
                            Name = item.Value.Name,
                            Type = item.Key
                        });
                    }
                    key++;
                }
            }
            if (EmploymentStatus != Guid.Empty)
            {

                Membership = Membership.Append(new Membership
                {
                    Rank = 0,
                    ParentId = (Guid)EmploymentStatus
                });
            }
        }
    }
}
