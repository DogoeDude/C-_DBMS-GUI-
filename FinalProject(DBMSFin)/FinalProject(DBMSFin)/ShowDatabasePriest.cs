using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace FinalProject_DBMSFin_
{
    public partial class ShowDatabasePriest : Form
    {
        private MySqlConnection con = new MySqlConnection();

        public ShowDatabasePriest()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            // Initialize the connection string
            string connstring = "server=localhost;uid=root;pwd=password;database=churchsched";
            con.ConnectionString = connstring;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        private void ShowDatabasePriest_Load(object sender, EventArgs e)
        {
            // Call the button1_Click method to execute the query and populate the DataGridView
            button1_Click(sender, e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Call a method to load data into the DataGridView and sort by Firstname
            LoadDataAndSort("Firstname");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Call a method to load data into the DataGridView and sort by EventDate and SchedTime
            LoadDataAndSort("EventDate, SchedTime");
        }

        private void LoadDataAndSort(string orderBy)
        {
            try
            {
                // Open the database connection
                con.Open();

                // Select data from both tables using an INNER JOIN operation and apply sorting
                string sql = $"SELECT personal_infodb.PersonID, personal_infodb.Firstname, personal_infodb.Lastname, " +
                             $"personal_infodb.ContactNum, schedsdb.EventDate, schedsdb.ChurchEvent, " +
                             $"schedsdb.SchedTime, " +
                             $"CASE WHEN schedsdb.DN = 'AM' THEN 'AM' ELSE 'PM' END AS DN " +
                             $"FROM personal_infodb " +
                             $"INNER JOIN schedsdb ON personal_infodb.PersonID = schedsdb.PersonSchedID " +
                             $"ORDER BY {orderBy}, CASE WHEN schedsdb.DN = 'AM' THEN 0 ELSE 1 END, schedsdb.SchedTime";

                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, con);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                // Display data in a DataGridView or another control
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


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Handle cell content click event if needed
        }
    }
}
