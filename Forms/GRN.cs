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
    public partial class GRN : Form
    {
        public GRN()
        {
            InitializeComponent();
            load_datagrid();
        }
        private string type;

        public string Type
        {
            get { return type; }
            set { type = value; }
        }


        DataTable dataset;
        private void Button1_Click(object sender, EventArgs e)
        {
            save_stock();
            update_grn();
            clear_all();
            load_datagrid();
            ActiveControl = textBox1;
        }

        private void load_datagrid()
        {
            MySqlConnection mycon = new MySqlConnection(connections.connection_string);
            MySqlCommand cmd = new MySqlCommand("Select item.item_id , item.item_name , grn.qty from item inner join grn on item.item_id =grn.Item_id ;", mycon);

            try
            {
                MySqlDataAdapter sda = new MySqlDataAdapter();
                sda.SelectCommand = cmd;
                dataset = new DataTable();
                sda.Fill(dataset);
                BindingSource bsource = new BindingSource();
                bsource.DataSource = dataset;
                dataGridView1.DataSource = bsource;
                sda.Update(dataset);

                this.dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                this.dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                this.dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[0].HeaderText = "Item ID";
                dataGridView1.Columns[1].HeaderText = "Item Name";
                dataGridView1.Columns[2].HeaderText = "Qty";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void update_grn()
        {
            string d = DateTime.Today.ToString("yyyy-MM-dd");
            string query = "insert into g_r_n(Item_id,qty,date) values ('" + textBox1.Text + "','" + textBox4.Text + "','" + d + "') ;";
            MySqlConnection mycon = new MySqlConnection(connections.connection_string);
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
        }

        private void clear_all()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
        }

        private void save_stock()
        {
            try
            {
                MySqlConnection mycon = new MySqlConnection(connections.connection_string);
                MySqlCommand select = new MySqlCommand("select * from grn where Item_id = '" + this.textBox1.Text + "' ;", mycon);
                MySqlCommand insert_data2 = new MySqlCommand("insert into grn(Item_id,Qty) values ('" + textBox1.Text + "','" + textBox4.Text + "') ;", mycon);
                MySqlDataReader reader;
                int count = 0;
                mycon.Open();
                reader = select.ExecuteReader();
                while (reader.Read())
                {
                    count = count + 1;
                }
                mycon.Close();
                if (count == 1)
                {
                    try
                    {
                        mycon.Open();
                        int sum = int.Parse(textBox4.Text);
                        string sql1;
                        sql1 = "update grn set qty = qty + " + sum + "  where Item_id ='" + textBox1.Text + "' ";
                        MySqlCommand cmd = new MySqlCommand(sql1, mycon);
                        cmd.ExecuteNonQuery();
                        mycon.Close();
                        MessageBox.Show("Saved");

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
                else if (count == 0)
                {

                    try
                    {
                        mycon.Open();
                        reader = insert_data2.ExecuteReader();
                        MessageBox.Show("Saved");
                        while (reader.Read())
                        {

                        }
                        mycon.Close();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                get_item_data();
            }
        }
        private void get_item_data()
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
                    string supplier;
                    pname = reader2["Item_name"].ToString();
                    supplier = reader2["supplier"].ToString();
                    textBox2.Text = pname;
                    textBox3.Text = supplier;
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

        private void validate_qty()
        {
            if (string.IsNullOrEmpty(textBox4.Text))
            {
                MessageBox.Show("Enter Valid Qty");
            }
            else
            {
                button1.Enabled = true;
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                validate_qty();
            }
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                SendKeys.Send("{TAB}");
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataView Dv = new DataView(dataset);
                Dv.RowFilter = string.Format("Item_Name LIKE '%{0}%'", textBox5.Text);
                dataGridView1.DataSource = Dv;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GRN_Load(object sender, EventArgs e)
        {
            if(type == "admin")
            {
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                textBox3.Enabled = true;
                textBox4.Enabled = true;
            } else if (type == "Top lavel emp")
            {
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                textBox3.Enabled = true;
                textBox4.Enabled = true;
            }
        }
    }
}
