using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Licenses
{
    public partial class frmShowPersonLicenseHistory : Form
    {
        int _PersonID = -1;
        public frmShowPersonLicenseHistory(int PersonID)
        {
            InitializeComponent();
            _PersonID = PersonID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmShowPersonLicenseHistory_Load(object sender, EventArgs e)
        {
            if(_PersonID != -1)
            {
                ctrlPersonCardWithFilter1.LoadPersonInfo(_PersonID);
                ctrlPersonCardWithFilter1.FilterEnable = false;
                ctrlDriverLicenses1.LoadDataByPersonID(_PersonID);
            }
            else
            {
                ctrlPersonCardWithFilter1.FilterEnable = true;                                
            }
                       
        }

        private void ctrlPersonCardWithFilter1_OnPersonSelected(int obj)
        {
            if (obj != -1)
                ctrlPersonCardWithFilter1.LoadPersonInfo(_PersonID);
            //else
                //clear cntrlDriverLicenses
        }
    }
}
