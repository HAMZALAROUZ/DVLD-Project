using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Licenses.International_Licenses
{
    public partial class frmShowInternationalLicenseInfo : Form
    {
        int _InternationalLicenseID = -1;
        public frmShowInternationalLicenseInfo(int InternationalLicenseID)
        {
            InitializeComponent();
            _InternationalLicenseID = InternationalLicenseID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {

        }

        private void frmShowInternationalLicenseInfo_Load(object sender, EventArgs e)
        {
            if(_InternationalLicenseID != -1)
            {
                ctrlDriverInternationalLicenseInfo1.LoadData(_InternationalLicenseID);
            }
        }
    }
}
