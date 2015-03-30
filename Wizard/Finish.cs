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
    public partial class Finish : MyUserControl, IWizard
    {
        public Finish()
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
    }
}
