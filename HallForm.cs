using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kino
{
    public partial class HallForm : Form
    {
        SqlConnection cn;
        SqlDataAdapter da;
        DataSet ds;

        public HallForm()
        {
            InitializeComponent();
            Init();
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            label5.MouseEnter += labelMouseEnter;
            label5.MouseLeave += labelMouseLeave;
            label5.Click += label5_Click;
        }

        void label5_Click(object sender, EventArgs e)
        {
            this.Close();            
        }

        void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            pictureBox1.ImageLocation = null;
            cn.Open();
            ds = new DataSet();
            da = new SqlDataAdapter();
            da.SelectCommand = cn.CreateCommand();
            da.SelectCommand.CommandText = @"select * from halls where hall_name='" + listBox1.Text + "'";
            da.SelectCommand.ExecuteNonQuery();
            da.Fill(ds, "halls");
            cn.Close();
            if (ds.Tables["Halls"].Rows.Count != 0)
            {
                pictureBox1.ImageLocation = MainForm.dbFolder + "Image\\hall\\" + ds.Tables["Halls"].Rows[0].ItemArray[5].ToString();
                textBox1.Text = ds.Tables["Halls"].Rows[0].ItemArray[4].ToString();
                label1.Text = ds.Tables["Halls"].Rows[0].ItemArray[2].ToString();
                label3.Text = ds.Tables["Halls"].Rows[0].ItemArray[3].ToString();
            }
        }

        void Init()
        {
            cn = new SqlConnection(MainForm.connectionString);

            cn.Open();
            ds = new DataSet();
            da = new SqlDataAdapter();
            da.SelectCommand = cn.CreateCommand();
            da.SelectCommand.CommandText = @"select * from halls";
            da.SelectCommand.ExecuteNonQuery();
            da.Fill(ds, "halls");
            cn.Close();
            listBox1.DataSource = ds.Tables["Halls"];
            listBox1.DisplayMember = "hall_name";

            cn.Open();
            ds = new DataSet();
            da = new SqlDataAdapter();
            da.SelectCommand = cn.CreateCommand();
            da.SelectCommand.CommandText = @"select * from halls where hall_name='" + listBox1.Text + "'";
            da.SelectCommand.ExecuteNonQuery();
            da.Fill(ds, "halls");
            cn.Close();
            pictureBox1.ImageLocation = MainForm.dbFolder + "Image\\hall\\" + ds.Tables["Halls"].Rows[0].ItemArray[5].ToString();
            textBox1.Text = ds.Tables["Halls"].Rows[0].ItemArray[4].ToString();
            label1.Text = ds.Tables["Halls"].Rows[0].ItemArray[2].ToString();
            label3.Text = ds.Tables["Halls"].Rows[0].ItemArray[3].ToString();
        }

        void labelMouseLeave(object sender, EventArgs e)
        {
            ((Label)sender).ForeColor = MainForm.colorLeave;
        }

        void labelMouseEnter(object sender, EventArgs e)
        {
            ((Label)sender).ForeColor = MainForm.colorEnter;
        }
    }
}
