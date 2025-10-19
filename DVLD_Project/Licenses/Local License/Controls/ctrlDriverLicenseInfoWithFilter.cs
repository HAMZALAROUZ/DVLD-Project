using DVLD_Buisness;
using DVLD_Project.Global_Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Licenses.Locale_License.Controls
{
    public partial class ctrlDriverLicenseInfoWithFilter : UserControl
    {
        int _LicenseID = -1;
        clsLicense _LicenseInfo;
        
        public event Action<int> OnLicenseSelected;
        protected virtual void LicenseSelected(int LicenseID)
        {
            Action<int> Handler = OnLicenseSelected;

            if (Handler != null)
            {
                Handler(LicenseID);
            }
        }

        public int LicenseID
        {
            get { return _LicenseID; }
        }

        public clsLicense LicenseInfo
        {
            get { return _LicenseInfo; }
        }

        public void FilterFocus()
        {
            txtLicenseID.Focus();
        }
        private bool _FilterEnable = true;
        public bool FilterEnable
        {
            get { return (_FilterEnable); }
            set { _FilterEnable = value; gbFilters.Enabled = _FilterEnable; }
        }

        public ctrlDriverLicenseInfoWithFilter()
        {
            InitializeComponent();
        }
        
        private void btnFind_Click(object sender, EventArgs e)
        {
            if (!clsValidation.IsNumber(txtLicenseID.Text))
            {
                MessageBox.Show("Error : Invalide License ID Value", "Invalide", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            

            if (int.TryParse(txtLicenseID.Text, out _LicenseID) && _LicenseID > 0)
            {
                LoadLicenseInfo(_LicenseID);
                
            }
        }
        public void LoadLicenseInfo(int LicenseID)
        {


            txtLicenseID.Text = LicenseID.ToString();
            ctrlDriverLicenseInfo1.LoadData(LicenseID);
            _LicenseID = ctrlDriverLicenseInfo1.LicenseID;
            _LicenseInfo = ctrlDriverLicenseInfo1.SelectedLicenseInfo;

            if (OnLicenseSelected != null && FilterEnable && ctrlDriverLicenseInfo1.SelectedLicenseInfo!=null)
                // Raise the event with a parameter
                OnLicenseSelected(_LicenseID);


        }
        private void txtLicenseID_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtLicenseID.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtLicenseID, "This field is required!");
            }
            else
            {
                //e.Cancel = false;
                errorProvider1.SetError(txtLicenseID, null);
            }
        }
      
        private void txtLicenseID_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);


            // Check if the pressed key is Enter (character code 13)
            if (e.KeyChar == (char)13)
            {

                btnFind.PerformClick();
            }
        }
    }
}
