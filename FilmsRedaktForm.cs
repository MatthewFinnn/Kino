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
    public partial class FilmsRedaktForm : Form
    {
        SqlConnection cn;
        SqlDataAdapter da;
        DataSet ds;
        Timer tm;

        public FilmsRedaktForm()
        {
            InitializeComponent();
            Init();
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
            if (textBox1.Text == "")
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
                da.UpdateCommand = cn.CreateCommand();
                da.UpdateCommand.CommandText = @"UPDATE films 
SET dimensional = '" + dimensionalCheckBox.Checked + @"',
movies = '" + textBox1.Text + @"',  
duration = " + maskedTextBox2.Text + @",  
date_start = '" + dateTimePicker1.Value + @"', 
date_end = '" + dateTimePicker2.Value + @"', 
genre = '" + textBox3.Text + @"',  
country = '" + textBox4.Text + @"',  
description = '" + textBox8.Text + @"',  
image = '" + textBox5.Text + @"',  
trailer = '" + textBox6.Text + @"',  
kinopoisk = '" + textBox7.Text + @"'  
where id_films=" + FilmsForm.idFilm;
                da.UpdateCommand.ExecuteNonQuery();
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

        void Init()
        {
            tm = new Timer();
            tm.Interval = 1000;

            cn = new SqlConnection(MainForm.connectionString);
            cn.Open();
            ds = new DataSet();
            da = new SqlDataAdapter();
            da.SelectCommand = cn.CreateCommand();
            da.SelectCommand.CommandText = @"select * from films where id_films=" + FilmsForm.idFilm;
            da.SelectCommand.ExecuteNonQuery();
            da.Fill(ds, "films");
            cn.Close();

            textBox1.Text = ds.Tables["films"].Rows[0].ItemArray[1].ToString();
            maskedTextBox2.Text = ds.Tables["films"].Rows[0].ItemArray[2].ToString();
            textBox3.Text = ds.Tables["films"].Rows[0].ItemArray[5].ToString();
            textBox4.Text = ds.Tables["films"].Rows[0].ItemArray[6].ToString();
            textBox8.Text = ds.Tables["films"].Rows[0].ItemArray[8].ToString();
            textBox5.Text = ds.Tables["films"].Rows[0].ItemArray[9].ToString();
            textBox6.Text = ds.Tables["films"].Rows[0].ItemArray[11].ToString();
            textBox7.Text = ds.Tables["films"].Rows[0].ItemArray[10].ToString();
            dateTimePicker1.Value = DateTime.Parse(ds.Tables["films"].Rows[0].ItemArray[3].ToString());
            dateTimePicker2.MinDate = dateTimePicker1.Value;
            dateTimePicker2.Value = DateTime.Parse(ds.Tables["films"].Rows[0].ItemArray[4].ToString());
            dimensionalCheckBox.Checked = bool.Parse(ds.Tables["films"].Rows[0].ItemArray[7].ToString());
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
