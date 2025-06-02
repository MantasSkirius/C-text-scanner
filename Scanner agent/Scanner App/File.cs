using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Scanner_App
{
    internal class Failas
    {
        private string failo_kelias;
        public Failas(string path)
        {
            failo_kelias = path;
        }
        public Dictionary<string, int> SkaiciuotiZodzius()
        {
            string teksto_stringas = File.ReadAllText(failo_kelias);
            string[] zodziai = Tokenizatorius.Tokenizuoti(teksto_stringas);
            Console.WriteLine("Dabar skaitomas failas: " + failo_kelias);
            Dictionary<string, int> Zodziu_daznis = new Dictionary<string, int>();
            foreach (string zodis in zodziai)
            {
                if (Zodziu_daznis.ContainsKey(zodis))
                {
                    Zodziu_daznis[zodis]++;
                }
                else
                {
                    Zodziu_daznis[zodis] = 1;
                }
            }
            return Zodziu_daznis;
        }
    }
}
