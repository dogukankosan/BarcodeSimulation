using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BarcodControl
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        readonly string cs = "DATA SOURCE=DOGUKAN\\SQLSERVER;INITIAL CATALOG=PRODUCTION;UID=sa;PWD=Etasql1";
        readonly SqlConnection conn = new SqlConnection();
        readonly SqlCommand cmd = new SqlCommand();
        readonly SqlDataAdapter da = new SqlDataAdapter();
        readonly DataSet ds = new DataSet();
        private bool down = false;
        private int x, y;
        void F2Click()
        {
            conn.ConnectionString = cs;
            string sql = "SELECT * FROM PRODUCTION.DBO.BARCODES WHERE BARCODE='" + txtBarcode.Text + "'";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql;
            da.SelectCommand = cmd;
            ds.Clear();
            ds.Reset();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count == 0)
            {
                lblMsg.Text = "Böyle bir barkod bulunamadı !";
                return;
            }
            sql = "SELECT * FROM PRODUCTION.DBO.BARCODE_PRODUCTION WHERE BARCODE='" + txtBarcode.Text + "'";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql;
            da.SelectCommand = cmd;
            ds.Clear();
            ds.Reset();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                lblMsg.Text = "Bu barkod daha önceden okutulmuş !";
                return;
            }
            sql = "SELECT * FROM PRODUCTION.DBO.PRODPLAN WHERE ITEMID IN (SELECT ITEMID FROM BARCODES WHERE BARCODE='" + txtBarcode.Text + "')";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql;
            da.SelectCommand = cmd;
            ds.Clear();
            ds.Reset();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count == 0)
            {
                lblMsg.Text = "Bu ürüne ait üretim planı bulunamadı !";
                return;
            }
            sql = "INSERT INTO PRODUCTION.DBO.BARCODE_PRODUCTION (BARCODE,DATE_,STATION) VALUES ('" + txtBarcode.Text + "',GETDATE(),1)";
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql;
            da.SelectCommand = cmd;
            ds.Clear();
            ds.Reset();
            da.Fill(ds);
            lblMsg.Text = "Barkod başarıyla eklendi.";
            txtBarcode.Text = "";
            txtBarcode.Focus();
        }
        void F3Click()
        {
            try
            {
                conn.ConnectionString = cs;
                string sql = "EXEC BARCODE_INSERT @BARCODE='" + txtBarcode.Text + "',@STATIONID=1";
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                da.SelectCommand = cmd;
                ds.Clear();
                ds.Reset();
                da.Fill(ds);
                lblMsg.Text = "Barkod başarıyla eklendi.";
                txtBarcode.Text = "";
                txtBarcode.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Bilgi Mesajı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            F2Click();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            F3Click();
        }
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (down)
            {
                this.SetDesktopLocation(MousePosition.X - x, MousePosition.Y - y);
            }
        }
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            down = false;
        }
        private void pictureBox2_Click_1(object sender, EventArgs e)
        {
            F2Click();
        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            F3Click();
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
                F2Click();
            else if (e.KeyCode == Keys.F3)
                F3Click();
            else if (e.KeyCode==Keys.Escape)
            Application.Exit();
        }
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            down = true;
            x = e.X;
            y = e.Y;
        }
    }
}