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

namespace DVLD_Project.User
{
    public partial class frmUserInfo : Form
    {

        int _UserID = -1;
        public frmUserInfo(int UserID)
        {
            InitializeComponent();
            _UserID = UserID;
        }

        private void ctrlUserCard1_Load(object sender, EventArgs e)
        {
            if (_UserID == -1)
            {
                MessageBox.Show("No User With This ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }

            ctrlUserCard1.LoadUserInfo(_UserID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
