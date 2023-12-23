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
            string connstring = "server=localhost;uid=root;pwd=password;database=churchsched";
            con.ConnectionString = connstring;
            comboBoxTime = new ComboBox();
            LoadDataIntoComboBox();
            this.KeyDown += new KeyEventHandler(MainPage_KeyDown);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }
        private void MainPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button3_Click(sender, e);
            }
        }
        private void MainPage_Load(object sender, EventArgs e)
        {
            string connstring = "server=localhost;uid=root;pwd=password;database=churchsched";
            con.ConnectionString = connstring;
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            try
            {
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
                if (!DoesPersonIdExist(personID))
                {
                    MessageBox.Show("PersonID does not exist. Please enter a valid PersonID.");
                    return;
                }
                if (comboBox1.Items.Count > 0 && comboBox1.SelectedItem != null)
                {
                    int churchSchedID = ((ChurchInfo)comboBox1.SelectedItem).ChurchID;

                    string churchEvent = textBox10.Text;
                    schedID = GetNextSchedID(); // Remove the 'int' type here
                    DateTime eventDate = dateTimePicker1.Value;// Assuming you have a ComboBox named comboBox2 with time options from 12:00:00 AM to 12:00:00 PM
                    TimeSpan schedTime = TimeSpan.Parse(comboBox2.SelectedItem.ToString());
                    con.Open();
                    string sql = "INSERT INTO schedsdb (SchedID, PersonSchedID, ChurchSchedID, EventDate, ChurchEvent, SchedTime) " +
                                 "VALUES (@SchedID, @PersonSchedID, @ChurchSchedID, @EventDate, @ChurchEvent, @SchedTime)";
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@SchedID", schedID);
                    cmd.Parameters.AddWithValue("@PersonSchedID", personID);
                    cmd.Parameters.AddWithValue("@ChurchSchedID", churchSchedID);
                    cmd.Parameters.AddWithValue("@EventDate", eventDate.ToString("yyyy-MM-dd")); // Format date as a string
                    cmd.Parameters.AddWithValue("@ChurchEvent", churchEvent);
                    cmd.Parameters.AddWithValue("@SchedTime", schedTime.ToString(@"hh\:mm\:ss")); // Format TimeSpan as a string
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Details saved successfully in schedsdb!");
                    ClearTextBoxes();
                }
                else
                {
                    MessageBox.Show("No items in the ComboBox or SelectedItem is null. Please check your data.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString());
            }
            finally
            {
                con.Close();
            }
        }
        private void ResetButton_Click(object sender, EventArgs e)
        {
            ClearTextBoxes();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ClearTextBoxes();
        }
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
            try
            {
                if (!int.TryParse(textBox11.Text, out int personID))
                {
                    MessageBox.Show("Invalid input for PersonID. Please enter a valid integer.");
                    return;
                }
                con.Open();
                string sql = "INSERT INTO personal_infoDb (PersonID, FirstName, LastName, ContactNum) " +
                             "VALUES (@PersonID, @FirstName, @LastName, @ContactNum)";
                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@PersonID", personID);
                cmd.Parameters.AddWithValue("@FirstName", textBox1.Text); // Assuming you have a TextBox named textBox1 for FirstName
                cmd.Parameters.AddWithValue("@LastName", textBox2.Text); // Assuming you have a TextBox named textBox2 for LastName
                cmd.Parameters.AddWithValue("@ContactNum", textBox3.Text); // Assuming you have a TextBox named textBox3 for ContactNum
                cmd.ExecuteNonQuery();

                MessageBox.Show("Details saved successfully in personal_infoDb!");
                ClearTextBoxes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString());
            }
            finally
            {
                con.Close();
            }
        }

        private void UpdatingPage_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Show();  // Make the current form visible again when UpdatingPage is closed
        }
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                UpdatingPage updatingPageForm = new UpdatingPage();
                updatingPageForm.FormClosed += UpdatingPage_FormClosed;  // Subscribe to FormClosed event
                updatingPageForm.Show();
                this.Hide();  // Hide the current form instead of closing it
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString());
            }
        }
        private int GetNextSchedID()
        {
            try
            {
                con.Open();
                string sql = "SELECT COALESCE(MAX(SchedID), 0) + 1 FROM schedsdb";
                MySqlCommand cmd = new MySqlCommand(sql, con);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error getting next SchedID: " + ex.ToString());
                return -1; 
            }
            finally
            {
                con.Close();
            }
        }
        private bool DoesPersonIdExist(int personId)
        {
            try
            {
                con.Open();
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
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
        }
        private void textBox10_TextChanged(object sender, EventArgs e)
        {
        }
        private void textBox11_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
        }

        private void comboBoxTime_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        public class ChurchInfo
        {
            public int ChurchID { get; set; }
            public string ChurchName { get; set; }
        }
        private void LoadDataIntoComboBox()
        {
            try
            {
                con.Open();
                string sql = "SELECT ChurchID, ChurchName FROM churchinfodb";
                MySqlCommand cmd = new MySqlCommand(sql, con);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    comboBox1.Items.Clear();
                    while (reader.Read())
                    {
                        // Assuming 'ChurchID' and 'ChurchName' are the column names in your table
                        int churchID = reader.GetInt32("ChurchID");
                        string churchName = reader.GetString("ChurchName");
                        // Create a new ChurchInfo object and add it to the ComboBox
                        ChurchInfo churchInfo = new ChurchInfo { ChurchID = churchID, ChurchName = churchName };
                        comboBox1.Items.Add(churchInfo);
                    }
                    comboBox1.DisplayMember = "ChurchName";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString());
            }
            finally
            {
                con.Close();
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(textBox1.Text, out int personID))
                {
                    MessageBox.Show("Invalid input Check all textboxes and enter valid details. ");
                    return;
                }
                con.Open();
                string sql = "INSERT INTO personal_infodb (PersonID, FirstName, LastName, ContactNum, Age, Barangay, City_Municipality, Province) " +
                             "VALUES (@PersonID, @FirstName, @LastName, @ContactNum, @Age, @Barangay, @CityMunicipality, @Province)";
                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@PersonID", personID);
                cmd.Parameters.AddWithValue("@FirstName", textBox2.Text);
                cmd.Parameters.AddWithValue("@LastName", textBox3.Text);
                cmd.Parameters.AddWithValue("@ContactNum", textBox4.Text);  // Correct data type for ContactNum
                cmd.Parameters.AddWithValue("@Age", textBox5.Text);  // Correct data type for Age
                cmd.Parameters.AddWithValue("@Barangay", textBox6.Text);
                cmd.Parameters.AddWithValue("@CityMunicipality", textBox7.Text);
                cmd.Parameters.AddWithValue("@Province", textBox8.Text);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Details saved successfully in personal_infodb!");

                ClearTextBoxes();
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062) // MySQL error code for duplicate entry
                {
                    MessageBox.Show("PersonID already exists. Please enter a unique PersonID.");
                }
                else
                {
                    MessageBox.Show("Error: " + ex.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString());
            }
            finally
            {
                con.Close();
            }
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(textBox9.Text, out int personID))
                {
                    MessageBox.Show("Invalid input for PersonID. Please enter a valid integer.");
                    return;
                }
                if (!DoesPersonIdExist(personID))
                {
                    MessageBox.Show("PersonID does not exist. Please enter a valid PersonID.");
                    return;
                }
                if (comboBox1.Items.Count > 0 && comboBox1.SelectedItem != null)
                {
                    int churchSchedID = ((ChurchInfo)comboBox1.SelectedItem).ChurchID;
                    string churchEvent = textBox10.Text;
                    int schedID = GetNextSchedID();
                    DateTime eventDate = dateTimePicker1.Value;
                    string selectedTime = comboBox2.SelectedItem.ToString();
                    TimeSpan parsedTime = TimeSpan.Parse(selectedTime);
                    string dayNight = comboBox3.SelectedItem?.ToString() ?? "N/A";
                    con.Open();
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
                    insertCmd.ExecuteNonQuery();
                    MessageBox.Show("Details saved successfully!");
                    ClearTextBoxes();
                }
                else
                {
                    MessageBox.Show("No items in the ComboBox or SelectedItem is null. Please check your data.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString());
            }
            finally
            {
                con.Close();
            }
        }
        private void textBox9_TextChanged_1(object sender, EventArgs e)
        {
        }
        private void textBox11_TextChanged_1(object sender, EventArgs e)
        {
        }
        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
        }
        private void dateTimePicker1_ValueChanged_1(object sender, EventArgs e)
        {
        }
        private void textBox10_TextChanged_1(object sender, EventArgs e)
        {
        }
        private void comboBox2_SelectedIndexChanged_1(object sender, EventArgs e)
        {
        }
        private void panel2_Paint(object sender, PaintEventArgs e)
        {
        }
        private void comboBox3_SelectedIndexChanged_1(object sender, EventArgs e)
        {
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                this.Hide(); // Hide the current form (MainPage)
                Table1Person table1PersonForm = new Table1Person();
                table1PersonForm.Text = "Personal Informations"; // Set the title of the form
                table1PersonForm.ShowDialog(); // Show the new form as a dialog
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString());
            }
        }
        private void button5_Click_1(object sender, EventArgs e)
        {
            try
            {
                this.Hide();
                Table2Events table2EventsForm = new Table2Events();
                table2EventsForm.Text = "Events";
                table2EventsForm.ShowDialog();
            }
            catch (Exception ex){
                MessageBox.Show("Error: " + ex.ToString());
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            this.Close();
            LogRegPage logRegPagerev = new LogRegPage();
            logRegPagerev.ShowDialog();
        }
    }
}
