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
    public partial class add_supplier : Form
    {
        public add_supplier()
        {
            InitializeComponent();
            load_table();
        }

        DataTable dataset;
        private void load_table()
        {
            MySqlConnection mycon = new MySqlConnection(connections.connection_string);
            MySqlCommand cmd = new MySqlCommand("Select * from supplier;", mycon);

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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Please Fill All The Data");
            }
            else
            {

                string query = "insert into supplier(name) values ('" + textBox1.Text + "') ;";
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
                    load_table();
                    textBox1.Clear();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        int sup_id;
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                    sup_id = int.Parse(row.Cells["id"].Value.ToString());
                    textBox1.Text = row.Cells["name"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Please select A Supplier");
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Delete", "Do you want to remove this Item ?", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {

                    string query = "delete from supplier where  id  ='" + sup_id + "' ;";
                    MySqlConnection mycon = new MySqlConnection(connections.connection_string);
                    MySqlCommand cmd = new MySqlCommand(query, mycon);
                    MySqlDataReader myreader;
                    try
                    {
                        mycon.Open();
                        myreader = cmd.ExecuteReader();
                        MessageBox.Show("Deleted");
                        while (myreader.Read())
                        {

                        }
                        mycon.Close();
                        textBox1.Clear();
                        load_table();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
    }
}
