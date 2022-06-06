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
    public partial class Return_from_bill : Form
    {
        public Return_from_bill()
        {
            InitializeComponent();
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

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

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                if (string.IsNullOrEmpty(textBox1.Text))
                {
                    get_pname();
                }
                else
                {
                    get_pname();
                    get_price();
                }
            }
        }


        private void get_price()
        {
            try
            {
                MySqlConnection mycon = new MySqlConnection(connections.connection_string);
                MySqlCommand select = new MySqlCommand("select * from detialed_invoice where inv_id ='" + textBox1.Text + "' and item_id  = '" + this.textBox2.Text + "' ;", mycon);
                MySqlDataReader reader2;
                mycon.Open();
                reader2 = select.ExecuteReader();
                if (reader2.Read())
                {
                    string price;
                    price = reader2["price"].ToString();
                    textBox4.Text = price;

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

        decimal cost;
        private void get_pname()
        {
            try
            {
                MySqlConnection mycon = new MySqlConnection(connections.connection_string);
                MySqlCommand select = new MySqlCommand("select * from item where  Item_id = '" + this.textBox2.Text + "' ;", mycon);
                MySqlDataReader reader2;
                mycon.Open();
                reader2 = select.ExecuteReader();
                if (reader2.Read())
                {
                    string name;
                    name = reader2["Item_name"].ToString();
                    cost = decimal.Parse(reader2["cost"].ToString());
                    label3.Text = name;
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

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void add_to_datagrid()
        {
            try
            {
                string item_id = textBox2.Text;
                int qty = int.Parse(textBox3.Text);
                string name = label3.Text;
                decimal price = decimal.Parse(textBox4.Text);
                decimal tota_cost = cost * qty;
                decimal total = qty * price;
                this.dataGridView1.Rows.Add(item_id, qty, name, total, tota_cost);

                cal_total_return();
                cal_total_return_cost();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        decimal profit_loss = 0;
        decimal total_cost = 0;
        private void cal_total_return_cost()
        {
            try
            {
                decimal sum = 0;

                for (int row = 0; row < dataGridView1.Rows.Count; row++)
                {
                    sum = sum + Convert.ToInt32(dataGridView1.Rows[row].Cells[4].Value);
                }
                total_cost = sum;
                
            }
            catch
            {

            }
        }

        private void cal_total_return()
        {
            try
            {
                decimal sum = 0;

                for (int row = 0; row < dataGridView1.Rows.Count; row++)
                {
                    sum = sum + Convert.ToInt32(dataGridView1.Rows[row].Cells[3].Value);
                }
                label5.Text = sum.ToString();
            }
            catch
            {

            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                try
                {
                    add_to_datagrid();
                    textBox2.Clear();
                    textBox3.Clear();
                    label3.Text = "Item Name";
                    textBox4.Clear();
                    button2.Enabled = true;
                }
                catch
                {

                }
            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                reduce_from_cash_box();
                update_stock();
                clear_all();
                button2.Enabled = false;
            }
            else
            {
                decimal i = decimal.Parse(label5.Text);
                profit_loss = i - total_cost;
                Reduse_from_invoice();
                reduse_from_detialed_invoice();
                reduce_from_cash_box();
                update_stock();
                clear_all();
                button2.Enabled = false;
            }
        }

       /* private void find_imei()
        {
            try
            {
                MySqlConnection mycon = new MySqlConnection(connections.connection_string);

                for (int row = 0; row < dataGridView1.Rows.Count; row++)
                {
                    int i = int.Parse(dataGridView1.Rows[row].Cells[0].Value.ToString());
                    string imie = dataGridView1.Rows[row].Cells[4].Value.ToString();
                    if (string.IsNullOrEmpty(imie))
                    {

                    }
                    else
                    {
                        
                    }

                }


            }
            catch
            {

            }
        }*/

      /*  private void Update_imei(string imie)
        {
            string query = "update manage_imie set  status ='" + "In Stock" + "',invoice_no ='" +0+ "' where imei  ='" + imei + "';";
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
        }*/

        private void reduce_from_cash_box()
        {
            if(bal >= 0)
            {
                decimal redues_amount = 0;
                decimal amount = decimal.Parse(label5.Text);
                redues_amount = 0 - amount;
                string d = DateTime.Today.ToString("yyyy-MM-dd");
                string query = "insert into Cash_box(slip_id,type,amount,date) values ('" + textBox1.Text + "','" + "Return" + "','" + redues_amount + "','" + d + "') ;";
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

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            
        }

        private void clear_all()
        {
            textBox1.Clear();
            dataGridView1.Rows.Clear();
        }

        private void update_stock()
        {
            try
            {
                MySqlConnection mycon = new MySqlConnection(connections.connection_string);
                mycon.Open();
                for (int row = 0; row < dataGridView1.Rows.Count; row++)
                {
                    string item_id = dataGridView1.Rows[row].Cells[0].Value.ToString();
                    int sto_qty = int.Parse(dataGridView1.Rows[row].Cells[1].Value.ToString());
                    string query2 = "update grn set  Qty = Qty + " + sto_qty + "  where item_id ='" + item_id + "'";
                    MySqlCommand cmd = new MySqlCommand(query2, mycon);

                    cmd.ExecuteNonQuery();

                }
                mycon.Close();
            }
            catch
            {

            }
        }

        private void reduse_from_detialed_invoice()
        {
            try
            {
                MySqlConnection mycon = new MySqlConnection(connections.connection_string);
                mycon.Open();
                for (int row = 0; row < dataGridView1.Rows.Count; row++)
                {
                    string item_id = dataGridView1.Rows[row].Cells[0].Value.ToString();
                    float sto_qty = float.Parse(dataGridView1.Rows[row].Cells[1].Value.ToString());
                    string query2 = "update detialed_invoice set  qty = qty - " + sto_qty + "  where item_id ='" + item_id + "'  and  inv_id = '" + textBox1.Text + "' ";
                    MySqlCommand cmd = new MySqlCommand(query2, mycon);

                    cmd.ExecuteNonQuery();

                }
                mycon.Close();
            }
            catch
            {

            }
        }

        private void Reduse_from_invoice()
        {
            try
            {
                decimal totalreturn = decimal.Parse(label5.Text);
                MySqlConnection mycon = new MySqlConnection(connections.connection_string);
                mycon.Open();
                string sql1;
                sql1 = "update invoice set sub_total = sub_total - " + totalreturn + " , profit = profit -'" + profit_loss + "' , balance = balance + '"+totalreturn+"' where id  ='" + textBox1.Text + "' ";
                MySqlCommand cmd = new MySqlCommand(sql1, mycon);
                cmd.ExecuteNonQuery();
                mycon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void Return_from_bill_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                get_bal();
            }
        }

        decimal bal;
        private void get_bal()
        {
            try
            {
                MySqlConnection mycon = new MySqlConnection(connections.connection_string);
                MySqlCommand select = new MySqlCommand("select * from invoice where  id  = '" + this.textBox1.Text + "' ;", mycon);
                MySqlDataReader reader2;
                mycon.Open();
                reader2 = select.ExecuteReader();
                if (reader2.Read())
                {
                    bal = decimal.Parse(reader2["balance"].ToString());
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
    }
}
