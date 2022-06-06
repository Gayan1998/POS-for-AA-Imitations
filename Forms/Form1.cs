using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PRINT_SHOP
{
    public partial class Loging : Form
    {
        public Loging()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                MySqlConnection mycon = new MySqlConnection(connections.connection_string);
                MySqlCommand select = new MySqlCommand("select * from loging where user_name = '" + this.textBox1.Text.Trim() + "' and password ='" + this.textBox2.Text.Trim() + "';", mycon);
                MySqlDataReader reader;
                mycon.Open();
                reader = select.ExecuteReader();
                int count = 0;
                while (reader.Read())
                {
                    count = count + 1;
                }
                if (count == 1)
                {
                    get_type();
                    MessageBox.Show("welcome");
                    this.Hide();
                    Dash d1 = new Dash();
                    d1.Type = type;
                    d1.User_name = U_name;
                    d1.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Invalid User Name or Password");
                    ActiveControl = textBox1;
                }

                mycon.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        string type;
        string U_name;
        private void get_type()
        {
            try
            {
                MySqlConnection mycon = new MySqlConnection(connections.connection_string);
                MySqlCommand select = new MySqlCommand("select * from loging where user_name = '" + this.textBox1.Text.Trim() + "' and password = '" + this.textBox2.Text.Trim() + "';", mycon);
                MySqlDataReader reader2;
                mycon.Open();
                reader2 = select.ExecuteReader();
                if (reader2.Read())
                {
                    type = reader2["type"].ToString();
                    U_name = reader2["user_name"].ToString();
                }
                else
                {
                    MessageBox.Show("No Item Found");
                }
                mycon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }
    }
}
