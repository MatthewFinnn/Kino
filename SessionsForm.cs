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
    public partial class SessionsForm : Form
    {
        SqlConnection cn;
        SqlDataAdapter da;
        DataSet ds;
        static public DateTime dt;
        static public string hallName, timeSession, movie, idSession, price1, price2;

        public SessionsForm()
        {
            InitializeComponent();
            Init();
            label5.MouseEnter += labelMouseEnter;
            label5.MouseLeave += labelMouseLeave;
            label13.MouseEnter += labelMouseEnter;
            label13.MouseLeave += labelMouseLeave;
            label18.MouseEnter += labelMouseEnter;
            label18.MouseLeave += labelMouseLeave;
            label16.MouseEnter += labelMouseEnter;
            label16.MouseLeave += labelMouseLeave;
            label5.Click += label5_Click;
            label1.Click += label1_Click;
            label14.Click += label14_Click;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            label15.TextChanged += label15_TextChanged;
            listBox2.SelectedIndexChanged += listBox2_SelectedIndexChanged;
            label16.Click += label16_Click;
            label13.Click += label13_Click;
            label18.Click += label18_Click;
        }

        void label18_Click(object sender, EventArgs e)
        {
            Form form = new SessionRedaktForm();
            form.Show();
            this.Hide();
            form.FormClosed += form_FormClosed;
        }

        void label13_Click(object sender, EventArgs e)
        {
            cn.Open();
            da = new SqlDataAdapter();
            da.DeleteCommand = cn.CreateCommand();
            da.DeleteCommand.CommandText = @"delete from sessions where id_session =" + idSession;
            da.DeleteCommand.ExecuteNonQuery();
            cn.Close();
            RefreshForm();
        }

        void label16_Click(object sender, EventArgs e)
        {

            Form form = new NewSessionForm();
            form.Show();
            this.Hide();
            form.FormClosed += form_FormClosed;
        }

        void form_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Visible = true;
            RefreshForm();
        }

        void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            timeSession = listBox2.Text;
            if (timeSession != "")
            {
                cn.Open();
                ds = new DataSet();
                da = new SqlDataAdapter();
                da.SelectCommand = cn.CreateCommand();
                da.SelectCommand.CommandText = @"select sessions.* from sessions inner join halls on sessions.hall=halls.id_hall
where hall_name ='" + hallName + "' and date_session='" + dt + "' and time_session='" + timeSession + "'";
                da.SelectCommand.ExecuteNonQuery();
                da.Fill(ds, "sessions");
                cn.Close();
                idSession = ds.Tables["sessions"].Rows[0].ItemArray[0].ToString();

                cn.Open();
                ds = new DataSet();
                da = new SqlDataAdapter();
                da.SelectCommand = cn.CreateCommand();
                da.SelectCommand.CommandText = @"select films.* from sessions inner join halls on sessions.hall=halls.id_hall 
inner join films on sessions.film=films.id_films 
where id_session=" + idSession;
                da.SelectCommand.ExecuteNonQuery();
                da.Fill(ds, "films");
                cn.Close();
                movie = label21.Text = ds.Tables["films"].Rows[0].ItemArray[1].ToString();
                label4.Text = ds.Tables["films"].Rows[0].ItemArray[2].ToString();

                label7.Text = DateTime.Parse(timeSession).AddMinutes(int.Parse(label4.Text)).ToLongTimeString();

                cn.Open();
                ds = new DataSet();
                da = new SqlDataAdapter();
                da.SelectCommand = cn.CreateCommand();
                da.SelectCommand.CommandText = @"select prices.* from prices where session=" + idSession;
                da.SelectCommand.ExecuteNonQuery();
                da.Fill(ds, "prices");
                cn.Close();

                string s = ds.Tables["Prices"].Rows[0].ItemArray[3].ToString();
                price1 = label9.Text = s.Substring(0, s.IndexOf(","));
                s = ds.Tables["Prices"].Rows[1].ItemArray[3].ToString();
                price2 = label11.Text = s.Substring(0, s.IndexOf(","));
                //label9.Text = ds.Tables["prices"].Rows[0].ItemArray[3].ToString();
                //label11.Text = ds.Tables["prices"].Rows[1].ItemArray[3].ToString();

                label13.Enabled = true;
                label18.Enabled = true;
            }
            else
            {
                label21.Text = null;
                label4.Text = null;
                label9.Text =null;
                label11.Text = null;
                label7.Text = null;
                label13.Enabled = false;
                label18.Enabled = false;
            }
        }

        void label15_TextChanged(object sender, EventArgs e)
        {        
            RefreshForm();
        }

        void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            hallName = comboBox1.Text;
            listBox2.Focus();
            RefreshForm();
        }

        void label14_Click(object sender, EventArgs e)
        {
            if (DateTime.Now.AddDays(6) > dt)
            {
                dt = dt.AddDays(1);
                label15.Text = label15.Text = dt.ToLongDateString();
            }
        }

        void label1_Click(object sender, EventArgs e)
        {
            if (DateTime.Now <= dt)
            {
                dt = dt.AddDays(-1);
                label15.Text = label15.Text = dt.ToLongDateString();
            }
        }

        void Init()
        {
            dt = DateTime.Today;
            label15.Text = dt.ToLongDateString();
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
            hallName = "Зал 1";

            RefreshForm();
        }

        void RefreshForm()
        {
            cn.Open();
            ds = new DataSet();
            da = new SqlDataAdapter();
            da.SelectCommand = cn.CreateCommand();
            da.SelectCommand.CommandText = @"select sessions.* from sessions inner join halls on sessions.hall=halls.id_hall 
where hall_name ='" + hallName + "' and date_session='" + dt + "' order by time_session";
            da.SelectCommand.ExecuteNonQuery();
            da.Fill(ds, "sessions");
            cn.Close();

            listBox2.DataSource = ds.Tables["sessions"];
            listBox2.DisplayMember = "time_session";
            timeSession = listBox2.Text;

            if (timeSession != "")
            {
                cn.Open();
                ds = new DataSet();
                da = new SqlDataAdapter();
                da.SelectCommand = cn.CreateCommand();
                da.SelectCommand.CommandText = @"select sessions.* from sessions inner join halls on sessions.hall=halls.id_hall
where hall_name ='" + hallName + "' and date_session='" + dt + "' and time_session='" + timeSession + "'";
                da.SelectCommand.ExecuteNonQuery();
                da.Fill(ds, "films");
                cn.Close();
                idSession = ds.Tables["films"].Rows[0].ItemArray[0].ToString();

                cn.Open();
                ds = new DataSet();
                da = new SqlDataAdapter();
                da.SelectCommand = cn.CreateCommand();
                da.SelectCommand.CommandText = @"select films.* from sessions inner join halls on sessions.hall=halls.id_hall 
inner join films on sessions.film=films.id_films 
where id_session=" + idSession;
                da.SelectCommand.ExecuteNonQuery();
                da.Fill(ds, "films");
                cn.Close();                
               movie= label21.Text = ds.Tables["films"].Rows[0].ItemArray[1].ToString();
                label4.Text = ds.Tables["films"].Rows[0].ItemArray[2].ToString();

                label7.Text = DateTime.Parse(timeSession).AddMinutes(int.Parse(label4.Text)).ToLongTimeString();

                cn.Open();
                ds = new DataSet();
                da = new SqlDataAdapter();
                da.SelectCommand = cn.CreateCommand();
                da.SelectCommand.CommandText = @"select prices.* from prices where session=" + idSession;
                da.SelectCommand.ExecuteNonQuery();
                da.Fill(ds, "prices");
                cn.Close();

                string s = ds.Tables["Prices"].Rows[0].ItemArray[3].ToString();
                price1 = label9.Text = s.Substring(0, s.IndexOf(","));
                s = ds.Tables["Prices"].Rows[1].ItemArray[3].ToString();
                price2 = label11.Text = s.Substring(0, s.IndexOf(","));
                //label9.Text = ds.Tables["prices"].Rows[0].ItemArray[3].ToString();
                //label11.Text = ds.Tables["prices"].Rows[1].ItemArray[3].ToString();

                label13.Enabled = true;
                label18.Enabled = true;
            }
            else
            {
                label21.Text = null;
                label4.Text = null;
                label9.Text = null;
                label11.Text = null;
                label7.Text = null;
                label13.Enabled = false;
                label18.Enabled = false;
            }            

        }

        void label5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void labelMouseLeave(object sender, EventArgs e)
        {
            ((Label)sender).ForeColor =MainForm.colorLeave;
        }

        void labelMouseEnter(object sender, EventArgs e)
        {
            ((Label)sender).ForeColor = MainForm.colorEnter;
        }
    }
}
