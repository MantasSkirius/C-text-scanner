using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanner_App
{
    internal class Program
    {
        public static void Main(string[] Katalogo_keliai)
        {
            //Šitos dalies su Katalogo kelių masyvu reikia, nes kompiliatorius tikisi matyti string[], o ne vieną string Main metodo argumentuose.
            string Katalogo_kelias;
            if (Katalogo_keliai.Length == 0 || Katalogo_keliai == null) {
                Katalogo_kelias = @"..\..\..\..\..\..\Tekstai skaitymui";
            }
            else
            {
                Katalogo_kelias = Katalogo_keliai[0];
            }
           
            string search_pattern = "*.txt";
            Console.WriteLine(Katalogo_kelias);

            //string Katalogo_kelias = @"..\..\..\..\Tekstai skaitymui";

            Agentas agent = new Agentas(Katalogo_kelias, search_pattern);
            
        }
    }
}
