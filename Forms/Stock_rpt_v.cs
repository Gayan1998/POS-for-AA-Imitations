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
    public partial class Stock_rpt_v : Form
    {
        public Stock_rpt_v()
        {
            InitializeComponent();
        }
        private int val;

        public int Val
        {
            get { return val; }
            set { val = value; }
        }

        private void Stock_rpt_v_Load(object sender, EventArgs e)
        {
            if(val == 1)
            {
                load_sock();
            }
            else if (val == 2)
            {
                load_sog();
            }
            else if (val == 3)
            {
                Load_sales();
            }
            else if (val == 4)
            {
                load_profit();
            }
            else if(val == 5)
            {
                load_cashbox();
            }
            else if(val == 6)
            {
                load_outstanding_payment();
            }
        }

   

        private void load_outstanding_payment()
        {
            this.WindowState = FormWindowState.Maximized;
            MySqlConnection mycon = new MySqlConnection(connections.connection_string);
            MySqlCommand cmd;
            MySqlCommand cmd2;
            MySqlDataAdapter dr;
            try
            {
                mycon.Open();
                DataTable dt = new DataTable();
                cmd = new MySqlCommand("select * from cust ", mycon);
                dr = new MySqlDataAdapter(cmd);
                dr.Fill(dt);
                mycon.Close();

                mycon.Open();
                DataTable dt1 = new DataTable();
                cmd2 = new MySqlCommand("select * from invoice where balance < 0 ", mycon);
                dr = new MySqlDataAdapter(cmd2);
                dr.Fill(dt1);
                mycon.Close();

        


                outsanding_payments_rpt cr2 = new outsanding_payments_rpt();
                cr2.Database.Tables["cust"].SetDataSource(dt);
                cr2.Database.Tables["invoice"].SetDataSource(dt1);
                this.crystalReportViewer1.ReportSource = cr2;
            }
            catch
            {

            }
        }

        private void load_cashbox()
        {
            this.WindowState = FormWindowState.Maximized;
            MySqlConnection mycon = new MySqlConnection(connections.connection_string);
            MySqlCommand cmd;
            MySqlDataAdapter dr;
            try
            {
                mycon.Open();
                DataTable dt = new DataTable();
                cmd = new MySqlCommand("select * from cash_box ", mycon);
                dr = new MySqlDataAdapter(cmd);
                dr.Fill(dt);
                mycon.Close();

          


                Cash_box_rpt cr2 = new Cash_box_rpt();
                cr2.Database.Tables["Cash_box"].SetDataSource(dt);
                this.crystalReportViewer1.ReportSource = cr2;
            }
            catch
            {

            }
        }

        private void load_profit()
        {
            this.WindowState = FormWindowState.Maximized;
            MySqlConnection mycon = new MySqlConnection(connections.connection_string);
            MySqlCommand cmd;
            MySqlCommand cmd2;
            MySqlCommand cmd3;
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
                cmd2 = new MySqlCommand("select * from detialed_invoice ", mycon);
                dr = new MySqlDataAdapter(cmd2);
                dr.Fill(dt1);
                mycon.Close();

                mycon.Open();
                DataTable dt3 = new DataTable();
                cmd3 = new MySqlCommand("select * from invoice ", mycon);
                dr = new MySqlDataAdapter(cmd3);
                dr.Fill(dt3);
                mycon.Close();


                Profit_rpt cr2 = new Profit_rpt();
                cr2.Database.Tables["item"].SetDataSource(dt);
                cr2.Database.Tables["detialed_invoice"].SetDataSource(dt1);
                cr2.Database.Tables["invoice"].SetDataSource(dt3);
                this.crystalReportViewer1.ReportSource = cr2;
            }
            catch
            {

            }
        }

        private void Load_sales()
        {
            this.WindowState = FormWindowState.Maximized;
            MySqlConnection mycon = new MySqlConnection(connections.connection_string);
            MySqlCommand cmd;
            MySqlCommand cmd2;
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
                cmd2 = new MySqlCommand("select * from detialed_invoice ", mycon);
                dr = new MySqlDataAdapter(cmd2);
                dr.Fill(dt1);
                mycon.Close();

                sales_rpt cr2 = new sales_rpt();
                cr2.Database.Tables["item"].SetDataSource(dt);
                cr2.Database.Tables["detialed_invoice"].SetDataSource(dt1);
                this.crystalReportViewer1.ReportSource = cr2;
            }
            catch
            {

            }
        }

        int min = 0;
        private void load_sog()
        {
            this.WindowState = FormWindowState.Maximized;
            MySqlConnection mycon = new MySqlConnection(connections.connection_string);
            MySqlCommand cmd;
            MySqlCommand cmd2;
            MySqlDataAdapter dr;
            try
            {
                get_min_qty();
                mycon.Open();
                DataTable dt = new DataTable();
                cmd = new MySqlCommand("select item.item_id , item.item_name ,grn.qty from item inner join grn on item.item_id = grn.Item_id where grn.qty < item.min_qty ;", mycon);
                dr = new MySqlDataAdapter(cmd);
                dr.Fill(dt);

                DataTable dt1 = new DataTable();
                cmd2 = new MySqlCommand("select * from item", mycon);
                dr = new MySqlDataAdapter(cmd2);
                dr.Fill(dt1);
                mycon.Close();

                sortageofgoods cr1 = new sortageofgoods();
                cr1.Database.Tables["item"].SetDataSource(dt1);
                cr1.Database.Tables["grn"].SetDataSource(dt);
                this.crystalReportViewer1.ReportSource = cr1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void get_min_qty()
        {
            try
            {
                MySqlConnection mycon = new MySqlConnection(connections.connection_string);
                MySqlCommand select = new MySqlCommand("select * from item  ;", mycon);
                MySqlDataReader reader2;
                mycon.Open();
                reader2 = select.ExecuteReader();
                if (reader2.Read())
                {
                    min = int.Parse(reader2["min_qty"].ToString());
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

        private void load_sock()
        {
            this.WindowState = FormWindowState.Maximized;
            MySqlConnection mycon = new MySqlConnection(connections.connection_string);
            MySqlCommand cmd;
            MySqlCommand cmd2;
            MySqlDataAdapter dr;
            try
            {
                mycon.Open();
                DataTable dt = new DataTable();
                cmd = new MySqlCommand("select * from grn", mycon);
                dr = new MySqlDataAdapter(cmd);
                dr.Fill(dt);

                DataTable dt1 = new DataTable();
                cmd2 = new MySqlCommand("select * from item ", mycon);
                dr = new MySqlDataAdapter(cmd2);
                dr.Fill(dt1);
                mycon.Close();

                stock_report cr1 = new stock_report();
                cr1.Database.Tables["item"].SetDataSource(dt1);
                cr1.Database.Tables["grn"].SetDataSource(dt);
                this.crystalReportViewer1.ReportSource = cr1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
