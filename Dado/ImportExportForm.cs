using MySql.Data.MySqlClient;
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
    public partial class ImportExportForm : Form
    {
        public ImportExportForm()
        {
            InitializeComponent();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            var dt = DB.GetDataTable("SELECT first_name,last_name,age,course,year,created_at FROM students ORDER BY created_at DESC");
            using (var sfd = new SaveFileDialog { Filter = "CSV|*.csv", FileName = "students_export.csv" })
            {
                if (sfd.ShowDialog() != DialogResult.OK) return;
                using (var sw = new StreamWriter(sfd.FileName))
                {
                    
                    sw.WriteLine("FirstName,LastName,Age,Course,Year,CreatedAt");
                    foreach (DataRow r in dt.Rows)
                    {
                        sw.WriteLine($"\"{r[0]}\",\"{r[1]}\",{r[2]},\"{r[3]}\",{r[4]},\"{r[5]}\"");
                    }
                }
                MessageBox.Show("Export complete.");
            }

        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog { Filter = "CSV|*.csv" })
            {
                if (ofd.ShowDialog() != DialogResult.OK) return;
                var lines = File.ReadAllLines(ofd.FileName);
                if (lines.Length <= 1) { MessageBox.Show("No data found."); return; }
                int inserted = 0;
                for (int i = 1; i < lines.Length; i++)
                {
                    var cols = SplitCsvLine(lines[i]);
                    if (cols.Length < 5) continue;
                    string sql = "INSERT INTO students (first_name,last_name,age,course,year) VALUES (@fn,@ln,@age,@course,@year)";
                    try
                    {
                        DB.ExecuteNonQuery(sql,
                            new MySqlParameter("@fn", cols[0]),
                            new MySqlParameter("@ln", cols[1]),
                            new MySqlParameter("@age", int.TryParse(cols[2], out int a) ? a : 0),
                            new MySqlParameter("@course", cols[3]),
                            new MySqlParameter("@year", int.TryParse(cols[4], out int y) ? y : 0)
                        );
                        inserted++;
                    }
                    catch { }
                }
                MessageBox.Show($"Imported {inserted} rows.");
            }
        }
        private string[] SplitCsvLine(string line)
        {
            var parts = new System.Collections.Generic.List<string>();
            bool inQuotes = false; var cur = "";
            for (int i = 0; i < line.Length; i++)
            {
                var c = line[i];
                if (c == '"') { inQuotes = !inQuotes; continue; }
                if (c == ',' && !inQuotes) { parts.Add(cur); cur = ""; continue; }
                cur += c;
            }
            parts.Add(cur);
            return parts.ToArray();
        }
    }
}
