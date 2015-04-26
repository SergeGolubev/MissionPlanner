﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MissionPlanner.Controls;
using System.IO;
using System.Threading;

namespace MissionPlanner.Wizard
{
    public partial class _5AccelCalib : MyUserControl, IWizard
    {
        byte count = 0;
        bool accelDispalying = true;
        static bool busy = false;
        DateTime lastScreenUpdate;
        Thread accel_thread;
        bool thread_started = false;
        DialCalib calibration;

        public _5AccelCalib()
        {
            InitializeComponent();
            hudWizard.Enabled = true;
            lastScreenUpdate = DateTime.Now;
            calibration = new DialCalib();
        }
        public int WizardValidate()
        {
            if (thread_started == true)
            {
                accel_thread.Abort();
            }
            accelDispalying = false;
            //MainV2.comPort.giveComport = false;
            
            return 1;
        }

        public bool WizardBusy()
        {
            busy = calibration.Busy();
            return busy;
        }

        
        private void BUT_start_Click(object sender, EventArgs e)
        {
            //myButton1.Enabled = false;
            ((MyButton)sender).Enabled = false;
           // BUT_continue.Enabled = true;
            accelDispalying = false; // stop displaying accelerometr horizon
            if (thread_started)
            {
                accel_thread.Abort();
                thread_started = false;
            }
   
            busy = true;

            // try
            //{
              //  // start the process off
                //MainV2.comPort.doCommand(MAVLink.MAV_CMD.PREFLIGHT_CALIBRATION, 0, 0, 0, 0, 1, 0, 0);
                //MainV2.comPort.giveComport = true;
            //}
            //catch { busy = false; CustomMessageBox.Show(Strings.ErrorNoResponce, Strings.ERROR); return; }

            calibration.Show();
            // start thread to update display 
            //System.Threading.ThreadPool.QueueUserWorkItem(readmessage, this);
            //BUT_continue.Focus();
        }


      /*  static void readmessage(object item)
        {
            _5AccelCalib local = (_5AccelCalib)item;
            
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
            busy = false;

            MainV2.comPort.giveComport = false;

            try
            {
                local.Invoke((MethodInvoker)delegate()
                {
                    local.imageLabel1.Text = "Done";
                    local.BUT_continue.Enabled = false; // было false
                    local.myButton1.Enabled = true;
                });
            }
            catch { }
        } */

       /* public void UpdateUserMessage()
        {
            this.Invoke((MethodInvoker)delegate()
            {
                if (MainV2.comPort.MAV.cs.message.ToLower().Contains("initi") ) // #### добавлено второе условие
                {
                    imageLabel1.Image = MissionPlanner.Properties.Resources.calibration01;
                    imageLabel1.Text = MainV2.comPort.MAV.cs.message;
                }
                if (MainV2.comPort.MAV.cs.message.ToLower().Contains("level") )
                {
                    imageLabel1.Image = MissionPlanner.Properties.Resources.calibration01;
                    imageLabel1.Text = MainV2.comPort.MAV.cs.message;
                }
                else if (MainV2.comPort.MAV.cs.message.ToLower().Contains("left") )
                {
                    imageLabel1.Image = MissionPlanner.Properties.Resources.calibration07;
                    imageLabel1.Text = MainV2.comPort.MAV.cs.message;
                }
                else if (MainV2.comPort.MAV.cs.message.ToLower().Contains("right") )
                {
                    imageLabel1.Image = MissionPlanner.Properties.Resources.calibration05;
                    imageLabel1.Text = MainV2.comPort.MAV.cs.message;
                }
                else if (MainV2.comPort.MAV.cs.message.ToLower().Contains("down") )
                {
                    imageLabel1.Image = MissionPlanner.Properties.Resources.calibration04;
                    imageLabel1.Text = MainV2.comPort.MAV.cs.message;
                }
                else if (MainV2.comPort.MAV.cs.message.ToLower().Contains("up") )
                {
                    imageLabel1.Image = MissionPlanner.Properties.Resources.calibration06;
                    imageLabel1.Text = MainV2.comPort.MAV.cs.message;
                }
                else if (MainV2.comPort.MAV.cs.message.ToLower().Contains("back") )
                {
                    imageLabel1.Image = MissionPlanner.Properties.Resources.calibration03;
                    imageLabel1.Text = MainV2.comPort.MAV.cs.message;
                }
                else if (MainV2.comPort.MAV.cs.message.ToLower().Contains("calibration") )
                {
                    imageLabel1.Image = MissionPlanner.Properties.Resources.calibration01;
                    imageLabel1.Text = MainV2.comPort.MAV.cs.message;
                }

                imageLabel1.Refresh();
            });
        } */

