using DVLD_Buisness;
using DVLD_Project.Apps.Application_Types;
using DVLD_Project.Apps.Local_Driving_License;
using DVLD_Project.Drivers;
using DVLD_Project.Global_Classes;
using DVLD_Project.People;
using DVLD_Project.Tests.Test_Type;
using DVLD_Project.User;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project
{
    public partial class Form1 : Form
    {
        clsUser LogedUser = clsGlobal.CurrentUser;
        public Form1()
        {
            InitializeComponent();
        }
        public Form1(clsUser _User)
        {
            InitializeComponent();
            LogedUser = _User;
        }
        private void peopleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frmlistPeople = new frmListPeople();
            frmlistPeople.ShowDialog();
        }

        private void signOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void employeesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmListUsers frmUsers = new FrmListUsers();
            frmUsers.ShowDialog();
        }

        private void driversToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListDrivers frm = new frmListDrivers();
            frm.ShowDialog();
        }

        private void currentUserInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUserInfo frm = new frmUserInfo(LogedUser.UserID);
            frm.ShowDialog();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmChangePassword frm = new frmChangePassword(LogedUser.UserID);
            frm.ShowDialog();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void manageApplicationTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListApplicationTypes frm = new frmListApplicationTypes();
            frm.ShowDialog();
        }

        private void manageTestTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListTestTypes frm = new frmListTestTypes();
            frm.ShowDialog();
        }

        private void manageLocalDrivingLicenseApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListLocalDrivingLicesnseApplications frm = new frmListLocalDrivingLicesnseApplications();
            frm.ShowDialog();
        }

        private void localLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateLocalDrivingLicesnseApplication frm = new frmAddUpdateLocalDrivingLicesnseApplication();
            frm.ShowDialog();
        }
    }
}
