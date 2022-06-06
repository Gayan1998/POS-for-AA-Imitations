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
    public partial class find_bill_v : Form
    {
        public find_bill_v()
        {
            InitializeComponent();
        }
        private int type;

        public int Type
        {
            get { return type; }
            set { type = value; }
        }

        private int id;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private void find_bill_v_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
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
                cmd2 = new MySqlCommand("select * from detialed_invoice where inv_id ='" + id.ToString() + "'", mycon);
                dr = new MySqlDataAdapter(cmd2);
                dr.Fill(dt1);
                mycon.Close();

                mycon.Open();
                DataTable dt3 = new DataTable();
                cmd3 = new MySqlCommand("select * from invoice where id  = '" + id.ToString() + "'", mycon);
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
                this.crystalReportViewer1.ReportSource = cr2;
            }
            catch
            {

            }
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
