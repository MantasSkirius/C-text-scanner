using System;
using System.Collections.Concurrent;
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
            string Katalogo_kelias = null;
            if (Katalogo_keliai.Length == 0 || Katalogo_keliai == null)
            {
                Console.WriteLine("Katalogo kelias nenurodytas, naudojamas numatytasis katalogas.");
                //Katalogo_kelias = @"..\..\..\..\..\..\Tekstai skaitymui";
            }
            else
            {
                Console.WriteLine("Katalogo kelias nurodytas: " + Katalogo_keliai[0]);
                Katalogo_kelias = Katalogo_keliai[0];
            }

            foreach (string katalogo_kels in Katalogo_keliai)
            {
                Console.WriteLine("ARgumentu spausdinimas: " + katalogo_kels);
            }

            string search_pattern = "*.txt";
            Console.WriteLine(Katalogo_kelias);

            //string Katalogo_kelias = @"..\..\..\..\Tekstai skaitymui";
            Console.WriteLine("Katalogo kelias su kuriuo kuriamas agentas: " + Katalogo_kelias);
            Agentas agent = new Agentas(Katalogo_kelias, search_pattern);

        }
    }
}
