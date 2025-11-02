using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUD
{
    public partial class Form1 : Form
    {
      
        public Form1()
        {
            this.Text = "Student Record Management System";
            this.Width = 1000;
            this.Height = 650;
            this.StartPosition = FormStartPosition.CenterScreen;

            InitializeComponent();
            LoadStudents();
        }
            
        private void LoadStudents(string search = "")
        {
            string sql = "SELECT id, first_name, last_name, age, course, year, created_at FROM students";
            MySqlParameter[] pars = null;
            if (!string.IsNullOrEmpty(search))
            {
                sql += " WHERE CONCAT(first_name,' ',last_name) LIKE @s OR course LIKE @s";
                pars = new MySqlParameter[] { new MySqlParameter("@s", "%" + search + "%") };
            }
            sql += " ORDER BY created_at DESC";


            var dt = DB.GetDataTable(sql, pars);
            dgv.DataSource = dt;
            if (dgv.Columns.Contains("id")) dgv.Columns["id"].Visible = false;
            lblStatus.Text = $"Showing {dt.Rows.Count} record(s)";
        }
        private int? GetSelectedId()
        {
            if (dgv.SelectedRows.Count == 0) return null;
            var row = dgv.SelectedRows[0];
            if (row.Cells["id"].Value == null) return null;
            return Convert.ToInt32(row.Cells["id"].Value);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var f = new AddEditForm())
            {
                if (f.ShowDialog() == DialogResult.OK) LoadStudents();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            var id = GetSelectedId();
            if (!id.HasValue)
            {
                MessageBox.Show("Select a student to edit.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            using (var f = new AddEditForm(id)) if (f.ShowDialog() == DialogResult.OK) LoadStudents();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var id = GetSelectedId();
            if (!id.HasValue) { MessageBox.Show("Select a student to delete.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
            var confirm = MessageBox.Show("Are you sure you want to delete the selected student?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;
            var rows = DB.ExecuteNonQuery("DELETE FROM students WHERE id = @id", new MySqlParameter("@id", id.Value));
            if (rows > 0) { MessageBox.Show("Student deleted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information); LoadStudents(); }
            else MessageBox.Show("Delete failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        
        }

        private void btnDetails_Click(object sender, EventArgs e)
        {
            var id = GetSelectedId();
            if (!id.HasValue) { MessageBox.Show("Select a student.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
            using (var f = new AddEditForm(id.Value))
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    LoadStudents(txtSearch.Text.Trim());
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                btnRefresh.Enabled = false;
                Cursor = Cursors.WaitCursor;
                
                LoadStudents(txtSearch.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error refreshing data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
                btnRefresh.Enabled = true;
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            using (var f = new SettingsForm()) if (f.ShowDialog() == DialogResult.OK) MessageBox.Show("Settings saved.");
        }

        private void btnImportExport_Click(object sender, EventArgs e)
        {
            using (var f = new ImportExportForm()) f.ShowDialog();
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            using (var f = new ReportForm()) f.ShowDialog();
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            using (var f = new UsernameManagementForm()) f.ShowDialog();
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            using (var f = new AboutForm()) f.ShowDialog();
        }
    }
}
