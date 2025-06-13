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
        private string agentoProgramosKelias = @"..\..\..\..\..\..\Scanner agent\Scanner App\bin\Debug\Scanner App.exe";
        private int ScannerCoreNumber = 2;//Pirmas core - Receiver, todėl pradedama nuo antrojo
        public Form1()
        {
            try
            {
                Process currentProcess = Process.GetCurrentProcess();
                currentProcess.ProcessorAffinity = (IntPtr)(int)Math.Pow(2, 0);
            }
            catch
            {
                MessageBox.Show("Nepavyko priskirti branduolio");
            }
            PriiemimoEile = new BlockingCollection<Dictionary<string, int>>();//Receiver objektams perduoti informaciją į DictionaryJoiner
            AtnaujinimoEile = new BlockingCollection<List<KeyValuePair<string, int>>>();//DictionaryJoiner perduoti informaciją į Textbox atnaujinimo funkciją
            InitializeComponent();
            Task atnaujintiTextbox = Task.Run(() => update_gridView_contents());
            DictionaryJoiner DictionaryJoiner = new DictionaryJoiner();
            Task jungtiDictionaries = Task.Run(() => DictionaryJoiner.IjungtiDictionaryJoiner(PriiemimoEile, AtnaujinimoEile));
        }

        private void GetInputFromPipe(string PipeName)
        {
            Receiver receiver = new Receiver(ref PriiemimoEile, PipeName);
        }


        private void label1_Click(object sender, EventArgs e)
        {

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
        private void callScannerProcess(int coreNumber, string katalogoKelias, string PipeName)
        {
            string coreNumberString = coreNumber.ToString();//int reikia paversti į string, kad būtų galima siūsti per proceso argumentus
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = agentoProgramosKelias,
                Arguments = $"\"{katalogoKelias}\" {coreNumberString} \"{PipeName}\"",//Šito reikia, kad kelias nebūtų suskaldytas į kelis string[] elementus, jei yra tarpų,
                //Tarp kintamųjų Arguments turi būti vienas tarpas
                UseShellExecute = false,
                CreateNoWindow = true//Išjungiau receiver konsolės langą, kad nereikėtų jame spausti enter, kad testų skenavimą.
            };
            MessageBox.Show("Pradetas procesas su keliu: " + startInfo.Arguments);
            Process process = Process.Start(startInfo);
            Task getInputFromPipe = Task.Run(() => GetInputFromPipe(PipeName));
        }

        private void folderDialog_and_AgentCreation()
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
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            button1.Enabled = false;//išjungiu mygtuką, kad vartotojas vėl nespaustų
            folderDialog_and_AgentCreation();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;//išjungiu mygtuką, kad vartotojas vėl nespaustų
            folderDialog_and_AgentCreation();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
