using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_Buisness;

namespace DVLD_Project.People
{
    public partial class frmListPeople : Form
    {
        public frmListPeople()
        {
            InitializeComponent();
        }

        private static DataTable _dtAllPeople = clsPerson.GetAllPeople();

        private DataTable _dtPeople = _dtAllPeople.DefaultView.ToTable(false, "PersonID", "NationalNo",
                                                       "FirstName", "SecondName", "ThirdName", "LastName",
                                                       "GenderCaption", "DateOfBirth", "CountryName",
                                                       "Phone", "Email");
        private void frmListPeople_Load(object sender, EventArgs e)
        {
            cbFilterBy.SelectedIndex = 0;
            dgvPeople.DataSource = _dtPeople;
            lblRecordsCount.Text = RowsCounter().ToString();

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterType = cbFilterBy.SelectedItem.ToString();
            DataView dv = _dtPeople.DefaultView;

            if (string.IsNullOrEmpty(txtFilterValue.Text) || FilterType == "None")
            {
                dv.RowFilter = string.Empty;
                lblRecordsCount.Text = RowsCounter().ToString();
                return;
            }

            switch (FilterType)
            {
                case "Person ID":
                    if (int.TryParse(txtFilterValue.Text, out int Id))
                        dv.RowFilter = $"PersonID = {Id}";
                    break;
                case "National No":
                    dv.RowFilter = $"[NationalNo] LIKE '{txtFilterValue.Text}%'";
                    break;
                case "First Name":
                    dv.RowFilter = $"[FirstName] LIKE '{txtFilterValue.Text}%'";
                    break;
                case "Second Name":
                    dv.RowFilter = $"[SecondName] LIKE '{txtFilterValue.Text}%'";
                    break;
                case "Third Name":
                    dv.RowFilter = $"[ThirdName] LIKE '{txtFilterValue.Text}%'";
                    break;
                case "Last Name":
                    dv.RowFilter = $"[LastName] LIKE '{txtFilterValue.Text}%'";
                    break;
                case "Nationality":
                    dv.RowFilter = $"[CountryName] LIKE '{txtFilterValue.Text}%'";
                    break;
                case "Gender":
                    dv.RowFilter = $"[GendorCaption] LIKE '{txtFilterValue.Text}%'";
                    break;
                case "Phone":
                    dv.RowFilter = $"[Phone] LIKE '{txtFilterValue.Text}%'";
                    break;
                case "Email":
                    dv.RowFilter = $"[Email] LIKE '{txtFilterValue.Text}%'";
                    break;
            }
            lblRecordsCount.Text = RowsCounter().ToString();

        }
        private int RowsCounter()
        {
            return dgvPeople.Rows.Count;
        }
        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.SelectedItem.ToString() == "Person ID")
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }


        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddUpdatePerson();
            frm.ShowDialog();
            _Refresh_dgvPeople();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ID = (int)dgvPeople.CurrentRow.Cells[0].Value;
            clsPerson person = clsPerson.Find(ID);
            if (person != null)
            {
                Form frm = new frmShowPersonInfo(ID);

                frm.ShowDialog();
                
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
                Form frm = new frmAddUpdatePerson((int)dgvPeople.CurrentRow.Cells[0].Value);

                frm.ShowDialog();
                _Refresh_dgvPeople();
            
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddUpdatePerson();
            frm.ShowDialog();
            _Refresh_dgvPeople();
        }
        void _Refresh_dgvPeople()
        {
            _dtAllPeople = clsPerson.GetAllPeople();
            _dtPeople = _dtAllPeople.DefaultView.ToTable(false, "PersonID", "NationalNo",
                                                       "FirstName", "SecondName", "ThirdName", "LastName",
                                                       "GenderCaption", "DateOfBirth", "CountryName",
                                                       "Phone", "Email");

            dgvPeople.DataSource = _dtPeople;
            lblRecordsCount.Text = dgvPeople.Rows.Count.ToString();
        }
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
    "Are you sure you want to delete this person?",
    "Confirm Delete",
    MessageBoxButtons.YesNo,
    MessageBoxIcon.Warning);



            if (result == DialogResult.Yes)
            {
                Control source = cmsPeople.SourceControl;
                int ID = -1;
                if (source is DataGridView dgv)
                {
                    // Get the currently selected row (or cell)
                    if (dgv.CurrentRow != null)
                    {
                        var row = dgv.CurrentRow;
                        ID = int.Parse(row.Cells["PersonID"].Value.ToString());

                    }
                }

                if (clsPerson.DeletePerson(ID))
                {
                    MessageBox.Show("Person deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _Refresh_dgvPeople();
                }
                else
                {
                    MessageBox.Show("Person Failed To delete.", "Delete Failed", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
        }
    }
}
