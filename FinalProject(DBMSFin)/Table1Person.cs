using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace FinalProject_DBMSFin_
{
    public partial class Table1Person : Form
    {
        private string ConnectionString = "server=localhost;uid=root;pwd=password;database=churchsched";
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
            try
            {
                int selectedIndex = dataGridView2.CurrentCell.RowIndex;

                // Get the value of the PersonID column for the selected row
                int personIDToDelete = Convert.ToInt32(dataGridView2.Rows[selectedIndex].Cells["PersonID"].Value);

                // Call the method to delete the row
                DeleteRowFromDatabase(personIDToDelete);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString());
            }
        }

        private void DeleteRowFromDatabase(int personID)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();

                    string deleteQuery = "DELETE FROM personal_infodb WHERE PersonID = @PersonID";

                    using (MySqlCommand deleteCommand = new MySqlCommand(deleteQuery, connection))
                    {
                        deleteCommand.Parameters.AddWithValue("@PersonID", personID);

                        int rowsAffected = deleteCommand.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Row deleted successfully.");
                            LoadDataIntoDataGridView(); // Refresh the data in the DataGridView
                        }
                        else
                        {
                            MessageBox.Show("No rows were deleted.");
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySQL Error: {ex.Number} - {ex.Message}");
                MessageBox.Show($"MySQL Error: {ex.Number} - {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                MessageBox.Show($"Error: {ex.Message}");
            }
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

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                string searchText = textBox1.Text.Trim();

                if (!string.IsNullOrEmpty(searchText))
                {
                    using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                    {
                        connection.Open();

                        string searchQuery = "SELECT PersonID, FirstName, LastName, ContactNum, Age, Barangay, City_Municipality, Province " +
                                             "FROM personal_infodb " +
                                             "WHERE FirstName LIKE @SearchText OR LastName LIKE @SearchText";

                        using (MySqlCommand searchCommand = new MySqlCommand(searchQuery, connection))
                        {
                            searchCommand.Parameters.AddWithValue("@SearchText", $"%{searchText}%");

                            using (MySqlDataAdapter adapter = new MySqlDataAdapter(searchCommand))
                            {
                                DataTable dataTable = new DataTable();
                                adapter.Fill(dataTable);

                                // Assuming dataGridView2 is the name of your DataGridView
                                dataGridView2.DataSource = dataTable;
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a search term.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching data: " + ex.ToString());
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                LoadDataIntoDataGridView();
                MessageBox.Show("Data refreshed successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error refreshing data: " + ex.ToString());
            }
        }
    }
}
