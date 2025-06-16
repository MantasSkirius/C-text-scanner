using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Windows.Forms;

namespace Receiver_master_GUI
{
    public partial class Form1 : Form
    {
        private BlockingCollection<Dictionary<string, int>> PriiemimoEile;
        private BlockingCollection<List<KeyValuePair<string, int>>> AtnaujinimoEile;
        private BlockingCollection<Tuple<string, Dictionary<string, int>>> PriiemimoEile2;
        private BlockingCollection<List<Tuple<string, KeyValuePair<string, int>>>> AtnaujinimoEile2;
        private string agentoProgramosKelias = @"..\..\..\..\..\..\Scanner agent\Scanner App\bin\Debug\Scanner App.exe";
        private int ScannerCoreNumber = 2;//Pirmas core - Receiver, todėl skaičiuoti pradedama nuo antrojo
        public Form1()
        {
            try
            {
                Process currentProcess = Process.GetCurrentProcess();
                currentProcess.ProcessorAffinity = (IntPtr)(int)Math.Pow(2, 0);//Kėlimo laipsniu reikia dėl keistų branduolių skaičių. 1, 2, 4, 8
            }
            catch
            {
                MessageBox.Show("Nepavyko priskirti procesoriaus branduolio");
            }
            InitializeComponent();
            PriiemimoEile = new BlockingCollection<Dictionary<string, int>>();//Receiver objektams perduoti informaciją į DictionaryJoiner
            PriiemimoEile2 = new BlockingCollection<Tuple<string, Dictionary<string, int>>>();
            AtnaujinimoEile = new BlockingCollection<List<KeyValuePair<string, int>>>();//DictionaryJoiner perduoti informaciją į Textbox atnaujinimo funkciją
            AtnaujinimoEile2 = new BlockingCollection<List<Tuple<string, KeyValuePair<string, int>>>>();
            Task atnaujintiTextbox = Task.Run(() => update_gridView_contents());
            Task atnaujintiTextbox2 = Task.Run(() => update_gridView2_contents());
            DictionaryJoiner DictionaryJoiner = new DictionaryJoiner();
            Task jungtiDictionaries = Task.Run(() => DictionaryJoiner.IjungtiDictionaryJoiner(PriiemimoEile, AtnaujinimoEile));
            DictionaryFileSeperatedJoiner dictionaryFileSeperatedJoiner = new DictionaryFileSeperatedJoiner();
            Task jungtiDictionariesSuFailuVardais = Task.Run(() => dictionaryFileSeperatedJoiner.IjungtiDictionaryJoinerSuFailuVardais(PriiemimoEile2, AtnaujinimoEile2));
        }

        private void GetInputFromPipe(string PipeName)
        {
            Receiver receiver = new Receiver(ref PriiemimoEile, ref PriiemimoEile2, PipeName);
        }

        private void update_gridView_contents()
        {
            foreach (List<KeyValuePair<string, int>> Dazniai in AtnaujinimoEile.GetConsumingEnumerable())
            {
                //Invoke paleidžia metodą ne per šį thread, o per UI thread (pagrindinį)
                dataGridView1.Invoke((MethodInvoker)delegate
                {
                    dataGridView1.DataSource = Dazniai;
                    dataGridView1.Columns[0].HeaderText = "Žodis";
                    dataGridView1.Columns[1].HeaderText = "Dažnis";
                });
            }
        }

        private void update_gridView2_contents()
        {
            foreach(List<Tuple<string, KeyValuePair<string, int>>> Dazniai in AtnaujinimoEile2.GetConsumingEnumerable())
            {
                List<Tuple<string, string, int>> turinys = new List<Tuple<string, string, int>>();
                foreach(Tuple<string, KeyValuePair<string, int>>daznis in Dazniai)//Pakeičių duomenų formatą, kad būtų lengviau parodyti lentelėje
                {
                    turinys.Add(new Tuple<string, string, int>(daznis.Item1, daznis.Item2.Key, daznis.Item2.Value));
                }
                dataGridView2.Invoke((MethodInvoker)delegate
                {
                    dataGridView2.DataSource = turinys;
                    dataGridView2.Columns[0].HeaderText = "Failas";
                    dataGridView2.Columns[1].HeaderText = "Žodis";
                    dataGridView2.Columns[2].HeaderText = "Dažnis";
                });
            }
        }
        private void callScannerProcess(int coreNumber, string katalogoKelias, string PipeName)
        {
            string coreNumberString = coreNumber.ToString();//int reikia paversti į string, kad būtų galima siūsti per proceso argumentus
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = agentoProgramosKelias,
                Arguments = $"\"{katalogoKelias}\" {coreNumberString} \"{PipeName}\"",//Šito reikia, kad kelias nebūtų suskaldytas į kelis string[] elementus, jei yra tarpų,
                //Tarp kintamųjų Arguments turi būti vienas tarpas
                CreateNoWindow = true,//Išjungiau receiver konsolės langą, kad nereikėtų jame spausti enter, kad testų skenavimą.
                UseShellExecute = false
            };
            MessageBox.Show("Pradetas procesas su keliu: " + startInfo.Arguments);
            Process process = Process.Start(startInfo);
            Task getInputFromPipe = Task.Run(() => GetInputFromPipe(PipeName));
        }

        private bool ShowFolderDialog_and_CreateAgent()
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Pasirinkite kataloga, iš kurio bus skanuojami .txt failai: ";
                if (folderDialog.ShowDialog(this) == DialogResult.OK)
                { 
                    MessageBox.Show("Pasirinktas katalogas: " + folderDialog.SelectedPath);
                    MessageBox.Show("Agento programos kelias: " + agentoProgramosKelias);
                    callScannerProcess(ScannerCoreNumber, folderDialog.SelectedPath, ("dazniuSiuntimoVamzdis" + ScannerCoreNumber));
                    ScannerCoreNumber++;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (ShowFolderDialog_and_CreateAgent())
            {
                button1.Enabled = false;//išjungiu mygtuką, kad vartotojas vėl nespaustų
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (ShowFolderDialog_and_CreateAgent())
            {
                button2.Enabled = false;//išjungiu mygtuką, kad vartotojas vėl nespaustų
            }
        }

    }
}