        private void BUT_continue_Click(object sender, EventArgs e)
        {
            count++;

            try
            {
                 MainV2.comPort.sendPacket(new MAVLink.mavlink_command_ack_t() { command = 1, result = count });
            }
            catch (Exception ex) { CustomMessageBox.Show(Strings.CommandFailed + ex); Wizard.instance.Close(); }
        }

        private void updateAccelerometrDisplay()
        {
            while (accelDispalying)
            {
                MainV2.comPort.requestDatastream(MAVLink.MAV_DATA_STREAM.EXTENDED_STATUS, MainV2.comPort.MAV.cs.ratestatus); // mode
                MainV2.comPort.requestDatastream(MAVLink.MAV_DATA_STREAM.POSITION, MainV2.comPort.MAV.cs.rateposition); // request gps
                MainV2.comPort.requestDatastream(MAVLink.MAV_DATA_STREAM.EXTRA1, MainV2.comPort.MAV.cs.rateattitude); // request attitude
                MainV2.comPort.requestDatastream(MAVLink.MAV_DATA_STREAM.EXTRA2, MainV2.comPort.MAV.cs.rateattitude); // request vfr
                MainV2.comPort.requestDatastream(MAVLink.MAV_DATA_STREAM.EXTRA3, MainV2.comPort.MAV.cs.ratesensors); // request extra stuff - tridge
                MainV2.comPort.requestDatastream(MAVLink.MAV_DATA_STREAM.RAW_SENSORS, MainV2.comPort.MAV.cs.ratesensors); // request raw sensor
                MainV2.comPort.requestDatastream(MAVLink.MAV_DATA_STREAM.RC_CHANNELS, MainV2.comPort.MAV.cs.raterc); // request rc inf
                MainV2.comPort.logreadmode = false;
                if (MainV2.comPort.logreadmode && MainV2.comPort.logplaybackfile != null)
                {
                    if (MainV2.comPort.BaseStream.IsOpen)
                    {
                        MainV2.comPort.logreadmode = false;
                        MainV2.comPort.logplaybackfile.Close();
                        MainV2.comPort.logplaybackfile = null;
                    }
                }
                MainV2.comPort.readPacket();
                updateBindingSource();
            }          
        }
        
        private void updateBindingSource()
        {

            // 25 Hz
            if ( this.lastScreenUpdate.AddMilliseconds(40) < DateTime.Now)
            {
                this.BeginInvoke((MethodInvoker)delegate()
                {
                        MainV2.comPort.MAV.cs.UpdateCurrentSettings(this.bindingSourceHud);
                });
                this.lastScreenUpdate = DateTime.Now;
            }
        }
        private void ShowHorizon(object sender, EventArgs e)
        {
            thread_started = true;
            accelDispalying = true;
            accel_thread = new Thread(updateAccelerometrDisplay);
            accel_thread.Start();
            //myButton1.Enabled = false;
        }

        private void hudWizard_Load(object sender, EventArgs e)
        {
            hudWizard.Controls["_batterylevel"].Hide();
            hudWizard.Controls["_gpshdop"].Hide();
        }

        public void AccelCalib_Close()
        {
            accelDispalying = false;
            accel_thread.Abort();
        }
    }
}
