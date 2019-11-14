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
            var list = new List<(DateTime Date, string Name)>();
            var d1 = new DateTime(2019, 10, 20);
            var d2 = new DateTime(2019, 10, 20);
            var d3 = new DateTime(2019, 10, 21);
            var d4 = new DateTime(2019, 10, 20);
            list.Add((d1, "d1"));
            list.Add((d2, "d2"));
            list.Add((d3, "d3"));
            list.Add((d4, "d4"));

            list.Sort();
        }
    }
}
