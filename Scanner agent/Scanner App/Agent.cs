using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scanner_App
{
    internal class Agentas
    {

        public string[] failu_keliai;
        public List<Failas> Failai;
        public BlockingCollection<Dictionary<string, int>> SiuntimoEile;

        public void FailuSiuntimas()
        {
            foreach(Dictionary<string, int>Dazniai in SiuntimoEile.GetConsumingEnumerable())
            {
                foreach(KeyValuePair<string, int>zodzioDaznis in Dazniai)
                {
                    Console.WriteLine(zodzioDaznis.Key + " " + zodzioDaznis.Value);
                    Thread.Sleep(10);
                }
            }
            for (int i = 0; i < 20; i++)
            {
                Console.WriteLine("NEW FILE");
            }
        }
        public Agentas(string katalogo_kelias, string search_pattern) {
            SiuntimoEile = new BlockingCollection<Dictionary<string, int>>();
            Failai = new List<Failas>();
            Task siuntimoTask = Task.Run(() => FailuSiuntimas());
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
                SiuntimoEile.Add(failas.SkaiciuotiZodzius());
                Thread.Sleep(3000);
            }
            SiuntimoEile.CompleteAdding();
            Task.WaitAll(siuntimoTask);
            while (true) { };
        }
    }
}
