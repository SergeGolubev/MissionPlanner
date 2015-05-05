using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CreateMap
{
    public partial class Form1 : Form
    {
        string ImagePath;
        string LogPath;
        string outPath;
        static string pathToVisualSFM = "visualSFM/visualSFM.exe";
        static string pathToCMPMVS = "CMPMVS/cmpmvs.exe";
        System.Diagnostics.ProcessStartInfo procSFM;
        System.Diagnostics.ProcessStartInfo procCMPMVS;
        bool runState = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ImagePath = textBox1.Text ;
            outPath = textBox2.Text;
            if (!runState)
            {
                if (!(outPath == null || ImagePath == null))// || LogPath == null))
                {
                    timer1.Enabled = true;
                    runState = true;
                    button1.Text = "Остановить";
                    label1.Text = "Шаг 1 из 3 : нахождение позиций камер";
                    //run create log to exif
                    /*procSFM = new System.Diagnostics.ProcessStartInfo();
                    procSFM.FileName = pathToVisualSFM; // Путь к приложению
                    procSFM.UseShellExecute = false;
                    procSFM.CreateNoWindow = true;
                    procSFM.RedirectStandardOutput = true;
                    procSFM.Arguments = "sfm+cmp " + ImagePath;
                    System.Diagnostics.Process.Start(procSFM);*/
                }
                else
                {
                    MessageBox.Show("Выберите путь к папке с изображениями и пусть для сохранения мкрты");
                };
            }
            else
            {
                timer1.Enabled = false;
                runState = false;
                button1.Text = "Запуск";
                progressBar1.Value = 0;
            };
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();
            ImagePath = fbd.SelectedPath;
            textBox1.Text = ImagePath;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();
            textBox2.Text = fbd.SelectedPath;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            progressBar1.PerformStep(); ;
        }
    }
}
