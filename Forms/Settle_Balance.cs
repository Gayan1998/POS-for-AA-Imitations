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
    public partial class Settle_Balance : Form
    {
        public Settle_Balance()
        {
            InitializeComponent();
        }

        private int cust;

        public int Cust
        {
            get { return cust; }
            set { cust = value; }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Select_cust se = new Select_cust();
            se.MdiParent = this.MdiParent;
            se.Val = 2;
            se.Show();
            this.Close();
        }
        DataTable dataset;

        private void Settle_Balance_Load(object sender, EventArgs e)
        {
            textBox6.Text = cust.ToString();
            load_cmb();
        }

        private void load_cmb()
        {
            string query = "select * from invoice where balance < 0 and cust_id ='"+textBox6.Text+"';";
            MySqlConnection mycon = new MySqlConnection(connections.connection_string);
            MySqlCommand cmd = new MySqlCommand(query, mycon);
            MySqlDataReader myreader;
            try
            {
                mycon.Open();
                myreader = cmd.ExecuteReader();
                while (myreader.Read())
                {
                    string id = myreader.GetString("id");
                    comboBox1.Items.Add(id);
                }
                mycon.Close();
            }
            catch
            {

            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            MySqlConnection mycon = new MySqlConnection(connections.connection_string);
            MySqlCommand cmd = new MySqlCommand("Select item.item_id , item.item_name , detialed_invoice.price , detialed_invoice.qty from item inner join detialed_invoice on item.item_id = detialed_invoice.item_id where detialed_invoice.inv_id = '" + comboBox1.Text + "' ;", mycon);

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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            get_bal();
        }

        private void get_bal()
        {
            try
            {
                MySqlConnection mycon = new MySqlConnection(connections.connection_string);
                MySqlCommand select = new MySqlCommand("select * from invoice where id  = '" + this.comboBox1.Text + "' ;", mycon);
                MySqlDataReader reader2;
                mycon.Open();
                reader2 = select.ExecuteReader();

                if (reader2.Read())
                {
                    string balance;
                    balance = reader2["balance"].ToString();
                    label4.Text = balance;
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

        private void button1_Click(object sender, EventArgs e)
        {
            string query = "update invoice set  pay = pay +'" + textBox1.Text + "',balance = balance + '" + textBox1.Text + "' where id  ='" + comboBox1.Text + "';";
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
            update_cash_box();
            clear_all();
        }

        private void update_cash_box()
        {
            string d = DateTime.Today.ToString("yyyy-MM-dd");
            string query = "insert into cash_box(slip_id,type,amount,date) values ('" + comboBox1.Text + "','" + "Balance Setele" + "','" + textBox1.Text + "','" + d + "') ;";
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

        private void clear_all()
        {
            textBox1.Clear();
        }

        private void label3_Click(object sender, EventArgs e)
        {


        }
    }
}
