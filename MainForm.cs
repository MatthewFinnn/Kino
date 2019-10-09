using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Kino
{
    public partial class MainForm : Form
    {
        static public string dbFolder;
        static public string connectionString;
        static public Color colorLeave, colorEnter;
        static public bool blnPass;
        static public string pass, url;
        Timer t;
        int ialbum = 0;
        string[] files;
        Form passform;

        public MainForm()
        {
            InitializeComponent();
            Init();
            t.Tick += t_Tick;
            label1.MouseEnter += labelMouseEnter;
            label1.MouseLeave += labelMouseLeave;
            label2.MouseEnter += labelMouseEnter;
            label2.MouseLeave += labelMouseLeave;
            label3.MouseEnter += labelMouseEnter;
            label3.MouseLeave += labelMouseLeave;
            label4.MouseEnter += labelMouseEnter;
            label4.MouseLeave += labelMouseLeave;
            label5.MouseEnter += labelMouseEnter;
            label5.MouseLeave += labelMouseLeave;
            label6.MouseEnter += labelMouseEnter;
            label6.MouseLeave += labelMouseLeave;
            label7.MouseEnter += labelMouseEnter;
            label7.MouseLeave += labelMouseLeave;
            label8.MouseEnter += labelMouseEnter;
            label8.MouseLeave += labelMouseLeave;
            label11.MouseEnter += labelMouseEnter;
            label11.MouseLeave += labelMouseLeave;
            label11.Click += label11_Click;
            label5.Click += label5_Click;
            label2.Click += label2_Click;
            label1.Click += label1_Click;
            label3.Click += label3_Click;
            label4.Click += label4_Click;
            label6.Click += label6_Click;
            label7.Click += label7_Click;
            label8.Click += label8_Click;
            //button1.Click += button1_Click;
            //button2.Click += button2_Click;
        }

        void label11_Click(object sender, EventArgs e)
        {
            Form form = new SessionsForm();
            form.Show();
            this.Hide();
            t.Stop();
            form.FormClosed += form_FormClosed;
        }

        void label8_Click(object sender, EventArgs e)
        {
            XDocument xDoc = XDocument.Load(dbFolder + "Setting.xml");
            xDoc.Element("setting").Element("ConnectionString").Value = textBox1.Text;
            xDoc.Element("setting").Element("password").Value = textBox2.Text;
            SqlConnection cn = new SqlConnection();
            //if (textBox2.Text != "")
            //{
            //    pass = textBox2.Text;
            //    xDoc.Save(dbFolder + "Setting.xml");
            //}
            //else
            //{
            //    MessageBox.Show("Пароль не может быть пустым!");
            //    textBox2.Text = pass;
            //}
            try
            {        
                cn.ConnectionString = textBox1.Text;
                cn.Open();
                connectionString = textBox1.Text;
                pass = textBox2.Text;
                xDoc.Save(dbFolder + "Setting.xml");
                label11.Enabled = label1.Enabled = label3.Enabled = label5.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                textBox1.Text = connectionString;
                textBox2.Text = pass;
            }
            finally
            {
                cn.Close();
            }
        }

        void label7_Click(object sender, EventArgs e)
        {
            if (label7.Text == "получить права администратора")
            {
                passform = new PassForm();
                passform.ShowDialog();
                if (blnPass)
                {
                    label7.Text = "выйти из прав администратора";
                    label4.Visible = !label4.Visible;
                    label11.Visible = !label11.Visible;
                }
            }
            else
            {
                blnPass = !blnPass;
                label7.Text = "получить права администратора";
                label4.Visible = !label4.Visible;
                label11.Visible = !label11.Visible;
                splitContainer2.Panel1Collapsed = true;
            }
        }

        void label6_Click(object sender, EventArgs e)
        {
            splitContainer2.Panel1Collapsed = true;
        }

        void label4_Click(object sender, EventArgs e)
        {
            splitContainer2.Panel2Collapsed = true;
        }

        void label5_Click(object sender, EventArgs e)
        {
            Form form = new ReservationForm();
            form.Show();
            this.Hide();
            t.Stop();
            form.FormClosed += form_FormClosed;
        }

        //void button2_Click(object sender, EventArgs e)
        //{
        //    if (button2.Text == "Вход")
        //    {
        //        Form form = new PassForm();
        //        form.ShowDialog();
        //        if (blnPass)
        //        {
        //            button2.Text = "Выход";
        //            button1.Visible = !button1.Visible;
        //        }
        //    }
        //    else
        //    {
        //        blnPass = !blnPass;
        //        button2.Text = "Вход";
        //        button1.Visible = !button1.Visible;
        //        splitContainer2.Panel2Collapsed = true;
        //    }

        //}

        //void button1_Click(object sender, EventArgs e)
        //{
        //    splitContainer2.Panel2Collapsed = !splitContainer2.Panel2Collapsed;
        //    splitContainer2.Panel1Collapsed = !splitContainer2.Panel2Collapsed;
        //}

        void label3_Click(object sender, EventArgs e)
        {
            Form form = new HallForm();
            form.Show();
            this.Hide();
            t.Stop();
            form.FormClosed += form_FormClosed;
        }

        void label1_Click(object sender, EventArgs e)
        {
            Form form = new FilmsForm();
            form.Show();
            this.Hide();
            t.Stop();
            form.FormClosed += form_FormClosed;
        }

        void form_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Visible = true;
            t.Start();
        }

        void label2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void labelMouseLeave(object sender, EventArgs e)
        {
            ((Label)sender).ForeColor = colorLeave;
        }

        void labelMouseEnter(object sender, EventArgs e)
        {
            ((Label)sender).ForeColor = colorEnter;
        }

        void t_Tick(object sender, EventArgs e)
        {
            if (ialbum == files.Length) { ialbum = 0; }
            pictureBox1.ImageLocation = files[ialbum];
            ialbum++;
        }

        string FindDirectory(string name)
        {
            string dir = Application.StartupPath;
            for (char slash = '\\'; dir != null; dir = Path.GetDirectoryName(dir))
            {
                string res = dir.TrimEnd(slash) + slash + name;
                if (Directory.Exists(res))
                    return res + slash;
            }
            return null;
        }

        void Init()
        {
            dbFolder = FindDirectory("Data");
            colorLeave = Color.DarkGray;
            colorEnter = Color.White;
            label11.Visible = false;

            splitContainer2.Panel1Collapsed = true;

            files = Directory.GetFiles(dbFolder + "Image\\album", "*.jpg", SearchOption.TopDirectoryOnly);
            pictureBox1.ImageLocation = files[ialbum];
            ialbum++;
            t = new Timer();
            t.Interval = 10000;
            t.Start();

            //label8.Text = ConfigurationManager.ConnectionStrings["KinoConnectionString"].ConnectionString;
            //label9.Text = pass = "pass";


            XDocument xDoc = XDocument.Load(dbFolder + "Setting.xml");
            textBox1.Text = connectionString = xDoc.Element("setting").Element("ConnectionString").Value;
            textBox2.Text = pass = xDoc.Element("setting").Element("password").Value;

            SqlConnection cn = new SqlConnection();
            //if (textBox2.Text != "")
            //{
            //    pass = textBox2.Text;
            //    xDoc.Save(dbFolder + "Setting.xml");
            //}
            //else
            //{
            //    MessageBox.Show("Пароль не может быть пустым!");
            //    textBox2.Text = pass;
            //}
            try
            {
                cn.ConnectionString = connectionString;
                cn.Open();
            }
            catch
            {
                label11.Enabled = label1.Enabled = label3.Enabled = label5.Enabled = false;
            }
            finally
            {
                cn.Close();
            }
        }


    }
}
