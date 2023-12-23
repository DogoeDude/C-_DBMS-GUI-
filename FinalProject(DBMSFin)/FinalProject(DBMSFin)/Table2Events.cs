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

        private void button2_Click(object sender, EventArgs e)//save button
        {
            // Implement save functionality here
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
    }
}
