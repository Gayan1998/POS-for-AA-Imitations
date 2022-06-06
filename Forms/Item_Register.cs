using MySql.Data.MySqlClient;
using PRINT_SHOP.repot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PRINT_SHOP.ds;

namespace PRINT_SHOP
{
    public partial class Item_Register : Form
    {
        public Item_Register()
        {
            InitializeComponent();
        }

        private void Item_Register_Load(object sender, EventArgs e)
        {
            load_datagrid();
            load_cmb();
            this.dataGridView1.VirtualMode = Enabled;
        }

        private void load_cmb()
        {
            string query = "select * from supplier;";
            MySqlConnection mycon = new MySqlConnection(connections.connection_string);
            MySqlCommand cmd = new MySqlCommand(query, mycon);
            MySqlDataReader myreader;
            try
            {
                mycon.Open();
                myreader = cmd.ExecuteReader();
                while (myreader.Read())
                {
                    string emp_name = myreader.GetString("name");
                    comboBox1.Items.Add(emp_name);
                }
                mycon.Close();
            }
            catch
            {

            }
        }

        DataTable dataset;
        private void load_datagrid()
        {
            MySqlConnection mycon = new MySqlConnection(connections.connection_string);
            MySqlCommand cmd = new MySqlCommand("Select * from item ORDER BY item_id  DESC;", mycon);

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
                this.dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                this.dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                this.dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                this.dataGridView1.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                this.dataGridView1.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[0].HeaderText = "Item ID";
                dataGridView1.Columns[1].HeaderText = "Item Name";
                dataGridView1.Columns[2].HeaderText = "Cost";
                dataGridView1.Columns[3].HeaderText = "Rate";
                dataGridView1.Columns[4].HeaderText = "Wholesale price";
                dataGridView1.Columns[5].HeaderText = "Type";
                dataGridView1.Columns[6].HeaderText = "Supplier";
                dataGridView1.Columns[7].HeaderText = "Code";
            }
            catch 
            {
                
            }
        }

