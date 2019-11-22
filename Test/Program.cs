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
            var d0 = new DateTime(2019, 10, 20);
            var d1 = new DateTime(2018, 10, 20);
            var d2 = new DateTime(2019, 10, 21);
            var d3 = new DateTime(2019, 10, 20);
            var d4 = new DateTime(2017, 10, 21);
            var d5 = new DateTime(2019, 10, 22);
            var d6 = new DateTime(2015, 10, 21);
            var d7 = new DateTime(2010, 10, 23);
            var d8 = new DateTime(1995, 10, 20);
            var d9 = new DateTime(2019, 10, 22);

            var list = new List<(DateTime Date, string Name)>();
            list.Add((d0, "d0"));
            list.Add((d1, "d1"));
            list.Add((d2, "d2"));
            list.Add((d3, "d3"));
            list.Add((d4, "d4"));
            list.Add((d5, "d5"));
            list.Add((d6, "d6"));
            list.Add((d7, "d7"));
            list.Add((d8, "d8"));
            list.Add((d9, "d9"));


            var l = list.OrderBy(p => p.Date.Month).ThenBy(p => p.Date.Day);
        }
    }
}
