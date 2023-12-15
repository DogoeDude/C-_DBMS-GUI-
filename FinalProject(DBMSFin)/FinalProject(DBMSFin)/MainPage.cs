using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace FinalProject_DBMSFin_
{
    public partial class MainPage : Form
    {
        private MySqlConnection con = new MySqlConnection();

        public MainPage()
        {
            InitializeComponent();
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
                // Open the database connection
                con.Open();

                // Get values from textboxes
                if (!int.TryParse(textBox1.Text, out int personID))
                {
                    MessageBox.Show("Invalid input for PersonID. Please enter a valid integer.");
                    return;
                }

                string firstname = textBox2.Text;
                string lastname = textBox3.Text;
                string contactNum = textBox4.Text;

                if (!int.TryParse(textBox5.Text, out int age))
                {
                    MessageBox.Show("Invalid input for Age. Please enter a valid integer.");
                    return;
                }

                string barangay = textBox6.Text;
                string cityMunicipality = textBox7.Text;
                string province = textBox8.Text;

                // Use parameterized query to prevent SQL injection
                string sql = "INSERT INTO personal_infoDb (PersonID, Firstname, Lastname, ContactNum, Age, Barangay, City_Municipality, Province) " +
                             "VALUES (@PersonID, @Firstname, @Lastname, @ContactNum, @Age, @Barangay, @CityMunicipality, @Province)";
                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@PersonID", personID);
                cmd.Parameters.AddWithValue("@Firstname", firstname);
                cmd.Parameters.AddWithValue("@Lastname", lastname);
                cmd.Parameters.AddWithValue("@ContactNum", contactNum);
                cmd.Parameters.AddWithValue("@Age", age);
                cmd.Parameters.AddWithValue("@Barangay", barangay);
                cmd.Parameters.AddWithValue("@CityMunicipality", cityMunicipality);
                cmd.Parameters.AddWithValue("@Province", province);

                // Execute the query
                cmd.ExecuteNonQuery();

                // Display success message
                MessageBox.Show("Information saved successfully!");

                // Clear textboxes after submission
                ClearTextBoxes();
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
    }
}
