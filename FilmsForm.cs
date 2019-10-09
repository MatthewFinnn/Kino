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
    public partial class FilmsForm : Form
    {
        static public string trailer;
        static public string idFilm;
        SqlConnection cn;
        SqlDataAdapter da;
        DataSet ds;
        int selectedIndex;

        public FilmsForm()
        {
            InitializeComponent();
            Init();
            label5.MouseEnter += labelMouseEnter;
            label5.MouseLeave += labelMouseLeave;
            label1.MouseEnter += labelMouseEnter;
            label1.MouseLeave += labelMouseLeave;
            label2.MouseEnter += labelMouseEnter;
            label2.MouseLeave += labelMouseLeave;
            label3.MouseEnter += labelMouseEnter;
            label3.MouseLeave += labelMouseLeave;
            label4.MouseEnter += labelMouseEnter;
            label4.MouseLeave += labelMouseLeave;
            label6.MouseEnter += labelMouseEnter;
            label6.MouseLeave += labelMouseLeave;
            label6.Click += label6_Click;
            label4.Click += label4_Click;
            label3.Click += label3_Click;
            label5.Click += label5_Click;
            label1.Click += label1_Click;
            label2.Click += label2_Click;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            //this.Deactivate += FilmsForm_Deactivate;
        }

        //void FilmsForm_Deactivate(object sender, EventArgs e)
        //{
        //    ((Form)(sender)).Close();
        //}

        void label6_Click(object sender, EventArgs e)
        {
            //this.Close();
            Form reservationForm = new ReservationForm();
            reservationForm.Show();
            this.Hide();
            reservationForm.FormClosed += reservationForm_FormClosed;
        }

        void reservationForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                this.Visible = true;
            }
            catch { }
        }

        void label4_Click(object sender, EventArgs e)
        {
            Form newFilmsForm = new NewFilmsForm();
            newFilmsForm.ShowDialog();
            cn.Open();
            ds = new DataSet();
            da = new SqlDataAdapter();
            da.SelectCommand = cn.CreateCommand();
            da.SelectCommand.CommandText = @"select * from films where date_end>='" + DateTime.Today + "'";
            da.SelectCommand.ExecuteNonQuery();
            da.Fill(ds, "films");
            cn.Close();
            listBox1.DataSource = ds.Tables["films"];
            listBox1.DisplayMember = "movies";
        }

        void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            pictureBox1.ImageLocation = null;
            cn.Open();
            ds = new DataSet();
            da = new SqlDataAdapter();
            da.SelectCommand = cn.CreateCommand();
            da.SelectCommand.CommandText = @"select * from films where movies='" + listBox1.Text + "'";
            da.SelectCommand.ExecuteNonQuery();
            da.Fill(ds, "films");
            cn.Close();
            if (ds.Tables["films"].Rows.Count > 0)
            {
                try
                {
                    pictureBox1.ImageLocation = ds.Tables["films"].Rows[0].ItemArray[9].ToString();
                }
                catch { }
                idFilm = ds.Tables["films"].Rows[0].ItemArray[0].ToString();                
                durationLabel1.Text = ds.Tables["films"].Rows[0].ItemArray[2].ToString();
                date_startLabel1.Text = ds.Tables["films"].Rows[0].ItemArray[3].ToString();
                date_endLabel1.Text = ds.Tables["films"].Rows[0].ItemArray[4].ToString();
                genreLabel1.Text = ds.Tables["films"].Rows[0].ItemArray[5].ToString();
                countryLabel1.Text = ds.Tables["films"].Rows[0].ItemArray[6].ToString();
                dimensionalCheckBox.Checked = bool.Parse(ds.Tables["films"].Rows[0].ItemArray[7].ToString());
                descriptionTextBox.Text = ds.Tables["films"].Rows[0].ItemArray[8].ToString();
                MainForm.url = ds.Tables["films"].Rows[0].ItemArray[10].ToString();
                trailer = ds.Tables["films"].Rows[0].ItemArray[11].ToString();
            }
            if ( listBox1.SelectedIndex!=0)
                selectedIndex = listBox1.SelectedIndex;
        }

        void label3_Click(object sender, EventArgs e)
        {
            Form filmsRedaktForm = new FilmsRedaktForm();
            filmsRedaktForm.ShowDialog();
            cn.Open();
            ds = new DataSet();
            da = new SqlDataAdapter();
            da.SelectCommand = cn.CreateCommand();
            da.SelectCommand.CommandText = @"select * from films where date_end>='" + DateTime.Today + "'";
            da.SelectCommand.ExecuteNonQuery();
            da.Fill(ds, "films");
            cn.Close();
            listBox1.DataSource = ds.Tables["films"];
            listBox1.DisplayMember = "movies";
            listBox1.SelectedIndex = selectedIndex;
        }

        void label2_Click(object sender, EventArgs e)
        {
            //cn.Open();
            //DataSet ds = new DataSet();
            //da = new SqlDataAdapter();
            //da.SelectCommand = cn.CreateCommand();
            //da.SelectCommand.CommandText = @"select * from films where movies='" + listBox1.Text + "'";
            //da.SelectCommand.ExecuteNonQuery();
            //da.Fill(ds, "films");
            //cn.Close();
            //trailer = ds.Tables["Films"].Rows[0].ItemArray[11].ToString();
            Form videoform = new VideoForm();
            videoform.ShowDialog();
        }

        void label1_Click(object sender, EventArgs e)
        {
            //cn.Open();
            //DataSet ds = new DataSet();
            //da = new SqlDataAdapter();
            //da.SelectCommand = cn.CreateCommand();
            //da.SelectCommand.CommandText = @"select * from films where movies='" + listBox1.Text + "'";
            //da.SelectCommand.ExecuteNonQuery();
            //da.Fill(ds, "films");
            //cn.Close();
           //url = ds.Tables["Films"].Rows[0].ItemArray[10].ToString();
            Form webform = new WebForm();
            webform.ShowDialog();
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


        void Init()
        {
            label3.Visible = label4.Visible = MainForm.blnPass;
            cn = new SqlConnection(MainForm.connectionString);

            cn.Open();
            ds = new DataSet();
            da = new SqlDataAdapter();
            da.SelectCommand = cn.CreateCommand();
            da.SelectCommand.CommandText = @"select * from films where date_end>='" + DateTime.Today + "'";
            da.SelectCommand.ExecuteNonQuery();
            da.Fill(ds, "films");
            cn.Close();
            listBox1.DataSource = ds.Tables["films"];
            listBox1.DisplayMember = "movies";

            cn.Open();
            ds = new DataSet();
            da = new SqlDataAdapter();
            da.SelectCommand = cn.CreateCommand();
            da.SelectCommand.CommandText = @"select * from films where movies='" + listBox1.Text + "'";
            da.SelectCommand.ExecuteNonQuery();
            da.Fill(ds, "films");
            cn.Close();
            if (ds.Tables["films"].Rows.Count != 0)
            {
                try
                {
                    pictureBox1.ImageLocation = ds.Tables["films"].Rows[0].ItemArray[9].ToString();
                }
                catch { }
                idFilm = ds.Tables["films"].Rows[0].ItemArray[0].ToString();
                durationLabel1.Text = ds.Tables["films"].Rows[0].ItemArray[2].ToString();
                date_startLabel1.Text = ds.Tables["films"].Rows[0].ItemArray[3].ToString();
                date_endLabel1.Text = ds.Tables["films"].Rows[0].ItemArray[4].ToString();
                genreLabel1.Text = ds.Tables["films"].Rows[0].ItemArray[5].ToString();
                countryLabel1.Text = ds.Tables["films"].Rows[0].ItemArray[6].ToString();
                dimensionalCheckBox.Checked = bool.Parse(ds.Tables["films"].Rows[0].ItemArray[7].ToString());
                descriptionTextBox.Text = ds.Tables["films"].Rows[0].ItemArray[8].ToString();
                MainForm.url = ds.Tables["films"].Rows[0].ItemArray[10].ToString();
                trailer = ds.Tables["films"].Rows[0].ItemArray[11].ToString();
            }
        }
    }
}
