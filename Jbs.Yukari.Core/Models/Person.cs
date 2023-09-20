using System.Xml.Linq;

namespace Jbs.Yukari.Core.Models
{
    public class Person : BasicInfo
    {
        public string Surname { get; set; }
        public string GivenName { get; set; }
        public string MiddleName { get; set; }
        public string KanaSurname { get; set; }
        public string KanaGivenName { get; set; }
        public string KanaMiddleName { get; set; }
        public string RomanSurname { get; set; }
        public string RomanGivenName { get; set; }
        public string RomanMiddleName { get; set; }
        public string TelephoneNumber { get; set; }
        public Role JobMode { get; set; }

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
        }

        public override void SerializeProperties()
        {
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
        }
    }
}
