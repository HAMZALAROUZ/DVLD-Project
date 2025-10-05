using DVLD_Buisness;
using DVLD_Project.Global_Classes;
using DVLD_Project.People;
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
    public partial class ctrlPersonCardWithFilter : UserControl
    {
        //Event
        public event Action<int> OnPersonSelected;
        //Method Raise Event
        protected virtual void PersonSelected(int PersonID)
        {
            Action<int> Handler = OnPersonSelected;

            if(Handler != null)
            {
                Handler(PersonID);
            }
        }

        private bool _ShowAddPerson = true;
        public bool ShowAddPerson
        {
            get
            {
                return _ShowAddPerson;
            }
            set
            {
                _ShowAddPerson= value;
                btnAddNewPerson.Visible = _ShowAddPerson;
            }
        }

        private bool _FilterEnable = true;
        public bool FilterEnable
        {
            get
            {
                return _FilterEnable;
            }
            set
            {
                _FilterEnable= value;
                gbFilters.Enabled = _FilterEnable;
            }
        }
        public ctrlPersonCardWithFilter()
        {
            InitializeComponent();
            
        }
        private int _PersonID = -1;

        public int PersonID
        {
            get
            {
                return ctrlPersonCard1.PersonID;
            }
        }

        public clsPerson SelectedPersonInfo
        {
            get { return ctrlPersonCard1.SelectedPersonInfo; }
        }
        public void LoadPersonInfo(int  PersonID)
        {
            cbFilterBy.SelectedIndex = 1;
            txtFilterValue.Text = PersonID.ToString();
            FindNow();
        }
        public void FindNow()
        {
            switch(cbFilterBy.SelectedItem)
            {
                case "Person ID":
                    ctrlPersonCard1.LoadData(int.Parse(txtFilterValue.Text));
                    break;
                case "National No.":
                    ctrlPersonCard1.LoadData(txtFilterValue.Text);
                    break;
                default:
                    break;
            }

            if(OnPersonSelected != null && FilterEnable)
            {
                PersonSelected(ctrlPersonCard1.PersonID);
            }
        }
        private void ctrlPersonCardWithFilter_Load(object sender, EventArgs e)
        {
            cbFilterBy.SelectedIndex = 0;
        }

        private void txtFilterValue_Validating(object sender, CancelEventArgs e)
        {
            
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFilterValue.Text))
                return;

            string Value = txtFilterValue.Text;

           FindNow();

        }
        void _DataBackEvent(object sender, clsPerson person)
        {
            cbFilterBy.SelectedIndex = 1;
            txtFilterValue.Text = person.PersonID.ToString();
            ctrlPersonCard1.LoadData(person.PersonID);
        }
        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson();
            frm.DataBack += _DataBackEvent;
            frm.ShowDialog();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar ==(char)13)
            {
                btnFind.PerformClick();
            }
            if(cbFilterBy.SelectedIndex == 1) 
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
