using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUD
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (var splash = new SplashScreen())
            {
                splash.ShowDialog();
            }


            using (var login = new LoginForm())
            {
                if (login.ShowDialog() == DialogResult.OK)
                {
                    Application.Run(new Form1());
                }
            }

        }
    }
}
