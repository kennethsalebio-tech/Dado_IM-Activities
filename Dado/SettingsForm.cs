using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUD
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }
        private void LoadConn()
        {
            try
            {
                var p = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dbconn.txt");
                if (File.Exists(p)) txtConn.Text = File.ReadAllText(p);
            }
            catch { }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            var cs = txtConn.Text.Trim();
            if (string.IsNullOrEmpty(cs)) { MessageBox.Show("Enter connection string."); return; }
            if (DB.TestConnection(cs, out string msg)) MessageBox.Show("Connection OK: " + msg);
            else MessageBox.Show("Connection failed: " + msg);
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            var cs = txtConn.Text.Trim();
            if (string.IsNullOrEmpty(cs)) { MessageBox.Show("Enter connection string."); return; }
            DB.SaveConnectionString(cs);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
