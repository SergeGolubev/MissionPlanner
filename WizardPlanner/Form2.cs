using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MissionPlanner
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
                Application.Exit();
                e.Cancel = false;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            //MainV2 mclass = new MainV2();
            //mclass.MenuConnect.PerformClick();
            //for (int i = 0; i < 200001; i++)
            //{
              //  progressBar1.Value = i*100/200000;
            //}
            
            MAVLinkInterface comPort = new MAVLinkInterface();
            comPort.BaseStream.BaudRate = 115200;
            comPort.giveComport = false;

            //log.Info("MenuConnect Start");

            // sanity check
            if (comPort.BaseStream.IsOpen && comPort.MAV.cs.groundspeed > 4)
            {
                if (DialogResult.No == CustomMessageBox.Show(Strings.Stillmoving, Strings.Disconnect, MessageBoxButtons.YesNo))
                {
                    return;
                }
            }

            try
            {
                //log.Info("Cleanup last logfiles");
                // cleanup from any previous sessions
                if (comPort.logfile != null)
                    comPort.logfile.Close();

                if (comPort.rawlogfile != null)
                    comPort.rawlogfile.Close();
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show(Strings.ErrorClosingLogFile + ex.Message, Strings.ERROR);
            }

            comPort.logfile = null;
            comPort.rawlogfile = null;

            // decide if this is a connect or disconnect
            if (comPort.BaseStream.IsOpen)
            {
                //doDisconnect();
              
            }
            else
            {
                doConnect();
                /*log.Info("We are connecting");
          switch (_connectionControl.CMB_serialport.Text)
          {
              case "TCP":
                  comPort.BaseStream = new TcpSerial();
                  break;
              case "UDP":
                  comPort.BaseStream = new UdpSerial();
                  break;
              case "UDPCl":
                  comPort.BaseStream = new UdpSerialConnect();
                  break;
              case "AUTO":
              default:
                  comPort.BaseStream = new SerialPort();
                  break;
          }*/
            }
           
        }

        private void button1_Click(object sender, EventArgs e)
        {

            Form3 form3 = new Form3();
            this.Hide();
            form3.ShowDialog();
            this.Close();
        }

       
    }
}
