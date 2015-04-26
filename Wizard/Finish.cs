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
    public partial class Finish : MyUserControl, IWizard
    {
        float throttle;
        float minThrottle;
        bool first_time;
        public Finish()
        {
            InitializeComponent();
            label4.Visible = false;
            first_time = true;
        }
        public int WizardValidate()
        {
            return 1;
        }
        public bool WizardBusy()
        {
            return false;
        }
        public void Finish_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        public void Finish_Close()
        {
            timer1.Stop();
        }
        public void Throttle_check()
        {
            label4.Visible = true;
            label2.Visible = false;
            label3.Visible = false;
            throttle = MainV2.comPort.MAV.cs.ch3in;
            minThrottle = 900; //(float)MainV2.comPort.MAV.param["RC3_MIN"];
            while (throttle >= minThrottle + 50)
            {
                throttle = MainV2.comPort.MAV.cs.ch3in;
            }
            label4.Visible = false;
            label2.Visible = true;
            label3.Visible = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (first_time)
            {
                first_time = false;
                Throttle_check();
            }
            throttle = MainV2.comPort.MAV.cs.ch3in;
            if (throttle >= minThrottle + 50)
            {
                label4.Visible = true;
            }
            label4.Visible = false;
        }
    }
}
