using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data;

namespace FinalProject_DBMSFin_
{
    public partial class ShowTable : Form
    {

        private MySqlConnection con = new MySqlConnection();

        public ShowTable()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void ShowTable_Load(object sender, EventArgs e)
        {
            // Initialize the connection string
            string connstring = "server=localhost;uid=root;pwd=password;database=churchsched"; // Replace with your actual connection string
            con.ConnectionString = connstring;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ShowEvents();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ShowRegisteredPersons();
        }

        private void ShowEvents()
        {
            try
            {
                // Open the database connection
                con.Open();

                // Select all records from the 'schedsdb' table
                string sql = "SELECT * FROM schedsdb";
                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, con);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                // Display data in the DataGridView
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                // Log the error or handle it appropriately for your application
                MessageBox.Show("Error: " + ex.ToString());
            }
            finally
            {
                // Close the database connection
                con.Close();
            }
        }

        private void ShowRegisteredPersons()
        {
            try
            {
                // Open the database connection
                con.Open();

                // Select all records from the 'personal_infodb' table
                string sql = "SELECT * FROM personal_infodb";
                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, con);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                // Display data in the DataGridView
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                // Log the error or handle it appropriately for your application
                MessageBox.Show("Error: " + ex.ToString());
            }
            finally
            {
                // Close the database connection
                con.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ShowCombinedData();
        }

        private void ShowCombinedData()
        {
            try
            {
                // Open the database connection
                con.Open();

                // Select data from both tables using a JOIN operation
                string sql = "SELECT personal_infodb.Firstname, personal_infodb.Lastname, personal_infodb.ContactNum, " +
                             "schedsdb.EventDate, schedsdb.ChurchEvent, schedsdb.SchedTime " +
                             "FROM personal_infodb " +
                             "INNER JOIN schedsdb ON personal_infodb.PersonID = schedsdb.PersonSchedID";

                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, con);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                // Display data in the DataGridView
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                // Log the error or handle it appropriately for your application
                MessageBox.Show("Error: " + ex.ToString());
            }
            finally
            {
                // Close the database connection
                con.Close();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Hide(); // Hide the current form (ShowTable)
            MainPage mainPage = new MainPage(); // Create an instance of the main page
            mainPage.Show(); // Show the main page
        }
    }
}