using System.Collections.Concurrent;
using System.Diagnostics;
using System.Windows.Forms;

namespace Receiver_master_GUI
{
    public partial class Form1 : Form
    {
        private BlockingCollection<Dictionary<string, int>> PriiemimoEile, AtnaujinimoEile;
        private string agentoProgramosKelias = @"..\..\..\..\..\..\Scanner agent\Scanner App\bin\Debug\Scanner App.exe";
        private int ScannerCoreNumber = 2;//Pirmas core - Receiver, todėl pradedama nuo antrojo
        public Form1()
        {
            PriiemimoEile = new BlockingCollection<Dictionary<string, int>>();//Receiver objektams perduoti informaciją į DictionaryJoiner
            AtnaujinimoEile = new BlockingCollection<Dictionary<string, int>>();//DictionaryJoiner perduoti informaciją į Textbox atnaujinimo funkciją
            InitializeComponent();
            Task atnaujintiTextbox = Task.Run(() => update_textbox_contents());
            DictionaryJoiner DictionaryJoiner = new DictionaryJoiner();
            Task jungtiDictionaries = Task.Run(() => DictionaryJoiner.IjungtiDictionaryJoiner(PriiemimoEile, AtnaujinimoEile));
        } 

        private void GetInputFromPipe()
        {
            Receiver receiver = new Receiver(ref PriiemimoEile);
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }
        
        private void update_textbox_contents()
        {
            foreach (Dictionary<string, int> Dazniai in AtnaujinimoEile.GetConsumingEnumerable())
            {
                string NaujasTekstas = "";
                ;
                foreach(KeyValuePair<string, int> daznis in Dazniai)
                {
                    NaujasTekstas += (daznis.Value + " " + daznis.Key + Environment.NewLine);
                }
                //Invoke paleidžia metodą ne per šį thread, o per UI threadą (pagrindinį)
                textBox1.Invoke((MethodInvoker)delegate {
                    textBox1.Text = NaujasTekstas;
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
                UseShellExecute = false
            };
            MessageBox.Show("Pradetas procesas su keliu: " + startInfo.Arguments);
            Process process = Process.Start(startInfo);
            Task getInputFromPipe = Task.Run(() => GetInputFromPipe());
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Pasirinkite kataloga, iš kurio bus skanuojami .txt failai: ";
                if (folderDialog.ShowDialog(this) == DialogResult.OK)
                {
                    MessageBox.Show("Pasirinktas katalogas: " + folderDialog.SelectedPath);
                    MessageBox.Show("Agento programos kelias: " + agentoProgramosKelias);
                    callScannerProcess(ScannerCoreNumber, folderDialog.SelectedPath, ("dazniuSiuntimoVamzdis"+ScannerCoreNumber));
                    ScannerCoreNumber++;
                }
            }
        }
    }
}
