using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MissionPlanner.Controls;

namespace MissionPlanner.Wizard
{
    public partial class DialCalib : Form
    {
        byte count = 0;
        bool busy;
        public DialCalib()
        {
            InitializeComponent();
            busy = false;
            Utilities.ThemeManager.ApplyThemeTo(this);
        }

        public void Calib_Load(object sender, EventArgs e)
        {
            busy = true;
            try
            {
                // start the process off
                MainV2.comPort.doCommand(MAVLink.MAV_CMD.PREFLIGHT_CALIBRATION, 0, 0, 0, 0, 1, 0, 0);
                MainV2.comPort.giveComport = true;
            }
            catch { busy = false; CustomMessageBox.Show(Strings.ErrorNoResponce, Strings.ERROR); return; }
            System.Threading.ThreadPool.QueueUserWorkItem(readmessage, this);
            BUT_continue.Focus();
        }

        public bool Busy()
        {
            return busy;
        }

        private void BUT_continue_Click(object sender, EventArgs e)
        {
            count++;
            if (BUT_continue.Text == "Finish")
            {
                busy = false;
                this.Close();
            }
            try
            {
                 MainV2.comPort.sendPacket(new MAVLink.mavlink_command_ack_t() { command = 1, result = count });
            }
            catch (Exception ex) { CustomMessageBox.Show(Strings.CommandFailed + ex); Wizard.instance.Close(); }
        }
        static void readmessage(object item)
        {
            DialCalib local = (DialCalib)item;

            // clean up history
            MainV2.comPort.MAV.cs.messages.Clear();
            while (!(MainV2.comPort.MAV.cs.message.ToLower().Contains("calibration successful") || MainV2.comPort.MAV.cs.message.ToLower().Contains("calibration failed")))
            {
                try
                {
                    System.Threading.Thread.Sleep(10);
                    // read the message
                    if (MainV2.comPort.BaseStream.BytesToRead > 4)
                        MainV2.comPort.readPacket();
                    // update cs with the message
                    MainV2.comPort.MAV.cs.UpdateCurrentSettings(null);
                    // update user display
                    local.UpdateUserMessage();
                }
                catch { break; }
            }

            MainV2.comPort.giveComport = false;

            try
            {
                local.CalibDone();
            }
            catch { }
        }

        public void UpdateUserMessage()
        {
            this.Invoke((MethodInvoker)delegate()
            {
                if (MainV2.comPort.MAV.cs.message.ToLower().Contains("initi") /*|| count == 0*/ ) // #### добавлено второе условие
                {
                    imageLabel1.Image = MissionPlanner.Properties.Resources.calibration01;
                    imageLabel1.Text = MainV2.comPort.MAV.cs.message;
                }
                if (MainV2.comPort.MAV.cs.message.ToLower().Contains("level") /*|| count == 1 */)
                {
                    imageLabel1.Image = MissionPlanner.Properties.Resources.calibration01;
                    imageLabel1.Text = MainV2.comPort.MAV.cs.message;
                }
                else if (MainV2.comPort.MAV.cs.message.ToLower().Contains("left") /*|| count == 2*/)
                {
                    imageLabel1.Image = MissionPlanner.Properties.Resources.calibration07;
                    imageLabel1.Text = MainV2.comPort.MAV.cs.message;
                }
                else if (MainV2.comPort.MAV.cs.message.ToLower().Contains("right") /*|| count == 3*/)
                {
                    imageLabel1.Image = MissionPlanner.Properties.Resources.calibration05;
                    imageLabel1.Text = MainV2.comPort.MAV.cs.message;
                }
                else if (MainV2.comPort.MAV.cs.message.ToLower().Contains("down") /*|| count == 4*/)
                {
                    imageLabel1.Image = MissionPlanner.Properties.Resources.calibration04;
                    imageLabel1.Text = MainV2.comPort.MAV.cs.message;
                }
                else if (MainV2.comPort.MAV.cs.message.ToLower().Contains("up") /*|| count == 5*/)
                {
                    imageLabel1.Image = MissionPlanner.Properties.Resources.calibration06;
                    imageLabel1.Text = MainV2.comPort.MAV.cs.message;
                }
                else if (MainV2.comPort.MAV.cs.message.ToLower().Contains("back") /*|| count == 6*/)
                {
                    imageLabel1.Image = MissionPlanner.Properties.Resources.calibration03;
                    imageLabel1.Text = MainV2.comPort.MAV.cs.message;
                }
                else if (MainV2.comPort.MAV.cs.message.ToLower().Contains("calibration") /*|| count == 7*/)
                {
                    imageLabel1.Image = MissionPlanner.Properties.Resources.calibration01;
                    imageLabel1.Text = MainV2.comPort.MAV.cs.message;
                }

                imageLabel1.Refresh();
            });
        }

        public void CalibDone()
        {
            this.Invoke((MethodInvoker)delegate()
            {
                this.imageLabel1.Text = "Done";
                this.BUT_continue.Text = "Finish";
            });
        }
    }
}
