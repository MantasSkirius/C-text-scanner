using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanner_App
{
    internal class Agentas
    {

        public string[] failu_keliai;
        public List<Failas> Failai;
        public Agentas(string katalogo_kelias, string search_pattern) {
            Failai = new List<Failas>();
            Console.WriteLine(katalogo_kelias);
            Console.WriteLine(search_pattern);
            string[] imagePaths = Directory.GetFiles(katalogo_kelias, search_pattern, SearchOption.AllDirectories);
            foreach (string imagePath in imagePaths)
            {
                Failai.Add(new Failas(imagePath));
                Console.WriteLine(imagePath);
            }
            foreach (Failas failas in Failai)
            {
                foreach(KeyValuePair<string, int>zodzio_daznis in failas.Zodziu_daznis)
                {
                    Console.WriteLine(zodzio_daznis.Key + " " + zodzio_daznis.Value);
                }
            }
        }
    }
}
