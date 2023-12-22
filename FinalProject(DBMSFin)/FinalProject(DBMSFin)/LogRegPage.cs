using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace FinalProject_DBMSFin_
{
    public partial class LogRegPage : Form
    {
        private MySqlConnection con = new MySqlConnection();

        public LogRegPage()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void label3_Click(object sender, EventArgs e)
        {
            // Whatever code you want for label3 click event
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Whatever code you want for pictureBox1 click event
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Perform registration logic if needed
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                string connstring = "server=localhost;uid=root;pwd=password;database=churchsched";
                con.ConnectionString = connstring;
                con.Open();

                string email = textBox1.Text; // Assuming you have a TextBox for email
                string password = textBox2.Text; // Assuming you have a TextBox for password

                // Use parameterized query to prevent SQL injection
                string sql = "SELECT * FROM loginfo WHERE email = @email AND epassword = @password";
                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@password", password);

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    // Login successful
                    MessageBox.Show("Login successful!");

                    // Hide the current form
                    this.Hide();

                    // Show the main page
                    MainPage mainPage = new MainPage();
                    mainPage.Show();
                }
                else
                {
                    // Login failed
                    MessageBox.Show("Invalid email or password.");
                }
            }
            catch (Exception ex)
            {
                // Log the error or handle it appropriately for your application
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // You can add code here if needed, for example, to validate or format the entered email
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            // You can add code here if needed, for example, to handle password strength validation
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Hide the current form (LogRegPage)
                this.Hide();

                // Show the 'ShowDatabasePriest' form
                ShowDatabasePriest showDatabasePriestForm = new ShowDatabasePriest();
                showDatabasePriestForm.Show();
            }
            catch (Exception ex)
            {
                // Log the error or handle it appropriately for your application
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
