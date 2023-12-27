using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace FinalProject_DBMSFin_
{
    public partial class UpdatingPage : Form
    {
        private MySqlConnection con = new MySqlConnection();
        private DataTable dataTable = new DataTable(); // DataTable to store the fetched data

        public UpdatingPage()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void UpdatingPage_Load(object sender, EventArgs e)
        {
            // Initialize the connection string
            string connstring = "server=localhost;uid=root;pwd=password;database=churchsched";
            con.ConnectionString = connstring;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Fetch data from personal_infodb and display in DataGridView
            DisplayData("SELECT * FROM personal_infodb WHERE Firstname LIKE @SearchTerm OR Lastname LIKE @SearchTerm");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Fetch data from schedsdb and display in DataGridView
            DisplayData("SELECT * FROM schedsdb WHERE ChurchEvent LIKE @SearchTerm OR EventDate LIKE @SearchTerm");
        }

        private void DisplayData(string query)
        {
            try
            {
                // Open the database connection
                con.Open();

                // Get the search term from textBox1
                string searchTerm = textBox1.Text;

                // Use parameterized query to prevent SQL injection
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");

                // Execute the query
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);

                // Explicitly define the DataTable schema based on your database table
                dataTable.Columns.Clear();
                dataTable.Columns.Add("PersonID", typeof(int));
                dataTable.Columns.Add("Firstname", typeof(string));
                dataTable.Columns.Add("Lastname", typeof(string));
                dataTable.Columns.Add("ContactNum", typeof(string));
                dataTable.Columns.Add("Age", typeof(int));
                dataTable.Columns.Add("Barangay", typeof(string));
                dataTable.Columns.Add("City_Municipality", typeof(string));
                dataTable.Columns.Add("Province", typeof(string));

                // Clear existing data
                dataTable.Clear();

                // Fill the DataTable with data from the database
                adapter.Fill(dataTable);

                // Display the results in a DataGridView
                dataGridView1.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                // Close the database connection
                con.Close();
            }
        }


        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Handle the cell edit event to update the database when a cell value is changed
            try
            {
                // Open the database connection
                con.Open();

                // Get the updated values from the DataGridView
                int rowIndex = e.RowIndex;
                DataGridViewRow row = dataGridView1.Rows[rowIndex];

                // Use parameterized query to prevent SQL injection
                string updateQuery = "UPDATE personal_infodb SET Firstname = @Firstname, Lastname = @Lastname, ContactNum = @ContactNum, age = @Age, barangay = @Barangay, City_Municipality = @City_Municipality, Province = @Province WHERE PersonID = @PersonID";
                MySqlCommand updateCmd = new MySqlCommand(updateQuery, con);
                updateCmd.Parameters.AddWithValue("@Firstname", row.Cells["Firstname"].Value);
                updateCmd.Parameters.AddWithValue("@Lastname", row.Cells["Lastname"].Value);
                updateCmd.Parameters.AddWithValue("@ContactNum", row.Cells["ContactNum"].Value);
                updateCmd.Parameters.AddWithValue("@Age", row.Cells["Age"].Value);
                updateCmd.Parameters.AddWithValue("@Barangay", row.Cells["Barangay"].Value);
                updateCmd.Parameters.AddWithValue("@City_Municipality", row.Cells["City_Municipality"].Value);
                updateCmd.Parameters.AddWithValue("@Province", row.Cells["Province"].Value);
                updateCmd.Parameters.AddWithValue("@PersonID", row.Cells["PersonID"].Value);

                // Execute the update query
                updateCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating record: " + ex.Message);
            }
            finally
            {
                // Close the database connection
                con.Close();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // You can add code here if needed
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            // Save changes to the database
            SaveChanges();
        }

        private void SaveChanges()
        {
            try
            {
                // Open the database connection
                con.Open();

                // Iterate through each row in the DataTable and update the database
                foreach (DataRow row in dataTable.Rows)
                {
                    // Use parameterized query to prevent SQL injection
                    string updateQuery = "UPDATE personal_infodb SET Firstname = @Firstname, Lastname = @Lastname, ContactNum = @ContactNum, Age = @Age, Barangay = @Barangay, City_Municipality = @City_Municipality, Province = @Province WHERE PersonID = @PersonID";
                    MySqlCommand updateCmd = new MySqlCommand(updateQuery, con);
                    updateCmd.Parameters.AddWithValue("@Firstname", row["Firstname"]);
                    updateCmd.Parameters.AddWithValue("@Lastname", row["Lastname"]);
                    updateCmd.Parameters.AddWithValue("@ContactNum", row["ContactNum"]);
                    updateCmd.Parameters.AddWithValue("@Age", row["Age"]);
                    updateCmd.Parameters.AddWithValue("@Barangay", row["Barangay"]);
                    updateCmd.Parameters.AddWithValue("@City_Municipality", row["City_Municipality"]);
                    updateCmd.Parameters.AddWithValue("@Province", row["Province"]);
                    updateCmd.Parameters.AddWithValue("@PersonID", row["PersonID"]);

                    // Execute the update query
                    updateCmd.ExecuteNonQuery();
                }

                MessageBox.Show("Changes saved successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving changes: " + ex.Message);
            }
            finally
            {
                // Close the database connection
                con.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Fetch data from schedsdb and display only Firstname and Lastname in DataGridView
            DisplaySchedsData("SELECT p.Firstname, p.Lastname FROM schedsdb s INNER JOIN personal_infodb p ON s.PersonSchedID = p.PersonID");
        }

        private void DisplaySchedsData(string query)
        {
            try
            {
                // Open the database connection
                con.Open();

                // Use parameterized query to prevent SQL injection
                MySqlCommand cmd = new MySqlCommand(query, con);

                // Execute the query
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);

                // Explicitly define the DataTable schema based on the columns you want to display
                DataTable schedsDataTable = new DataTable();
                schedsDataTable.Columns.Add("Firstname", typeof(string));
                schedsDataTable.Columns.Add("Lastname", typeof(string));
                schedsDataTable.Columns.Add("SchedID", typeof(int));
                schedsDataTable.Columns.Add("PeronSchedID", typeof(int));
                schedsDataTable.Columns.Add("ChurchSchedID", typeof(int));
                schedsDataTable.Columns.Add("EventDate", typeof(DateTime));
                schedsDataTable.Columns.Add("ChurchEvent", typeof(string));
                schedsDataTable.Columns.Add("SchedTime", typeof(string));

                // Clear existing data
                schedsDataTable.Clear();

                // Fill the DataTable with data from the database
                adapter.Fill(schedsDataTable);

                // Display the results in a DataGridView
                dataGridView1.DataSource = schedsDataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                // Close the database connection
                con.Close();
            }
        }
    }
}
