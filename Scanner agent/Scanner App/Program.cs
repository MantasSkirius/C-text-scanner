using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanner_App
{
    internal class Program
    {

        public static void Main(string[] argumentai)
        {
            //Argumentu formatas:  $"\"{katalogoKelias}\" {coreNumberString} \"{PipeName}\
            //Šitos dalies su Katalogo kelių masyvu reikia, nes kompiliatorius tikisi matyti string[] Main metodo argumentuose.
            string Katalogo_kelias = @"..\..\..\..\..\..\Tekstai skaitymui";
            int ScannerCoreNumber = 2;
            string search_pattern = "*.txt";
            string PipeName = "dazniuSiuntimoVamzdis2";
            //Dictionary<int, int> intToCore = new Dictionary<int, int>
            //{
            //    {2,0x2},
            //    {3,0x4},
            //    {4,0x8}
            //};
            if (argumentai.Length == 3 && argumentai != null)
            {
                Katalogo_kelias = argumentai[0];
                ScannerCoreNumber = int.Parse(argumentai[1]);
                PipeName = argumentai[2];
                Console.WriteLine("Atsiūsta informacija: " + ScannerCoreNumber + " " + Katalogo_kelias + " " + PipeName);
            }
            else
            {
                Console.WriteLine("Katalogo kelias nenurodytas, naudojamas numatytasis katalogas.");
            }
            try {
                Process currentProcess = Process.GetCurrentProcess();
                //currentProcess.ProcessorAffinity = (IntPtr)intToCore[ScannerCoreNumber];
                currentProcess.ProcessorAffinity = (IntPtr)(int)Math.Pow(2, ScannerCoreNumber);
            }
            catch {
                Console.WriteLine("Nepavyko priskirti branduolio");
                while (true) { }
            }




            Console.WriteLine(Katalogo_kelias);
            Console.WriteLine("Katalogo kelias su kuriuo kuriamas agentas: " + Katalogo_kelias);
            Agentas agent = new Agentas(Katalogo_kelias, search_pattern, PipeName);

        }
    }
}
