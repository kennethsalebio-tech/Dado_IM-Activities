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
    public partial class UsernameManagementForm : Form
    {
        public UsernameManagementForm()
        {
            InitializeComponent();
        }
        private void LoadUsers()
        {
            var dt = DB.GetDataTable("SELECT id,username,role,created_at FROM users ORDER BY created_at DESC");
            dgv.DataSource = dt; if (dgv.Columns.Contains("id")) dgv.Columns["id"].Visible = false;
        }

        private int? GetSelectedId()
        {
            if (dgv.SelectedRows.Count == 0) return null;
            var row = dgv.SelectedRows[0]; if (row.Cells["id"].Value == null) return null; return Convert.ToInt32(row.Cells["id"].Value);
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            using (var f = new UserEditForm()) if (f.ShowDialog() == DialogResult.OK) LoadUsers();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            var id = GetSelectedId(); if (!id.HasValue) { MessageBox.Show("Select a user."); return; }
            using (var f = new UserEditForm(id.Value)) if (f.ShowDialog() == DialogResult.OK) LoadUsers();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            var id = GetSelectedId(); if (!id.HasValue) { MessageBox.Show("Select a user."); return; }
            var confirm = MessageBox.Show("Delete user?", "Confirm", MessageBoxButtons.YesNo); if (confirm != DialogResult.Yes) return;
            DB.ExecuteNonQuery("DELETE FROM users WHERE id=@id", new MySqlParameter("@id", id.Value)); LoadUsers();
        }
    }
}
