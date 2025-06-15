using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace Receiver_master_GUI
{
    internal class Receiver
    {
    //Kai pakviečiama - klausytis Pipe ir gautą json paversti į Dictionarį ir idėtį į Priiemimo eilę.
        private Dictionary<string, int> JsonToDictionary(string zinute)
        {
            Tuple<String, Dictionary<string, int>> Dazniai;
            Dazniai = JsonConvert.DeserializeObject<Tuple<String, Dictionary<string, int>>>(zinute);
            return Dazniai.Item2;
        }

        public Receiver(ref BlockingCollection<Dictionary<string, int>> PriiemimoEile, string PipeName)
        {
            using var server = new NamedPipeServerStream(PipeName, PipeDirection.In);
            MessageBox.Show("Waiting for agent to connect...");
            server.WaitForConnection();
            using var reader = new StreamReader(server);
            string message;
            while (true)
            {
                while ((message = reader.ReadLine()) != null)
                {
                    PriiemimoEile.Add(JsonToDictionary(message));
                    MessageBox.Show("Received from agent: " + message);
                }
            }

        }
    }
}
