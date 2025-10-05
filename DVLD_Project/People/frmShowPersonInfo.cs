using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_Buisness;

namespace DVLD_Project.People
{
    public partial class frmShowPersonInfo : Form
    {
        public frmShowPersonInfo()
        {
            InitializeComponent();
        }

        public frmShowPersonInfo(int _PersonID)
        {
            InitializeComponent();

            ctrlPersonCard1.LoadData(_PersonID);
            
        }
        public frmShowPersonInfo(string _NationalNo)
        {
            InitializeComponent();

            ctrlPersonCard1.LoadData(_NationalNo);

        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
