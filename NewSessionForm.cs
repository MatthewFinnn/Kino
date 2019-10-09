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
    public partial class NewSessionForm : Form
    {
        SqlConnection cn;
        SqlDataAdapter da;
        DataSet ds;
        string idHall, idFilm, idSession;
        Timer tm;

        public NewSessionForm()
        {
            InitializeComponent();
            Init();
            label5.MouseEnter += labelMouseEnter;
            label5.MouseLeave += labelMouseLeave;
            label7.MouseEnter += labelMouseEnter;
            label7.MouseLeave += labelMouseLeave;
            label5.Click += label5_Click;
            dateTimePicker1.ValueChanged += dateTimePicker1_ValueChanged;
            label7.Click += label7_Click;
        }

        void label7_Click(object sender, EventArgs e)
        {
            bool bln = true;
            DateTime date;
            if (!DateTime.TryParse(maskedTextBox1.Text, out date))
            {
                bln = false;
                maskedTextBox1.BackColor = Color.Red;
                tm.Start();
                tm.Tick += tm_Tick;
            }
            else if (maskedTextBox2.Text == "" || int.Parse(maskedTextBox2.Text)%1000!=0)
            {
                bln = false;
                maskedTextBox2.BackColor = Color.Red;
                tm.Start();
                tm.Tick += tm_Tick;
            }
            else if (maskedTextBox3.Text == "" || int.Parse(maskedTextBox3.Text) % 1000 != 0)
            {
                bln = false;
                maskedTextBox3.BackColor = Color.Red;
                tm.Start();
                tm.Tick += tm_Tick;
            }
            if (bln)
            {
                cn.Open();
                ds = new DataSet();
                da = new SqlDataAdapter();
                da.SelectCommand = cn.CreateCommand();
                da.SelectCommand.CommandText = @"select * from Halls where hall_name='" + comboBox1.Text + "'";
                da.SelectCommand.ExecuteNonQuery();
                da.Fill(ds, "Halls");
                cn.Close();
                idHall = ds.Tables["Halls"].Rows[0].ItemArray[0].ToString();

                cn.Open();
                ds = new DataSet();
                da = new SqlDataAdapter();
                da.SelectCommand = cn.CreateCommand();
                da.SelectCommand.CommandText = @"select * from Films where movies='" + comboBox2.Text + "'";
                da.SelectCommand.ExecuteNonQuery();
                da.Fill(ds, "Films");
                cn.Close();
                idFilm = ds.Tables["Films"].Rows[0].ItemArray[0].ToString();

                cn.Open();
                da = new SqlDataAdapter();
                da.InsertCommand = cn.CreateCommand();
                da.InsertCommand.CommandText = @"INSERT INTO Sessions
VALUES (" + idHall + ", '" + dateTimePicker1.Value + "','" + maskedTextBox1.Text + "', " + idFilm + ")";
                da.InsertCommand.ExecuteNonQuery();
                cn.Close();

                cn.Open();
                ds = new DataSet();
                da = new SqlDataAdapter();
                da.SelectCommand = cn.CreateCommand();
                da.SelectCommand.CommandText = "select * from Sessions where hall=" + idHall + " and date_session='" + dateTimePicker1.Value + "' and time_session='" + maskedTextBox1.Text + "' and film=" + idFilm;
                da.SelectCommand.ExecuteNonQuery();
                da.Fill(ds, "Sessions");
                cn.Close();
                idSession = ds.Tables["Sessions"].Rows[0].ItemArray[0].ToString();

                cn.Open();
                da = new SqlDataAdapter();
                da.UpdateCommand = cn.CreateCommand();
                da.UpdateCommand.CommandText = @"UPDATE Prices 
SET price = '" + maskedTextBox2.Text + @"'
where session=" + idSession + " and (sector=11 or sector=21)";
                da.UpdateCommand.ExecuteNonQuery();
                da.UpdateCommand.CommandText = @"UPDATE Prices 
SET price = '" + maskedTextBox3.Text + @"'
where session=" + idSession + " and (sector=12 or sector=22)";
                da.UpdateCommand.ExecuteNonQuery();
                cn.Close();
                this.Close();
            }
        }

        void tm_Tick(object sender, EventArgs e)
        {
            if (maskedTextBox1.BackColor == Color.Red)
            {
                maskedTextBox1.BackColor = Color.White;
                maskedTextBox1.Focus();
                maskedTextBox1.SelectAll();
            }
            else if (maskedTextBox2.BackColor == Color.Red)
            {
                maskedTextBox2.BackColor = Color.White;
                maskedTextBox2.Focus();
                maskedTextBox2.SelectAll();
            }
            else if (maskedTextBox3.BackColor == Color.Red)
            {
                maskedTextBox3.BackColor = Color.White;
                maskedTextBox3.Focus();
                maskedTextBox3.SelectAll();
            }
            tm.Stop();
        }

        void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            cn.Open();
            ds = new DataSet();
            da = new SqlDataAdapter();
            da.SelectCommand = cn.CreateCommand();
            da.SelectCommand.CommandText = @"select * from films where date_end>='" + dateTimePicker1.Value + "' and date_start<='" + dateTimePicker1.Value + "'";
            da.SelectCommand.ExecuteNonQuery();
            da.Fill(ds, "films");
            cn.Close();

            comboBox2.DataSource = ds.Tables["films"];
            comboBox2.DisplayMember = "movies";
        }

        void Init()
        {
            tm = new Timer();
            tm.Interval = 1000;
            splitContainer1.Panel2Collapsed = true;
            dateTimePicker1.MinDate = DateTime.Today;
            dateTimePicker1.Value = SessionsForm.dt;
            cn = new SqlConnection(MainForm.connectionString);
            cn.Open();
            ds = new DataSet();
            da = new SqlDataAdapter();
            da.SelectCommand = cn.CreateCommand();
            da.SelectCommand.CommandText = @"select * from Halls";
            da.SelectCommand.ExecuteNonQuery();
            da.Fill(ds, "Halls");
            cn.Close();

            comboBox1.DataSource = ds.Tables["Halls"];
            comboBox1.DisplayMember = "hall_name";

            cn.Open();
            ds = new DataSet();
            da = new SqlDataAdapter();
            da.SelectCommand = cn.CreateCommand();
            da.SelectCommand.CommandText = @"select * from films where date_end>='" + SessionsForm.dt + "' and date_start<='" + SessionsForm.dt + "'";
            da.SelectCommand.ExecuteNonQuery();
            da.Fill(ds, "films");
            cn.Close();

            comboBox2.DataSource = ds.Tables["films"];
            comboBox2.DisplayMember = "movies";
            //maskedTextBox2.Text = maskedTextBox3.Text = "0";
        }

        void label5_Click(object sender, EventArgs e)
        {
            this.Close();
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
