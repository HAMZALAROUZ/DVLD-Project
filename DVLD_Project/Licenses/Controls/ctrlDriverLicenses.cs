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

namespace DVLD_Project.Licenses.Controls
{
    public partial class ctrlDriverLicenses : UserControl
    {
        private int _DriverID = -1;
        private clsDriver _Driver;
        DataTable _dtDriverLocalLicensesHistory;
        DataTable _dtDriverInternationalLicensesHistory;

        //public int DriverID { get { return _DriverID; } }
        //public clsDriver DriverInfo {  get { return _Driver; } }
        public ctrlDriverLicenses()
        {
            InitializeComponent();

            lblLocalLicensesRecords.Text = "0";
            tbInternationalLicenses.Text = "0";
        }
        private void _LoadInternationalLicenseInfo()
        {
            _dtDriverInternationalLicensesHistory = clsInternationalLicense.GetDriverInternationalLicenses(_DriverID);

            dgvInternationalLicensesHistory.DataSource = _dtDriverInternationalLicensesHistory;
            lblInternationalLicensesRecords.Text = dgvInternationalLicensesHistory.Rows.Count.ToString();

            if (dgvInternationalLicensesHistory.Rows.Count > 0)
            {
                dgvInternationalLicensesHistory.Columns[0].HeaderText = "Int.License ID";
                dgvInternationalLicensesHistory.Columns[0].Width = 160;

                dgvInternationalLicensesHistory.Columns[1].HeaderText = "Application ID";
                dgvInternationalLicensesHistory.Columns[1].Width = 130;

                dgvInternationalLicensesHistory.Columns[2].HeaderText = "L.License ID";
                dgvInternationalLicensesHistory.Columns[2].Width = 130;

                dgvInternationalLicensesHistory.Columns[3].HeaderText = "Issue Date";
                dgvInternationalLicensesHistory.Columns[3].Width = 180;

                dgvInternationalLicensesHistory.Columns[4].HeaderText = "Expiration Date";
                dgvInternationalLicensesHistory.Columns[4].Width = 180;

                dgvInternationalLicensesHistory.Columns[5].HeaderText = "Is Active";
                dgvInternationalLicensesHistory.Columns[5].Width = 120;

            }
        }

        private void _LoadLocalLicenseInfo()
        {
            _dtDriverLocalLicensesHistory = clsLicense.GetDriverLicenses(_DriverID);

            dgvLocalLicensesHistory.DataSource = _dtDriverLocalLicensesHistory;
            lblLocalLicensesRecords.Text = dgvLocalLicensesHistory.Rows.Count.ToString();

            if (dgvLocalLicensesHistory.Rows.Count > 0)
            {
                dgvLocalLicensesHistory.Columns[0].HeaderText = "Lic.ID";
                dgvLocalLicensesHistory.Columns[0].Width = 110;

                dgvLocalLicensesHistory.Columns[1].HeaderText = "App.ID";
                dgvLocalLicensesHistory.Columns[1].Width = 110;

                dgvLocalLicensesHistory.Columns[2].HeaderText = "Class Name";
                dgvLocalLicensesHistory.Columns[2].Width = 270;

                dgvLocalLicensesHistory.Columns[3].HeaderText = "Issue Date";
                dgvLocalLicensesHistory.Columns[3].Width = 170;

                dgvLocalLicensesHistory.Columns[4].HeaderText = "Expiration Date";
                dgvLocalLicensesHistory.Columns[4].Width = 170;

                dgvLocalLicensesHistory.Columns[5].HeaderText = "Is Active";
                dgvLocalLicensesHistory.Columns[5].Width = 110;

            }
        }
        public void LoadDataByPersonID(int PersonID)
        {
            _Driver = clsDriver.GetDriverInfoByPersonID(PersonID);

            if (_Driver != null)
            {
                _DriverID = _Driver.DriverID;
            }

            _LoadLocalLicenseInfo();
            _LoadInternationalLicenseInfo();
        }
        public void LoadData(int DriverID)
        {
            _DriverID = DriverID;
            _Driver = clsDriver.GetDriverInfoByDriverID(DriverID);


            _LoadLocalLicenseInfo();
            _LoadInternationalLicenseInfo();

        }

        private void tcDriverLicenses_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }
    }
}
