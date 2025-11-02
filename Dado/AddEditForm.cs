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
    public partial class AddEditForm : Form
    {
        private int? editingId;
        public AddEditForm(int? studentId = null)
        {
            editingId = studentId;
            InitializeComponent();
            if (editingId.HasValue) LoadStudent(editingId.Value);
        }
        private void LoadStudent(int id)
        {
            try
            {
                var dt = DB.GetDataTable(
                    "SELECT first_name, last_name, age, course, year, created_at FROM students WHERE id = @id",
                    new MySqlParameter("@id", id)
                );

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Student not found.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                    return;
                }

                var r = dt.Rows[0];
                txtFirstName.Text = r["first_name"]?.ToString() ?? "";
                txtLastName.Text = r["last_name"]?.ToString() ?? "";
                txtAge.Text = r["age"] != DBNull.Value ? r["age"].ToString() : "";
                txtCourse.Text = r["course"]?.ToString() ?? "";
                txtYear.Text = r["year"] != DBNull.Value ? r["year"].ToString() : "";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading student: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string fn = txtFirstName.Text.Trim();
            string ln = txtLastName.Text.Trim();
            string course = txtCourse.Text.Trim();

            if (string.IsNullOrEmpty(fn) || string.IsNullOrEmpty(ln))
            {
                MessageBox.Show("First and last name are required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtAge.Text.Trim(), out int age)) age = 0;
            if (!int.TryParse(txtYear.Text.Trim(), out int year)) year = 0;

            try
            {
                if (editingId.HasValue)
                {
                    int rows = DB.ExecuteNonQuery(
                        "UPDATE students SET first_name=@fn, last_name=@ln, age=@age, course=@course, year=@year WHERE id=@id",
                        new MySqlParameter("@fn", fn),
                        new MySqlParameter("@ln", ln),
                        new MySqlParameter("@age", age),
                        new MySqlParameter("@course", course),
                        new MySqlParameter("@year", year),
                        new MySqlParameter("@id", editingId.Value)
                    );

                    if (rows > 0)
                    {
                        MessageBox.Show("Student updated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("No changes were made.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    DB.ExecuteNonQuery(
                        "INSERT INTO students (first_name, last_name, age, course, year) VALUES (@fn,@ln,@age,@course,@year)",
                        new MySqlParameter("@fn", fn),
                        new MySqlParameter("@ln", ln),
                        new MySqlParameter("@age", age),
                        new MySqlParameter("@course", course),
                        new MySqlParameter("@year", year)
                    );

                    MessageBox.Show("Student added.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving student: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to cancel?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                this.Close();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (!editingId.HasValue) return;
            var confirm = MessageBox.Show("Are you sure you want to DELETE this student? This action cannot be undone.", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            try
            {
                int rows = DB.ExecuteNonQuery("DELETE FROM students WHERE id=@id", new MySqlParameter("@id", editingId.Value));
                if (rows > 0)
                {
                    MessageBox.Show("Student deleted.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Delete failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting student: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
