using System.Diagnostics;

namespace Receiver_master_GUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            //D:\AAA PROGRAMAVIMAS\C# namu darbas\Scanner agent\Scanner App\bin\Debug\Scanner App.exe
            
            InitializeComponent();
        }

        private void GetInputFromPipe()
        {
            Receiver receiver = new Receiver();
            foreach (KeyValuePair<string, int> daznis in receiver.Dazniai)
            {
                textBox1.AppendText(daznis.Value + " " + daznis.Key + Environment.NewLine);
            }
        }


        private void label1_Click(object sender, EventArgs e)
        {

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
                        //Arguments = katalogoKelias,
                        UseShellExecute = false
                    };
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
