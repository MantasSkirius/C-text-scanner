using System;
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
        public Dictionary<string, int> Dazniai { get; set; }
        public Receiver()
        {
            using var server = new NamedPipeServerStream("dazniuSiuntimoVamzdis", PipeDirection.In);
            MessageBox.Show("Waiting for agent to connect...");
            server.WaitForConnection();

            using var reader = new StreamReader(server);
            string message = reader.ReadLine();

            MessageBox.Show("Received from agent: " + message);
            Dazniai = JsonConvert.DeserializeObject<Dictionary<string, int>>(message);
        }
    }
}
