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
            var d1 = new DateTime(2019, 10, 20);
            var d2 = new DateTime(2019, 10, 21);
            var d3 = new DateTime(2019, 10, 22);
            var d4 = new DateTime(2019, 10, 23);

            var l1 = new List<DateTime>();
            l1.Add(d1);
            l1.Add(d2);
            l1.Add(d3);

            var l2 = new List<DateTime>();
            l2.Add(d3);
            l2.Add(d4);

            var d11 = DateTime.Now;
            var l = l1.Union(l2);
            var d12 = DateTime.Now;
            Console.WriteLine(d12 - d11);

            var d21 = DateTime.Now;
            var m = l1.Concat(l2).Distinct();
            var d22 = DateTime.Now;
            Console.WriteLine(d22 - d21);
        }
    }
}
