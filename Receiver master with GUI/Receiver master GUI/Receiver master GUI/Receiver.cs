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
    //Šitos klasės darbas - kai pakviečiama - klausytis Pipe ir gautą json paversti į Dictionarį ir idėtį į Priiemimo eilę.
        private Dictionary<string, int> JsonToDictionary(string zinute)
        {
            Dictionary<string, int> Dazniai;
            Dazniai = JsonConvert.DeserializeObject<Dictionary<string, int>>(zinute);
            return Dazniai;
        }

        public Receiver(ref BlockingCollection<Dictionary<string, int>> PriiemimoEile)
        {
            using var server = new NamedPipeServerStream("dazniuSiuntimoVamzdis", PipeDirection.In);
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
