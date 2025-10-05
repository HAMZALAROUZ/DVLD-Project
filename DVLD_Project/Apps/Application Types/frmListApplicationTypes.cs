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

namespace DVLD_Project.Apps.Application_Types
{
    public partial class frmListApplicationTypes : Form
    {
        DataTable _dtAllAppTypes = new DataTable();
        public frmListApplicationTypes()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int AppTypeID = (int)dgvApplicationTypes.CurrentRow.Cells[0].Value;
            frmEditApplicationType frm = new frmEditApplicationType(AppTypeID);
            frm.ShowDialog();

            Refresh_DGVApplicationTypes();

        }
        void Refresh_DGVApplicationTypes()
        {
            _dtAllAppTypes = clsApplicationType.GetAllApplicationTypes();
            dgvApplicationTypes.DataSource = _dtAllAppTypes;

            lblRecordsCount.Text = dgvApplicationTypes.RowCount.ToString();
        }
        private void frmListApplicationTypes_Load(object sender, EventArgs e)
        {
            _dtAllAppTypes = clsApplicationType.GetAllApplicationTypes();
            dgvApplicationTypes.DataSource = _dtAllAppTypes;
            
            lblRecordsCount.Text = dgvApplicationTypes.RowCount.ToString();

            dgvApplicationTypes.Columns[0].HeaderText = "ID";
            dgvApplicationTypes.Columns[0].Width = 110;

            dgvApplicationTypes.Columns[1].HeaderText = "Title";
            dgvApplicationTypes.Columns[1].Width = 400;

            dgvApplicationTypes.Columns[2].HeaderText = "Fees";
            dgvApplicationTypes.Columns[2].Width = 100;
        }
    }
}
