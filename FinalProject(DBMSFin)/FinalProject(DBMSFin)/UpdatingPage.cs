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
                dataTable.Clear(); // Clear existing data
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
                string updateQuery = "UPDATE YourTableName SET Column1 = @Column1, Column2 = @Column2 WHERE YourPrimaryKeyColumn = @PrimaryKeyValue";
                MySqlCommand updateCmd = new MySqlCommand(updateQuery, con);
                updateCmd.Parameters.AddWithValue("@Column1", row.Cells["YourColumn1"].Value);
                updateCmd.Parameters.AddWithValue("@Column2", row.Cells["YourColumn2"].Value);
                updateCmd.Parameters.AddWithValue("@PrimaryKeyValue", row.Cells["YourPrimaryKeyColumn"].Value);

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
                    string updateQuery = "UPDATE YourTableName SET Column1 = @Column1, Column2 = @Column2 WHERE YourPrimaryKeyColumn = @PrimaryKeyValue";
                    MySqlCommand updateCmd = new MySqlCommand(updateQuery, con);
                    updateCmd.Parameters.AddWithValue("@Column1", row["YourColumn1"]);
                    updateCmd.Parameters.AddWithValue("@Column2", row["YourColumn2"]);
                    updateCmd.Parameters.AddWithValue("@PrimaryKeyValue", row["YourPrimaryKeyColumn"]);

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
    }
}
