using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace FinalProject_DBMSFin_
{
    public partial class Table1Person : Form
    {
        public Table1Person()
        {
            InitializeComponent();
            LoadDataIntoDataGridView();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        private void LoadDataIntoDataGridView()
        {
            try
            {
                string connstring = "server=localhost;uid=root;pwd=password;database=churchsched";
                using (MySqlConnection con = new MySqlConnection(connstring))
                {
                    con.Open();
                    string sql = "SELECT PersonID, FirstName, LastName, ContactNum, Age, Barangay, City_Municipality, Province FROM personal_infodb";
                    MySqlCommand cmd = new MySqlCommand(sql, con);

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Assuming dataGridView2 is the name of your DataGridView
                        dataGridView2.DataSource = dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString());
            }
        }

        private void Table1Person_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable changes = ((DataTable)dataGridView2.DataSource).GetChanges();
                if (changes != null)
                {
                    UpdateChangesInDatabase(changes);
                    ((DataTable)dataGridView2.DataSource).AcceptChanges();

                    MessageBox.Show("Changes saved successfully!");
                }
                else
                {
                    MessageBox.Show("No changes to save.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving changes: " + ex.ToString());
            }
        }
        private void UpdateChangesInDatabase(DataTable dataTable)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection("server=localhost;uid=root;pwd=password;database=churchsched"))
                {
                    con.Open();

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM personal_infodb", con))
                    {
                        using (MySqlCommandBuilder builder = new MySqlCommandBuilder(adapter))
                        {
                            adapter.Update(dataTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating changes to the database: " + ex.ToString());
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            MainPage mainPageForm = new MainPage();
            mainPageForm.Show();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
