using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Solo_Project
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 login = new Form1();
            login.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.Hide();
            UserInfo user = new UserInfo();
            user.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void grid1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'userDatabaseDataSet.product' table. You can move, or remove it, as needed.
            this.productTableAdapter.Fill(this.userDatabaseDataSet.product);

        }
    }
}
