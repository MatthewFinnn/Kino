using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kino
{
    public partial class NewFilmsForm : Form
    {
        SqlConnection cn;
        SqlDataAdapter da;
        DataSet ds;
        Timer tm;

        public NewFilmsForm()
        {
            InitializeComponent();
            label5.MouseEnter += labelMouseEnter;
            label5.MouseLeave += labelMouseLeave;
            label7.MouseEnter += labelMouseEnter;
            label7.MouseLeave += labelMouseLeave;
            label7.Click += label7_Click;
            label5.Click += label5_Click;
            dateTimePicker1.ValueChanged += dateTimePicker1_ValueChanged;
        }

        void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker2.MinDate = dateTimePicker1.Value;
        }

        void label7_Click(object sender, EventArgs e)
        {
            bool bln = true;
            if (textBox1.Text=="")
            {
                bln = false;
                textBox1.BackColor = Color.Red;
                tm.Start();
                tm.Tick += tm_Tick;
            }
            else if (maskedTextBox2.Text == "")
            {
                bln = false;
                maskedTextBox2.BackColor = Color.Red;
                tm.Start();
                tm.Tick += tm_Tick;
            }
            else if (textBox3.Text == "")
            {
                bln = false;
                textBox3.BackColor = Color.Red;
                tm.Start();
                tm.Tick += tm_Tick;
            }
            else if (textBox4.Text == "")
            {
                bln = false;
                textBox4.BackColor = Color.Red;
                tm.Start();
                tm.Tick += tm_Tick;
            }
            else if (textBox5.Text == "")
            {
                bln = false;
                textBox5.BackColor = Color.Red;
                tm.Start();
                tm.Tick += tm_Tick;
            }
            else if (textBox6.Text == "")
            {
                bln = false;
                textBox6.BackColor = Color.Red;
                tm.Start();
                tm.Tick += tm_Tick;
            }
            else if (textBox7.Text == "")
            {
                bln = false;
                textBox7.BackColor = Color.Red;
                tm.Start();
                tm.Tick += tm_Tick;
            }

            if (bln)
            {

                cn.Open();
                da.InsertCommand = cn.CreateCommand();
                da.InsertCommand.CommandText = @"INSERT INTO Films
VALUES ('" + textBox1.Text + "'," + maskedTextBox2.Text + ",'" + dateTimePicker1.Value + "','" + dateTimePicker2.Value + "', '" + textBox3.Text + "','" + textBox4.Text + "', '" + dimensionalCheckBox.Checked + "','" + textBox8.Text + "', '" + textBox5.Text + "', '" + textBox6.Text + "',  '" + textBox7.Text + "')";
                da.InsertCommand.ExecuteNonQuery();
                cn.Close();
                this.Close();
            }
        }

        void tm_Tick(object sender, EventArgs e)
        {
            if (textBox1.BackColor == Color.Red)
            {
                textBox1.BackColor = Color.White;
                textBox1.Focus();
                textBox1.SelectAll();
            }
            else if (maskedTextBox2.BackColor == Color.Red)
            {
                maskedTextBox2.BackColor = Color.White;
                maskedTextBox2.Focus();
                maskedTextBox2.SelectAll();
            }
            else if (textBox3.BackColor == Color.Red)
            {
                textBox3.BackColor = Color.White;
                textBox3.Focus();
                textBox3.SelectAll();
            }
            else if (textBox4.BackColor == Color.Red)
            {
                textBox4.BackColor = Color.White;
                textBox4.Focus();
                textBox4.SelectAll();
            }
            else if (textBox5.BackColor == Color.Red)
            {
                textBox5.BackColor = Color.White;
                textBox5.Focus();
                textBox5.SelectAll();
            }
            else if (textBox6.BackColor == Color.Red)
            {
                textBox6.BackColor = Color.White;
                textBox6.Focus();
                textBox6.SelectAll();
            }
            else if (textBox7.BackColor == Color.Red)
            {
                textBox7.BackColor = Color.White;
                textBox7.Focus();
                textBox7.SelectAll();
            }
            tm.Stop();
        }

        void label5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FilmsRedaktForm_Load(object sender, EventArgs e)
        {
            tm = new Timer();
            tm.Interval = 1000;

            cn = new SqlConnection(MainForm.connectionString);
            ds = new DataSet();
            da = new SqlDataAdapter();
            dateTimePicker2.MinDate = dateTimePicker1.Value;
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
