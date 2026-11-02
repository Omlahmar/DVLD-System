using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Licenses.Controls
{
    public partial class ctrlDriverLicenses : UserControl
    {
        private int _DriverID;
        private clsDriver _Driver;
        private DataTable _dtDriverLocalLicensesHistory;
        private DataTable _dtDriverInternationalLicensesHistory;
        public ctrlDriverLicenses()
        {
            InitializeComponent();
        }

        private void DesignDataGridViewInternationalLicense()
        {
            // خلفية عامة
            dgvInternationalLicensesHistory.BackgroundColor = Color.White;
            dgvInternationalLicensesHistory.BorderStyle = BorderStyle.None;

            // ألوان الهيدر
            dgvInternationalLicensesHistory.ColumnHeadersDefaultCellStyle.BackColor = Color.DodgerBlue;
            dgvInternationalLicensesHistory.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvInternationalLicensesHistory.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvInternationalLicensesHistory.ColumnHeadersHeight = 40;
            dgvInternationalLicensesHistory.EnableHeadersVisualStyles = false;

            // ألوان الصفوف
            dgvInternationalLicensesHistory.DefaultCellStyle.BackColor = Color.White;
            dgvInternationalLicensesHistory.DefaultCellStyle.ForeColor = Color.Black;
            dgvInternationalLicensesHistory.DefaultCellStyle.SelectionBackColor = Color.LightSkyBlue;
            dgvInternationalLicensesHistory.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvInternationalLicensesHistory.DefaultCellStyle.Font = new Font("Segoe UI", 10);

            // ألوان الصفوف المتناوبة
            dgvInternationalLicensesHistory.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 248, 255);

            // الحدود
            dgvInternationalLicensesHistory.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvInternationalLicensesHistory.GridColor = Color.LightGray;

            // حجم الصفوف
            dgvInternationalLicensesHistory.RowTemplate.Height = 35;
            dgvInternationalLicensesHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // منع إضافة صفوف يدوياً
            dgvInternationalLicensesHistory.AllowUserToAddRows = false;
        }
        private void _LoadInternationalLicenseInfo()
        {
            DesignDataGridViewInternationalLicense();
            _dtDriverInternationalLicensesHistory = clsDriver.GetInternationalLicenses(_DriverID);


            dgvInternationalLicensesHistory.DataSource = _dtDriverInternationalLicensesHistory;
            //lblInternationalLicensesRecords.Text = dgvInternationalLicensesHistory.Rows.Count.ToString();

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

        //DONE
        private void DesignDataGridView()
        {
            // خلفية عامة
            dgvLocalLicensesHistory.BackgroundColor = Color.White;
            dgvLocalLicensesHistory.BorderStyle = BorderStyle.None;

            // ألوان الهيدر
            dgvLocalLicensesHistory.ColumnHeadersDefaultCellStyle.BackColor = Color.DodgerBlue;
            dgvLocalLicensesHistory.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvLocalLicensesHistory.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvLocalLicensesHistory.ColumnHeadersHeight = 40;
            dgvLocalLicensesHistory.EnableHeadersVisualStyles = false;

            // ألوان الصفوف
            dgvLocalLicensesHistory.DefaultCellStyle.BackColor = Color.White;
            dgvLocalLicensesHistory.DefaultCellStyle.ForeColor = Color.Black;
            dgvLocalLicensesHistory.DefaultCellStyle.SelectionBackColor = Color.LightSkyBlue;
            dgvLocalLicensesHistory.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvLocalLicensesHistory.DefaultCellStyle.Font = new Font("Segoe UI", 10);

            // ألوان الصفوف المتناوبة
            dgvLocalLicensesHistory.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 248, 255);

            // الحدود
            dgvLocalLicensesHistory.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvLocalLicensesHistory.GridColor = Color.LightGray;

            // حجم الصفوف
            dgvLocalLicensesHistory.RowTemplate.Height = 35;
            dgvLocalLicensesHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // منع إضافة صفوف يدوياً
            dgvLocalLicensesHistory.AllowUserToAddRows = false;
        }
        private void _LoadLocalLicenseInfo()
        {
            DesignDataGridView();
            _dtDriverLocalLicensesHistory = clsDriver.GetLicenses(_DriverID);


            dgvLocalLicensesHistory.DataSource = _dtDriverLocalLicensesHistory;
            //lblLocalLicensesRecords.Text = dgvLocalLicensesHistory.Rows.Count.ToString();

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
        private void ctrlDriverLicenses_Load(object sender, EventArgs e)
        {

        }
        public void LoadInfoByPersonID(int PersonID)
        {

            _Driver = clsDriver.FindByPersonID(PersonID);
            if (_Driver != null)
            {
                _DriverID = clsDriver.FindByPersonID(PersonID).DriverID;
            }

            _LoadLocalLicenseInfo();
            _LoadInternationalLicenseInfo();
        }
        public void Clear()
        {
           // _dtDriverLocalLicensesHistory.Clear();
            if (_dtDriverLocalLicensesHistory != null)
                _dtDriverLocalLicensesHistory.Clear();

            if (_dtDriverInternationalLicensesHistory != null)
                _dtDriverInternationalLicensesHistory.Clear();

            // لو عندك DataGridView
            dgvLocalLicensesHistory.DataSource = null;
            dgvInternationalLicensesHistory.DataSource = null;

        }
        public void LoadInfo(int DriverID)
        {
            _DriverID = DriverID;
            _Driver = clsDriver.FindByDriverID(_DriverID);

            _LoadLocalLicenseInfo();
            _LoadInternationalLicenseInfo();

        }
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void dgvLocalLicensesHistory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
