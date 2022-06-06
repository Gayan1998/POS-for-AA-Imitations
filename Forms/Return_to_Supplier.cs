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
    public partial class Return_to_Supplier : Form
    {
        public Return_to_Supplier()
        {
            InitializeComponent();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                get_info();
            }
        }
        private void get_info()
        {

            try
            {
                MySqlConnection mycon = new MySqlConnection(connections.connection_string);
                MySqlCommand select = new MySqlCommand("select * from item where Item_id  = '" + this.textBox1.Text + "' ;", mycon);
                MySqlDataReader reader2;
                mycon.Open();
                reader2 = select.ExecuteReader();

                if (reader2.Read())
                {
                    string pname;
                    string price;
                    pname = reader2["Item_name"].ToString();
                    textBox2.Text = pname;

                    price = reader2["supplier"].ToString();
                    textBox3.Text = price;

                }
                else
                {
                    MessageBox.Show("No Item Found");
                }
                mycon.Close();
            }
            catch
            {

            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                add_data();
            }
        }

        private void add_data()
        {
            try
            {
                if(string.IsNullOrEmpty(textBox1.Text)|| string.IsNullOrEmpty(textBox4.Text))
                {
                    MessageBox.Show("Please Fill all the data");
                }
                else
                {
               
                        int id = int.Parse(textBox1.Text);
                        string name = textBox2.Text;
                        string supp = textBox3.Text;
                        int qty = int.Parse(textBox4.Text);
                        this.dataGridView1.Rows.Add(id, name, supp, qty);
                    
                }
            }
            catch
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                update_stock();
               // save_return();
                clear_all();
                MessageBox.Show("Done");
            }
            catch
            {

            }
        }

        private void clear_all()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            dataGridView1.Rows.Clear();
        }

        private void save_return()
        {
            try
            {
                string item_id;
                MySqlConnection mycon = new MySqlConnection(connections.connection_string);
                MySqlDataReader myreader;
                for (int row = 0; row < dataGridView1.Rows.Count; row++)
                {
                    string d = DateTime.Today.ToString("yyyy-MM-dd");
                    item_id = dataGridView1.Rows[row].Cells[0].Value.ToString();
                    int qty = int.Parse(dataGridView1.Rows[row].Cells[4].Value.ToString());
                    string supp = dataGridView1.Rows[row].Cells[2].Value.ToString();
                    string query2 = " insert into return_supp (Item_ID, supplier, qty,date) values ('" + item_id + "','" + supp + "','" + qty + "','"+d+"');";
                    MySqlCommand cmd2 = new MySqlCommand(query2, mycon);
                    mycon.Open();
                    myreader = cmd2.ExecuteReader();
                    mycon.Close();
                }
            }
            catch
            {

            }
        }

      /*  private void reset_imei()
        {
            try
            {
                string connection = "datasource=localhost;port=3306;username=root;password=;";
                MySqlConnection mycon = new MySqlConnection(connection);

                for (int row = 0; row < dataGridView1.Rows.Count; row++)
                {
                    int i = int.Parse(dataGridView1.Rows[row].Cells[0].Value.ToString());
                    string imie = dataGridView1.Rows[row].Cells[3].Value.ToString();
                    if (string.IsNullOrEmpty(imie))
                    {

                    }
                    else
                    {
                        Update_imei(imie);
                    }

                }


            }
            catch
            {

            }
        }*/

       /* private void Update_imei(string imie)
        {
            string connection = "datasource=localhost;port=3306;username=root;password=;";
            string query = "update ps.manage_imie set  status ='" + "Returned" + "',invoice_no ='" + "0" + "' where imei  ='" + imie + "';";
            MySqlConnection mycon = new MySqlConnection(connection);
            MySqlCommand cmd = new MySqlCommand(query, mycon);
            MySqlDataReader myreader;
            try
            {
                mycon.Open();
                myreader = cmd.ExecuteReader();
                while (myreader.Read())
                {

                }
                mycon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }*/

        private void update_stock()
        {
            try
            {
                MySqlConnection mycon = new MySqlConnection(connections.connection_string);

                for (int row = 0; row < dataGridView1.Rows.Count; row++)
                {
                    string item_id = dataGridView1.Rows[row].Cells[0].Value.ToString();
                    float sto_qty = int.Parse(dataGridView1.Rows[row].Cells[4].Value.ToString());
                    string query2 = "update grn set  qty = qty - " + sto_qty + "  where Item_id ='" + item_id.ToString() + "' ";
                    MySqlCommand cmd = new MySqlCommand(query2, mycon);
                    mycon.Open();
                    cmd.ExecuteNonQuery();
                    mycon.Close();
                }


            }
            catch
            {

            }
        }
    }
}
