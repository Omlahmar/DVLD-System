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

namespace DVLD.Tests
{
    public partial class frmListTestTypes : Form
    {
        private DataTable _dtAllTestTypes;

        public frmListTestTypes()
        {
            InitializeComponent();
        }
        private void DesignDataGridView()
        {
            // خلفية عامة
            dgvTestTypes.BackgroundColor = Color.White;
            dgvTestTypes.BorderStyle = BorderStyle.None;

            // ألوان الهيدر
            dgvTestTypes.ColumnHeadersDefaultCellStyle.BackColor = Color.DodgerBlue;
            dgvTestTypes.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvTestTypes.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvTestTypes.ColumnHeadersHeight = 40;
            dgvTestTypes.EnableHeadersVisualStyles = false;

            // ألوان الصفوف
            dgvTestTypes.DefaultCellStyle.BackColor = Color.White;
            dgvTestTypes.DefaultCellStyle.ForeColor = Color.Black;
            dgvTestTypes.DefaultCellStyle.SelectionBackColor = Color.LightSkyBlue;
            dgvTestTypes.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvTestTypes.DefaultCellStyle.Font = new Font("Segoe UI", 10);

            // ألوان الصفوف المتناوبة
            dgvTestTypes.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 248, 255);

            // الحدود
            dgvTestTypes.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvTestTypes.GridColor = Color.LightGray;

            // حجم الصفوف
            dgvTestTypes.RowTemplate.Height = 35;
            dgvTestTypes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // منع إضافة صفوف يدوياً
            dgvTestTypes.AllowUserToAddRows = false;
        }
        private void frmListTestTypes_Load(object sender, EventArgs e)
        {
            DesignDataGridView();
            _dtAllTestTypes = clsTestType.GetAllTestTypes();
            dgvTestTypes.DataSource = _dtAllTestTypes;
           // lblRecordsCount.Text = dgvTestTypes.Rows.Count.ToString();

            dgvTestTypes.Columns[0].HeaderText = "ID";
            dgvTestTypes.Columns[0].Width = 90;

            dgvTestTypes.Columns[1].HeaderText = "Title";
            dgvTestTypes.Columns[1].Width = 100;

            dgvTestTypes.Columns[2].HeaderText = "Description";
            dgvTestTypes.Columns[2].Width = 400;

            dgvTestTypes.Columns[3].HeaderText = "Fees";
            dgvTestTypes.Columns[3].Width = 100;
        }

        private void dgvTestTypes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvTestTypes.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 120, 215);
            dgvTestTypes.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvTestTypes.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            dgvTestTypes.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 248, 255);
            dgvTestTypes.DefaultCellStyle.SelectionBackColor = Color.DodgerBlue;
            dgvTestTypes.DefaultCellStyle.SelectionForeColor = Color.White;

            dgvTestTypes.BorderStyle = BorderStyle.None;
            dgvTestTypes.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvTestTypes.EnableHeadersVisualStyles = false;
        }

        private void EditCm_Click(object sender, EventArgs e)
        {
            Form frm = new frmEditTestType((clsTestType.enTestType)dgvTestTypes.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            frmListTestTypes_Load(null, null);
        }
    }
}
