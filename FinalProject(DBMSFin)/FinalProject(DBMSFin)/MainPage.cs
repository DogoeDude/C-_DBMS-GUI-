using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace FinalProject_DBMSFin_
{
    public partial class MainPage : Form
    {
        private MySqlConnection con = new MySqlConnection();

        private System.Windows.Forms.ComboBox comboBoxTime;

        public MainPage()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            // Initialize the connection string
            string connstring = "server=localhost;uid=root;pwd=password;database=churchsched";
            con.ConnectionString = connstring;

            // Create an instance of ComboBox
            comboBoxTime = new ComboBox();

            // Load data into the ComboBox when the form is initialized
            LoadDataIntoComboBox();
        }

        private void MainPage_Load(object sender, EventArgs e)
        {
            // Initialize the connection string
            string connstring = "server=localhost;uid=root;pwd=password;database=churchsched";
            con.ConnectionString = connstring;
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Get values from textboxes and controls
                if (!int.TryParse(textBox9.Text, out int schedID))
                {
                    MessageBox.Show("Invalid input for SchedID. Please enter a valid integer.");
                    return;
                }

                if (!int.TryParse(textBox11.Text, out int personID))
                {
                    MessageBox.Show("Invalid input for PersonID. Please enter a valid integer.");
                    return;
                }

                // Check if the entered PersonID exists in personal_infoDb
                if (!DoesPersonIdExist(personID))
                {
                    MessageBox.Show("PersonID does not exist. Please enter a valid PersonID.");
                    return;
                }

                // Check if there are items in comboBox1
                if (comboBox1.Items.Count > 0 && comboBox1.SelectedItem != null)
                {
                    int churchSchedID = ((ChurchInfo)comboBox1.SelectedItem).ChurchID;

                    string churchEvent = textBox10.Text;

                    // Get the next available SchedID
                    schedID = GetNextSchedID(); // Remove the 'int' type here

                    // Get the selected date from DateTimePicker
                    DateTime eventDate = dateTimePicker1.Value;

                    // Assuming you have a ComboBox named comboBox2 with time options from 12:00:00 AM to 12:00:00 PM
                    TimeSpan schedTime = TimeSpan.Parse(comboBox2.SelectedItem.ToString());

                    // Open the database connection
                    con.Open();

                    // Use parameterized query to prevent SQL injection
                    string sql = "INSERT INTO schedsdb (SchedID, PersonSchedID, ChurchSchedID, EventDate, ChurchEvent, SchedTime) " +
                                 "VALUES (@SchedID, @PersonSchedID, @ChurchSchedID, @EventDate, @ChurchEvent, @SchedTime)";
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@SchedID", schedID);
                    cmd.Parameters.AddWithValue("@PersonSchedID", personID);
                    cmd.Parameters.AddWithValue("@ChurchSchedID", churchSchedID);
                    cmd.Parameters.AddWithValue("@EventDate", eventDate.ToString("yyyy-MM-dd")); // Format date as a string
                    cmd.Parameters.AddWithValue("@ChurchEvent", churchEvent);
                    cmd.Parameters.AddWithValue("@SchedTime", schedTime.ToString(@"hh\:mm\:ss")); // Format TimeSpan as a string

                    // Execute the query
                    cmd.ExecuteNonQuery();

                    // Display success message
                    MessageBox.Show("Details saved successfully!");

                    // Clear textboxes and controls after submission
                    ClearTextBoxes();
                }
                else
                {
                    MessageBox.Show("No items in the ComboBox or SelectedItem is null. Please check your data.");
                }
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


        private void ResetButton_Click(object sender, EventArgs e)
        {
            // Clear textboxes when Reset button is clicked
            ClearTextBoxes();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // This is your "Reset" button click event
            // Clear textboxes when the "Reset" button (button2) is clicked
            ClearTextBoxes();
        }

        // Helper method to clear textboxes
        private void ClearTextBoxes()
        {
            foreach (Control control in Controls)
            {
                if (control is TextBox)
                {
                    ((TextBox)control).Clear();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SubmitButton_Click(sender, e);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Create an instance of ShowTable form
            ShowTable showTableForm = new ShowTable();

            // Show the ShowTable form
            showTableForm.Show();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                // Create an instance of UpdatingPage form
                UpdatingPage updatingPageForm = new UpdatingPage();

                // Show the UpdatingPage form
                updatingPageForm.Show();

                // Close the current form (MainPage)
                this.Close();
            }
            catch (Exception ex)
            {
                // Log the error or handle it appropriately for your application
                MessageBox.Show("Error: " + ex.ToString());
            }
        }

        // ... (your existing code)

        // Helper method to get the next available SchedID
        private int GetNextSchedID()
        {
            try
            {
                con.Open();

                // Query to get the next available SchedID
                string sql = "SELECT COALESCE(MAX(SchedID), 0) + 1 FROM schedsdb";
                MySqlCommand cmd = new MySqlCommand(sql, con);

                // Execute the query and return the result
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error getting next SchedID: " + ex.ToString());
                return -1; // Return a default value or handle the error as appropriate
            }
            finally
            {
                con.Close();
            }
        }

        // ... (your existing code)

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                // Get values from textboxes and controls
                if (!int.TryParse(textBox9.Text, out int personID))
                {
                    MessageBox.Show("Invalid input for PersonID. Please enter a valid integer.");
                    return;
                }

                // Check if the entered PersonID exists in personal_infoDb
                if (!DoesPersonIdExist(personID))
                {
                    MessageBox.Show("PersonID does not exist. Please enter a valid PersonID.");
                    return;
                }

                // Check if there are items in comboBox1
                if (comboBox1.Items.Count > 0 && comboBox1.SelectedItem != null)
                {
                    int churchSchedID = ((ChurchInfo)comboBox1.SelectedItem).ChurchID;
                    string churchEvent = textBox10.Text;

                    // Get the next available SchedID
                    int schedID = GetNextSchedID();

                    // Get the selected date from DateTimePicker
                    DateTime eventDate = dateTimePicker1.Value;

                    // Combine the selected time from comboBox2
                    string selectedTime = comboBox2.SelectedItem.ToString();

                    // Parse the combined time string to a TimeSpan object
                    TimeSpan parsedTime = TimeSpan.Parse(selectedTime);

                    // Determine whether it's AM or PM and set the value for the DN column
                    string dayNight = comboBox3.SelectedItem?.ToString() ?? "N/A";

                    // Open the database connection
                    con.Open();

                    // Insert data into the 'schedsdb' table
                    string insertSql = "INSERT INTO schedsdb (SchedID, PersonSchedID, ChurchSchedID, EventDate, ChurchEvent, SchedTime, DN) " +
                                        "VALUES (@SchedID, @PersonSchedID, @ChurchSchedID, @EventDate, @ChurchEvent, @SchedTime, @DN)";
                    MySqlCommand insertCmd = new MySqlCommand(insertSql, con);
                    insertCmd.Parameters.AddWithValue("@SchedID", schedID);
                    insertCmd.Parameters.AddWithValue("@PersonSchedID", personID);
                    insertCmd.Parameters.AddWithValue("@ChurchSchedID", churchSchedID);
                    insertCmd.Parameters.AddWithValue("@EventDate", eventDate.ToString("yyyy-MM-dd")); // Format date as a string
                    insertCmd.Parameters.AddWithValue("@ChurchEvent", churchEvent);
                    insertCmd.Parameters.AddWithValue("@SchedTime", parsedTime.ToString()); // Store TimeSpan as a string
                    insertCmd.Parameters.AddWithValue("@DN", dayNight); // Set the value for the DN column

                    // Execute the query
                    insertCmd.ExecuteNonQuery();

                    // Display success message
                    MessageBox.Show("Details saved successfully!");

                    // Clear textboxes after submission
                    ClearTextBoxes();
                }
                else
                {
                    MessageBox.Show("No items in the ComboBox or SelectedItem is null. Please check your data.");
                }
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






        // ... (your existing code)


        // Helper method to check if PersonID exists in personal_infoDb
        private bool DoesPersonIdExist(int personId)
        {
            try
            {
                con.Open();

                // Check if the PersonID exists in the personal_infoDb table
                string checkSql = "SELECT COUNT(*) FROM personal_infoDb WHERE PersonID = @PersonID";
                MySqlCommand checkCmd = new MySqlCommand(checkSql, con);
                checkCmd.Parameters.AddWithValue("@PersonID", personId);

                int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                return count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error checking PersonID: " + ex.ToString());
                return false;
            }
            finally
            {
                con.Close();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Your code here
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Your code here
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            // Your code here
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            // Your code here
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            // Your code here
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            // Your code here
        }

        private void comboBoxTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Your code here
        }

        // ... (your existing code)

        // Assuming ChurchInfo is a class representing the items in the ComboBox
        public class ChurchInfo
        {
            public int ChurchID { get; set; }
            public string ChurchName { get; set; }
        }

        // Populate the ComboBox with ChurchInfo items
        // Inside your LoadDataIntoComboBox() method
        private void LoadDataIntoComboBox()
        {
            try
            {
                // Open the database connection
                con.Open();

                // Select data from the 'churchinfodb' table
                string sql = "SELECT ChurchID, ChurchName FROM churchinfodb";
                MySqlCommand cmd = new MySqlCommand(sql, con);

                // Execute the query and read the data
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    // Clear existing items in the ComboBox
                    comboBox1.Items.Clear();

                    // Iterate through the result set and add items to the ComboBox
                    while (reader.Read())
                    {
                        // Assuming 'ChurchID' and 'ChurchName' are the column names in your table
                        int churchID = reader.GetInt32("ChurchID");
                        string churchName = reader.GetString("ChurchName");

                        // Create a new ChurchInfo object and add it to the ComboBox
                        ChurchInfo churchInfo = new ChurchInfo { ChurchID = churchID, ChurchName = churchName };
                        comboBox1.Items.Add(churchInfo);
                    }

                    // Set DisplayMember to the property you want to display
                    comboBox1.DisplayMember = "ChurchName";
                    // Set ValueMember to the property you want to use as the value
                    // comboBox1.ValueMember = "ChurchID"; // Uncomment and adjust if needed
                }
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

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
