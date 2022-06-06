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

namespace PRINT_SHOP
{
    public partial class Whole_Sales : Form
    {
        public static Whole_Sales instance;
        public TextBox tb_id;
        public TextBox cus_id;
        public TextBox rpid;
        public Whole_Sales()
        {
            InitializeComponent();
            instance = this;
            tb_id = textBox3;
            cus_id = textBox2;
        }
        private int cust;

        private string rid;
        public string Rid
        {
            get { return rid; }
            set { rid = value; }
        }


        public int Cust
        {
            get { return cust; }
            set { cust = value; }
        }
        private string emp;

        public string Emp
        {
            get { return emp; }
            set { emp = value; }
        }

        private void Retial_Sales_Load(object sender, EventArgs e)
        {
            LOad_table();
            textBox2.Text = cust.ToString();
            comboBox2.Items.Add("Retail");
            comboBox2.Items.Add("Wholesale");
            comboBox2.SelectedIndex = 0;
        }

        private void LOad_table()
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
  
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Select_cust se = new Select_cust();
            se.MdiParent = this.MdiParent;
            se.Val = 1;
            se.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox8.Text))
            {
                MessageBox.Show("Pay Cannot be empty");
            }
            else
            {
                    Save_invoice();
                    save_detialed_invoice();
                    save_cashbox();
                    update_stock();
                    MessageBox.Show("Done");
                    clear_all_2();
                    button2.Enabled = false;
                    button3.Enabled = true;
                    ActiveControl = textBox3;
                    comboBox2.SelectedIndex = 0;
            }
        }

        private void save_cashbox()
        {
            try
            {
                decimal i = decimal.Parse(textBox8.Text);
                if (i > 0)
                {
                    string d = DateTime.Today.ToString("yyyy-MM-dd");
                    string query = "insert into cash_box(slip_id,type,amount,date) values ('" + invoice_id + "','" + "Cash Sale" + "','" + label8.Text + "','" + d + "') ;";
                    MySqlConnection mycon = new MySqlConnection(connections.connection_string);
                    MySqlCommand cmd = new MySqlCommand(query, mycon);
                    MySqlDataReader myreader;
                    try
                    {
                        mycon.Open();
                        myreader = cmd.ExecuteReader();
                        mycon.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void clear_all_2()
        {
            dataGridView2.Rows.Clear();
            label8.Text = "0.00";
            label10.Text = "0.00";
            textBox8.Clear();
           
        }

        private void update_stock()
        {
            try
            {
                MySqlConnection mycon = new MySqlConnection(connections.connection_string);

                for (int row = 0; row < dataGridView2.Rows.Count; row++)
                {
                    string item_id = dataGridView2.Rows[row].Cells[0].Value.ToString();
                    float sto_qty = int.Parse(dataGridView2.Rows[row].Cells[4].Value.ToString());
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

        private void save_detialed_invoice()
        {
            try
            {
                string item_id;
                MySqlConnection mycon = new MySqlConnection(connections.connection_string);
                MySqlDataReader myreader;
                for (int row = 0; row < dataGridView2.Rows.Count; row++)
                {
                    string d = DateTime.Today.ToString("yyyy-MM-dd");
                    item_id = dataGridView2.Rows[row].Cells[0].Value.ToString();
                    decimal price = decimal.Parse(dataGridView2.Rows[row].Cells[2].Value.ToString());
                    int qty = int.Parse(dataGridView2.Rows[row].Cells[4].Value.ToString());
                    decimal dis = decimal.Parse(dataGridView2.Rows[row].Cells[3].Value.ToString());
                    string query2 = " insert into detialed_invoice (inv_id, item_id, price,discount,qty,date) values ('" + invoice_id + "','" + item_id + "', '" + price + "','"+dis+"','" + qty + "','" + d + "');";
                    MySqlCommand cmd2 = new MySqlCommand(query2, mycon);
                    mycon.Open();
                    myreader = cmd2.ExecuteReader();
                    mycon.Close();
                }
            }
            catch(Exception ex)
            {
                
            }
        }

        int invoice_id;
        private void Save_invoice()
        {
            string d = DateTime.Today.ToString("yyyy-MM-dd");
            string query = "insert into invoice(cust_id,sub_total,profit,pay,balance,date,emp) values ('" + textBox2.Text + "','" + label8.Text + "','"+ total_profit + "','" + textBox8.Text + "','"+label10.Text+"','" + d+ "','"+emp+"') ;";
            MySqlConnection mycon = new MySqlConnection(connections.connection_string);
            MySqlCommand cmd = new MySqlCommand(query, mycon);
            MySqlDataReader myreader;
            try
            {
                mycon.Open();
                myreader = cmd.ExecuteReader();
                invoice_id = (int)cmd.LastInsertedId;
                mycon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
                load_a4();
                button3.Enabled = false;
                ActiveControl = textBox3;
        }
        private void load_a4()
        {
             MySqlConnection mycon = new MySqlConnection(connections.connection_string);
              MySqlCommand cmd;
              MySqlCommand cmd2;
              MySqlCommand cmd3;
              MySqlCommand cmd4;
              MySqlDataAdapter dr;
              try
              {
                  mycon.Open();
                  DataTable dt = new DataTable();
                  cmd = new MySqlCommand("select * from item ", mycon);
                  dr = new MySqlDataAdapter(cmd);
                  dr.Fill(dt);
                  mycon.Close();

                  mycon.Open();
                  DataTable dt1 = new DataTable();
                  cmd2 = new MySqlCommand("select * from detialed_invoice where inv_id ='" + invoice_id.ToString() + "'", mycon);
                  dr = new MySqlDataAdapter(cmd2);
                  dr.Fill(dt1);
                  mycon.Close();

                  mycon.Open();
                  DataTable dt3 = new DataTable();
                  cmd3 = new MySqlCommand("select * from invoice where id  = '" + invoice_id.ToString() + "'", mycon);
                  dr = new MySqlDataAdapter(cmd3);
                  dr.Fill(dt3);
                  mycon.Close();

                  mycon.Open();
                  DataTable dt4 = new DataTable();
                  cmd4 = new MySqlCommand("select * from cust ", mycon);
                  dr = new MySqlDataAdapter(cmd4);
                  dr.Fill(dt4);
                  mycon.Close();

                  a4_bill cr2 = new a4_bill();
                  cr2.Database.Tables["item"].SetDataSource(dt);
                  cr2.Database.Tables["detialed_invoice"].SetDataSource(dt1);
                  cr2.Database.Tables["invoice"].SetDataSource(dt3);
                  cr2.Database.Tables["cust"].SetDataSource(dt4);
                  cr2.PrintToPrinter(1, false, 0, 0);
                  cr2.Close();
                  cr2.Dispose();

            }
              catch
              {

              }
            
        }


        private void textBox12_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox12.Text))
            {
                MessageBox.Show("Please Enter Valid Bill Number");
            }
            else
            {
                int inv_no = int.Parse(textBox12.Text);
                find_bill_v se = new find_bill_v();
                se.ID = inv_no;
                se.MdiParent = this.MdiParent;
                se.Show();
            }

        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (comboBox2.Text == "Retail")
                    {
                        get_retial_info();
                        get_qty();
                    }
                    else
                    {
                        get_item_info_wholesale();
                        get_qty();


                    }
                    SendKeys.Send("{TAB}");
                }
                else if(e.KeyCode == Keys.Space)
                {
                    textBox8.Select();
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        int mqty = 0;
        private void get_qty()
        {
            try
            {
                MySqlConnection mycon = new MySqlConnection(connections.connection_string);
                MySqlCommand select = new MySqlCommand("select * from grn where Item_id  = '" + this.textBox3.Text.Trim() + "' ;", mycon);
                MySqlDataReader reader2;
                mycon.Open();
                reader2 = select.ExecuteReader();

                if (reader2.Read())
                {
                    mqty = int.Parse(reader2["Qty"].ToString());
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

    

        private void get_retial_info()
        {
            try
            {
                MySqlConnection mycon = new MySqlConnection(connections.connection_string);
                MySqlCommand select = new MySqlCommand("select * from item where Item_id  = '" + this.textBox3.Text.Trim() + "' ;", mycon);
                MySqlDataReader reader2;
                mycon.Open();
                reader2 = select.ExecuteReader();

                if (reader2.Read())
                {
                    string pname;
                    string price;
                    string warrenty;
                    pname = reader2["Item_name"].ToString();
                    textBox4.Text = pname;

                    price = reader2["rate"].ToString();
                    textBox5.Text = price;


                    textBox6.Text = "0";

                    cost = decimal.Parse(reader2["Cost"].ToString());
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
        private void get_item_info_wholesale()
        {
            try
            {
                MySqlConnection mycon = new MySqlConnection(connections.connection_string);
                MySqlCommand select = new MySqlCommand("select * from item where Item_id  = '" + this.textBox3.Text + "' ;", mycon);
                MySqlDataReader reader2;
                mycon.Open();
                reader2 = select.ExecuteReader();

                if (reader2.Read())
                {
                    string pname;
                    string price;
                    string warrenty;
                    pname = reader2["Item_name"].ToString();
                    textBox4.Text = pname;

                    price = reader2["wholesale"].ToString();
                    textBox5.Text = price;

                    textBox6.Text = "0";

                    cost = decimal.Parse(reader2["Cost"].ToString());
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

        private void textBox6_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }


        private void textBox7_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void cal_sub_total()
        {
            float sum = 0;

            for (int row = 0; row < dataGridView2.Rows.Count; row++)
            {
                sum = sum + Convert.ToInt32(dataGridView2.Rows[row].Cells[5].Value);
            }
            label8.Text = sum.ToString();
        }

        private void add_to_datagrid()
        {
            try
            {
                string id = textBox3.Text;
                string name = textBox4.Text;
                decimal price = decimal.Parse(textBox5.Text);
                int qty = int.Parse(textBox7.Text);
                decimal dis = decimal.Parse(textBox6.Text);

                this.dataGridView2.Rows.Add(id, name, price, dis, qty, total, profit);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        decimal profit;
        decimal total;
        decimal total_cost;
        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
           if(e.KeyChar == 13)
            {
                if (string.IsNullOrEmpty(textBox7.Text))
                {
                    MessageBox.Show("Qty Cant Be Empty !");
                }
                else
                {
                    decimal pr = decimal.Parse(textBox5.Text);
                    if (pr>= min_r)
                    {
                        decimal price = decimal.Parse(textBox5.Text);
                        int qty = int.Parse(textBox7.Text);
                        total = (price * qty);
                        total_cost = cost * qty;
                        profit = total - total_cost;
                        if (qty <= mqty)
                        {
                            add_to_datagrid();
                            cal_sub_total();
                            calTotDiscount();
                            cal_profit();
                            clear_();
                        }
                        else
                        {
                            MessageBox.Show("Invalid qty");
                        }
                    }
                    else
                    {
                        MessageBox.Show("price Is Too Low");
                    }
                }
            }
        }

        private void calTotDiscount()
        {
            decimal sum = 0;

            for (int row = 0; row < dataGridView2.Rows.Count; row++)
            {
                sum = sum + Convert.ToInt32(dataGridView2.Rows[row].Cells[3].Value);
            }
            label14.Text = sum.ToString();
        }

        decimal total_profit;
        private void cal_profit()
        {
            decimal sum = 0;

            for (int row = 0; row < dataGridView2.Rows.Count; row++)
            {
                sum = sum + Convert.ToInt32(dataGridView2.Rows[row].Cells[6].Value);
            }
            total_profit = sum;
            
        }

        private void clear_()
        {
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();

        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                decimal price = decimal.Parse(textBox5.Text);
                decimal dis = decimal.Parse(textBox6.Text);
                decimal new_price = price - dis;
                textBox5.Text = new_price.ToString();
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView2.Columns["Delete"].Index && e.RowIndex >= 0)
            {
                dataGridView2.Rows.Remove(dataGridView2.Rows[e.RowIndex]);
                cal_sub_total();
                cal_profit();
                calTotDiscount();
            }
        }

        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                try
                {
                    decimal sub = decimal.Parse(label8.Text);
                    decimal pay = decimal.Parse(textBox8.Text);
                    decimal bal = pay - sub;
                    label10.Text = bal.ToString();
                    button2.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox2.SelectedIndex == 1)
            {
                label12.Text = "Wholesale";
            }
            else
            {
                label12.Text = "Retail";
            }
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
           if(e.KeyChar == 13)
            {
               if (comboBox2.Text == "Retail")
                {
                    get_retial_price();
                }
                else
                {
                    get_wholesale_price();
                }
                decimal dis;
                decimal p = decimal.Parse(textBox5.Text);
                dis = r-p;
                textBox6.Text = dis.ToString();
                ActiveControl = textBox7;
            }
        }

        decimal min_r;
        decimal r;
        private void get_retial_price()
        {
            try
            {
                MySqlConnection mycon = new MySqlConnection(connections.connection_string);
                MySqlCommand select = new MySqlCommand("select * from item where Item_id  = '" + this.textBox3.Text + "' ;", mycon);
                MySqlDataReader reader2;
                mycon.Open();
                reader2 = select.ExecuteReader();

                if (reader2.Read())
                {

                    r = decimal.Parse(reader2["rate"].ToString());
                   
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


        private void get_wholesale_price()
        {
            try
            {
                MySqlConnection mycon = new MySqlConnection(connections.connection_string);
                MySqlCommand select = new MySqlCommand("select * from item where Item_id  = '" + this.textBox3.Text + "' ;", mycon);
                MySqlDataReader reader2;
                mycon.Open();
                reader2 = select.ExecuteReader();

                if (reader2.Read())
                {

                    r = decimal.Parse(reader2["wholesale"].ToString());
                    
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

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            select_id se = new select_id();
            se.MdiParent = this.MdiParent;
            se.Show();
            ActiveControl = textBox3;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                try
                {
                    MySqlConnection mycon = new MySqlConnection(connections.connection_string);
                    MySqlCommand select = new MySqlCommand("select * from cust where id  = '" + this.textBox2.Text + "' ;", mycon);
                    MySqlDataReader reader2;
                    mycon.Open();
                    reader2 = select.ExecuteReader();

                    if (reader2.Read())
                    {

                        textBox1.Text = reader2["name"].ToString();
                        textBox9.Text = reader2["shop_name"].ToString();
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

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }
    }
}
