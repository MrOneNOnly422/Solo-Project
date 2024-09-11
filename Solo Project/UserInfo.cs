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

namespace Solo_Project
{
    public partial class UserInfo : Form
    {
        private string username;
        string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"G:\\SOLO\\Solo Project\\UserDatabase.mdf\";Integrated Security=True";
        private SqlConnection conn;

        public UserInfo()
        {
            InitializeComponent();
            conn = new SqlConnection(connectionString);
        }

        public UserInfo(string username) : this()
        {
            this.username = username;
        }

        private void UserInfo_Load(object sender, EventArgs e)
        {
            txtuser.Text = this.username;

            try
            {
                conn.Open();

                string query = "SELECT Email FROM registration WHERE Username = @Username";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@Username", this.username);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    txtemail.Text = reader["Email"].ToString();
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
            Dashboard dash = new Dashboard();
            dash.Show();
        }
    }
}
