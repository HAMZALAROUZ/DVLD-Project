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

namespace DVLD_Project.Tests
{
    public partial class frmTakeTest : Form
    {
        int _LDLA_ID = -1;
        clsTestType.enTestType _TestTypeID;
        public frmTakeTest(int LDLA_ID,clsTestType.enTestType TestTypeID)
        {
            InitializeComponent();
            _LDLA_ID = LDLA_ID;
            _TestTypeID = TestTypeID;
        }

        private void frmTakeTest_Load(object sender, EventArgs e)
        {
            ctrlSecheduledTest1.LoadData(_LDLA_ID, _TestTypeID);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            clsTestAppointment testAppointment = clsTestAppointment.GetLastTestAppointment(_LDLA_ID,_TestTypeID);
            clsTest Test = new clsTest();

            if (testAppointment != null && !testAppointment.IsLocked)
            {
                Test.TestAppointmentID = testAppointment.TestAppointmentID;
                Test.CreatedByUserID = clsGlobal.CurrentUser.UserID;

                if(txtNotes.Text.Trim() != "")
                    Test.Notes = txtNotes.Text.Trim();
                else
                    Test.Notes = null;

                if(rbPass.Checked)
                    Test.TestResult = true;
                else
                    Test.TestResult = false;
                
                if(Test.Save())
                {
                    MessageBox.Show($"Test Results Saved ID = {Test.TestID}","Information",MessageBoxButtons.OK, MessageBoxIcon.Information);
                    testAppointment.IsLocked = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Test Results Not Saved","Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }

            }
            else
            {
                MessageBox.Show("Conditions Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }

        }
    }
}
