using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient; // Add this for MySQL support

namespace FinalProject_DBMSFin_
{
    public partial class Table2Events : Form
    {
        private const string ConnectionString = "Server=localhost;Database=churchsched;Uid=root;Pwd=password;";

        public Table2Events()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        private void Table2Events_Load(object sender, EventArgs e)
        {
            LoadDataFromDatabase();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            MainPage mainPageForm = new MainPage();
            mainPageForm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveChangesToDatabase();
        }

        private void SaveChangesToDatabase()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Assuming "schedsdb" is the name of your table
                    string selectQuery = "SELECT * FROM schedsdb";
                    string updateQuery = "UPDATE schedsdb SET PersonSchedID = @PersonSchedID, ChurchSchedID = @ChurchSchedID, EventDate = @EventDate, ChurchEvent = @ChurchEvent, SchedTime = @SchedTime, DN = @DN WHERE SchedID = @SchedID";

                    using (MySqlCommand selectCommand = new MySqlCommand(selectQuery, connection))
                    using (MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection))
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(selectCommand))
                    using (MySqlCommandBuilder commandBuilder = new MySqlCommandBuilder(adapter))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Assuming dataGridView1 is the name of your DataGridView
                        dataGridView1.EndEdit();
                        adapter.Update(dataTable);
                    }
                }

                MessageBox.Show("Changes saved successfully.");
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
            int selectedIndex = dataGridView1.CurrentCell.RowIndex;

            // Get the value of the SchedID column for the selected row
            int schedIDToDelete = Convert.ToInt32(dataGridView1.Rows[selectedIndex].Cells["SchedID"].Value);

            // Call the method to delete the row
            DeleteRowFromDatabase(schedIDToDelete);
        }

        private void DeleteRowFromDatabase(int schedID)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();

                    string deleteQuery = "DELETE FROM schedsdb WHERE SchedID = @SchedID";

                    using (MySqlCommand deleteCommand = new MySqlCommand(deleteQuery, connection))
                    {
                        deleteCommand.Parameters.AddWithValue("@SchedID", schedID);

                        int rowsAffected = deleteCommand.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Row deleted successfully.");
                            LoadDataFromDatabase(); // Refresh the data in the DataGridView
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

        private void LoadDataFromDatabase()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT schedsdb.SchedID, " +
                                           "personal_infodb.Firstname, " +
                                           "personal_infodb.Lastname, " +
                                           "churchinfodb.ChurchName, " +
                                           "schedsdb.EventDate, " +
                                           "schedsdb.ChurchEvent, " +
                                           "schedsdb.SchedTime, " +
                                           "schedsdb.DN " +
                                   "FROM schedsdb " +
                                   "INNER JOIN personal_infodb ON schedsdb.PersonSchedID = personal_infodb.PersonID " +
                                   "INNER JOIN churchinfodb ON schedsdb.ChurchSchedID = churchinfodb.ChurchID";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            // Assuming dataGridView1 is the name of your DataGridView
                            dataGridView1.DataSource = dataTable;
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
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                LoadDataFromDatabase();
                MessageBox.Show("Data refreshed successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error refreshing data: " + ex.ToString());
            }
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

                        string searchQuery = "SELECT schedsdb.SchedID, " +
                                             "personal_infodb.Firstname, " +
                                             "personal_infodb.Lastname, " +
                                             "churchinfodb.ChurchName, " +
                                             "schedsdb.EventDate, " +
                                             "schedsdb.ChurchEvent, " +
                                             "schedsdb.SchedTime, " +
                                             "schedsdb.DN " +
                                             "FROM schedsdb " +
                                             "INNER JOIN personal_infodb ON schedsdb.PersonSchedID = personal_infodb.PersonID " +
                                             "INNER JOIN churchinfodb ON schedsdb.ChurchSchedID = churchinfodb.ChurchID " +
                                             $"WHERE personal_infodb.Firstname LIKE @SearchText OR personal_infodb.Lastname LIKE @SearchText";

                        using (MySqlCommand searchCommand = new MySqlCommand(searchQuery, connection))
                        {
                            searchCommand.Parameters.AddWithValue("@SearchText", $"%{searchText}%");

                            using (MySqlDataAdapter adapter = new MySqlDataAdapter(searchCommand))
                            {
                                DataTable dataTable = new DataTable();
                                adapter.Fill(dataTable);

                                // Assuming dataGridView1 is the name of your DataGridView
                                dataGridView1.DataSource = dataTable;
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


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
