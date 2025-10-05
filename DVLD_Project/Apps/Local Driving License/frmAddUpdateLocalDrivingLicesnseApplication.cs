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

namespace DVLD_Project.Apps.Local_Driving_License
{
    public partial class frmAddUpdateLocalDrivingLicesnseApplication : Form
    {
        private enum _enMode { _Addnew=0,_Update=1}
        private _enMode Mode = _enMode._Addnew;

        private int _LDLA_ID = -1;
        private clsLocalDrivingLicenseApplication _LDLA = new clsLocalDrivingLicenseApplication();

        //I do this To get Fees Of this Application
        clsApplicationType AppType = clsApplicationType.GetApplicationTypeInfoByID(1);

        //for add new
        public frmAddUpdateLocalDrivingLicesnseApplication()
        {
            InitializeComponent();
            Mode= _enMode._Addnew;
        }
        //for update
        public frmAddUpdateLocalDrivingLicesnseApplication(int LDLA_ID)
        {
            InitializeComponent();
            _LDLA_ID = LDLA_ID;
            Mode = _enMode._Update;
        }
        private void btnApplicationInfoNext_Click(object sender, EventArgs e)
        {
            if(Mode == _enMode._Addnew)
            {
                if(ctrlPersonCardWithFilter1.PersonID == -1)
                {
                    MessageBox.Show("Select A Person First", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    tpApplicationInfo.Enabled = true;
                    tcApplicationInfo.SelectedTab = tpApplicationInfo;
                    btnSave.Enabled = true;


                    lblApplicationDate.Text = DateTime.Now.ToString();
                    lblFees.Text = AppType.ApplicationFees.ToString(); //Fees Of All Application Service is 15$
                    lblCreatedByUser.Text = clsGlobal.CurrentUser.UserName.ToString();
                }
            }else
            {
                tcApplicationInfo.SelectedTab = tpApplicationInfo;
            }
        }
        private void ResetValues()
        {
            lblLocalDrivingLicebseApplicationID.Text = "[???]";
            lblApplicationDate.Text = "[??/??/????]";
            lblFees.Text = "[$$$]";
            lblCreatedByUser.Text = "[????]";

            //This Is To Fill ComboBox With Data
            DataTable _dtAllLicencesClasses = clsLicenseClass.GetAllLicenseClasses();
            if (_dtAllLicencesClasses.Rows.Count > 0)
            {
                cbLicenseClass.DataSource = _dtAllLicencesClasses;
                cbLicenseClass.DisplayMember = "ClassName";
                cbLicenseClass.ValueMember = "LicenseClassID";
            }
        }

        private void FillFormWithData()
        {
            lblLocalDrivingLicebseApplicationID.Text = _LDLA.LocalDrivingLicenseApplicationID.ToString();
            lblApplicationDate.Text = _LDLA.ApplicationDate.ToString();
            lblFees.Text = _LDLA.ApplicationTypeInfo.ApplicationFees.ToString();
            lblCreatedByUser.Text = _LDLA.CreatedByUserID.ToString();
        }
        private void frmAddUpdateLocalDrivingLicesnseApplication_Load(object sender, EventArgs e)
        {
            //Add New Mode
            if(Mode == _enMode._Addnew)
            {
                ResetValues();
                lblTitle.Text = "New Local Driving License Application";
                btnSave.Enabled = false;
                tpApplicationInfo.Enabled = false;
            }
            else //Update Mode
            {
                ResetValues();
                lblTitle.Text = "Update Local Driving License Application";

                _LDLA = clsLocalDrivingLicenseApplication.FindByID(_LDLA_ID);

                if(_LDLA != null)
                {
                    ctrlPersonCardWithFilter1.LoadPersonInfo(_LDLA.ApplicantPersonID);
                    ctrlPersonCardWithFilter1.FilterEnable = false;

                    FillFormWithData();
                    btnSave.Enabled = true ;
                }
                else
                {
                    MessageBox.Show("No Local Driving License Application With This ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int LicenseID = (int)cbLicenseClass.SelectedValue;
            clsPerson _Person = ctrlPersonCardWithFilter1.SelectedPersonInfo;


            if (_Person.PersonID != -1)
            {
                _LDLA.ApplicantPersonID = _Person.PersonID;
            }
            else
            {
                MessageBox.Show("No Person Selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(clsLicense.FindActiveLicenseID(_Person.PersonID,LicenseID) != -1)
            {
                MessageBox.Show("This Person Already Has This License","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }

            if (clsApplication.GetActiveApplicationIDForLicenseClass(_Person.PersonID,1,LicenseID) != -1)
            {
                MessageBox.Show("Sorry This Person Has Same Application Before","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }

           

           
            _LDLA.ApplicationDate = DateTime.Now;//The date of creat applocation, this not changed in update mode
            _LDLA.ApplicationStatus = clsApplication.enApplicationStatus.New;
            _LDLA.ApplicationTypeID = (int)clsApplication.enApplicationType.NewDrivingLicense;
            _LDLA.LastStatusDate = DateTime.Now;//changed when any updates happend
            _LDLA.CreatedByUserID = clsGlobal.CurrentUser.UserID;
            _LDLA.PaidFees = AppType.ApplicationFees;

            _LDLA.LicenseClassID = Convert.ToInt32(cbLicenseClass.SelectedValue);

            if(_LDLA.Save())
            {
                MessageBox.Show("New Application Saved", "Confirm", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblLocalDrivingLicebseApplicationID.Text = _LDLA.LocalDrivingLicenseApplicationID.ToString();
            }
            else
            {
                MessageBox.Show("New Application Not Saved", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }

            
            

        }
    }
}
