﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MissionPlanner.Wizard
{
    public partial class Finish : MyUserControl, IWizard
    {
        float throttle;
        float minThrottle;
        public Finish()
        {
            InitializeComponent();
            label4.Visible = false;
        }
        public int WizardValidate()
        {
            return 1;
        }
        public bool WizardBusy()
        {
            return false;
        }
        public void FinishStartShow()
        {
            label4.Visible = true;
            label2.Visible = false;
            label3.Visible = false;
            throttle = MainV2.comPort.MAV.cs.ch3in;
            minThrottle = (float)MainV2.comPort.MAV.param["RC3_MIN"];
            while (throttle >= minThrottle + 50)
            {
                throttle = MainV2.comPort.MAV.cs.ch3in;
            }
            label4.Visible = false;
            label2.Visible = true;
            label3.Visible = true;
        }
    }
}
