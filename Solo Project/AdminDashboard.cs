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
using System.Xml.Linq;

namespace Solo_Project
{
    public partial class AdminDashboard : Form
    {
        string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"G:\\SOLO\\Solo Project\\UserDatabase.mdf\";Integrated Security=True";
        private SqlConnection conn;
        public AdminDashboard()
        {
            InitializeComponent();
            conn = new SqlConnection(connectionString);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.Hide();
            AdminLogin admin = new AdminLogin();
            admin.Show();
        }

        private void AdminDashboard_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'productDatabase.product' table. You can move, or remove it, as needed.
            this.productTableAdapter.Fill(this.productDatabase.product);
            LoadDataGrid();

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string bookName = txtName.Text;
                    string author = txtAuthor.Text;
                    int stock = int.Parse(txtStock.Text);
                    decimal price = decimal.Parse(txtPrice.Text);

                    string sqlQuery = @"
                    INSERT INTO product (BookName, Author, Stock, Price) 
                    VALUES (@BookName, @Author, @Stock, @Price);
                    SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@BookName", bookName);
                        command.Parameters.AddWithValue("@Author", author);
                        command.Parameters.AddWithValue("@Stock", stock);
                        command.Parameters.AddWithValue("@Price", price);

                        int newProductID = Convert.ToInt32(command.ExecuteScalar());

                        MessageBox.Show("Data added with Product ID: " + newProductID);

                        LoadDataGrid(); // Refresh the DataGridView
                    }
                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Invalid input format: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void LoadDataGrid()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sqlQuery = "SELECT * FROM product";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);

                        // Assuming grid1 is your DataGridView control
                        grid1.DataSource = dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }
        private void grid1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void grid1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < grid1.Rows.Count)
            {
                string productid = grid1.Rows[e.RowIndex].Cells["ProductID"].Value.ToString();

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string sqlQuery = "SELECT * FROM product WHERE ProductID = @ProductID";

                        using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                        {
                            command.Parameters.AddWithValue("@ProductID", productid);

                            SqlDataReader reader = command.ExecuteReader();

                            if (reader.Read())
                            {
                                txtName.Text = reader["Name"].ToString();
                                txtAuthor.Text = reader["Author"].ToString();
                                txtStock.Text = reader["Stock"].ToString();
                                txtPrice.Text = reader["Price"].ToString();
                                // Add any additional fields here
                            }

                            reader.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (grid1.SelectedRows.Count > 0)
                {
                    int productId = (int)grid1.SelectedRows[0].Cells["ProductID"].Value;

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string bookName = txtName.Text;
                        string author = txtAuthor.Text;
                        int stock = int.Parse(txtStock.Text);
                        decimal price = decimal.Parse(txtPrice.Text);

                        string sqlQuery = @"
                        UPDATE product 
                        SET BookName = @BookName, 
                            Author = @Author, 
                            Stock = @Stock, 
                            Price = @Price 
                        WHERE ProductID = @ProductID";

                        using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                        {
                            command.Parameters.AddWithValue("@BookName", bookName);
                            command.Parameters.AddWithValue("@Author", author);
                            command.Parameters.AddWithValue("@Stock", stock);
                            command.Parameters.AddWithValue("@Price", price);
                            command.Parameters.AddWithValue("@ProductID", productId);

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Data updated successfully!");
                            }
                            else
                            {
                                MessageBox.Show("No data found with the specified Product ID.");
                            }

                            LoadDataGrid(); // Refresh the DataGridView
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select a row to edit.");
                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Invalid input format: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (grid1.SelectedRows.Count > 0)
                {
                    int productId = (int)grid1.SelectedRows[0].Cells["ProductID"].Value;

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string sqlQuery = "DELETE FROM product WHERE ProductID = @ProductID";

                        using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                        {
                            command.Parameters.AddWithValue("@ProductID", productId);

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Data deleted successfully!");
                            }
                            else
                            {
                                MessageBox.Show("No data found with the specified Product ID.");
                            }

                            LoadDataGrid(); // Refresh the DataGridView
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select a row to delete.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
    }

