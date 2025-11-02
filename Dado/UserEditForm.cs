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
    public partial class UserEditForm : Form
    {
        private int? editingId;
        public UserEditForm(int? id = null)
        {
            if (editingId.HasValue) LoadUser(editingId.Value);
        }
        private void LoadUser(int id)
        {
            var dt = DB.GetDataTable("SELECT * FROM users WHERE id=@id", new MySqlParameter("@id", id)); if (dt.Rows.Count == 0) return; var r = dt.Rows[0]; txtUser.Text = r["username"].ToString(); cboRole.SelectedItem = r["role"].ToString();
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUser.Text) || string.IsNullOrWhiteSpace(txtPass.Text)) { MessageBox.Show("Username and password required."); return; }
            if (editingId.HasValue)
            {
                DB.ExecuteNonQuery("UPDATE users SET username=@u,password=@p,role=@r WHERE id=@id",
                    new MySqlParameter("@u", txtUser.Text.Trim()), new MySqlParameter("@p", txtPass.Text.Trim()), new MySqlParameter("@r", cboRole.SelectedItem.ToString()), new MySqlParameter("@id", editingId.Value));
            }
            else
            {
                DB.ExecuteNonQuery("INSERT INTO users (username,password,role) VALUES (@u,@p,@r)", new MySqlParameter("@u", txtUser.Text.Trim()), new MySqlParameter("@p", txtPass.Text.Trim()), new MySqlParameter("@r", cboRole.SelectedItem.ToString()));
            }
            this.DialogResult = DialogResult.OK; this.Close();
        }
    }
}
