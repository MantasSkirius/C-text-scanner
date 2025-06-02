using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Receiver_master
{
    internal class Receiver
    {
        public Receiver() {
            using var server = new NamedPipeServerStream("dazniuSiuntimoVamzdis", PipeDirection.In);
            Console.WriteLine("Waiting for agent to connect...");
            server.WaitForConnection();

            using var reader = new StreamReader(server);
            string message = reader.ReadLine();

            Console.WriteLine("Received from agent: " + message);
            Dictionary<string, int> Dazniai = JsonConvert.DeserializeObject<Dictionary<string, int>>(message);
            foreach(KeyValuePair<string, int> daznis in Dazniai)
            {
                Console.WriteLine(daznis.Key + " " + daznis.Value);
            }
            while (true) { }
        }
    }
}
