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

namespace DVLD.Applications.Application_Types
{
    public partial class frmListApplicationTypes : Form
    {
        private DataTable _dtAllApplicationTypes;

        public frmListApplicationTypes()
        {
            InitializeComponent();
        }
        private void DesignDataGridView()
        {
            // خلفية عامة
            dgvApplicationTypes.BackgroundColor = Color.White;
            dgvApplicationTypes.BorderStyle = BorderStyle.None;

            // ألوان الهيدر
            dgvApplicationTypes.ColumnHeadersDefaultCellStyle.BackColor = Color.DodgerBlue;
            dgvApplicationTypes.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvApplicationTypes.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvApplicationTypes.ColumnHeadersHeight = 40;
            dgvApplicationTypes.EnableHeadersVisualStyles = false;

            // ألوان الصفوف
            dgvApplicationTypes.DefaultCellStyle.BackColor = Color.White;
            dgvApplicationTypes.DefaultCellStyle.ForeColor = Color.Black;
            dgvApplicationTypes.DefaultCellStyle.SelectionBackColor = Color.LightSkyBlue;
            dgvApplicationTypes.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvApplicationTypes.DefaultCellStyle.Font = new Font("Segoe UI", 10);

            // ألوان الصفوف المتناوبة
            dgvApplicationTypes.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 248, 255);

            // الحدود
            dgvApplicationTypes.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvApplicationTypes.GridColor = Color.LightGray;

            // حجم الصفوف
            dgvApplicationTypes.RowTemplate.Height = 35;
            dgvApplicationTypes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // منع إضافة صفوف يدوياً
            dgvApplicationTypes.AllowUserToAddRows = false;
        }
        private void frmListApplicationTypes_Load(object sender, EventArgs e)
        {
            DesignDataGridView();
            _dtAllApplicationTypes = clsApplicationType.GetAllApplicationTypes();
            dgvApplicationTypes.DataSource = _dtAllApplicationTypes;
            //lblRecordsCount.Text = dgvApplicationTypes.Rows.Count.ToString();

            dgvApplicationTypes.Columns[0].HeaderText = "ID";
            dgvApplicationTypes.Columns[0].Width = 110;

            dgvApplicationTypes.Columns[1].HeaderText = "Title";
            dgvApplicationTypes.Columns[1].Width = 400;

            dgvApplicationTypes.Columns[2].HeaderText = "Fees";
            dgvApplicationTypes.Columns[2].Width = 100;
        }

        private void dgvApplicationTypes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvApplicationTypes.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 120, 215);
            dgvApplicationTypes.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvApplicationTypes.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            dgvApplicationTypes.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 248, 255);
            dgvApplicationTypes.DefaultCellStyle.SelectionBackColor = Color.DodgerBlue;
            dgvApplicationTypes.DefaultCellStyle.SelectionForeColor = Color.White;

            dgvApplicationTypes.BorderStyle = BorderStyle.None;
            dgvApplicationTypes.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvApplicationTypes.EnableHeadersVisualStyles = false;
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmEditApplicationType((int)dgvApplicationTypes.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            frmListApplicationTypes_Load(null, null);
        }
    }
}
