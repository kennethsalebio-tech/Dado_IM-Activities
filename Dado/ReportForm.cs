using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUD
{
    public partial class ReportForm : Form
    {
        private DataTable dt;
        private PrintDocument pd = new PrintDocument();
        public ReportForm()
        {
            InitializeComponent();
            dt = DB.GetDataTable("SELECT first_name,last_name,age,course,year,created_at FROM students ORDER BY created_at DESC");
            pd.PrintPage += Pd_PrintPage;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            var pp = new PrintPreviewDialog { Document = pd, Width = 800, Height = 600 };
            pp.ShowDialog();
        }
        private void Pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            int y = 20;
            e.Graphics.DrawString("Students Report", new System.Drawing.Font("Arial", 14), System.Drawing.Brushes.Black, 20, y); y += 30;
            foreach (DataRow r in dt.Rows)
            {
                string line = $"{r[0]} {r[1]} | Age: {r[2]} | {r[3]} | Year {r[4]} | {r[5]}";
                e.Graphics.DrawString(line, new System.Drawing.Font("Arial", 10), System.Drawing.Brushes.Black, 20, y); y += 20;
                if (y > e.MarginBounds.Bottom - 40) { e.HasMorePages = true; return; }
            }
            e.HasMorePages = false;
        }
    }
}
