using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;


namespace Solo_Project
{
    public partial class AdminLogin : Form
    {
        string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"G:\\SOLO(Final)\\SOLO\\Solo Project\\UserDatabase.mdf\";Integrated Security=True";
        private SqlConnection conn;
        public AdminLogin()
        {
            InitializeComponent();
            conn = new SqlConnection();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Form1 login = new Form1();
            login.Show();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                PasswordTxt.UseSystemPasswordChar = false;
            }
            else
            {
                PasswordTxt.UseSystemPasswordChar = true;
            }
        }

        private void Login_Click(object sender, EventArgs e)
        {
            string email = EmailTxt.Text;
            string password = PasswordTxt.Text;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Password FROM AdminRegistration WHERE Email = @Email";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    string storedPassword = command.ExecuteScalar()?.ToString();

                    if (storedPassword != null && VerifyPassword(password, storedPassword))
                    {
                        AdminDashboard dash = new AdminDashboard();
                        MessageBox.Show("Login Success", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Hide();
                        dash.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Incorrect email or password", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
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

        private bool VerifyPassword(string inputPassword, string storedPassword)
        {
            string hashedInputPassword = HashPassword(inputPassword);
            return hashedInputPassword == storedPassword;
        }

        private void AdminLogin_Load(object sender, EventArgs e)
        {

        }
    }
}
