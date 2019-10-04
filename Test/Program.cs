using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var foo = new Foo { Date = new DateTime(2019,1,1), Text = "1337" };
            //foo.Date.ToUniversalTime();
            var ser = new JavaScriptSerializer();
            var result = ser.Serialize(foo);
            var d = ser.Deserialize(result, typeof(Foo)) as Foo;
            d.Date = d.Date.ToLocalTime();

        }
    }

    class Foo
    {
        public DateTime Date { get; set; }
        public string Text { get; set; }
    }
}