        int update_id;
        int i_id;
        private void button1_Click(object sender, EventArgs e)
        {
             if(string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text)|| string.IsNullOrEmpty(textBox4.Text)|| string.IsNullOrEmpty(comboBox1.Text))
            {
                MessageBox.Show("Please Fill All The Data");
            }
            else
            {
                string query = "insert into item(Item_name,cost,rate,type,code,wholesale,supplier) values (@item_name,@cost,@rate,@type,@code,@wholesale,@supplier)";
                MySqlConnection mycon = new MySqlConnection(connections.connection_string);
                MySqlCommand cmd = new MySqlCommand(query, mycon);
                cmd.Parameters.AddWithValue("@item_name", textBox2.Text);
                cmd.Parameters.AddWithValue("@cost", textBox4.Text);
                cmd.Parameters.AddWithValue("@rate", textBox3.Text);
                cmd.Parameters.AddWithValue("@type", comboBox2.Text);
                cmd.Parameters.AddWithValue("@code", textBox11.Text);
                cmd.Parameters.AddWithValue("@wholesale", textBox7.Text);
                cmd.Parameters.AddWithValue("@supplier", comboBox1.Text);
              
                MySqlDataReader myreader;
                try
                {
                    mycon.Open();
                    myreader = cmd.ExecuteReader();
                    i_id = (int)cmd.LastInsertedId;
                    MessageBox.Show("Saved");
                    while (myreader.Read())
                    {

                    }
                    mycon.Close();
                    add_stock();
                    update_grn();
                    clear_all();
                    load_datagrid();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void update_grn()
        {
            if (string.IsNullOrEmpty(textBox6.Text))
            {

            }
            else
            {
                string d = DateTime.Today.ToString("yyyy-MM-dd");
                string query = "insert into g_r_n(Item_id,qty,date) values ('" + i_id + "','" + textBox6.Text + "','" + d + "') ;";
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
        }

        private void add_stock()
        {
            try
            {
                if (string.IsNullOrEmpty(textBox6.Text))
                {

                }
                else
                {
                    
                    string query = "insert into grn(Item_id ,qty) values ('" + i_id + "','" + textBox6.Text + "') ;";
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
            }
            catch
            {

            }
        }

        private void clear_all()
        {
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox7.Clear();
            textBox11.Clear();
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

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                    update_id = int.Parse(row.Cells["Item_id"].Value.ToString());
                    textBox2.Text = row.Cells["Item_name"].Value.ToString();
                    textBox3.Text = row.Cells["rate"].Value.ToString();
                    textBox4.Text = row.Cells["cost"].Value.ToString();
                    textBox7.Text = row.Cells["wholesale"].Value.ToString();
                    comboBox1.Text = row.Cells["supplier"].Value.ToString();
                    comboBox2.Text = row.Cells["type"].Value.ToString();
                    textBox11.Text = row.Cells["code"].Value.ToString();
                    button2.Enabled = true;
                    button3.Enabled = true;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string query = "update item set  Item_name ='" + textBox2.Text + "',cost ='" + textBox4.Text + "',rate ='" + textBox3.Text + "',wholesale = '"+textBox7.Text+"',supplier ='" + comboBox1.Text + "', code = '"+textBox11.Text+ "',type = '"+comboBox2.Text+ "'  where Item_id  ='" + update_id + "';";
            MySqlConnection mycon = new MySqlConnection(connections.connection_string);
            MySqlCommand cmd = new MySqlCommand(query, mycon);
            MySqlDataReader myreader;
            try
            {
                mycon.Open();
                myreader = cmd.ExecuteReader();
                MessageBox.Show("Saved");
                while (myreader.Read())
                {

                }
                mycon.Close();
                load_datagrid();
                clear_all();
                button2.Enabled = false;
                button3.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Delete", "Do you want to remove this Item ?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                delete_item();
                delete_stock();
                load_datagrid();
                clear_all();
                MessageBox.Show("Deleted");
                button3.Enabled = false;
                button2.Enabled = false;
            }
        }

        private void delete_stock()
        {
            string query = "delete from grn where  Item_id  ='" + update_id + "' ;";
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

        private void delete_item()
        {
            string query = "delete from item where  Item_id  ='" + update_id + "' ;";
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

        private void button4_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox8.Text))
            {
                MessageBox.Show("Please Enter Valid Number of Copies");
            }
            else
            {
                Load_barcode();
                clear_all();
                
            }
        }

        private void Load_barcode()
        {
            int copies = int.Parse(textBox8.Text);
            MySqlConnection mycon = new MySqlConnection(connections.connection_string);
            MySqlCommand cmd;
            MySqlDataAdapter dr;
            try
            {
                mycon.Open();
                DataTable dt = new DataTable();
                cmd = new MySqlCommand("select * from item where Item_id  = '"+update_id+"' ", mycon);
                dr = new MySqlDataAdapter(cmd);
                dr.Fill(dt);
                mycon.Close();

                    barcode cr2 = new barcode();
                    cr2.Database.Tables["item"].SetDataSource(dt);
                    cr2.PrintToPrinter(copies, false, 0, 0);
                    cr2.Close();
                    cr2.Dispose();

            }
            catch
            {

            }
            ActiveControl = textBox2;
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataView Dv = new DataView(dataset);
                Dv.RowFilter = string.Format("Item_Name LIKE '%{0}%'", textBox9.Text);
                dataGridView1.DataSource = Dv;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBox10_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
          
        }

        private void comboBox3_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            clear_all();
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            MySqlConnection mycon = new MySqlConnection(connections.connection_string);
            MySqlCommand cmd = new MySqlCommand("Select * from item ;", mycon);

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
                this.dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                this.dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                this.dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                this.dataGridView1.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                this.dataGridView1.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                this.dataGridView1.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                this.dataGridView1.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                this.dataGridView1.Columns[10].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                this.dataGridView1.Columns[11].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
        }
        int val = 0;
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            val = 1;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            val = 2;
        }

        private void textBox11_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void textBox5_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void textBox12_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void textBox1_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
