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
    public partial class select_id : Form
    {
        public static select_id instance;
        public select_id()
        {
            InitializeComponent();
            instance = this; 
        }
        DataTable dataset;
        private void select_id_Load(object sender, EventArgs e)
        {
            MySqlConnection mycon = new MySqlConnection(connections.connection_string);
            MySqlCommand cmd = new MySqlCommand("Select Item_id , Item_name from item ORDER BY item_id  DESC;", mycon);

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
                dataGridView1.Columns[0].HeaderText = "Item ID";
                dataGridView1.Columns[1].HeaderText = "Item Name";
            }
            catch
            {

            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataView Dv = new DataView(dataset);
                Dv.RowFilter = string.Format("Item_Name LIKE '%{0}%'", textBox1.Text);
                dataGridView1.DataSource = Dv;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
            int item_id = int.Parse(row.Cells["Item_id"].Value.ToString());
            Whole_Sales.instance.tb_id.Text = item_id.ToString();
            this.Close();
        }
    }
}
