using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MissionPlanner.Wizard
{
    public partial class DS_Check : MyUserControl, IWizard
    {
        public DS_Check()
        {
            InitializeComponent();
        }
        public int WizardValidate()
        {
            return 1;
        }

        public bool WizardBusy()
        {
            return false;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
