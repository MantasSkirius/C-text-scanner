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
        public string teksto_stringas;
        public Dictionary<string, int> Zodziu_daznis { get; set; }
        public Failas(string path)
        {
            teksto_stringas = File.ReadAllText(path);
            string[] zodziai = Tokenizatorius.Tokenizuoti(teksto_stringas);
            Zodziu_daznis = SkaiciuotiZodzius(zodziai);
        }
        protected Dictionary<string, int> SkaiciuotiZodzius(string[] zodziai)
        {
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
