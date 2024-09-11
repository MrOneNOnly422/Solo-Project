using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace Solo_Project
{
    public partial class Register : Form
    {
        string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"G:\\SOLO\\Solo Project\\UserDatabase.mdf\";Integrated Security=True";
        private SqlConnection conn;

        public Register()
        {
            InitializeComponent();
            conn = new SqlConnection(connectionString);
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (PasswordTxt.Text != ConfirmTxt.Text)
                {
                    MessageBox.Show("Password does not match.");
                    return;
                }

                conn.Open();

                SqlCommand checkCommand = new SqlCommand();
                checkCommand.Connection = conn;
                checkCommand.CommandText = "SELECT COUNT(*) FROM registration WHERE [Username] = @Username OR [Email] = @Email";
                checkCommand.Parameters.AddWithValue("@Username", UserTxt.Text);
                checkCommand.Parameters.AddWithValue("@Email", EmailTxt.Text);
                int count = (int)checkCommand.ExecuteScalar();

                if (count > 0)
                {
                    MessageBox.Show("Username or email already exists.");
                    return;
                }

                string hashedPassword = HashPassword(PasswordTxt.Text);

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandText = "INSERT INTO registration ([Username], [Email], [Password]) VALUES (@Username, @Email, @Password)";
                command.Parameters.AddWithValue("@Username", UserTxt.Text);
                command.Parameters.AddWithValue("@Email", EmailTxt.Text);
                command.Parameters.AddWithValue("@Password", hashedPassword);
                command.ExecuteNonQuery();

                conn.Close();

                MessageBox.Show("Registration successful!");
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void Register_Load(object sender, EventArgs e)
        {
            // Code for form load event (if needed)
        }

        private void AdminRegister_Click(object sender, EventArgs e)
        {
            try
            {
                if (PasswordTxt.Text != ConfirmTxt.Text)
                {
                    MessageBox.Show("Password does not match.");
                    return;
                }

                conn.Open();

                SqlCommand checkCommand = new SqlCommand();
                checkCommand.Connection = conn;
                checkCommand.CommandText = "SELECT COUNT(*) FROM AdminRegistration WHERE [Username] = @Username OR [Email] = @Email";
                checkCommand.Parameters.AddWithValue("@Username", UserTxt.Text);
                checkCommand.Parameters.AddWithValue("@Email", EmailTxt.Text);
                int count = (int)checkCommand.ExecuteScalar();

                if (count > 0)
                {
                    MessageBox.Show("Username or email already exists.");
                    return;
                }

                string hashedPassword = HashPassword(PasswordTxt.Text);

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandText = "INSERT INTO AdminRegistration ([Username], [Email], [Password]) VALUES (@Username, @Email, @Password)";
                command.Parameters.AddWithValue("@Username", UserTxt.Text);
                command.Parameters.AddWithValue("@Email", EmailTxt.Text);
                command.Parameters.AddWithValue("@Password", hashedPassword);
                command.ExecuteNonQuery();

                conn.Close();

                MessageBox.Show("You are now an Admin!!");
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
