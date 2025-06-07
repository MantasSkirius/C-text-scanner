using System.Collections.Concurrent;
using System.Diagnostics;
using System.Windows.Forms;

namespace Receiver_master_GUI
{
    public partial class Form1 : Form
    {
        private BlockingCollection<Dictionary<string, int>> PriiemimoEile;
        public Form1()
        {
            PriiemimoEile = new BlockingCollection<Dictionary<string, int>>();
            InitializeComponent();
            Task atnaujintiTextbox = Task.Run(() => update_textbox_contents());
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
            foreach (Dictionary<string, int> Dazniai in PriiemimoEile.GetConsumingEnumerable())
            {
                foreach(KeyValuePair<string, int> daznis in Dazniai)
                {
                    //Invoke paleidžia metodą ne per šį thread, o per UI threadą (pagrindinį)
                    textBox1.Invoke((MethodInvoker)delegate {
                        textBox1.AppendText(daznis.Value + " " + daznis.Key + Environment.NewLine);
                    });
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Pasirinkite kataloga, iš kurio bus skanuojami .txt failai: ";
                if (folderDialog.ShowDialog(this) == DialogResult.OK)
                {
                    MessageBox.Show("Jūs pasirinkote katalogą: " + folderDialog.SelectedPath);
                    //string agentoProgramosKelias = Path.Combine(Application.StartupPath, @"..\..\..\..\..\..\Scanner agent\Scanner App\bin\Debug\Scanner App.exe");
                    string agentoProgramosKelias = @"..\..\..\..\..\..\Scanner agent\Scanner App\bin\Debug\Scanner App.exe";
                    //string agentoProgramosKelias = @"D:\AAA PROGRAMAVIMAS\C# namu darbas\Scanner agent\Scanner App\bin\Debug";
                    MessageBox.Show("Agento programos kelias: " + agentoProgramosKelias);
                    string katalogoKelias = folderDialog.SelectedPath;


                    ProcessStartInfo startInfo = new ProcessStartInfo()
                    {
                        FileName = agentoProgramosKelias,
                        Arguments = $"\"{katalogoKelias}\"",//Šito reikia, kad kelias nebūtų suskaldytas į kelis string[] elementus, jei yra tarpų
                        UseShellExecute = false
                    };  
                    MessageBox.Show("Pradetas procesas su keliu: " + startInfo.Arguments);
                    using (Process process = Process.Start(startInfo))
                    {
                        //process.WaitForExit();
                        //Console.WriteLine("The program exited with code: " + process.ExitCode);
                    }
                    Task getInput = Task.Run(() => GetInputFromPipe());
                }
            }
        }
    }
}
