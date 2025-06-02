using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanner_App
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            string search_pattern = "*.txt";
            string Katalogo_kelias = @"..\..\..\..\Tekstai skaitymui";
            Agentas agent = new Agentas(Katalogo_kelias, search_pattern);

        }
    }
}
