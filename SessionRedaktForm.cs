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
    public partial class SessionRedaktForm : Form
    {
        SqlConnection cn;
        SqlDataAdapter da;
        DataSet ds;
        string idHall, idFilm;
        Timer tm;

        public SessionRedaktForm()
        {
            InitializeComponent();
            Init();
            label5.MouseEnter += labelMouseEnter;
            label5.MouseLeave += labelMouseLeave;
            //label6.MouseEnter += labelMouseEnter;
            //label6.MouseLeave += labelMouseLeave;
            label7.MouseEnter += labelMouseEnter;
            label7.MouseLeave += labelMouseLeave;
            //label8.MouseEnter += labelMouseEnter;
            //label8.MouseLeave += labelMouseLeave;
            label5.Click += label5_Click;
            //label6.Click += label6_Click;
            //label8.Click += label8_Click;
            label7.Click += label7_Click;
            maskedTextBox1.GotFocus += maskedTextBoxGotFocus;
            maskedTextBox2.GotFocus += maskedTextBoxGotFocus;
            maskedTextBox3.GotFocus += maskedTextBoxGotFocus;
        }

        void maskedTextBoxGotFocus(object sender, EventArgs e)
        {
            ((MaskedTextBox)(sender)).SelectAll();
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
            else if (maskedTextBox2.Text == "" || int.Parse(maskedTextBox2.Text) % 1000 != 0)
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
                da.SelectCommand.CommandText = @"select * from Halls where hall_name='" + textBox4.Text + "'";
                da.SelectCommand.ExecuteNonQuery();
                da.Fill(ds, "Halls");
                idHall = ds.Tables["Halls"].Rows[0].ItemArray[0].ToString();

                da.SelectCommand = cn.CreateCommand();
                da.SelectCommand.CommandText = @"select * from Films where movies='" + textBox6.Text + "'";
                da.SelectCommand.ExecuteNonQuery();
                da.Fill(ds, "Films");
                idFilm = ds.Tables["Films"].Rows[0].ItemArray[0].ToString();

                da.UpdateCommand = cn.CreateCommand();
                da.UpdateCommand.CommandText = @"UPDATE sessions 
                SET hall = " + idHall + @", 
                date_session = '" + SessionsForm.dt + @"', 
                time_session = '" + maskedTextBox1.Text + @"',  
                film = " + idFilm + @"
                where id_session=" + SessionsForm.idSession;
                da.UpdateCommand.ExecuteNonQuery();

                da.UpdateCommand.CommandText = @"UPDATE Prices 
                SET price = '" + maskedTextBox2.Text + @"'
                where session=" + SessionsForm.idSession + " and (sector=11 or sector=21)";
                da.UpdateCommand.ExecuteNonQuery();

                da.UpdateCommand.CommandText = @"UPDATE Prices 
                SET price = '" + maskedTextBox3.Text + @"'
                where session=" + SessionsForm.idSession + " and (sector=12 or sector=22)";
                da.UpdateCommand.ExecuteNonQuery();
                cn.Close();
                this.Close();
            }
        }

        void Init()
        {
            tm = new Timer();
            tm.Interval = 1000;
            splitContainer1.Panel2Collapsed = true;
            textBox5.Text = SessionsForm.dt.ToLongDateString();
            textBox5.Enabled = false;
            textBox4.Text = SessionsForm.hallName;
            textBox4.Enabled = false;
            textBox6.Text = SessionsForm.movie;
            textBox6.Enabled = false;
            cn = new SqlConnection(MainForm.connectionString);

            //cn.Open();
            //ds = new DataSet();
            //da = new SqlDataAdapter();
            //da.SelectCommand = cn.CreateCommand();
            //da.SelectCommand.CommandText = @"select * from films where date_end>'" + SessionsForm.dt + "' and date_start<'" + SessionsForm.dt + "'";
            //da.SelectCommand.ExecuteNonQuery();
            //da.Fill(ds, "films");
            //cn.Close();

            //comboBox2.DataSource = ds.Tables["films"];
            //comboBox2.DisplayMember = "movies";

            maskedTextBox1.Text = SessionsForm.timeSession;
            maskedTextBox2.Text = SessionsForm.price1;
            maskedTextBox3.Text = SessionsForm.price2;
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
