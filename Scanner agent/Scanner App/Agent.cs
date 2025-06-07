using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
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

        public void FailuSiuntimas(string PipeName)
        {
            var client = new NamedPipeClientStream(".", PipeName, PipeDirection.Out);
            client.Connect();
            var writer = new StreamWriter(client) { AutoFlush = true };
            foreach (Dictionary<string, int>Dazniai in SiuntimoEile.GetConsumingEnumerable())
            {
                string jsonDazniai = JsonConvert.SerializeObject(Dazniai);
                Console.WriteLine(jsonDazniai);
                writer.WriteLine(jsonDazniai);
            }
        }

        public void FailuSkaitymas()
        {
            foreach (Failas failas in Failai)
            {
                SiuntimoEile.Add(failas.SkaiciuotiZodzius());
            }
            SiuntimoEile.CompleteAdding();
        }
        public Agentas(string katalogo_kelias, string search_pattern, string PipeName) {
            SiuntimoEile = new BlockingCollection<Dictionary<string, int>>();
            Failai = new List<Failas>();

            Console.WriteLine(katalogo_kelias);
            Console.WriteLine(search_pattern);
            string[] filePaths;
            Console.WriteLine("Katalogo kelias: " + katalogo_kelias + " vambzdžio vardas: " + PipeName);
            try {
                filePaths = Directory.GetFiles(katalogo_kelias, search_pattern, SearchOption.AllDirectories);
            }
            catch
            {
                Console.WriteLine("Katalogas nerastas arba netinkamas kelias.");
                while (true) { }
            }
            
            
            foreach (string imagePath in filePaths)
            {
                Failai.Add(new Failas(imagePath));
                Console.WriteLine(imagePath);
            }
            
            Task siuntimoTask = Task.Run(() => FailuSiuntimas(PipeName));
            Task skaitymoTask = Task.Run(() => FailuSkaitymas());
            Task.WaitAll(siuntimoTask, skaitymoTask);
        }
    }
}
