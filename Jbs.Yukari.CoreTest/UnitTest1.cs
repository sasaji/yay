using Jbs.Yukari.Core.Data;
using Jbs.Yukari.Core.Models;

namespace Jbs.Yukari.CoreTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var database = new Database();
            var sql = new Sql(database);
            var person = new Person
            {
                Yid = Guid.Parse("BB8863C3-55A5-4D68-83C5-00254186ED6C"),
                Id = "10001341",
                Name = "Îˆä ”ü˜a"
            };
            person.SerializeProperties();
            sql.Save(person);
        }
    }
}