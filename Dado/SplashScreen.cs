using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;



namespace CRUD
{
    public partial class SplashScreen : Form
    {
        private Timer t = new Timer();
        public SplashScreen()
        {
            InitializeComponent();
            this.Controls.Add(guna2HtmlLabel1); this.Controls.Add(guna2HtmlLabel2);

            t.Interval = 900; // show for ~0.9s
            t.Tick += (s, e) => { t.Stop(); this.Close(); };
            this.Shown += (s, e) => t.Start();

        }



    }
}
