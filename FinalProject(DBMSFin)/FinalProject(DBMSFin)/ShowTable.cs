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

            // Add CellEndEdit event handler
            dataGridView1.CellEndEdit += dataGridView1_CellEndEdit;
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

                // Select data from both tables using JOIN operations
                string sql = "SELECT personal_infodb.Firstname, personal_infodb.Lastname, personal_infodb.ContactNum, " +
                             "schedsdb.EventDate, schedsdb.ChurchEvent, schedsdb.SchedTime, churchinfodb.ChurchName " +
                             "FROM personal_infodb " +
                             "INNER JOIN schedsdb ON personal_infodb.PersonID = schedsdb.PersonSchedID " +
                             "INNER JOIN churchinfodb ON schedsdb.ChurchSchedID = churchinfodb.ChurchID";

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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                // Open the database connection
                con.Open();

                // Get the search term from TextBox
                string searchName = textBox1.Text.Trim();

                // Select records from the 'personal_infodb' table based on the search term
                string sql = "SELECT * FROM personal_infodb WHERE Firstname LIKE @SearchTerm OR Lastname LIKE @SearchTerm";
                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, con);
                adapter.SelectCommand.Parameters.AddWithValue("@SearchTerm", $"%{searchName}%");

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

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            // Refresh the data when the PictureBox is clicked
            ShowRegisteredPersons();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if any row is selected
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    // Display a confirmation dialog
                    DialogResult result = MessageBox.Show("Are you sure you want to delete this record?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    // Check the user's choice
                    if (result == DialogResult.Yes)
                    {
                        // Get the selected row
                        DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                        // Verify that the selected row has the "PersonID" column
                        if (selectedRow.Cells["PersonID"] != null && selectedRow.Cells["PersonID"].Value != null)
                        {
                            try
                            {
                                // Open the database connection
                                con.Open();

                                // Get the value of the PersonID or any unique identifier from the selected row
                                int personIDToDelete = Convert.ToInt32(selectedRow.Cells["PersonID"].Value);

                                // Check if there are related rows in other tables
                                bool hasRelatedRows = CheckForRelatedRows(personIDToDelete);

                                // Prompt the user for confirmation
                                if (hasRelatedRows && MessageBox.Show("This record has related data. Deleting it will also delete associated data. Do you want to proceed?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                                {
                                    return; // If the user decides not to proceed, exit the method
                                }

                                // Perform the delete operation using a DELETE SQL statement
                                string deleteSql = "DELETE FROM personal_infodb WHERE PersonID = @PersonID";
                                MySqlCommand deleteCmd = new MySqlCommand(deleteSql, con);
                                deleteCmd.Parameters.AddWithValue("@PersonID", personIDToDelete);
                                deleteCmd.ExecuteNonQuery();

                                // Display success message
                                MessageBox.Show("Record deleted successfully!");

                                // Refresh the DataGridView to reflect the changes
                                ShowRegisteredPersons();
                            }
                            catch (Exception ex)
                            {
                                // Log the error or handle it appropriately for your application
                                MessageBox.Show("Error deleting record: " + ex.ToString());
                            }
                            finally
                            {
                                // Close the database connection
                                con.Close();
                            }
                        }
                        else
                        {
                            MessageBox.Show("The selected row does not contain the 'PersonID' column.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select a row to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                // Log the error or display the exception details
                MessageBox.Show("Error: " + ex.ToString());
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Check if any cell is edited
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    // Get the edited value and the column name
                    object editedValue = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                    string columnName = dataGridView1.Columns[e.ColumnIndex].Name;

                    // Get the primary key value (assuming the PersonID is the primary key)
                    int personID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["PersonID"].Value);

                    // Open the database connection
                    con.Open();

                    // Update the edited value in the database
                    string updateSql = $"UPDATE personal_infodb SET {columnName} = @EditedValue WHERE PersonID = @PersonID";
                    MySqlCommand updateCmd = new MySqlCommand(updateSql, con);
                    updateCmd.Parameters.AddWithValue("@EditedValue", editedValue);
                    updateCmd.Parameters.AddWithValue("@PersonID", personID);
                    updateCmd.ExecuteNonQuery();

                    // Display success message
                    MessageBox.Show("Changes saved successfully!");
                }
            }
            catch (Exception ex)
            {
                // Log the error or handle it appropriately for your application
                MessageBox.Show("Error saving changes: " + ex.ToString());
            }
            finally
            {
                // Close the database connection
                con.Close();
            }
        }

        private bool CheckForRelatedRows(int personID)
        {
            MySqlConnection checkConnection = new MySqlConnection(con.ConnectionString);

            try
            {
                // Open the new database connection
                checkConnection.Open();

                // Check if there are related rows in other tables (modify the SQL query as needed)
                string checkSql = "SELECT COUNT(*) FROM other_table WHERE PersonID = @PersonID";
                MySqlCommand checkCmd = new MySqlCommand(checkSql, checkConnection);
                checkCmd.Parameters.AddWithValue("@PersonID", personID);

                int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                return count > 0;
            }
            catch (Exception ex)
            {
                // Log the error or handle it appropriately for your application
                MessageBox.Show("Error checking for related rows: " + ex.ToString());
                return false; // Return false in case of an error
            }
            finally
            {
                // Close the new database connection
                if (checkConnection.State == ConnectionState.Open)
                    checkConnection.Close();
            }
        }

        private void DeleteSelectedRow(string tableName)
        {
            try
            {
                // Check if any row is selected
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    // Display a confirmation dialog
                    DialogResult result = MessageBox.Show("Are you sure you want to delete this record?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    // Check the user's choice
                    if (result == DialogResult.Yes)
                    {
                        // Get the selected row
                        DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                        // Verify that the selected row has the primary key column
                        if (selectedRow.Cells["PersonID"] != null && selectedRow.Cells["PersonID"].Value != null)
                        {
                            try
                            {
                                // Open the database connection
                                con.Open();

                                // Get the value of the primary key from the selected row
                                int primaryKeyValue = Convert.ToInt32(selectedRow.Cells["PersonID"].Value);

                                // Check if there are related rows in other tables
                                bool hasRelatedRows = CheckForRelatedRows(primaryKeyValue);

                                // Prompt the user for confirmation
                                if (hasRelatedRows && MessageBox.Show("This record has related data. Deleting it will also delete associated data. Do you want to proceed?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                                {
                                    return; // If the user decides not to proceed, exit the method
                                }

                                // Perform the delete operation using a DELETE SQL statement
                                string deleteSql = $"DELETE FROM {tableName} WHERE PersonID = @PrimaryKeyValue";
                                MySqlCommand deleteCmd = new MySqlCommand(deleteSql, con);
                                deleteCmd.Parameters.AddWithValue("@PrimaryKeyValue", primaryKeyValue);
                                deleteCmd.ExecuteNonQuery();

                                // Display success message
                                MessageBox.Show("Record deleted successfully!");

                                // Refresh the DataGridView to reflect the changes
                                if (tableName == "schedsdb")
                                {
                                    ShowEvents();
                                }
                                else if (tableName == "personal_infodb")
                                {
                                    ShowRegisteredPersons();
                                }
                            }
                            catch (Exception ex)
                            {
                                // Log the error or handle it appropriately for your application
                                MessageBox.Show($"Error deleting record from {tableName}: " + ex.ToString());
                            }
                            finally
                            {
                                // Close the database connection
                                con.Close();
                            }
                        }
                        else
                        {
                            MessageBox.Show($"The selected row does not contain the primary key column.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select a row to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                // Log the error or display the exception details
                MessageBox.Show("Error: " + ex.ToString());
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            // Add your code for button4_Click_1 here, or leave it empty if not needed
        }

        private void button6_Click_1(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {

        }
    }
}
