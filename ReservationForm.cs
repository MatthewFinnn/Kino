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
    public partial class ReservationForm : Form
    {
        SqlConnection cn;
        SqlDataAdapter da;
        DateTime dt;
        string idHall, idSession, row, seat, sector, idUser, idTicket;
        int price, sum, service;
        DataSet ds;
        int locationX, LocationY;
        Color colorPaid, colorReservation, colorActive;
        static public string tel;


        public ReservationForm()
        {
            InitializeComponent();
            Init();
            label6.MouseEnter += labelMouseEnter;
            label6.MouseLeave += labelMouseLeave;
            label7.MouseEnter += labelMouseEnter;
            label7.MouseLeave += labelMouseLeave;
            label8.MouseEnter += labelMouseEnter;
            label8.MouseLeave += labelMouseLeave;
            label9.MouseEnter += labelMouseEnter;
            label9.MouseLeave += labelMouseLeave;
            label10.MouseEnter += labelMouseEnter;
            label10.MouseLeave += labelMouseLeave;
            label11.MouseEnter += labelMouseEnter;
            label11.MouseLeave += labelMouseLeave;
            label12.MouseEnter += labelMouseEnter;
            label12.MouseLeave += labelMouseLeave;
            label13.MouseEnter += labelMouseEnter;
            label13.MouseLeave += labelMouseLeave;
            label5.MouseEnter += labelMouseEnter;
            label5.MouseLeave += labelMouseLeave;
            label14.MouseEnter += labelMouseEnter;
            label14.MouseLeave += labelMouseLeave;
            label81.MouseEnter += labelMouseEnter;
            label81.MouseLeave += labelMouseLeave;
            label82.MouseEnter += labelMouseEnter;
            label82.MouseLeave += labelMouseLeave;
            label83.MouseEnter += labelMouseEnter;
            label83.MouseLeave += labelMouseLeave;
            label84.MouseEnter += labelMouseEnter;
            label84.MouseLeave += labelMouseLeave;
            label84.Click += label84_Click;
            label83.Click += label83_Click;
            label82.Click += label82_Click;
            label81.Click += label81_Click;
            label14.Click += label14_Click;
            label5.Click += label5_Click;
            label13.Click += label13_Click;
            label11.Click += label11_Click;
            label10.Click += label10_Click;
            label8.Click += label8_Click;
            label9.Click += label9_Click;
            label6.Click += label6_Click;
            label7.Click += label7_Click;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            listBox2.SelectedIndexChanged += listBox2_SelectedIndexChanged;
            pictureBox2.Click += pictureBox2_Click;
            pictureBox3.Click += pictureBox3_Click;
            pictureBox4.Click += pictureBox4_Click;
            pictureBox5.Click += pictureBox5_Click;
            pictureBox6.Click += pictureBox6_Click;
            label12.Click += label12_Click;
            FormClosing += ReservationForm_FormClosing;
            listBox1.DoubleClick += listBox1_DoubleClick;
        }

        void listBox1_DoubleClick(object sender, EventArgs e)
        {
            //this.Close();
            Form filmForm = new FilmsForm();
            filmForm.Show();
            this.Hide();
            filmForm.FormClosed += filmForm_FormClosed;
        }

        void filmForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                this.Visible = true;
            }
            catch { }
        }

        void ReservationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Rollback();
        }

        void label13_Click(object sender, EventArgs e)
        {
            splitContainer3.Panel2Collapsed = true;
            label12.Visible = false;
        }

        void label12_Click(object sender, EventArgs e)
        {
            label12.Visible = false;
            Paid();
        }

        void pictureBox6_Click(object sender, EventArgs e)
        {
            MainForm.url = "http://kino.bycard.by/erip_desc?x=877104";
            Form form = new WebForm();
            form.Show();
            form.FormClosed += form_FormClosed;
        }

        void pictureBox5_Click(object sender, EventArgs e)
        {
            MainForm.url = "https://velcom.ipay.by/ipay_velcom/!iSOU.Login?srv_no=403851&pers_acc=877104";
            Form form = new WebForm();
            form.Show();
            form.FormClosed += form_FormClosed;
        }

        void pictureBox4_Click(object sender, EventArgs e)
        {
            MainForm.url = "https://mts.ipay.by:4443/pls/iPay/!iSOU.Login?srv_no=548&pers_acc=877104";
            Form form = new WebForm();
            form.Show();
            form.FormClosed += form_FormClosed;
        }

        void pictureBox3_Click(object sender, EventArgs e)
        {
            MainForm.url = "https://webpay.by/";
            Form form = new WebForm();
            form.Show();
            form.FormClosed += form_FormClosed;
        }

        void pictureBox2_Click(object sender, EventArgs e)
        {
            MainForm.url = "https://pay117.paysec.by/pay/order.cfm";
            Form form = new WebForm();
            form.Show();
            form.FormClosed += form_FormClosed;
        }

        void form_FormClosed(object sender, FormClosedEventArgs e)
        {
            label12.Visible = true;
        }

        void label84_Click(object sender, EventArgs e)
        {
            Reserv();
        }

        void label83_Click(object sender, EventArgs e)
        {
            Reserv();
        }

        void label82_Click(object sender, EventArgs e)
        {
            RefreshHall();
        }

        void label81_Click(object sender, EventArgs e)
        {
            RefreshHall();
        }

        void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            label21.Text = listBox2.Text;
            cn.Open();
            ds = new DataSet();
            da = new SqlDataAdapter();
            da.SelectCommand = cn.CreateCommand();
            da.SelectCommand.CommandText = @"select Halls.* from Sessions inner join films on sessions.film=films.id_films inner join halls on halls.id_hall=sessions.hall where date_session='" + dt + "' and movies='" + listBox1.Text + "' and time_session='" + listBox2.Text + "'";
            da.SelectCommand.ExecuteNonQuery();
            da.Fill(ds, "Halls");
            cn.Close();
            if (ds.Tables["Halls"].Rows.Count != 0)
            {
                label21.Text = listBox2.Text + " / " + ds.Tables["Halls"].Rows[0].ItemArray[1].ToString();
                idHall = ds.Tables["Halls"].Rows[0].ItemArray[0].ToString();
            }
        }

        void label5_Click(object sender, EventArgs e)
        {
            if (DateTime.Now <= dt)
            {
                dt = dt.AddDays(-1);
                label20.Text = label15.Text = dt.ToLongDateString();
                pictureBox1.ImageLocation = null;
                label21.Text = null;
                idHall = null;
                RefreshForm();
            }
        }

        void label14_Click(object sender, EventArgs e)
        {
            if (DateTime.Now.AddDays(6) > dt)
            {
                dt = dt.AddDays(1);
                label20.Text = label15.Text = dt.ToLongDateString();
                pictureBox1.ImageLocation = null;
                label21.Text = null;
                idHall = null;
                RefreshForm();
            }
        }


        #region labelClick

        void label10_Click(object sender, EventArgs e)
        {
            splitContainer3.Panel1Collapsed = true;
        }

        void label11_Click(object sender, EventArgs e)
        {
            Rollback();
        }

        void label8_Click(object sender, EventArgs e)
        {
            splitContainer3.Panel1Collapsed = true;
        }

        void label9_Click(object sender, EventArgs e)
        {
            Rollback();
        }

        void label7_Click(object sender, EventArgs e)
        {
            cn.Open();
            ds = new DataSet();
            da = new SqlDataAdapter();
            da.SelectCommand = cn.CreateCommand();
            da.SelectCommand.CommandText = @"select Sessions.* from Sessions inner join films on sessions.film=films.id_films inner join halls on halls.id_hall=sessions.hall where date_session='" + dt + "' and movies='" + listBox1.Text + "' and time_session='" + listBox2.Text + "'";
            da.SelectCommand.ExecuteNonQuery();
            da.Fill(ds, "Sessions");
            cn.Close();


            if (idHall == "1")
            {
                idSession = ds.Tables["Sessions"].Rows[0].ItemArray[0].ToString();
                splitContainer2.Panel1Collapsed = true;
                splitContainer4.Panel2Collapsed = true;
                //InitHall();
                RefreshHall();
            }
            else if (idHall == "2")
            {
                idSession = ds.Tables["Sessions"].Rows[0].ItemArray[0].ToString();
                splitContainer2.Panel1Collapsed = true;
                splitContainer4.Panel1Collapsed = true;
                //InitHall();
                RefreshHall();
            }
        }
        #endregion

        void seatButtonClick(object sender, EventArgs e)
        {
            string place = ((Button)sender).Name.Replace("p", "");
            cn.Open();
            da = new SqlDataAdapter();
            ds = new DataSet();
            da.SelectCommand = cn.CreateCommand();
            da.SelectCommand.CommandText = @"Select * from Tickets
            where session=" + idSession + " and place=" + place;
            da.SelectCommand.ExecuteNonQuery();
            da.Fill(ds, "Tickets");
            idTicket = ds.Tables["Tickets"].Rows[0].ItemArray[0].ToString();
            cn.Close();
            // регистрируемся
            if (tel.Length != 18 && !MainForm.blnPass)
            {
                Form userForm = new UserForm();
                userForm.ShowDialog();
                if (tel.Length == 18)
                {
                    DateTime dateN = DateTime.Now;
                    cn.Open();
                    da = new SqlDataAdapter();
                    ds = new DataSet();
                    da.SelectCommand = cn.CreateCommand();
                    da.SelectCommand.CommandText = @"Select * from UsersData 
                                        where tel='" + tel + "' and CAST(dateUser as time)>'" + dateN.AddMinutes(-15).ToShortTimeString() + "' and CAST(dateUser as date)='" + dateN.ToShortDateString() + "'";
                    da.SelectCommand.ExecuteNonQuery();
                    da.Fill(ds, "UsersData");
                    if (ds.Tables["UsersData"].Rows.Count > 0)
                    {
                        idUser = ds.Tables["UsersData"].Rows[0].ItemArray[0].ToString();
                        da.UpdateCommand = cn.CreateCommand();
                        da.UpdateCommand.CommandText = @"UPDATE UsersData 
                                        SET redakt = 'True', dateUser='" + dateN + "' where id_user=" + idUser;
                        da.UpdateCommand.ExecuteNonQuery();
                        cn.Close();
                        RefreshHall();
                    }
                    else
                    {
                        da = new SqlDataAdapter();
                        ds = new DataSet();
                        da.InsertCommand = cn.CreateCommand();
                        da.InsertCommand.CommandText = @"INSERT INTO UsersData
                                            VALUES ('" + tel + "', '" + dateN + "', 'True')";
                        da.InsertCommand.ExecuteNonQuery();
                        da.SelectCommand = cn.CreateCommand();
                        da.SelectCommand.CommandText = @"Select * from UsersData 
                                            where tel='" + tel + "' and dateUser='" + dateN + "'";
                        da.SelectCommand.ExecuteNonQuery();
                        da.Fill(ds, "UsersData");
                        idUser = ds.Tables["UsersData"].Rows[0].ItemArray[0].ToString();
                        cn.Close();
                    }

                }
            }
            else if (MainForm.blnPass && idUser == null)
            {
                DateTime dateN = DateTime.Now;
                cn.Open();
                da = new SqlDataAdapter();
                ds = new DataSet();
                da.InsertCommand = cn.CreateCommand();
                da.InsertCommand.CommandText = @"INSERT INTO UsersData
                VALUES ('admin', '" + dateN + "', 'True')";
                da.InsertCommand.ExecuteNonQuery();
                da.SelectCommand = cn.CreateCommand();
                da.SelectCommand.CommandText = @"Select * from UsersData 
                where tel='admin' and dateUser='" + dateN + "'";
                da.SelectCommand.ExecuteNonQuery();
                da.Fill(ds, "UsersData");
                idUser = ds.Tables["UsersData"].Rows[0].ItemArray[0].ToString();
                cn.Close();
            }
            ////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (idUser != null)
            {
                cn.Open();
                ds = new DataSet();
                da = new SqlDataAdapter();
                da.SelectCommand = cn.CreateCommand();
                da.SelectCommand.CommandText = @"Select UsersTicket.* from UsersTicket inner join UsersData on UsersTicket.id_user=UsersData.id_user
                                    where id_ticket=" + idTicket + " and UsersTicket.id_user!=" + idUser + " and redakt='True'";
                da.SelectCommand.ExecuteNonQuery();
                da.Fill(ds, "UsersTicket");
                cn.Close();
                if (ds.Tables["UsersTicket"].Rows.Count > 0)
                {
                    MessageBox.Show("Место занято или редактируется!\nОбновите данные");
                }
                else
                {
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //if (tel.Length == 18) // пользователь
                    if (!MainForm.blnPass)
                    {
                        if (((Button)sender).BackColor == colorActive)
                        {
                            listBox3.Items.Remove(row + " ряд / " + seat + " место / " + price);
                            label66.Text = (int.Parse(label66.Text) - service).ToString();
                            sum -= (price + service);
                            label67.Text = sum.ToString();
                            ((Button)sender).BackColor = Control.DefaultBackColor;

                            cn.Open();
                            ds = new DataSet();
                            da = new SqlDataAdapter();
                            da.SelectCommand = cn.CreateCommand();
                            da.SelectCommand.CommandText = @"Select * from Tickets
                                            where id_ticket=" + idTicket;
                            da.SelectCommand.ExecuteNonQuery();
                            da.Fill(ds, "Tickets");
                            if (ds.Tables["Tickets"].Rows[0].ItemArray[6].ToString() == "False")
                            {
                                da.DeleteCommand = cn.CreateCommand();
                                da.DeleteCommand.CommandText = @"delete from UsersTicket 
                                    where id_ticket=" + idTicket + " and id_user=" + idUser;
                                da.DeleteCommand.ExecuteNonQuery();
                            }
                            da.UpdateCommand = cn.CreateCommand();
                            da.UpdateCommand.CommandText = @"UPDATE Tickets 
                                SET reservation = 'False' where id_ticket=" + idTicket;
                            da.UpdateCommand.ExecuteNonQuery();
                            cn.Close();
                        }
                        else if (((Button)sender).BackColor == Control.DefaultBackColor)
                        {
                            listBox3.Items.Add(row + " ряд / " + seat + " место / " + price);
                            label66.Text = (int.Parse(label66.Text) + service).ToString();
                            sum += price + service;
                            label67.Text = sum.ToString();
                            ((Button)sender).BackColor = colorActive;

                            cn.Open();
                            ds = new DataSet();
                            da = new SqlDataAdapter();
                            da.SelectCommand = cn.CreateCommand();
                            da.SelectCommand.CommandText = @"Select * from Tickets
                                            where id_ticket=" + idTicket;
                            da.SelectCommand.ExecuteNonQuery();
                            da.Fill(ds, "Tickets");
                            if (ds.Tables["Tickets"].Rows[0].ItemArray[6].ToString() == "False")
                            {
                                da.InsertCommand = cn.CreateCommand();
                                da.InsertCommand.CommandText = @"INSERT INTO UsersTicket
                                                VALUES (" + idTicket + ", " + idUser + ")";
                                da.InsertCommand.ExecuteNonQuery();
                                da.UpdateCommand = cn.CreateCommand();
                            }
                            da = new SqlDataAdapter();
                            da.UpdateCommand = cn.CreateCommand();
                            da.UpdateCommand.CommandText = @"UPDATE Tickets 
                                            SET reservation = 'True' where id_ticket=" + idTicket;
                            da.UpdateCommand.ExecuteNonQuery();
                            cn.Close();
                        }
                    }
                    /////////////////////////////////////////////////////////////////////////// админ
                    else if (MainForm.blnPass)
                    {
                        if (((Button)sender).BackColor == colorActive || ((Button)sender).BackColor == colorReservation)
                        {
                            cn.Open();
                            ds = new DataSet();
                            da = new SqlDataAdapter();
                            da.SelectCommand = cn.CreateCommand();
                            da.SelectCommand.CommandText = @"Select * from Tickets
                                            where id_ticket=" + idTicket;
                            da.SelectCommand.ExecuteNonQuery();
                            da.Fill(ds, "Tickets");
                            cn.Close();
                            if (ds.Tables["Tickets"].Rows[0].ItemArray[7].ToString() == "True")
                            {
                                cn.Open();
                                da = new SqlDataAdapter();
                                da.UpdateCommand = cn.CreateCommand();
                                da.UpdateCommand.CommandText = @"UPDATE Tickets 
                                                SET reservation = 'False', paid='True'
                                                where id_ticket=" + idTicket;
                                da.UpdateCommand.ExecuteNonQuery();
                                da.DeleteCommand = cn.CreateCommand();
                                da.DeleteCommand.CommandText = @"delete from UsersTicket
                                                where id_ticket=" + idTicket + " and id_user=" + idUser;
                                da.DeleteCommand.ExecuteNonQuery();
                                cn.Close();
                                ((Button)sender).BackColor = colorPaid;
                                listBox3.Items.Remove(row + " ряд / " + seat + " место / " + price);
                                label66.Text = (int.Parse(label66.Text) + service).ToString();
                                sum += (price + service) / 2;
                                label67.Text = sum.ToString();
                            }
                            else
                            {
                                cn.Open();
                                ds = new DataSet();
                                da = new SqlDataAdapter();
                                da.SelectCommand = cn.CreateCommand();
                                da.SelectCommand.CommandText = @"Select * from UsersTicket
                                            where id_ticket=" + idTicket + " and id_user=" + idUser;
                                da.SelectCommand.ExecuteNonQuery();
                                da.Fill(ds, "UsersTicket");
                                if (ds.Tables["UsersTicket"].Rows.Count == 0)
                                {
                                    da.InsertCommand = cn.CreateCommand();
                                    da.InsertCommand.CommandText = @"INSERT INTO UsersTicket
                                                VALUES (" + idTicket + ", " + idUser + ")";
                                    da.InsertCommand.ExecuteNonQuery();
                                    da.UpdateCommand = cn.CreateCommand();
                                    da.UpdateCommand.CommandText = @"UPDATE Tickets SET reservationTemp = reservation, paidTemp = paid
                                                where id_ticket=" + idTicket;
                                    da.UpdateCommand.ExecuteNonQuery();

                                    listBox3.Items.Add(row + " ряд / " + seat + " место / " + price);
                                    label66.Text = (int.Parse(label66.Text) + service).ToString();
                                    sum += price + service;
                                    label67.Text = sum.ToString();
                                }
                                //ds = new DataSet();
                                da = new SqlDataAdapter();
                                da.UpdateCommand = cn.CreateCommand();
                                da.UpdateCommand.CommandText = @"UPDATE Tickets 
                                            SET reservation = 'False', paid='True'
                                            where id_ticket=" + idTicket;
                                da.UpdateCommand.ExecuteNonQuery();
                                cn.Close();
                                ((Button)sender).BackColor = colorPaid;
                            }
                        }
                        else if (((Button)sender).BackColor == colorPaid)
                        {
                            cn.Open();
                            ds = new DataSet();
                            da = new SqlDataAdapter();
                            da.SelectCommand = cn.CreateCommand();
                            da.SelectCommand.CommandText = @"Select * from Tickets
                                            where id_ticket=" + idTicket;
                            da.SelectCommand.ExecuteNonQuery();
                            da.Fill(ds, "Tickets");
                            cn.Close();
                            if (ds.Tables["Tickets"].Rows[0].ItemArray[6].ToString() == "False" && ds.Tables["Tickets"].Rows[0].ItemArray[7].ToString() == "False")
                            {
                                cn.Open();
                                da = new SqlDataAdapter();
                                da.UpdateCommand = cn.CreateCommand();
                                da.UpdateCommand.CommandText = @"UPDATE Tickets 
                                                SET reservation = 'False', paid='False' 
                                                where id_ticket=" + idTicket;
                                da.UpdateCommand.ExecuteNonQuery();
                                da.DeleteCommand = cn.CreateCommand();
                                da.DeleteCommand.CommandText = @"delete from UsersTicket
                                                where id_ticket=" + idTicket + " and id_user=" + idUser;
                                da.DeleteCommand.ExecuteNonQuery();
                                cn.Close();
                                ((Button)sender).BackColor = Control.DefaultBackColor;
                                listBox3.Items.Remove(row + " ряд / " + seat + " место / " + price);
                                label66.Text = (int.Parse(label66.Text) - service).ToString();
                                sum -= price + service;
                                label67.Text = sum.ToString();
                            }
                            else
                            {

                                cn.Open();
                                ds = new DataSet();
                                da = new SqlDataAdapter();
                                da.SelectCommand = cn.CreateCommand();
                                da.SelectCommand.CommandText = @"Select * from UsersTicket
                                            where id_ticket=" + idTicket + " and id_user=" + idUser;
                                da.SelectCommand.ExecuteNonQuery();
                                da.Fill(ds, "UsersTicket");
                                if (ds.Tables["UsersTicket"].Rows.Count == 0)
                                {
                                    da.InsertCommand = cn.CreateCommand();
                                    da.InsertCommand.CommandText = @"INSERT INTO UsersTicket
                                                VALUES (" + idTicket + ", " + idUser + ")";
                                    da.InsertCommand.ExecuteNonQuery();
                                    da.UpdateCommand = cn.CreateCommand();
                                    da.UpdateCommand.CommandText = @"UPDATE Tickets SET reservationTemp = reservation, paidTemp = paid
                                                where id_ticket=" + idTicket;
                                    da.UpdateCommand.ExecuteNonQuery();
                                    listBox3.Items.Add(row + " ряд / " + seat + " место / " + price);
                                    label66.Text = (int.Parse(label66.Text) - service).ToString();
                                    sum -= (price + service) / 2;
                                    label67.Text = sum.ToString();
                                }
                                //ds = new DataSet();
                                da = new SqlDataAdapter();
                                da.UpdateCommand = cn.CreateCommand();
                                da.UpdateCommand.CommandText = @"UPDATE Tickets 
                            SET reservation = 'False', paid='False'
                            where id_ticket=" + idTicket;
                                da.UpdateCommand.ExecuteNonQuery();
                                cn.Close();
                                ((Button)sender).BackColor = Control.DefaultBackColor;
                            }
                        }
                        else if (((Button)sender).BackColor == Control.DefaultBackColor)
                        {
                            cn.Open();
                            ds = new DataSet();
                            da = new SqlDataAdapter();
                            da.SelectCommand = cn.CreateCommand();
                            da.SelectCommand.CommandText = @"Select * from Tickets
                                            where id_ticket=" + idTicket;
                            da.SelectCommand.ExecuteNonQuery();
                            da.Fill(ds, "Tickets");
                            cn.Close();
                            if (ds.Tables["Tickets"].Rows[0].ItemArray[6].ToString() == "True")
                            {
                                cn.Open();
                                da = new SqlDataAdapter();
                                da.UpdateCommand = cn.CreateCommand();
                                da.UpdateCommand.CommandText = @"UPDATE Tickets 
                                                SET reservation = 'True', paid='False'
                                                where id_ticket=" + idTicket;
                                da.UpdateCommand.ExecuteNonQuery();
                                da.DeleteCommand = cn.CreateCommand();
                                da.DeleteCommand.CommandText = @"delete from UsersTicket
                                                where id_ticket=" + idTicket + " and id_user=" + idUser;
                                da.DeleteCommand.ExecuteNonQuery();
                                cn.Close();
                                ((Button)sender).BackColor = colorActive;

                                listBox3.Items.Remove(row + " ряд / " + seat + " место / " + price);
                                label66.Text = (int.Parse(label66.Text) - service).ToString();
                                sum -= price + service;
                                label67.Text = sum.ToString();
                            }
                            else
                            {

                                cn.Open();
                                ds = new DataSet();
                                da = new SqlDataAdapter();
                                da.SelectCommand = cn.CreateCommand();
                                da.SelectCommand.CommandText = @"Select * from UsersTicket
                                                                        where id_ticket=" + idTicket + " and id_user=" + idUser;
                                da.SelectCommand.ExecuteNonQuery();
                                da.Fill(ds, "UsersTicket");
                                if (ds.Tables["UsersTicket"].Rows.Count == 0)
                                {
                                    da.InsertCommand = cn.CreateCommand();
                                    da.InsertCommand.CommandText = @"INSERT INTO UsersTicket
                                                                            VALUES (" + idTicket + ", " + idUser + ")";
                                    da.InsertCommand.ExecuteNonQuery();
                                    da.UpdateCommand = cn.CreateCommand();
                                    da.UpdateCommand.CommandText = @"UPDATE Tickets SET reservationTemp = reservation, paidTemp = paid
                                                                            where id_ticket=" + idTicket;
                                    da.UpdateCommand.ExecuteNonQuery();
                                    listBox3.Items.Add(row + " ряд / " + seat + " место / " + price);
                                    label66.Text = (int.Parse(label66.Text) + service).ToString();
                                    sum += price + service;
                                    label67.Text = sum.ToString();
                                }
                                da = new SqlDataAdapter();
                                da.UpdateCommand = cn.CreateCommand();
                                da.UpdateCommand.CommandText = @"UPDATE Tickets 
                            SET reservation = 'True', paid='False'
                            where id_ticket=" + idTicket;
                                da.UpdateCommand.ExecuteNonQuery();
                                cn.Close();
                                ((Button)sender).BackColor = colorActive;
                            }
                        }
                    }
                }
            }
        }

        void seatMouseLeave(object sender, EventArgs e)
        {
            if (((Button)sender).BackColor == colorActive || ((Button)sender).BackColor == Control.DefaultBackColor || MainForm.blnPass)
            {
                //((Button)sender).Size = new System.Drawing.Size(20, 20);
                //((Button)sender).Location = new System.Drawing.Point(locationX, LocationY);
            }
        }

        void seatMouseLeave2(object sender, EventArgs e)
        {
            if (((Button)sender).BackColor == colorActive || ((Button)sender).BackColor == Control.DefaultBackColor || MainForm.blnPass)
            {
               // ((Button)sender).Location = new System.Drawing.Point(locationX, LocationY);
               // ((Button)sender).Size = new System.Drawing.Size(40, 40);
            }
        }

        void seatMouseEnter(object sender, EventArgs e)
        {
            if (((Button)sender).BackColor == colorActive || ((Button)sender).BackColor == Control.DefaultBackColor || MainForm.blnPass)
            {
                locationX = ((Button)sender).Location.X;
                LocationY = ((Button)sender).Location.Y;
               // ((Button)sender).Location = new System.Drawing.Point(locationX - 5, LocationY - 5);
                //((Button)sender).Size = new System.Drawing.Size(30, 30);
                string idPlace = ((Button)sender).Name.Replace("p", "");
                cn.Open();
                ds = new DataSet();
                da = new SqlDataAdapter();
                da.SelectCommand = cn.CreateCommand();
                da.SelectCommand.CommandText = @"select Places.* from Places where id_place=" + idPlace;
                da.SelectCommand.ExecuteNonQuery();
                da.Fill(ds, "Places");
                cn.Close();
                sector = ds.Tables["Places"].Rows[0].ItemArray[1].ToString();
                row = ds.Tables["Places"].Rows[0].ItemArray[2].ToString();
                seat = ds.Tables["Places"].Rows[0].ItemArray[3].ToString();

                cn.Open();
                ds = new DataSet();
                da = new SqlDataAdapter();
                da.SelectCommand = cn.CreateCommand();
                da.SelectCommand.CommandText = @"select Prices.* from Prices where session=" + idSession + " and sector=" + sector;
                da.SelectCommand.ExecuteNonQuery();
                da.Fill(ds, "Prices");
                cn.Close();
                string s = ds.Tables["Prices"].Rows[0].ItemArray[3].ToString();
                s = s.Substring(0, s.IndexOf(","));
                price = int.Parse(s);
                toolTip1.SetToolTip(((Button)sender), row + " ряд, " + seat + @" место
" + price);
            }
        }

        void seatMouseEnter2(object sender, EventArgs e)
        {
            if (((Button)sender).BackColor == colorActive || ((Button)sender).BackColor == Control.DefaultBackColor || MainForm.blnPass)
            {
                locationX = ((Button)sender).Location.X;
                LocationY = ((Button)sender).Location.Y;
               // ((Button)sender).Size = new System.Drawing.Size(60, 60);
                //((Button)sender).Location = new System.Drawing.Point(locationX - 10, LocationY - 10);
                string idPlace = ((Button)sender).Name.Replace("p", "");
                cn.Open();
                ds = new DataSet();
                da = new SqlDataAdapter();
                da.SelectCommand = cn.CreateCommand();
                da.SelectCommand.CommandText = @"select Places.* from Places where id_place=" + idPlace;
                da.SelectCommand.ExecuteNonQuery();
                da.Fill(ds, "Places");
                cn.Close();
                sector = ds.Tables["Places"].Rows[0].ItemArray[1].ToString();
                row = ds.Tables["Places"].Rows[0].ItemArray[2].ToString();
                seat = ds.Tables["Places"].Rows[0].ItemArray[3].ToString();

                cn.Open();
                ds = new DataSet();
                da = new SqlDataAdapter();
                da.SelectCommand = cn.CreateCommand();
                da.SelectCommand.CommandText = @"select Prices.* from Prices where session=" + idSession + " and sector=" + sector;
                da.SelectCommand.ExecuteNonQuery();
                da.Fill(ds, "Prices");
                cn.Close();
                string s = ds.Tables["Prices"].Rows[0].ItemArray[3].ToString();
                s = s.Substring(0, s.IndexOf(","));
                price = int.Parse(s);
                toolTip1.SetToolTip(((Button)sender), row + " ряд, " + seat + @" место
" + price);
            }
        }

        void label6_Click(object sender, EventArgs e)
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


        #region hall

        void Rollback()
        {
            tel = "";
            listBox3.Items.Clear();
            label66.Text = "0";
            label67.Text = "0";
            splitContainer2.Panel2Collapsed = true;
            sum = 0;

            if (idUser != null)
            {
                cn.Open();
                ds = new DataSet();
                da = new SqlDataAdapter();

                da.UpdateCommand = cn.CreateCommand();
                da.UpdateCommand.CommandText = @"UPDATE Tickets
SET reservation = reservationTemp, paid = paidTemp, sale_date=null
from Tickets inner join UsersTicket on Tickets.id_ticket=UsersTicket.id_ticket
where session=" + idSession + " and id_user=" + idUser;
                da.UpdateCommand.ExecuteNonQuery();
                da.UpdateCommand.CommandText = @"UPDATE UsersData
SET redakt='False'
where id_user=" + idUser;
                da.UpdateCommand.ExecuteNonQuery();
                da.SelectCommand = cn.CreateCommand();
                da.SelectCommand.CommandText = @"select Tickets.* from Tickets inner join UsersTicket on Tickets.id_ticket=UsersTicket.id_ticket
where session=" + idSession + " and id_user=" + idUser;
                da.SelectCommand.ExecuteNonQuery();
                da.Fill(ds, "Tickets");
                int rc = ds.Tables["Tickets"].Rows.Count;

                for (int i = 0; i < rc; i++)
                {
                    if (ds.Tables["Tickets"].Rows[i].ItemArray[6].ToString() == "False" && ds.Tables["Tickets"].Rows[i].ItemArray[7].ToString() == "False")
                    {
                        da.DeleteCommand = cn.CreateCommand();
                        da.DeleteCommand.CommandText = @"delete from UsersTicket
where id_ticket=" + ds.Tables["Tickets"].Rows[i].ItemArray[0].ToString() + " and id_user=" + idUser;
                        da.DeleteCommand.ExecuteNonQuery();
                    }
                }
                da.DeleteCommand = cn.CreateCommand();
                da.DeleteCommand.CommandText = @"delete from UsersData
where redakt='False' and id_user not in (select id_user from UsersTicket)";
                da.DeleteCommand.ExecuteNonQuery();
                cn.Close();
                idUser = null;
            }
        }

        void Paid()
        {
            if (idTicket != null && idUser != null)
            {
                cn.Open();
                da = new SqlDataAdapter();
                da.UpdateCommand = cn.CreateCommand();
                da.UpdateCommand.CommandText = @"UPDATE Tickets
SET reservation='false', paid='true',reservationTemp='false', paidTemp='true', sale_date='" + DateTime.Today + @"'
from Tickets inner join UsersTicket on Tickets.id_ticket=UsersTicket.id_ticket
where session=" + idSession + " and id_user=" + idUser;
                da.UpdateCommand.ExecuteNonQuery();
                da.UpdateCommand.CommandText = @"UPDATE UsersData
SET redakt='False'
where id_user=" + idUser;
                da.UpdateCommand.ExecuteNonQuery();
                cn.Close();
            }
            idUser = null;
            tel = "";
            listBox3.Items.Clear();
            label66.Text = "0";
            label67.Text = "0";
            sum = 0;
            RefreshHall();
        }

        void Reserv()
        {
            if (idTicket != null && idUser != null)
            {
                cn.Open();
                da = new SqlDataAdapter();
                da.UpdateCommand = cn.CreateCommand();
                da.UpdateCommand.CommandText = @"UPDATE Tickets
SET reservationTemp=reservation, paidTemp=paid, sale_date='"+DateTime.Now+@"'
from Tickets inner join UsersTicket on Tickets.id_ticket=UsersTicket.id_ticket
where session=" + idSession + " and id_user=" + idUser;
                da.UpdateCommand.ExecuteNonQuery();
                da.UpdateCommand.CommandText = @"UPDATE UsersData
SET redakt='False'
where id_user=" + idUser;
                da.UpdateCommand.ExecuteNonQuery();

                da.SelectCommand = cn.CreateCommand();
                da.SelectCommand.CommandText = @"select Tickets.* from Tickets inner join UsersTicket on Tickets.id_ticket=UsersTicket.id_ticket
where session=" + idSession + " and id_user=" + idUser;
                da.SelectCommand.ExecuteNonQuery();
                da.Fill(ds, "Tickets");
                int rc = ds.Tables["Tickets"].Rows.Count;

                for (int i = 0; i < rc; i++)
                {
                    if (ds.Tables["Tickets"].Rows[i].ItemArray[6].ToString() == "False" && ds.Tables["Tickets"].Rows[i].ItemArray[7].ToString() == "False")
                    {
                        da.DeleteCommand = cn.CreateCommand();
//                        da.DeleteCommand.CommandText = @"delete from UsersTicket
//where id_ticket=" + ds.Tables["Tickets"].Rows[i].ItemArray[0].ToString() + " and id_user=" + idUser;
                        da.DeleteCommand.CommandText = @"delete from UsersTicket
where id_ticket=" + ds.Tables["Tickets"].Rows[i].ItemArray[0].ToString();
                        da.DeleteCommand.ExecuteNonQuery();
                    }
                }          
     
                da.DeleteCommand = cn.CreateCommand();
                da.DeleteCommand.CommandText = @"delete from UsersData
where redakt='False' and id_user not in (select id_user from UsersTicket)";
                da.DeleteCommand.ExecuteNonQuery();
                cn.Close();
            }
            idUser = null;
            tel = "";
            listBox3.Items.Clear();
            label66.Text = "0";
            label67.Text = "0";
            sum = 0;
            RefreshHall();
        }

        void RefreshHall()
        {
            InitHall();           

            if (idUser != null)
            {
                cn.Open();
                ds = new DataSet();
                da = new SqlDataAdapter();
                da.SelectCommand = cn.CreateCommand();
                da.SelectCommand.CommandText = @"select * from Tickets inner join UsersTicket on Tickets.id_ticket=UsersTicket.id_ticket
where session=" + idSession + " and id_user=" + idUser;
                da.SelectCommand.ExecuteNonQuery();
                da.Fill(ds, "Tickets");
                cn.Close();
                int rc = ds.Tables["Tickets"].Rows.Count;
                for (int i = 0; i < rc; i++)
                {
                    if (ds.Tables["Tickets"].Rows[i].ItemArray[5].ToString() == "True")
                    {
                        switch (ds.Tables["Tickets"].Rows[i].ItemArray[3].ToString())
                        {
                            case "2001": p2001.BackColor = colorActive; break;
                            case "2002": p2002.BackColor = colorActive; break;
                            case "2003": p2003.BackColor = colorActive; break;
                            case "2004": p2004.BackColor = colorActive; break;
                            case "2005": p2005.BackColor = colorActive; break;
                            case "2006": p2006.BackColor = colorActive; break;
                            case "2007": p2007.BackColor = colorActive; break;
                            case "2008": p2008.BackColor = colorActive; break;
                            case "2009": p2009.BackColor = colorActive; break;
                            case "2010": p2010.BackColor = colorActive; break;
                            case "2011": p2011.BackColor = colorActive; break;
                            case "2012": p2012.BackColor = colorActive; break;
                            case "2013": p2013.BackColor = colorActive; break;
                            case "2014": p2014.BackColor = colorActive; break;
                            case "2015": p2015.BackColor = colorActive; break;
                            case "2016": p2016.BackColor = colorActive; break;
                            case "2017": p2017.BackColor = colorActive; break;
                            case "2018": p2018.BackColor = colorActive; break;
                            case "2019": p2019.BackColor = colorActive; break;
                            case "2020": p2020.BackColor = colorActive; break;
                            case "2021": p2021.BackColor = colorActive; break;
                            case "2022": p2022.BackColor = colorActive; break;
                            case "2023": p2023.BackColor = colorActive; break;
                            case "2024": p2024.BackColor = colorActive; break;
                            case "2025": p2025.BackColor = colorActive; break;
                            case "2026": p2026.BackColor = colorActive; break;
                            case "2027": p2027.BackColor = colorActive; break;
                            case "2028": p2028.BackColor = colorActive; break;
                            case "2029": p2029.BackColor = colorActive; break;
                            case "2030": p2030.BackColor = colorActive; break;
                            case "2031": p2031.BackColor = colorActive; break;
                            case "2032": p2032.BackColor = colorActive; break;
                            case "2033": p2033.BackColor = colorActive; break;
                            case "2034": p2034.BackColor = colorActive; break;
                            case "2035": p2035.BackColor = colorActive; break;
                            case "2036": p2036.BackColor = colorActive; break;
                            case "2037": p2037.BackColor = colorActive; break;
                            case "2038": p2038.BackColor = colorActive; break;
                            case "2039": p2039.BackColor = colorActive; break;
                            case "2040": p2040.BackColor = colorActive; break;
                            case "2041": p2041.BackColor = colorActive; break;
                            case "2042": p2042.BackColor = colorActive; break;
                            case "2043": p2043.BackColor = colorActive; break;
                            case "2044": p2044.BackColor = colorActive; break;
                            case "2045": p2045.BackColor = colorActive; break;
                            case "2046": p2046.BackColor = colorActive; break;
                            case "2047": p2047.BackColor = colorActive; break;
                            case "2048": p2048.BackColor = colorActive; break;
                            case "2049": p2049.BackColor = colorActive; break;
                            case "2050": p2050.BackColor = colorActive; break;
                            case "1001": p1001.BackColor = colorActive; break;
                            case "1002": p1002.BackColor = colorActive; break;
                            case "1003": p1003.BackColor = colorActive; break;
                            case "1004": p1004.BackColor = colorActive; break;
                            case "1005": p1005.BackColor = colorActive; break;
                            case "1006": p1006.BackColor = colorActive; break;
                            case "1007": p1007.BackColor = colorActive; break;
                            case "1008": p1008.BackColor = colorActive; break;
                            case "1009": p1009.BackColor = colorActive; break;
                            case "1010": p1010.BackColor = colorActive; break;
                            case "1011": p1011.BackColor = colorActive; break;
                            case "1012": p1012.BackColor = colorActive; break;
                            case "1013": p1013.BackColor = colorActive; break;
                            case "1014": p1014.BackColor = colorActive; break;
                            case "1015": p1015.BackColor = colorActive; break;
                            case "1016": p1016.BackColor = colorActive; break;
                            case "1017": p1017.BackColor = colorActive; break;
                            case "1018": p1018.BackColor = colorActive; break;
                            case "1019": p1019.BackColor = colorActive; break;
                            case "1020": p1020.BackColor = colorActive; break;
                            case "1021": p1021.BackColor = colorActive; break;
                            case "1022": p1022.BackColor = colorActive; break;
                            case "1023": p1023.BackColor = colorActive; break;
                            case "1024": p1024.BackColor = colorActive; break;
                            case "1025": p1025.BackColor = colorActive; break;
                            case "1026": p1026.BackColor = colorActive; break;
                            case "1027": p1027.BackColor = colorActive; break;
                            case "1028": p1028.BackColor = colorActive; break;
                            case "1029": p1029.BackColor = colorActive; break;
                            case "1030": p1030.BackColor = colorActive; break;
                            case "1031": p1031.BackColor = colorActive; break;
                            case "1032": p1032.BackColor = colorActive; break;
                            case "1033": p1033.BackColor = colorActive; break;
                            case "1034": p1034.BackColor = colorActive; break;
                            case "1035": p1035.BackColor = colorActive; break;
                            case "1036": p1036.BackColor = colorActive; break;
                            case "1037": p1037.BackColor = colorActive; break;
                            case "1038": p1038.BackColor = colorActive; break;
                            case "1039": p1039.BackColor = colorActive; break;
                            case "1040": p1040.BackColor = colorActive; break;
                            case "1041": p1041.BackColor = colorActive; break;
                            case "1042": p1042.BackColor = colorActive; break;
                            case "1043": p1043.BackColor = colorActive; break;
                            case "1044": p1044.BackColor = colorActive; break;
                            case "1045": p1045.BackColor = colorActive; break;
                            case "1046": p1046.BackColor = colorActive; break;
                            case "1047": p1047.BackColor = colorActive; break;
                            case "1048": p1048.BackColor = colorActive; break;
                            case "1049": p1049.BackColor = colorActive; break;
                            case "1050": p1050.BackColor = colorActive; break;
                            case "1051": p1051.BackColor = colorActive; break;
                            case "1052": p1052.BackColor = colorActive; break;
                            case "1053": p1053.BackColor = colorActive; break;
                            case "1054": p1054.BackColor = colorActive; break;
                            case "1055": p1055.BackColor = colorActive; break;
                            case "1056": p1056.BackColor = colorActive; break;
                            case "1057": p1057.BackColor = colorActive; break;
                            case "1058": p1058.BackColor = colorActive; break;
                            case "1059": p1059.BackColor = colorActive; break;
                            case "1060": p1060.BackColor = colorActive; break;
                            case "1061": p1061.BackColor = colorActive; break;
                            case "1062": p1062.BackColor = colorActive; break;
                            case "1063": p1063.BackColor = colorActive; break;
                            case "1064": p1064.BackColor = colorActive; break;
                            case "1065": p1065.BackColor = colorActive; break;
                            case "1066": p1066.BackColor = colorActive; break;
                            case "1067": p1067.BackColor = colorActive; break;
                            case "1068": p1068.BackColor = colorActive; break;
                            case "1069": p1069.BackColor = colorActive; break;
                            case "1070": p1070.BackColor = colorActive; break;
                            case "1071": p1071.BackColor = colorActive; break;
                            case "1072": p1072.BackColor = colorActive; break;
                            case "1073": p1073.BackColor = colorActive; break;
                            case "1074": p1074.BackColor = colorActive; break;
                            case "1075": p1075.BackColor = colorActive; break;
                            case "1076": p1076.BackColor = colorActive; break;
                            case "1077": p1077.BackColor = colorActive; break;
                            case "1078": p1078.BackColor = colorActive; break;
                            case "1079": p1079.BackColor = colorActive; break;
                            case "1080": p1080.BackColor = colorActive; break;
                            case "1081": p1081.BackColor = colorActive; break;
                            case "1082": p1082.BackColor = colorActive; break;
                            case "1083": p1083.BackColor = colorActive; break;
                            case "1084": p1084.BackColor = colorActive; break;
                            case "1085": p1085.BackColor = colorActive; break;
                            case "1086": p1086.BackColor = colorActive; break;
                            case "1087": p1087.BackColor = colorActive; break;
                            case "1088": p1088.BackColor = colorActive; break;
                            case "1089": p1089.BackColor = colorActive; break;
                            case "1090": p1090.BackColor = colorActive; break;
                            case "1091": p1091.BackColor = colorActive; break;
                            case "1092": p1092.BackColor = colorActive; break;
                            case "1093": p1093.BackColor = colorActive; break;
                            case "1094": p1094.BackColor = colorActive; break;
                            case "1095": p1095.BackColor = colorActive; break;
                            case "1096": p1096.BackColor = colorActive; break;
                            case "1097": p1097.BackColor = colorActive; break;
                            case "1098": p1098.BackColor = colorActive; break;
                            case "1099": p1099.BackColor = colorActive; break;
                            case "1100": p1100.BackColor = colorActive; break;
                            case "1101": p1101.BackColor = colorActive; break;
                            case "1102": p1102.BackColor = colorActive; break;
                            case "1103": p1103.BackColor = colorActive; break;
                            case "1104": p1104.BackColor = colorActive; break;
                            case "1105": p1105.BackColor = colorActive; break;
                            case "1106": p1106.BackColor = colorActive; break;
                            case "1107": p1107.BackColor = colorActive; break;
                            case "1108": p1108.BackColor = colorActive; break;
                            case "1109": p1109.BackColor = colorActive; break;
                            case "1110": p1110.BackColor = colorActive; break;
                            case "1111": p1111.BackColor = colorActive; break;
                            case "1112": p1112.BackColor = colorActive; break;
                            case "1113": p1113.BackColor = colorActive; break;
                            case "1114": p1114.BackColor = colorActive; break;
                            case "1115": p1115.BackColor = colorActive; break;
                            case "1116": p1116.BackColor = colorActive; break;
                            case "1117": p1117.BackColor = colorActive; break;
                            case "1118": p1118.BackColor = colorActive; break;
                            case "1119": p1119.BackColor = colorActive; break;
                            case "1120": p1120.BackColor = colorActive; break;
                            case "1121": p1121.BackColor = colorActive; break;
                            case "1122": p1122.BackColor = colorActive; break;
                            case "1123": p1123.BackColor = colorActive; break;
                            case "1124": p1124.BackColor = colorActive; break;
                            case "1125": p1125.BackColor = colorActive; break;
                            case "1126": p1126.BackColor = colorActive; break;
                            case "1127": p1127.BackColor = colorActive; break;
                            case "1128": p1128.BackColor = colorActive; break;
                            case "1129": p1129.BackColor = colorActive; break;
                            case "1130": p1130.BackColor = colorActive; break;
                            case "1131": p1131.BackColor = colorActive; break;
                            case "1132": p1132.BackColor = colorActive; break;
                            case "1133": p1133.BackColor = colorActive; break;
                            case "1134": p1134.BackColor = colorActive; break;
                            case "1135": p1135.BackColor = colorActive; break;
                            case "1136": p1136.BackColor = colorActive; break;
                            case "1137": p1137.BackColor = colorActive; break;
                            case "1138": p1138.BackColor = colorActive; break;
                            case "1139": p1139.BackColor = colorActive; break;
                            case "1140": p1140.BackColor = colorActive; break;
                            case "1141": p1141.BackColor = colorActive; break;
                            case "1142": p1142.BackColor = colorActive; break;
                            case "1143": p1143.BackColor = colorActive; break;
                            case "1144": p1144.BackColor = colorActive; break;
                            case "1145": p1145.BackColor = colorActive; break;
                            case "1146": p1146.BackColor = colorActive; break;
                            case "1147": p1147.BackColor = colorActive; break;
                            case "1148": p1148.BackColor = colorActive; break;
                            case "1149": p1149.BackColor = colorActive; break;
                            case "1150": p1150.BackColor = colorActive; break;
                            case "1151": p1151.BackColor = colorActive; break;
                            case "1152": p1152.BackColor = colorActive; break;
                            case "1153": p1153.BackColor = colorActive; break;
                            case "1154": p1154.BackColor = colorActive; break;
                            case "1155": p1155.BackColor = colorActive; break;
                            case "1156": p1156.BackColor = colorActive; break;
                            case "1157": p1157.BackColor = colorActive; break;
                            case "1158": p1158.BackColor = colorActive; break;
                            case "1159": p1159.BackColor = colorActive; break;
                            case "1160": p1160.BackColor = colorActive; break;
                            case "1161": p1161.BackColor = colorActive; break;
                            case "1162": p1162.BackColor = colorActive; break;
                            case "1163": p1163.BackColor = colorActive; break;
                            case "1164": p1164.BackColor = colorActive; break;
                            case "1165": p1165.BackColor = colorActive; break;
                            case "1166": p1166.BackColor = colorActive; break;
                            case "1167": p1167.BackColor = colorActive; break;
                            case "1168": p1168.BackColor = colorActive; break;
                            case "1169": p1169.BackColor = colorActive; break;
                            case "1170": p1170.BackColor = colorActive; break;
                            case "1171": p1171.BackColor = colorActive; break;
                            case "1172": p1172.BackColor = colorActive; break;
                            case "1173": p1173.BackColor = colorActive; break;
                            case "1174": p1174.BackColor = colorActive; break;
                            case "1175": p1175.BackColor = colorActive; break;
                            case "1176": p1176.BackColor = colorActive; break;
                            case "1177": p1177.BackColor = colorActive; break;
                            case "1178": p1178.BackColor = colorActive; break;
                            case "1179": p1179.BackColor = colorActive; break;
                            case "1180": p1180.BackColor = colorActive; break;
                            case "1181": p1181.BackColor = colorActive; break;
                            case "1182": p1182.BackColor = colorActive; break;
                            case "1183": p1183.BackColor = colorActive; break;
                            case "1184": p1184.BackColor = colorActive; break;
                            case "1185": p1185.BackColor = colorActive; break;
                            case "1186": p1186.BackColor = colorActive; break;
                            case "1187": p1187.BackColor = colorActive; break;
                            case "1188": p1188.BackColor = colorActive; break;
                            case "1189": p1189.BackColor = colorActive; break;
                            case "1190": p1190.BackColor = colorActive; break;
                            case "1191": p1191.BackColor = colorActive; break;
                            case "1192": p1192.BackColor = colorActive; break;
                            case "1193": p1193.BackColor = colorActive; break;
                            case "1194": p1194.BackColor = colorActive; break;
                            case "1195": p1195.BackColor = colorActive; break;
                            case "1196": p1196.BackColor = colorActive; break;
                            case "1197": p1197.BackColor = colorActive; break;
                            case "1198": p1198.BackColor = colorActive; break;
                            case "1199": p1199.BackColor = colorActive; break;
                            case "1200": p1200.BackColor = colorActive; break;
                            case "1201": p1201.BackColor = colorActive; break;
                            case "1202": p1202.BackColor = colorActive; break;
                            case "1203": p1203.BackColor = colorActive; break;
                            case "1204": p1204.BackColor = colorActive; break;
                            case "1205": p1205.BackColor = colorActive; break;
                            case "1206": p1206.BackColor = colorActive; break;
                            case "1207": p1207.BackColor = colorActive; break;
                            case "1208": p1208.BackColor = colorActive; break;
                            case "1209": p1209.BackColor = colorActive; break;
                            case "1210": p1210.BackColor = colorActive; break;
                            case "1211": p1211.BackColor = colorActive; break;
                            case "1212": p1212.BackColor = colorActive; break;
                            case "1213": p1213.BackColor = colorActive; break;
                            case "1214": p1214.BackColor = colorActive; break;
                            case "1215": p1215.BackColor = colorActive; break;
                            case "1216": p1216.BackColor = colorActive; break;
                            case "1217": p1217.BackColor = colorActive; break;
                            case "1218": p1218.BackColor = colorActive; break;
                            case "1219": p1219.BackColor = colorActive; break;
                            case "1220": p1220.BackColor = colorActive; break;
                            case "1221": p1221.BackColor = colorActive; break;
                            case "1222": p1222.BackColor = colorActive; break;
                            case "1223": p1223.BackColor = colorActive; break;
                            case "1224": p1224.BackColor = colorActive; break;
                            case "1225": p1225.BackColor = colorActive; break;
                            case "1226": p1226.BackColor = colorActive; break;
                            case "1227": p1227.BackColor = colorActive; break;
                            case "1228": p1228.BackColor = colorActive; break;
                            case "1229": p1229.BackColor = colorActive; break;
                            case "1230": p1230.BackColor = colorActive; break;
                            case "1231": p1231.BackColor = colorActive; break;
                            case "1232": p1232.BackColor = colorActive; break;
                            case "1233": p1233.BackColor = colorActive; break;
                            case "1234": p1234.BackColor = colorActive; break;
                            case "1235": p1235.BackColor = colorActive; break;
                            case "1236": p1236.BackColor = colorActive; break;
                            case "1237": p1237.BackColor = colorActive; break;
                            case "1238": p1238.BackColor = colorActive; break;
                            case "1239": p1239.BackColor = colorActive; break;
                            case "1240": p1240.BackColor = colorActive; break;
                            case "1241": p1241.BackColor = colorActive; break;
                            case "1242": p1242.BackColor = colorActive; break;
                            case "1243": p1243.BackColor = colorActive; break;
                            case "1244": p1244.BackColor = colorActive; break;
                            case "1245": p1245.BackColor = colorActive; break;
                            case "1246": p1246.BackColor = colorActive; break;
                            case "1247": p1247.BackColor = colorActive; break;
                            case "1248": p1248.BackColor = colorActive; break;
                            case "1249": p1249.BackColor = colorActive; break;
                            case "1250": p1250.BackColor = colorActive; break;
                            case "1251": p1251.BackColor = colorActive; break;
                            case "1252": p1252.BackColor = colorActive; break;
                            case "1253": p1253.BackColor = colorActive; break;
                            case "1254": p1254.BackColor = colorActive; break;
                            case "1255": p1255.BackColor = colorActive; break;
                            case "1256": p1256.BackColor = colorActive; break;
                            case "1257": p1257.BackColor = colorActive; break;
                            case "1258": p1258.BackColor = colorActive; break;
                            case "1259": p1259.BackColor = colorActive; break;
                            case "1260": p1260.BackColor = colorActive; break;
                            case "1261": p1261.BackColor = colorActive; break;
                            case "1262": p1262.BackColor = colorActive; break;
                            case "1263": p1263.BackColor = colorActive; break;
                            case "1264": p1264.BackColor = colorActive; break;
                            case "1265": p1265.BackColor = colorActive; break;
                            case "1266": p1266.BackColor = colorActive; break;
                            case "1267": p1267.BackColor = colorActive; break;
                            case "1268": p1268.BackColor = colorActive; break;
                            case "1269": p1269.BackColor = colorActive; break;
                            case "1270": p1270.BackColor = colorActive; break;
                            case "1271": p1271.BackColor = colorActive; break;
                            case "1272": p1272.BackColor = colorActive; break;
                            case "1273": p1273.BackColor = colorActive; break;
                            case "1274": p1274.BackColor = colorActive; break;
                            case "1275": p1275.BackColor = colorActive; break;
                            case "1276": p1276.BackColor = colorActive; break;
                            case "1277": p1277.BackColor = colorActive; break;
                            case "1278": p1278.BackColor = colorActive; break;
                            case "1279": p1279.BackColor = colorActive; break;
                            case "1280": p1280.BackColor = colorActive; break;
                            case "1281": p1281.BackColor = colorActive; break;
                            case "1282": p1282.BackColor = colorActive; break;
                            case "1283": p1283.BackColor = colorActive; break;
                            case "1284": p1284.BackColor = colorActive; break;
                            case "1285": p1285.BackColor = colorActive; break;
                            case "1286": p1286.BackColor = colorActive; break;
                            case "1287": p1287.BackColor = colorActive; break;
                            case "1288": p1288.BackColor = colorActive; break;
                            case "1289": p1289.BackColor = colorActive; break;
                            case "1290": p1290.BackColor = colorActive; break;
                            case "1291": p1291.BackColor = colorActive; break;
                            case "1292": p1292.BackColor = colorActive; break;
                            case "1293": p1293.BackColor = colorActive; break;
                            case "1294": p1294.BackColor = colorActive; break;
                            case "1295": p1295.BackColor = colorActive; break;
                            case "1296": p1296.BackColor = colorActive; break;
                            case "1297": p1297.BackColor = colorActive; break;
                            case "1298": p1298.BackColor = colorActive; break;
                            case "1299": p1299.BackColor = colorActive; break;
                            case "1300": p1300.BackColor = colorActive; break;
                            case "1301": p1301.BackColor = colorActive; break;
                            case "1302": p1302.BackColor = colorActive; break;
                            case "1303": p1303.BackColor = colorActive; break;
                            case "1304": p1304.BackColor = colorActive; break;
                            case "1305": p1305.BackColor = colorActive; break;
                            case "1306": p1306.BackColor = colorActive; break;
                            case "1307": p1307.BackColor = colorActive; break;
                            case "1308": p1308.BackColor = colorActive; break;
                            case "1309": p1309.BackColor = colorActive; break;
                            case "1310": p1310.BackColor = colorActive; break;
                            case "1311": p1311.BackColor = colorActive; break;
                            case "1312": p1312.BackColor = colorActive; break;
                            case "1313": p1313.BackColor = colorActive; break;
                            case "1314": p1314.BackColor = colorActive; break;
                            case "1315": p1315.BackColor = colorActive; break;
                            case "1316": p1316.BackColor = colorActive; break;
                            case "1317": p1317.BackColor = colorActive; break;
                            case "1318": p1318.BackColor = colorActive; break;
                            case "1319": p1319.BackColor = colorActive; break;
                            case "1320": p1320.BackColor = colorActive; break;
                            case "1321": p1321.BackColor = colorActive; break;
                            case "1322": p1322.BackColor = colorActive; break;
                            case "1323": p1323.BackColor = colorActive; break;
                            case "1324": p1324.BackColor = colorActive; break;
                            case "1325": p1325.BackColor = colorActive; break;
                            case "1326": p1326.BackColor = colorActive; break;
                            case "1327": p1327.BackColor = colorActive; break;
                            case "1328": p1328.BackColor = colorActive; break;
                            case "1329": p1329.BackColor = colorActive; break;
                            case "1330": p1330.BackColor = colorActive; break;
                            case "1331": p1331.BackColor = colorActive; break;
                            case "1332": p1332.BackColor = colorActive; break;
                            case "1333": p1333.BackColor = colorActive; break;
                            case "1334": p1334.BackColor = colorActive; break;
                            case "1335": p1335.BackColor = colorActive; break;
                            case "1336": p1336.BackColor = colorActive; break;
                            case "1337": p1337.BackColor = colorActive; break;
                            case "1338": p1338.BackColor = colorActive; break;
                            case "1339": p1339.BackColor = colorActive; break;
                            case "1340": p1340.BackColor = colorActive; break;
                            case "1341": p1341.BackColor = colorActive; break;
                            case "1342": p1342.BackColor = colorActive; break;
                            case "1343": p1343.BackColor = colorActive; break;
                            case "1344": p1344.BackColor = colorActive; break;
                            case "1345": p1345.BackColor = colorActive; break;
                            case "1346": p1346.BackColor = colorActive; break;
                            case "1347": p1347.BackColor = colorActive; break;
                            case "1348": p1348.BackColor = colorActive; break;
                            case "1349": p1349.BackColor = colorActive; break;
                            case "1350": p1350.BackColor = colorActive; break;
                            case "1351": p1351.BackColor = colorActive; break;
                            case "1352": p1352.BackColor = colorActive; break;
                            case "1353": p1353.BackColor = colorActive; break;
                            case "1354": p1354.BackColor = colorActive; break;
                            case "1355": p1355.BackColor = colorActive; break;
                            case "1356": p1356.BackColor = colorActive; break;
                            case "1357": p1357.BackColor = colorActive; break;
                            case "1358": p1358.BackColor = colorActive; break;
                            case "1359": p1359.BackColor = colorActive; break;
                            case "1360": p1360.BackColor = colorActive; break;
                            case "1361": p1361.BackColor = colorActive; break;
                            case "1362": p1362.BackColor = colorActive; break;
                            case "1363": p1363.BackColor = colorActive; break;
                            case "1364": p1364.BackColor = colorActive; break;
                            case "1365": p1365.BackColor = colorActive; break;
                            case "1366": p1366.BackColor = colorActive; break;
                            case "1367": p1367.BackColor = colorActive; break;
                            case "1368": p1368.BackColor = colorActive; break;
                            case "1369": p1369.BackColor = colorActive; break;
                            case "1370": p1370.BackColor = colorActive; break;
                            case "1371": p1371.BackColor = colorActive; break;
                            case "1372": p1372.BackColor = colorActive; break;
                            case "1373": p1373.BackColor = colorActive; break;
                            case "1374": p1374.BackColor = colorActive; break;
                            case "1375": p1375.BackColor = colorActive; break;
                            case "1376": p1376.BackColor = colorActive; break;
                            case "1377": p1377.BackColor = colorActive; break;
                            case "1378": p1378.BackColor = colorActive; break;
                            case "1379": p1379.BackColor = colorActive; break;
                            case "1380": p1380.BackColor = colorActive; break;
                            case "1381": p1381.BackColor = colorActive; break;
                            case "1382": p1382.BackColor = colorActive; break;
                            case "1383": p1383.BackColor = colorActive; break;
                            case "1384": p1384.BackColor = colorActive; break;
                            case "1385": p1385.BackColor = colorActive; break;
                            case "1386": p1386.BackColor = colorActive; break;
                            case "1387": p1387.BackColor = colorActive; break;
                            case "1388": p1388.BackColor = colorActive; break;
                            case "1389": p1389.BackColor = colorActive; break;
                            case "1390": p1390.BackColor = colorActive; break;
                            case "1391": p1391.BackColor = colorActive; break;
                            case "1392": p1392.BackColor = colorActive; break;
                            case "1393": p1393.BackColor = colorActive; break;
                            case "1394": p1394.BackColor = colorActive; break;
                            case "1395": p1395.BackColor = colorActive; break;
                            case "1396": p1396.BackColor = colorActive; break;
                            case "1397": p1397.BackColor = colorActive; break;
                            case "1398": p1398.BackColor = colorActive; break;
                            case "1399": p1399.BackColor = colorActive; break;
                            case "1400": p1400.BackColor = colorActive; break;
                            case "1401": p1401.BackColor = colorActive; break;
                            case "1402": p1402.BackColor = colorActive; break;
                            case "1403": p1403.BackColor = colorActive; break;
                            case "1404": p1404.BackColor = colorActive; break;
                            case "1405": p1405.BackColor = colorActive; break;
                            case "1406": p1406.BackColor = colorActive; break;
                            case "1407": p1407.BackColor = colorActive; break;
                            case "1408": p1408.BackColor = colorActive; break;
                            case "1409": p1409.BackColor = colorActive; break;
                            case "1410": p1410.BackColor = colorActive; break;
                            case "1411": p1411.BackColor = colorActive; break;
                            case "1412": p1412.BackColor = colorActive; break;
                            case "1413": p1413.BackColor = colorActive; break;
                            case "1414": p1414.BackColor = colorActive; break;
                            case "1415": p1415.BackColor = colorActive; break;
                            case "1416": p1416.BackColor = colorActive; break;
                            case "1417": p1417.BackColor = colorActive; break;
                            case "1418": p1418.BackColor = colorActive; break;
                            case "1419": p1419.BackColor = colorActive; break;
                            case "1420": p1420.BackColor = colorActive; break;
                            case "1421": p1421.BackColor = colorActive; break;
                            case "1422": p1422.BackColor = colorActive; break;
                            case "1423": p1423.BackColor = colorActive; break;
                            case "1424": p1424.BackColor = colorActive; break;
                            case "1425": p1425.BackColor = colorActive; break;
                            case "1426": p1426.BackColor = colorActive; break;
                            case "1427": p1427.BackColor = colorActive; break;
                            case "1428": p1428.BackColor = colorActive; break;
                            case "1429": p1429.BackColor = colorActive; break;
                            case "1430": p1430.BackColor = colorActive; break;
                            case "1431": p1431.BackColor = colorActive; break;
                            case "1432": p1432.BackColor = colorActive; break;
                            case "1433": p1433.BackColor = colorActive; break;
                            case "1434": p1434.BackColor = colorActive; break;
                            case "1435": p1435.BackColor = colorActive; break;
                            case "1436": p1436.BackColor = colorActive; break;
                            case "1437": p1437.BackColor = colorActive; break;
                            case "1438": p1438.BackColor = colorActive; break;
                            case "1439": p1439.BackColor = colorActive; break;
                            case "1440": p1440.BackColor = colorActive; break;
                            case "1441": p1441.BackColor = colorActive; break;
                            case "1442": p1442.BackColor = colorActive; break;
                            case "1443": p1443.BackColor = colorActive; break;
                            case "1444": p1444.BackColor = colorActive; break;
                            case "1445": p1445.BackColor = colorActive; break;
                            case "1446": p1446.BackColor = colorActive; break;
                            case "1447": p1447.BackColor = colorActive; break;
                            case "1448": p1448.BackColor = colorActive; break;
                            case "1449": p1449.BackColor = colorActive; break;
                            case "1450": p1450.BackColor = colorActive; break;
                            case "1451": p1451.BackColor = colorActive; break;
                            case "1452": p1452.BackColor = colorActive; break;
                            case "1453": p1453.BackColor = colorActive; break;
                            case "1454": p1454.BackColor = colorActive; break;
                            case "1455": p1455.BackColor = colorActive; break;
                            case "1456": p1456.BackColor = colorActive; break;
                            case "1457": p1457.BackColor = colorActive; break;
                            case "1458": p1458.BackColor = colorActive; break;
                            case "1459": p1459.BackColor = colorActive; break;
                            case "1460": p1460.BackColor = colorActive; break;
                            case "1461": p1461.BackColor = colorActive; break;
                            case "1462": p1462.BackColor = colorActive; break;
                            case "1463": p1463.BackColor = colorActive; break;
                            case "1464": p1464.BackColor = colorActive; break;
                            case "1465": p1465.BackColor = colorActive; break;
                            case "1466": p1466.BackColor = colorActive; break;
                            case "1467": p1467.BackColor = colorActive; break;
                            case "1468": p1468.BackColor = colorActive; break;
                            case "1469": p1469.BackColor = colorActive; break;
                            case "1470": p1470.BackColor = colorActive; break;
                            case "1471": p1471.BackColor = colorActive; break;
                            case "1472": p1472.BackColor = colorActive; break;
                            case "1473": p1473.BackColor = colorActive; break;
                            case "1474": p1474.BackColor = colorActive; break;
                            case "1475": p1475.BackColor = colorActive; break;
                            case "1476": p1476.BackColor = colorActive; break;
                            case "1477": p1477.BackColor = colorActive; break;
                            case "1478": p1478.BackColor = colorActive; break;
                            case "1479": p1479.BackColor = colorActive; break;
                            case "1480": p1480.BackColor = colorActive; break;
                            case "1481": p1481.BackColor = colorActive; break;
                            case "1482": p1482.BackColor = colorActive; break;
                            case "1483": p1483.BackColor = colorActive; break;
                            case "1484": p1484.BackColor = colorActive; break;
                            case "1485": p1485.BackColor = colorActive; break;
                            case "1486": p1486.BackColor = colorActive; break;
                            case "1487": p1487.BackColor = colorActive; break;
                            case "1488": p1488.BackColor = colorActive; break;
                            case "1489": p1489.BackColor = colorActive; break;
                            case "1490": p1490.BackColor = colorActive; break;
                            case "1491": p1491.BackColor = colorActive; break;
                            case "1492": p1492.BackColor = colorActive; break;
                            case "1493": p1493.BackColor = colorActive; break;
                            case "1494": p1494.BackColor = colorActive; break;
                            case "1495": p1495.BackColor = colorActive; break;
                            case "1496": p1496.BackColor = colorActive; break;
                            case "1497": p1497.BackColor = colorActive; break;
                            case "1498": p1498.BackColor = colorActive; break;
                            case "1499": p1499.BackColor = colorActive; break;
                            case "1500": p1500.BackColor = colorActive; break;
                            case "1501": p1501.BackColor = colorActive; break;
                            case "1502": p1502.BackColor = colorActive; break;
                            case "1503": p1503.BackColor = colorActive; break;
                            case "1504": p1504.BackColor = colorActive; break;
                            case "1505": p1505.BackColor = colorActive; break;
                            case "1506": p1506.BackColor = colorActive; break;
                            case "1507": p1507.BackColor = colorActive; break;
                            case "1508": p1508.BackColor = colorActive; break;
                            case "1509": p1509.BackColor = colorActive; break;
                            case "1510": p1510.BackColor = colorActive; break;
                            case "1511": p1511.BackColor = colorActive; break;
                            case "1512": p1512.BackColor = colorActive; break;
                            case "1513": p1513.BackColor = colorActive; break;
                            case "1514": p1514.BackColor = colorActive; break;
                        }
                    }
                }
            }

        }

        void InitHall()
        {
            ClearSeat();

            DateTime dateN = DateTime.Now;
            cn.Open();
            da = new SqlDataAdapter();
            ds = new DataSet();
            da.UpdateCommand = cn.CreateCommand();
            da.UpdateCommand.CommandText = @"Update Tickets
set reservation='False',reservationTemp='False', sale_date=null
from UsersTicket inner join Tickets on Tickets.id_ticket=UsersTicket.id_ticket
inner join UsersData on UsersData.id_user=UsersTicket.id_user
where (CAST(dateUser as time)<'" + dateN.AddMinutes(-15).ToShortTimeString() + "' and CAST(dateUser as date)='" + dateN.ToShortDateString() + "') or CAST(dateUser as date)<'" + dateN.ToShortDateString() + "'";
            da.UpdateCommand.ExecuteNonQuery();
//            da.DeleteCommand = cn.CreateCommand();
//            da.DeleteCommand.CommandText = @"delete from UsersTicket
//where id_user in
//(select UsersTicket.id_user from UsersTicket inner join Tickets on Tickets.id_ticket=UsersTicket.id_ticket
//inner join UsersData on UsersData.id_user=UsersTicket.id_user
//where reservation='False' and ((CAST(dateUser as time)<'" + dateN.AddMinutes(-15).ToShortTimeString() + "' and CAST(dateUser as date)='" + dateN.ToShortDateString() + "') or CAST(dateUser as date)<'" + dateN.ToShortDateString() + "'))";
//            da.DeleteCommand.ExecuteNonQuery();
//            da.DeleteCommand.CommandText = @"delete from UsersData
//where redakt='False' and id_user not in
//(select id_user from UsersTicket)";
//            da.DeleteCommand.ExecuteNonQuery();

            da.SelectCommand = cn.CreateCommand();
            da.SelectCommand.CommandText = @"select Tickets.* from Tickets
where session=" + idSession;
            da.SelectCommand.ExecuteNonQuery();
            da.Fill(ds, "Tickets");
            int rc = ds.Tables["Tickets"].Rows.Count;

            for (int i = 0; i < rc; i++)
            {
                if (ds.Tables["Tickets"].Rows[i].ItemArray[6].ToString() == "False" && ds.Tables["Tickets"].Rows[i].ItemArray[7].ToString() == "False")
                {
                    da.DeleteCommand = cn.CreateCommand();
                    da.DeleteCommand.CommandText = @"delete from UsersTicket
where id_ticket=" + ds.Tables["Tickets"].Rows[i].ItemArray[0].ToString();
                    da.DeleteCommand.ExecuteNonQuery();
                }
            }
            da.DeleteCommand = cn.CreateCommand();
            da.DeleteCommand.CommandText = @"delete from UsersData
where redakt='False' and id_user not in (select id_user from UsersTicket)";
            da.DeleteCommand.ExecuteNonQuery();

            ds = new DataSet();
            da = new SqlDataAdapter();
            da.SelectCommand = cn.CreateCommand();
            da.SelectCommand.CommandText = @"select * from Tickets where session=" + idSession;
            da.SelectCommand.ExecuteNonQuery();
            da.Fill(ds, "Tickets");
            cn.Close();

            rc = ds.Tables["Tickets"].Rows.Count;
            for (int i = 0; i < rc; i++)
            {
                if (ds.Tables["Tickets"].Rows[i].ItemArray[5].ToString() == "True")
                {
                    switch (ds.Tables["Tickets"].Rows[i].ItemArray[3].ToString())
                    {
                        case "2001": p2001.BackColor = colorReservation; break;
                        case "2002": p2002.BackColor = colorReservation; break;
                        case "2003": p2003.BackColor = colorReservation; break;
                        case "2004": p2004.BackColor = colorReservation; break;
                        case "2005": p2005.BackColor = colorReservation; break;
                        case "2006": p2006.BackColor = colorReservation; break;
                        case "2007": p2007.BackColor = colorReservation; break;
                        case "2008": p2008.BackColor = colorReservation; break;
                        case "2009": p2009.BackColor = colorReservation; break;
                        case "2010": p2010.BackColor = colorReservation; break;
                        case "2011": p2011.BackColor = colorReservation; break;
                        case "2012": p2012.BackColor = colorReservation; break;
                        case "2013": p2013.BackColor = colorReservation; break;
                        case "2014": p2014.BackColor = colorReservation; break;
                        case "2015": p2015.BackColor = colorReservation; break;
                        case "2016": p2016.BackColor = colorReservation; break;
                        case "2017": p2017.BackColor = colorReservation; break;
                        case "2018": p2018.BackColor = colorReservation; break;
                        case "2019": p2019.BackColor = colorReservation; break;
                        case "2020": p2020.BackColor = colorReservation; break;
                        case "2021": p2021.BackColor = colorReservation; break;
                        case "2022": p2022.BackColor = colorReservation; break;
                        case "2023": p2023.BackColor = colorReservation; break;
                        case "2024": p2024.BackColor = colorReservation; break;
                        case "2025": p2025.BackColor = colorReservation; break;
                        case "2026": p2026.BackColor = colorReservation; break;
                        case "2027": p2027.BackColor = colorReservation; break;
                        case "2028": p2028.BackColor = colorReservation; break;
                        case "2029": p2029.BackColor = colorReservation; break;
                        case "2030": p2030.BackColor = colorReservation; break;
                        case "2031": p2031.BackColor = colorReservation; break;
                        case "2032": p2032.BackColor = colorReservation; break;
                        case "2033": p2033.BackColor = colorReservation; break;
                        case "2034": p2034.BackColor = colorReservation; break;
                        case "2035": p2035.BackColor = colorReservation; break;
                        case "2036": p2036.BackColor = colorReservation; break;
                        case "2037": p2037.BackColor = colorReservation; break;
                        case "2038": p2038.BackColor = colorReservation; break;
                        case "2039": p2039.BackColor = colorReservation; break;
                        case "2040": p2040.BackColor = colorReservation; break;
                        case "2041": p2041.BackColor = colorReservation; break;
                        case "2042": p2042.BackColor = colorReservation; break;
                        case "2043": p2043.BackColor = colorReservation; break;
                        case "2044": p2044.BackColor = colorReservation; break;
                        case "2045": p2045.BackColor = colorReservation; break;
                        case "2046": p2046.BackColor = colorReservation; break;
                        case "2047": p2047.BackColor = colorReservation; break;
                        case "2048": p2048.BackColor = colorReservation; break;
                        case "2049": p2049.BackColor = colorReservation; break;
                        case "2050": p2050.BackColor = colorReservation; break;
                        case "1001": p1001.BackColor = colorReservation; break;
                        case "1002": p1002.BackColor = colorReservation; break;
                        case "1003": p1003.BackColor = colorReservation; break;
                        case "1004": p1004.BackColor = colorReservation; break;
                        case "1005": p1005.BackColor = colorReservation; break;
                        case "1006": p1006.BackColor = colorReservation; break;
                        case "1007": p1007.BackColor = colorReservation; break;
                        case "1008": p1008.BackColor = colorReservation; break;
                        case "1009": p1009.BackColor = colorReservation; break;
                        case "1010": p1010.BackColor = colorReservation; break;
                        case "1011": p1011.BackColor = colorReservation; break;
                        case "1012": p1012.BackColor = colorReservation; break;
                        case "1013": p1013.BackColor = colorReservation; break;
                        case "1014": p1014.BackColor = colorReservation; break;
                        case "1015": p1015.BackColor = colorReservation; break;
                        case "1016": p1016.BackColor = colorReservation; break;
                        case "1017": p1017.BackColor = colorReservation; break;
                        case "1018": p1018.BackColor = colorReservation; break;
                        case "1019": p1019.BackColor = colorReservation; break;
                        case "1020": p1020.BackColor = colorReservation; break;
                        case "1021": p1021.BackColor = colorReservation; break;
                        case "1022": p1022.BackColor = colorReservation; break;
                        case "1023": p1023.BackColor = colorReservation; break;
                        case "1024": p1024.BackColor = colorReservation; break;
                        case "1025": p1025.BackColor = colorReservation; break;
                        case "1026": p1026.BackColor = colorReservation; break;
                        case "1027": p1027.BackColor = colorReservation; break;
                        case "1028": p1028.BackColor = colorReservation; break;
                        case "1029": p1029.BackColor = colorReservation; break;
                        case "1030": p1030.BackColor = colorReservation; break;
                        case "1031": p1031.BackColor = colorReservation; break;
                        case "1032": p1032.BackColor = colorReservation; break;
                        case "1033": p1033.BackColor = colorReservation; break;
                        case "1034": p1034.BackColor = colorReservation; break;
                        case "1035": p1035.BackColor = colorReservation; break;
                        case "1036": p1036.BackColor = colorReservation; break;
                        case "1037": p1037.BackColor = colorReservation; break;
                        case "1038": p1038.BackColor = colorReservation; break;
                        case "1039": p1039.BackColor = colorReservation; break;
                        case "1040": p1040.BackColor = colorReservation; break;
                        case "1041": p1041.BackColor = colorReservation; break;
                        case "1042": p1042.BackColor = colorReservation; break;
                        case "1043": p1043.BackColor = colorReservation; break;
                        case "1044": p1044.BackColor = colorReservation; break;
                        case "1045": p1045.BackColor = colorReservation; break;
                        case "1046": p1046.BackColor = colorReservation; break;
                        case "1047": p1047.BackColor = colorReservation; break;
                        case "1048": p1048.BackColor = colorReservation; break;
                        case "1049": p1049.BackColor = colorReservation; break;
                        case "1050": p1050.BackColor = colorReservation; break;
                        case "1051": p1051.BackColor = colorReservation; break;
                        case "1052": p1052.BackColor = colorReservation; break;
                        case "1053": p1053.BackColor = colorReservation; break;
                        case "1054": p1054.BackColor = colorReservation; break;
                        case "1055": p1055.BackColor = colorReservation; break;
                        case "1056": p1056.BackColor = colorReservation; break;
                        case "1057": p1057.BackColor = colorReservation; break;
                        case "1058": p1058.BackColor = colorReservation; break;
                        case "1059": p1059.BackColor = colorReservation; break;
                        case "1060": p1060.BackColor = colorReservation; break;
                        case "1061": p1061.BackColor = colorReservation; break;
                        case "1062": p1062.BackColor = colorReservation; break;
                        case "1063": p1063.BackColor = colorReservation; break;
                        case "1064": p1064.BackColor = colorReservation; break;
                        case "1065": p1065.BackColor = colorReservation; break;
                        case "1066": p1066.BackColor = colorReservation; break;
                        case "1067": p1067.BackColor = colorReservation; break;
                        case "1068": p1068.BackColor = colorReservation; break;
                        case "1069": p1069.BackColor = colorReservation; break;
                        case "1070": p1070.BackColor = colorReservation; break;
                        case "1071": p1071.BackColor = colorReservation; break;
                        case "1072": p1072.BackColor = colorReservation; break;
                        case "1073": p1073.BackColor = colorReservation; break;
                        case "1074": p1074.BackColor = colorReservation; break;
                        case "1075": p1075.BackColor = colorReservation; break;
                        case "1076": p1076.BackColor = colorReservation; break;
                        case "1077": p1077.BackColor = colorReservation; break;
                        case "1078": p1078.BackColor = colorReservation; break;
                        case "1079": p1079.BackColor = colorReservation; break;
                        case "1080": p1080.BackColor = colorReservation; break;
                        case "1081": p1081.BackColor = colorReservation; break;
                        case "1082": p1082.BackColor = colorReservation; break;
                        case "1083": p1083.BackColor = colorReservation; break;
                        case "1084": p1084.BackColor = colorReservation; break;
                        case "1085": p1085.BackColor = colorReservation; break;
                        case "1086": p1086.BackColor = colorReservation; break;
                        case "1087": p1087.BackColor = colorReservation; break;
                        case "1088": p1088.BackColor = colorReservation; break;
                        case "1089": p1089.BackColor = colorReservation; break;
                        case "1090": p1090.BackColor = colorReservation; break;
                        case "1091": p1091.BackColor = colorReservation; break;
                        case "1092": p1092.BackColor = colorReservation; break;
                        case "1093": p1093.BackColor = colorReservation; break;
                        case "1094": p1094.BackColor = colorReservation; break;
                        case "1095": p1095.BackColor = colorReservation; break;
                        case "1096": p1096.BackColor = colorReservation; break;
                        case "1097": p1097.BackColor = colorReservation; break;
                        case "1098": p1098.BackColor = colorReservation; break;
                        case "1099": p1099.BackColor = colorReservation; break;
                        case "1100": p1100.BackColor = colorReservation; break;
                        case "1101": p1101.BackColor = colorReservation; break;
                        case "1102": p1102.BackColor = colorReservation; break;
                        case "1103": p1103.BackColor = colorReservation; break;
                        case "1104": p1104.BackColor = colorReservation; break;
                        case "1105": p1105.BackColor = colorReservation; break;
                        case "1106": p1106.BackColor = colorReservation; break;
                        case "1107": p1107.BackColor = colorReservation; break;
                        case "1108": p1108.BackColor = colorReservation; break;
                        case "1109": p1109.BackColor = colorReservation; break;
                        case "1110": p1110.BackColor = colorReservation; break;
                        case "1111": p1111.BackColor = colorReservation; break;
                        case "1112": p1112.BackColor = colorReservation; break;
                        case "1113": p1113.BackColor = colorReservation; break;
                        case "1114": p1114.BackColor = colorReservation; break;
                        case "1115": p1115.BackColor = colorReservation; break;
                        case "1116": p1116.BackColor = colorReservation; break;
                        case "1117": p1117.BackColor = colorReservation; break;
                        case "1118": p1118.BackColor = colorReservation; break;
                        case "1119": p1119.BackColor = colorReservation; break;
                        case "1120": p1120.BackColor = colorReservation; break;
                        case "1121": p1121.BackColor = colorReservation; break;
                        case "1122": p1122.BackColor = colorReservation; break;
                        case "1123": p1123.BackColor = colorReservation; break;
                        case "1124": p1124.BackColor = colorReservation; break;
                        case "1125": p1125.BackColor = colorReservation; break;
                        case "1126": p1126.BackColor = colorReservation; break;
                        case "1127": p1127.BackColor = colorReservation; break;
                        case "1128": p1128.BackColor = colorReservation; break;
                        case "1129": p1129.BackColor = colorReservation; break;
                        case "1130": p1130.BackColor = colorReservation; break;
                        case "1131": p1131.BackColor = colorReservation; break;
                        case "1132": p1132.BackColor = colorReservation; break;
                        case "1133": p1133.BackColor = colorReservation; break;
                        case "1134": p1134.BackColor = colorReservation; break;
                        case "1135": p1135.BackColor = colorReservation; break;
                        case "1136": p1136.BackColor = colorReservation; break;
                        case "1137": p1137.BackColor = colorReservation; break;
                        case "1138": p1138.BackColor = colorReservation; break;
                        case "1139": p1139.BackColor = colorReservation; break;
                        case "1140": p1140.BackColor = colorReservation; break;
                        case "1141": p1141.BackColor = colorReservation; break;
                        case "1142": p1142.BackColor = colorReservation; break;
                        case "1143": p1143.BackColor = colorReservation; break;
                        case "1144": p1144.BackColor = colorReservation; break;
                        case "1145": p1145.BackColor = colorReservation; break;
                        case "1146": p1146.BackColor = colorReservation; break;
                        case "1147": p1147.BackColor = colorReservation; break;
                        case "1148": p1148.BackColor = colorReservation; break;
                        case "1149": p1149.BackColor = colorReservation; break;
                        case "1150": p1150.BackColor = colorReservation; break;
                        case "1151": p1151.BackColor = colorReservation; break;
                        case "1152": p1152.BackColor = colorReservation; break;
                        case "1153": p1153.BackColor = colorReservation; break;
                        case "1154": p1154.BackColor = colorReservation; break;
                        case "1155": p1155.BackColor = colorReservation; break;
                        case "1156": p1156.BackColor = colorReservation; break;
                        case "1157": p1157.BackColor = colorReservation; break;
                        case "1158": p1158.BackColor = colorReservation; break;
                        case "1159": p1159.BackColor = colorReservation; break;
                        case "1160": p1160.BackColor = colorReservation; break;
                        case "1161": p1161.BackColor = colorReservation; break;
                        case "1162": p1162.BackColor = colorReservation; break;
                        case "1163": p1163.BackColor = colorReservation; break;
                        case "1164": p1164.BackColor = colorReservation; break;
                        case "1165": p1165.BackColor = colorReservation; break;
                        case "1166": p1166.BackColor = colorReservation; break;
                        case "1167": p1167.BackColor = colorReservation; break;
                        case "1168": p1168.BackColor = colorReservation; break;
                        case "1169": p1169.BackColor = colorReservation; break;
                        case "1170": p1170.BackColor = colorReservation; break;
                        case "1171": p1171.BackColor = colorReservation; break;
                        case "1172": p1172.BackColor = colorReservation; break;
                        case "1173": p1173.BackColor = colorReservation; break;
                        case "1174": p1174.BackColor = colorReservation; break;
                        case "1175": p1175.BackColor = colorReservation; break;
                        case "1176": p1176.BackColor = colorReservation; break;
                        case "1177": p1177.BackColor = colorReservation; break;
                        case "1178": p1178.BackColor = colorReservation; break;
                        case "1179": p1179.BackColor = colorReservation; break;
                        case "1180": p1180.BackColor = colorReservation; break;
                        case "1181": p1181.BackColor = colorReservation; break;
                        case "1182": p1182.BackColor = colorReservation; break;
                        case "1183": p1183.BackColor = colorReservation; break;
                        case "1184": p1184.BackColor = colorReservation; break;
                        case "1185": p1185.BackColor = colorReservation; break;
                        case "1186": p1186.BackColor = colorReservation; break;
                        case "1187": p1187.BackColor = colorReservation; break;
                        case "1188": p1188.BackColor = colorReservation; break;
                        case "1189": p1189.BackColor = colorReservation; break;
                        case "1190": p1190.BackColor = colorReservation; break;
                        case "1191": p1191.BackColor = colorReservation; break;
                        case "1192": p1192.BackColor = colorReservation; break;
                        case "1193": p1193.BackColor = colorReservation; break;
                        case "1194": p1194.BackColor = colorReservation; break;
                        case "1195": p1195.BackColor = colorReservation; break;
                        case "1196": p1196.BackColor = colorReservation; break;
                        case "1197": p1197.BackColor = colorReservation; break;
                        case "1198": p1198.BackColor = colorReservation; break;
                        case "1199": p1199.BackColor = colorReservation; break;
                        case "1200": p1200.BackColor = colorReservation; break;
                        case "1201": p1201.BackColor = colorReservation; break;
                        case "1202": p1202.BackColor = colorReservation; break;
                        case "1203": p1203.BackColor = colorReservation; break;
                        case "1204": p1204.BackColor = colorReservation; break;
                        case "1205": p1205.BackColor = colorReservation; break;
                        case "1206": p1206.BackColor = colorReservation; break;
                        case "1207": p1207.BackColor = colorReservation; break;
                        case "1208": p1208.BackColor = colorReservation; break;
                        case "1209": p1209.BackColor = colorReservation; break;
                        case "1210": p1210.BackColor = colorReservation; break;
                        case "1211": p1211.BackColor = colorReservation; break;
                        case "1212": p1212.BackColor = colorReservation; break;
                        case "1213": p1213.BackColor = colorReservation; break;
                        case "1214": p1214.BackColor = colorReservation; break;
                        case "1215": p1215.BackColor = colorReservation; break;
                        case "1216": p1216.BackColor = colorReservation; break;
                        case "1217": p1217.BackColor = colorReservation; break;
                        case "1218": p1218.BackColor = colorReservation; break;
                        case "1219": p1219.BackColor = colorReservation; break;
                        case "1220": p1220.BackColor = colorReservation; break;
                        case "1221": p1221.BackColor = colorReservation; break;
                        case "1222": p1222.BackColor = colorReservation; break;
                        case "1223": p1223.BackColor = colorReservation; break;
                        case "1224": p1224.BackColor = colorReservation; break;
                        case "1225": p1225.BackColor = colorReservation; break;
                        case "1226": p1226.BackColor = colorReservation; break;
                        case "1227": p1227.BackColor = colorReservation; break;
                        case "1228": p1228.BackColor = colorReservation; break;
                        case "1229": p1229.BackColor = colorReservation; break;
                        case "1230": p1230.BackColor = colorReservation; break;
                        case "1231": p1231.BackColor = colorReservation; break;
                        case "1232": p1232.BackColor = colorReservation; break;
                        case "1233": p1233.BackColor = colorReservation; break;
                        case "1234": p1234.BackColor = colorReservation; break;
                        case "1235": p1235.BackColor = colorReservation; break;
                        case "1236": p1236.BackColor = colorReservation; break;
                        case "1237": p1237.BackColor = colorReservation; break;
                        case "1238": p1238.BackColor = colorReservation; break;
                        case "1239": p1239.BackColor = colorReservation; break;
                        case "1240": p1240.BackColor = colorReservation; break;
                        case "1241": p1241.BackColor = colorReservation; break;
                        case "1242": p1242.BackColor = colorReservation; break;
                        case "1243": p1243.BackColor = colorReservation; break;
                        case "1244": p1244.BackColor = colorReservation; break;
                        case "1245": p1245.BackColor = colorReservation; break;
                        case "1246": p1246.BackColor = colorReservation; break;
                        case "1247": p1247.BackColor = colorReservation; break;
                        case "1248": p1248.BackColor = colorReservation; break;
                        case "1249": p1249.BackColor = colorReservation; break;
                        case "1250": p1250.BackColor = colorReservation; break;
                        case "1251": p1251.BackColor = colorReservation; break;
                        case "1252": p1252.BackColor = colorReservation; break;
                        case "1253": p1253.BackColor = colorReservation; break;
                        case "1254": p1254.BackColor = colorReservation; break;
                        case "1255": p1255.BackColor = colorReservation; break;
                        case "1256": p1256.BackColor = colorReservation; break;
                        case "1257": p1257.BackColor = colorReservation; break;
                        case "1258": p1258.BackColor = colorReservation; break;
                        case "1259": p1259.BackColor = colorReservation; break;
                        case "1260": p1260.BackColor = colorReservation; break;
                        case "1261": p1261.BackColor = colorReservation; break;
                        case "1262": p1262.BackColor = colorReservation; break;
                        case "1263": p1263.BackColor = colorReservation; break;
                        case "1264": p1264.BackColor = colorReservation; break;
                        case "1265": p1265.BackColor = colorReservation; break;
                        case "1266": p1266.BackColor = colorReservation; break;
                        case "1267": p1267.BackColor = colorReservation; break;
                        case "1268": p1268.BackColor = colorReservation; break;
                        case "1269": p1269.BackColor = colorReservation; break;
                        case "1270": p1270.BackColor = colorReservation; break;
                        case "1271": p1271.BackColor = colorReservation; break;
                        case "1272": p1272.BackColor = colorReservation; break;
                        case "1273": p1273.BackColor = colorReservation; break;
                        case "1274": p1274.BackColor = colorReservation; break;
                        case "1275": p1275.BackColor = colorReservation; break;
                        case "1276": p1276.BackColor = colorReservation; break;
                        case "1277": p1277.BackColor = colorReservation; break;
                        case "1278": p1278.BackColor = colorReservation; break;
                        case "1279": p1279.BackColor = colorReservation; break;
                        case "1280": p1280.BackColor = colorReservation; break;
                        case "1281": p1281.BackColor = colorReservation; break;
                        case "1282": p1282.BackColor = colorReservation; break;
                        case "1283": p1283.BackColor = colorReservation; break;
                        case "1284": p1284.BackColor = colorReservation; break;
                        case "1285": p1285.BackColor = colorReservation; break;
                        case "1286": p1286.BackColor = colorReservation; break;
                        case "1287": p1287.BackColor = colorReservation; break;
                        case "1288": p1288.BackColor = colorReservation; break;
                        case "1289": p1289.BackColor = colorReservation; break;
                        case "1290": p1290.BackColor = colorReservation; break;
                        case "1291": p1291.BackColor = colorReservation; break;
                        case "1292": p1292.BackColor = colorReservation; break;
                        case "1293": p1293.BackColor = colorReservation; break;
                        case "1294": p1294.BackColor = colorReservation; break;
                        case "1295": p1295.BackColor = colorReservation; break;
                        case "1296": p1296.BackColor = colorReservation; break;
                        case "1297": p1297.BackColor = colorReservation; break;
                        case "1298": p1298.BackColor = colorReservation; break;
                        case "1299": p1299.BackColor = colorReservation; break;
                        case "1300": p1300.BackColor = colorReservation; break;
                        case "1301": p1301.BackColor = colorReservation; break;
                        case "1302": p1302.BackColor = colorReservation; break;
                        case "1303": p1303.BackColor = colorReservation; break;
                        case "1304": p1304.BackColor = colorReservation; break;
                        case "1305": p1305.BackColor = colorReservation; break;
                        case "1306": p1306.BackColor = colorReservation; break;
                        case "1307": p1307.BackColor = colorReservation; break;
                        case "1308": p1308.BackColor = colorReservation; break;
                        case "1309": p1309.BackColor = colorReservation; break;
                        case "1310": p1310.BackColor = colorReservation; break;
                        case "1311": p1311.BackColor = colorReservation; break;
                        case "1312": p1312.BackColor = colorReservation; break;
                        case "1313": p1313.BackColor = colorReservation; break;
                        case "1314": p1314.BackColor = colorReservation; break;
                        case "1315": p1315.BackColor = colorReservation; break;
                        case "1316": p1316.BackColor = colorReservation; break;
                        case "1317": p1317.BackColor = colorReservation; break;
                        case "1318": p1318.BackColor = colorReservation; break;
                        case "1319": p1319.BackColor = colorReservation; break;
                        case "1320": p1320.BackColor = colorReservation; break;
                        case "1321": p1321.BackColor = colorReservation; break;
                        case "1322": p1322.BackColor = colorReservation; break;
                        case "1323": p1323.BackColor = colorReservation; break;
                        case "1324": p1324.BackColor = colorReservation; break;
                        case "1325": p1325.BackColor = colorReservation; break;
                        case "1326": p1326.BackColor = colorReservation; break;
                        case "1327": p1327.BackColor = colorReservation; break;
                        case "1328": p1328.BackColor = colorReservation; break;
                        case "1329": p1329.BackColor = colorReservation; break;
                        case "1330": p1330.BackColor = colorReservation; break;
                        case "1331": p1331.BackColor = colorReservation; break;
                        case "1332": p1332.BackColor = colorReservation; break;
                        case "1333": p1333.BackColor = colorReservation; break;
                        case "1334": p1334.BackColor = colorReservation; break;
                        case "1335": p1335.BackColor = colorReservation; break;
                        case "1336": p1336.BackColor = colorReservation; break;
                        case "1337": p1337.BackColor = colorReservation; break;
                        case "1338": p1338.BackColor = colorReservation; break;
                        case "1339": p1339.BackColor = colorReservation; break;
                        case "1340": p1340.BackColor = colorReservation; break;
                        case "1341": p1341.BackColor = colorReservation; break;
                        case "1342": p1342.BackColor = colorReservation; break;
                        case "1343": p1343.BackColor = colorReservation; break;
                        case "1344": p1344.BackColor = colorReservation; break;
                        case "1345": p1345.BackColor = colorReservation; break;
                        case "1346": p1346.BackColor = colorReservation; break;
                        case "1347": p1347.BackColor = colorReservation; break;
                        case "1348": p1348.BackColor = colorReservation; break;
                        case "1349": p1349.BackColor = colorReservation; break;
                        case "1350": p1350.BackColor = colorReservation; break;
                        case "1351": p1351.BackColor = colorReservation; break;
                        case "1352": p1352.BackColor = colorReservation; break;
                        case "1353": p1353.BackColor = colorReservation; break;
                        case "1354": p1354.BackColor = colorReservation; break;
                        case "1355": p1355.BackColor = colorReservation; break;
                        case "1356": p1356.BackColor = colorReservation; break;
                        case "1357": p1357.BackColor = colorReservation; break;
                        case "1358": p1358.BackColor = colorReservation; break;
                        case "1359": p1359.BackColor = colorReservation; break;
                        case "1360": p1360.BackColor = colorReservation; break;
                        case "1361": p1361.BackColor = colorReservation; break;
                        case "1362": p1362.BackColor = colorReservation; break;
                        case "1363": p1363.BackColor = colorReservation; break;
                        case "1364": p1364.BackColor = colorReservation; break;
                        case "1365": p1365.BackColor = colorReservation; break;
                        case "1366": p1366.BackColor = colorReservation; break;
                        case "1367": p1367.BackColor = colorReservation; break;
                        case "1368": p1368.BackColor = colorReservation; break;
                        case "1369": p1369.BackColor = colorReservation; break;
                        case "1370": p1370.BackColor = colorReservation; break;
                        case "1371": p1371.BackColor = colorReservation; break;
                        case "1372": p1372.BackColor = colorReservation; break;
                        case "1373": p1373.BackColor = colorReservation; break;
                        case "1374": p1374.BackColor = colorReservation; break;
                        case "1375": p1375.BackColor = colorReservation; break;
                        case "1376": p1376.BackColor = colorReservation; break;
                        case "1377": p1377.BackColor = colorReservation; break;
                        case "1378": p1378.BackColor = colorReservation; break;
                        case "1379": p1379.BackColor = colorReservation; break;
                        case "1380": p1380.BackColor = colorReservation; break;
                        case "1381": p1381.BackColor = colorReservation; break;
                        case "1382": p1382.BackColor = colorReservation; break;
                        case "1383": p1383.BackColor = colorReservation; break;
                        case "1384": p1384.BackColor = colorReservation; break;
                        case "1385": p1385.BackColor = colorReservation; break;
                        case "1386": p1386.BackColor = colorReservation; break;
                        case "1387": p1387.BackColor = colorReservation; break;
                        case "1388": p1388.BackColor = colorReservation; break;
                        case "1389": p1389.BackColor = colorReservation; break;
                        case "1390": p1390.BackColor = colorReservation; break;
                        case "1391": p1391.BackColor = colorReservation; break;
                        case "1392": p1392.BackColor = colorReservation; break;
                        case "1393": p1393.BackColor = colorReservation; break;
                        case "1394": p1394.BackColor = colorReservation; break;
                        case "1395": p1395.BackColor = colorReservation; break;
                        case "1396": p1396.BackColor = colorReservation; break;
                        case "1397": p1397.BackColor = colorReservation; break;
                        case "1398": p1398.BackColor = colorReservation; break;
                        case "1399": p1399.BackColor = colorReservation; break;
                        case "1400": p1400.BackColor = colorReservation; break;
                        case "1401": p1401.BackColor = colorReservation; break;
                        case "1402": p1402.BackColor = colorReservation; break;
                        case "1403": p1403.BackColor = colorReservation; break;
                        case "1404": p1404.BackColor = colorReservation; break;
                        case "1405": p1405.BackColor = colorReservation; break;
                        case "1406": p1406.BackColor = colorReservation; break;
                        case "1407": p1407.BackColor = colorReservation; break;
                        case "1408": p1408.BackColor = colorReservation; break;
                        case "1409": p1409.BackColor = colorReservation; break;
                        case "1410": p1410.BackColor = colorReservation; break;
                        case "1411": p1411.BackColor = colorReservation; break;
                        case "1412": p1412.BackColor = colorReservation; break;
                        case "1413": p1413.BackColor = colorReservation; break;
                        case "1414": p1414.BackColor = colorReservation; break;
                        case "1415": p1415.BackColor = colorReservation; break;
                        case "1416": p1416.BackColor = colorReservation; break;
                        case "1417": p1417.BackColor = colorReservation; break;
                        case "1418": p1418.BackColor = colorReservation; break;
                        case "1419": p1419.BackColor = colorReservation; break;
                        case "1420": p1420.BackColor = colorReservation; break;
                        case "1421": p1421.BackColor = colorReservation; break;
                        case "1422": p1422.BackColor = colorReservation; break;
                        case "1423": p1423.BackColor = colorReservation; break;
                        case "1424": p1424.BackColor = colorReservation; break;
                        case "1425": p1425.BackColor = colorReservation; break;
                        case "1426": p1426.BackColor = colorReservation; break;
                        case "1427": p1427.BackColor = colorReservation; break;
                        case "1428": p1428.BackColor = colorReservation; break;
                        case "1429": p1429.BackColor = colorReservation; break;
                        case "1430": p1430.BackColor = colorReservation; break;
                        case "1431": p1431.BackColor = colorReservation; break;
                        case "1432": p1432.BackColor = colorReservation; break;
                        case "1433": p1433.BackColor = colorReservation; break;
                        case "1434": p1434.BackColor = colorReservation; break;
                        case "1435": p1435.BackColor = colorReservation; break;
                        case "1436": p1436.BackColor = colorReservation; break;
                        case "1437": p1437.BackColor = colorReservation; break;
                        case "1438": p1438.BackColor = colorReservation; break;
                        case "1439": p1439.BackColor = colorReservation; break;
                        case "1440": p1440.BackColor = colorReservation; break;
                        case "1441": p1441.BackColor = colorReservation; break;
                        case "1442": p1442.BackColor = colorReservation; break;
                        case "1443": p1443.BackColor = colorReservation; break;
                        case "1444": p1444.BackColor = colorReservation; break;
                        case "1445": p1445.BackColor = colorReservation; break;
                        case "1446": p1446.BackColor = colorReservation; break;
                        case "1447": p1447.BackColor = colorReservation; break;
                        case "1448": p1448.BackColor = colorReservation; break;
                        case "1449": p1449.BackColor = colorReservation; break;
                        case "1450": p1450.BackColor = colorReservation; break;
                        case "1451": p1451.BackColor = colorReservation; break;
                        case "1452": p1452.BackColor = colorReservation; break;
                        case "1453": p1453.BackColor = colorReservation; break;
                        case "1454": p1454.BackColor = colorReservation; break;
                        case "1455": p1455.BackColor = colorReservation; break;
                        case "1456": p1456.BackColor = colorReservation; break;
                        case "1457": p1457.BackColor = colorReservation; break;
                        case "1458": p1458.BackColor = colorReservation; break;
                        case "1459": p1459.BackColor = colorReservation; break;
                        case "1460": p1460.BackColor = colorReservation; break;
                        case "1461": p1461.BackColor = colorReservation; break;
                        case "1462": p1462.BackColor = colorReservation; break;
                        case "1463": p1463.BackColor = colorReservation; break;
                        case "1464": p1464.BackColor = colorReservation; break;
                        case "1465": p1465.BackColor = colorReservation; break;
                        case "1466": p1466.BackColor = colorReservation; break;
                        case "1467": p1467.BackColor = colorReservation; break;
                        case "1468": p1468.BackColor = colorReservation; break;
                        case "1469": p1469.BackColor = colorReservation; break;
                        case "1470": p1470.BackColor = colorReservation; break;
                        case "1471": p1471.BackColor = colorReservation; break;
                        case "1472": p1472.BackColor = colorReservation; break;
                        case "1473": p1473.BackColor = colorReservation; break;
                        case "1474": p1474.BackColor = colorReservation; break;
                        case "1475": p1475.BackColor = colorReservation; break;
                        case "1476": p1476.BackColor = colorReservation; break;
                        case "1477": p1477.BackColor = colorReservation; break;
                        case "1478": p1478.BackColor = colorReservation; break;
                        case "1479": p1479.BackColor = colorReservation; break;
                        case "1480": p1480.BackColor = colorReservation; break;
                        case "1481": p1481.BackColor = colorReservation; break;
                        case "1482": p1482.BackColor = colorReservation; break;
                        case "1483": p1483.BackColor = colorReservation; break;
                        case "1484": p1484.BackColor = colorReservation; break;
                        case "1485": p1485.BackColor = colorReservation; break;
                        case "1486": p1486.BackColor = colorReservation; break;
                        case "1487": p1487.BackColor = colorReservation; break;
                        case "1488": p1488.BackColor = colorReservation; break;
                        case "1489": p1489.BackColor = colorReservation; break;
                        case "1490": p1490.BackColor = colorReservation; break;
                        case "1491": p1491.BackColor = colorReservation; break;
                        case "1492": p1492.BackColor = colorReservation; break;
                        case "1493": p1493.BackColor = colorReservation; break;
                        case "1494": p1494.BackColor = colorReservation; break;
                        case "1495": p1495.BackColor = colorReservation; break;
                        case "1496": p1496.BackColor = colorReservation; break;
                        case "1497": p1497.BackColor = colorReservation; break;
                        case "1498": p1498.BackColor = colorReservation; break;
                        case "1499": p1499.BackColor = colorReservation; break;
                        case "1500": p1500.BackColor = colorReservation; break;
                        case "1501": p1501.BackColor = colorReservation; break;
                        case "1502": p1502.BackColor = colorReservation; break;
                        case "1503": p1503.BackColor = colorReservation; break;
                        case "1504": p1504.BackColor = colorReservation; break;
                        case "1505": p1505.BackColor = colorReservation; break;
                        case "1506": p1506.BackColor = colorReservation; break;
                        case "1507": p1507.BackColor = colorReservation; break;
                        case "1508": p1508.BackColor = colorReservation; break;
                        case "1509": p1509.BackColor = colorReservation; break;
                        case "1510": p1510.BackColor = colorReservation; break;
                        case "1511": p1511.BackColor = colorReservation; break;
                        case "1512": p1512.BackColor = colorReservation; break;
                        case "1513": p1513.BackColor = colorReservation; break;
                        case "1514": p1514.BackColor = colorReservation; break;
                    }
                }
                if (ds.Tables["Tickets"].Rows[i].ItemArray[4].ToString() == "True")
                {
                    switch (ds.Tables["Tickets"].Rows[i].ItemArray[3].ToString())
                    {
                        case "2001": p2001.BackColor = colorPaid; break;
                        case "2002": p2002.BackColor = colorPaid; break;
                        case "2003": p2003.BackColor = colorPaid; break;
                        case "2004": p2004.BackColor = colorPaid; break;
                        case "2005": p2005.BackColor = colorPaid; break;
                        case "2006": p2006.BackColor = colorPaid; break;
                        case "2007": p2007.BackColor = colorPaid; break;
                        case "2008": p2008.BackColor = colorPaid; break;
                        case "2009": p2009.BackColor = colorPaid; break;
                        case "2010": p2010.BackColor = colorPaid; break;
                        case "2011": p2011.BackColor = colorPaid; break;
                        case "2012": p2012.BackColor = colorPaid; break;
                        case "2013": p2013.BackColor = colorPaid; break;
                        case "2014": p2014.BackColor = colorPaid; break;
                        case "2015": p2015.BackColor = colorPaid; break;
                        case "2016": p2016.BackColor = colorPaid; break;
                        case "2017": p2017.BackColor = colorPaid; break;
                        case "2018": p2018.BackColor = colorPaid; break;
                        case "2019": p2019.BackColor = colorPaid; break;
                        case "2020": p2020.BackColor = colorPaid; break;
                        case "2021": p2021.BackColor = colorPaid; break;
                        case "2022": p2022.BackColor = colorPaid; break;
                        case "2023": p2023.BackColor = colorPaid; break;
                        case "2024": p2024.BackColor = colorPaid; break;
                        case "2025": p2025.BackColor = colorPaid; break;
                        case "2026": p2026.BackColor = colorPaid; break;
                        case "2027": p2027.BackColor = colorPaid; break;
                        case "2028": p2028.BackColor = colorPaid; break;
                        case "2029": p2029.BackColor = colorPaid; break;
                        case "2030": p2030.BackColor = colorPaid; break;
                        case "2031": p2031.BackColor = colorPaid; break;
                        case "2032": p2032.BackColor = colorPaid; break;
                        case "2033": p2033.BackColor = colorPaid; break;
                        case "2034": p2034.BackColor = colorPaid; break;
                        case "2035": p2035.BackColor = colorPaid; break;
                        case "2036": p2036.BackColor = colorPaid; break;
                        case "2037": p2037.BackColor = colorPaid; break;
                        case "2038": p2038.BackColor = colorPaid; break;
                        case "2039": p2039.BackColor = colorPaid; break;
                        case "2040": p2040.BackColor = colorPaid; break;
                        case "2041": p2041.BackColor = colorPaid; break;
                        case "2042": p2042.BackColor = colorPaid; break;
                        case "2043": p2043.BackColor = colorPaid; break;
                        case "2044": p2044.BackColor = colorPaid; break;
                        case "2045": p2045.BackColor = colorPaid; break;
                        case "2046": p2046.BackColor = colorPaid; break;
                        case "2047": p2047.BackColor = colorPaid; break;
                        case "2048": p2048.BackColor = colorPaid; break;
                        case "2049": p2049.BackColor = colorPaid; break;
                        case "2050": p2050.BackColor = colorPaid; break;
                        case "1001": p1001.BackColor = colorPaid; break;
                        case "1002": p1002.BackColor = colorPaid; break;
                        case "1003": p1003.BackColor = colorPaid; break;
                        case "1004": p1004.BackColor = colorPaid; break;
                        case "1005": p1005.BackColor = colorPaid; break;
                        case "1006": p1006.BackColor = colorPaid; break;
                        case "1007": p1007.BackColor = colorPaid; break;
                        case "1008": p1008.BackColor = colorPaid; break;
                        case "1009": p1009.BackColor = colorPaid; break;
                        case "1010": p1010.BackColor = colorPaid; break;
                        case "1011": p1011.BackColor = colorPaid; break;
                        case "1012": p1012.BackColor = colorPaid; break;
                        case "1013": p1013.BackColor = colorPaid; break;
                        case "1014": p1014.BackColor = colorPaid; break;
                        case "1015": p1015.BackColor = colorPaid; break;
                        case "1016": p1016.BackColor = colorPaid; break;
                        case "1017": p1017.BackColor = colorPaid; break;
                        case "1018": p1018.BackColor = colorPaid; break;
                        case "1019": p1019.BackColor = colorPaid; break;
                        case "1020": p1020.BackColor = colorPaid; break;
                        case "1021": p1021.BackColor = colorPaid; break;
                        case "1022": p1022.BackColor = colorPaid; break;
                        case "1023": p1023.BackColor = colorPaid; break;
                        case "1024": p1024.BackColor = colorPaid; break;
                        case "1025": p1025.BackColor = colorPaid; break;
                        case "1026": p1026.BackColor = colorPaid; break;
                        case "1027": p1027.BackColor = colorPaid; break;
                        case "1028": p1028.BackColor = colorPaid; break;
                        case "1029": p1029.BackColor = colorPaid; break;
                        case "1030": p1030.BackColor = colorPaid; break;
                        case "1031": p1031.BackColor = colorPaid; break;
                        case "1032": p1032.BackColor = colorPaid; break;
                        case "1033": p1033.BackColor = colorPaid; break;
                        case "1034": p1034.BackColor = colorPaid; break;
                        case "1035": p1035.BackColor = colorPaid; break;
                        case "1036": p1036.BackColor = colorPaid; break;
                        case "1037": p1037.BackColor = colorPaid; break;
                        case "1038": p1038.BackColor = colorPaid; break;
                        case "1039": p1039.BackColor = colorPaid; break;
                        case "1040": p1040.BackColor = colorPaid; break;
                        case "1041": p1041.BackColor = colorPaid; break;
                        case "1042": p1042.BackColor = colorPaid; break;
                        case "1043": p1043.BackColor = colorPaid; break;
                        case "1044": p1044.BackColor = colorPaid; break;
                        case "1045": p1045.BackColor = colorPaid; break;
                        case "1046": p1046.BackColor = colorPaid; break;
                        case "1047": p1047.BackColor = colorPaid; break;
                        case "1048": p1048.BackColor = colorPaid; break;
                        case "1049": p1049.BackColor = colorPaid; break;
                        case "1050": p1050.BackColor = colorPaid; break;
                        case "1051": p1051.BackColor = colorPaid; break;
                        case "1052": p1052.BackColor = colorPaid; break;
                        case "1053": p1053.BackColor = colorPaid; break;
                        case "1054": p1054.BackColor = colorPaid; break;
                        case "1055": p1055.BackColor = colorPaid; break;
                        case "1056": p1056.BackColor = colorPaid; break;
                        case "1057": p1057.BackColor = colorPaid; break;
                        case "1058": p1058.BackColor = colorPaid; break;
                        case "1059": p1059.BackColor = colorPaid; break;
                        case "1060": p1060.BackColor = colorPaid; break;
                        case "1061": p1061.BackColor = colorPaid; break;
                        case "1062": p1062.BackColor = colorPaid; break;
                        case "1063": p1063.BackColor = colorPaid; break;
                        case "1064": p1064.BackColor = colorPaid; break;
                        case "1065": p1065.BackColor = colorPaid; break;
                        case "1066": p1066.BackColor = colorPaid; break;
                        case "1067": p1067.BackColor = colorPaid; break;
                        case "1068": p1068.BackColor = colorPaid; break;
                        case "1069": p1069.BackColor = colorPaid; break;
                        case "1070": p1070.BackColor = colorPaid; break;
                        case "1071": p1071.BackColor = colorPaid; break;
                        case "1072": p1072.BackColor = colorPaid; break;
                        case "1073": p1073.BackColor = colorPaid; break;
                        case "1074": p1074.BackColor = colorPaid; break;
                        case "1075": p1075.BackColor = colorPaid; break;
                        case "1076": p1076.BackColor = colorPaid; break;
                        case "1077": p1077.BackColor = colorPaid; break;
                        case "1078": p1078.BackColor = colorPaid; break;
                        case "1079": p1079.BackColor = colorPaid; break;
                        case "1080": p1080.BackColor = colorPaid; break;
                        case "1081": p1081.BackColor = colorPaid; break;
                        case "1082": p1082.BackColor = colorPaid; break;
                        case "1083": p1083.BackColor = colorPaid; break;
                        case "1084": p1084.BackColor = colorPaid; break;
                        case "1085": p1085.BackColor = colorPaid; break;
                        case "1086": p1086.BackColor = colorPaid; break;
                        case "1087": p1087.BackColor = colorPaid; break;
                        case "1088": p1088.BackColor = colorPaid; break;
                        case "1089": p1089.BackColor = colorPaid; break;
                        case "1090": p1090.BackColor = colorPaid; break;
                        case "1091": p1091.BackColor = colorPaid; break;
                        case "1092": p1092.BackColor = colorPaid; break;
                        case "1093": p1093.BackColor = colorPaid; break;
                        case "1094": p1094.BackColor = colorPaid; break;
                        case "1095": p1095.BackColor = colorPaid; break;
                        case "1096": p1096.BackColor = colorPaid; break;
                        case "1097": p1097.BackColor = colorPaid; break;
                        case "1098": p1098.BackColor = colorPaid; break;
                        case "1099": p1099.BackColor = colorPaid; break;
                        case "1100": p1100.BackColor = colorPaid; break;
                        case "1101": p1101.BackColor = colorPaid; break;
                        case "1102": p1102.BackColor = colorPaid; break;
                        case "1103": p1103.BackColor = colorPaid; break;
                        case "1104": p1104.BackColor = colorPaid; break;
                        case "1105": p1105.BackColor = colorPaid; break;
                        case "1106": p1106.BackColor = colorPaid; break;
                        case "1107": p1107.BackColor = colorPaid; break;
                        case "1108": p1108.BackColor = colorPaid; break;
                        case "1109": p1109.BackColor = colorPaid; break;
                        case "1110": p1110.BackColor = colorPaid; break;
                        case "1111": p1111.BackColor = colorPaid; break;
                        case "1112": p1112.BackColor = colorPaid; break;
                        case "1113": p1113.BackColor = colorPaid; break;
                        case "1114": p1114.BackColor = colorPaid; break;
                        case "1115": p1115.BackColor = colorPaid; break;
                        case "1116": p1116.BackColor = colorPaid; break;
                        case "1117": p1117.BackColor = colorPaid; break;
                        case "1118": p1118.BackColor = colorPaid; break;
                        case "1119": p1119.BackColor = colorPaid; break;
                        case "1120": p1120.BackColor = colorPaid; break;
                        case "1121": p1121.BackColor = colorPaid; break;
                        case "1122": p1122.BackColor = colorPaid; break;
                        case "1123": p1123.BackColor = colorPaid; break;
                        case "1124": p1124.BackColor = colorPaid; break;
                        case "1125": p1125.BackColor = colorPaid; break;
                        case "1126": p1126.BackColor = colorPaid; break;
                        case "1127": p1127.BackColor = colorPaid; break;
                        case "1128": p1128.BackColor = colorPaid; break;
                        case "1129": p1129.BackColor = colorPaid; break;
                        case "1130": p1130.BackColor = colorPaid; break;
                        case "1131": p1131.BackColor = colorPaid; break;
                        case "1132": p1132.BackColor = colorPaid; break;
                        case "1133": p1133.BackColor = colorPaid; break;
                        case "1134": p1134.BackColor = colorPaid; break;
                        case "1135": p1135.BackColor = colorPaid; break;
                        case "1136": p1136.BackColor = colorPaid; break;
                        case "1137": p1137.BackColor = colorPaid; break;
                        case "1138": p1138.BackColor = colorPaid; break;
                        case "1139": p1139.BackColor = colorPaid; break;
                        case "1140": p1140.BackColor = colorPaid; break;
                        case "1141": p1141.BackColor = colorPaid; break;
                        case "1142": p1142.BackColor = colorPaid; break;
                        case "1143": p1143.BackColor = colorPaid; break;
                        case "1144": p1144.BackColor = colorPaid; break;
                        case "1145": p1145.BackColor = colorPaid; break;
                        case "1146": p1146.BackColor = colorPaid; break;
                        case "1147": p1147.BackColor = colorPaid; break;
                        case "1148": p1148.BackColor = colorPaid; break;
                        case "1149": p1149.BackColor = colorPaid; break;
                        case "1150": p1150.BackColor = colorPaid; break;
                        case "1151": p1151.BackColor = colorPaid; break;
                        case "1152": p1152.BackColor = colorPaid; break;
                        case "1153": p1153.BackColor = colorPaid; break;
                        case "1154": p1154.BackColor = colorPaid; break;
                        case "1155": p1155.BackColor = colorPaid; break;
                        case "1156": p1156.BackColor = colorPaid; break;
                        case "1157": p1157.BackColor = colorPaid; break;
                        case "1158": p1158.BackColor = colorPaid; break;
                        case "1159": p1159.BackColor = colorPaid; break;
                        case "1160": p1160.BackColor = colorPaid; break;
                        case "1161": p1161.BackColor = colorPaid; break;
                        case "1162": p1162.BackColor = colorPaid; break;
                        case "1163": p1163.BackColor = colorPaid; break;
                        case "1164": p1164.BackColor = colorPaid; break;
                        case "1165": p1165.BackColor = colorPaid; break;
                        case "1166": p1166.BackColor = colorPaid; break;
                        case "1167": p1167.BackColor = colorPaid; break;
                        case "1168": p1168.BackColor = colorPaid; break;
                        case "1169": p1169.BackColor = colorPaid; break;
                        case "1170": p1170.BackColor = colorPaid; break;
                        case "1171": p1171.BackColor = colorPaid; break;
                        case "1172": p1172.BackColor = colorPaid; break;
                        case "1173": p1173.BackColor = colorPaid; break;
                        case "1174": p1174.BackColor = colorPaid; break;
                        case "1175": p1175.BackColor = colorPaid; break;
                        case "1176": p1176.BackColor = colorPaid; break;
                        case "1177": p1177.BackColor = colorPaid; break;
                        case "1178": p1178.BackColor = colorPaid; break;
                        case "1179": p1179.BackColor = colorPaid; break;
                        case "1180": p1180.BackColor = colorPaid; break;
                        case "1181": p1181.BackColor = colorPaid; break;
                        case "1182": p1182.BackColor = colorPaid; break;
                        case "1183": p1183.BackColor = colorPaid; break;
                        case "1184": p1184.BackColor = colorPaid; break;
                        case "1185": p1185.BackColor = colorPaid; break;
                        case "1186": p1186.BackColor = colorPaid; break;
                        case "1187": p1187.BackColor = colorPaid; break;
                        case "1188": p1188.BackColor = colorPaid; break;
                        case "1189": p1189.BackColor = colorPaid; break;
                        case "1190": p1190.BackColor = colorPaid; break;
                        case "1191": p1191.BackColor = colorPaid; break;
                        case "1192": p1192.BackColor = colorPaid; break;
                        case "1193": p1193.BackColor = colorPaid; break;
                        case "1194": p1194.BackColor = colorPaid; break;
                        case "1195": p1195.BackColor = colorPaid; break;
                        case "1196": p1196.BackColor = colorPaid; break;
                        case "1197": p1197.BackColor = colorPaid; break;
                        case "1198": p1198.BackColor = colorPaid; break;
                        case "1199": p1199.BackColor = colorPaid; break;
                        case "1200": p1200.BackColor = colorPaid; break;
                        case "1201": p1201.BackColor = colorPaid; break;
                        case "1202": p1202.BackColor = colorPaid; break;
                        case "1203": p1203.BackColor = colorPaid; break;
                        case "1204": p1204.BackColor = colorPaid; break;
                        case "1205": p1205.BackColor = colorPaid; break;
                        case "1206": p1206.BackColor = colorPaid; break;
                        case "1207": p1207.BackColor = colorPaid; break;
                        case "1208": p1208.BackColor = colorPaid; break;
                        case "1209": p1209.BackColor = colorPaid; break;
                        case "1210": p1210.BackColor = colorPaid; break;
                        case "1211": p1211.BackColor = colorPaid; break;
                        case "1212": p1212.BackColor = colorPaid; break;
                        case "1213": p1213.BackColor = colorPaid; break;
                        case "1214": p1214.BackColor = colorPaid; break;
                        case "1215": p1215.BackColor = colorPaid; break;
                        case "1216": p1216.BackColor = colorPaid; break;
                        case "1217": p1217.BackColor = colorPaid; break;
                        case "1218": p1218.BackColor = colorPaid; break;
                        case "1219": p1219.BackColor = colorPaid; break;
                        case "1220": p1220.BackColor = colorPaid; break;
                        case "1221": p1221.BackColor = colorPaid; break;
                        case "1222": p1222.BackColor = colorPaid; break;
                        case "1223": p1223.BackColor = colorPaid; break;
                        case "1224": p1224.BackColor = colorPaid; break;
                        case "1225": p1225.BackColor = colorPaid; break;
                        case "1226": p1226.BackColor = colorPaid; break;
                        case "1227": p1227.BackColor = colorPaid; break;
                        case "1228": p1228.BackColor = colorPaid; break;
                        case "1229": p1229.BackColor = colorPaid; break;
                        case "1230": p1230.BackColor = colorPaid; break;
                        case "1231": p1231.BackColor = colorPaid; break;
                        case "1232": p1232.BackColor = colorPaid; break;
                        case "1233": p1233.BackColor = colorPaid; break;
                        case "1234": p1234.BackColor = colorPaid; break;
                        case "1235": p1235.BackColor = colorPaid; break;
                        case "1236": p1236.BackColor = colorPaid; break;
                        case "1237": p1237.BackColor = colorPaid; break;
                        case "1238": p1238.BackColor = colorPaid; break;
                        case "1239": p1239.BackColor = colorPaid; break;
                        case "1240": p1240.BackColor = colorPaid; break;
                        case "1241": p1241.BackColor = colorPaid; break;
                        case "1242": p1242.BackColor = colorPaid; break;
                        case "1243": p1243.BackColor = colorPaid; break;
                        case "1244": p1244.BackColor = colorPaid; break;
                        case "1245": p1245.BackColor = colorPaid; break;
                        case "1246": p1246.BackColor = colorPaid; break;
                        case "1247": p1247.BackColor = colorPaid; break;
                        case "1248": p1248.BackColor = colorPaid; break;
                        case "1249": p1249.BackColor = colorPaid; break;
                        case "1250": p1250.BackColor = colorPaid; break;
                        case "1251": p1251.BackColor = colorPaid; break;
                        case "1252": p1252.BackColor = colorPaid; break;
                        case "1253": p1253.BackColor = colorPaid; break;
                        case "1254": p1254.BackColor = colorPaid; break;
                        case "1255": p1255.BackColor = colorPaid; break;
                        case "1256": p1256.BackColor = colorPaid; break;
                        case "1257": p1257.BackColor = colorPaid; break;
                        case "1258": p1258.BackColor = colorPaid; break;
                        case "1259": p1259.BackColor = colorPaid; break;
                        case "1260": p1260.BackColor = colorPaid; break;
                        case "1261": p1261.BackColor = colorPaid; break;
                        case "1262": p1262.BackColor = colorPaid; break;
                        case "1263": p1263.BackColor = colorPaid; break;
                        case "1264": p1264.BackColor = colorPaid; break;
                        case "1265": p1265.BackColor = colorPaid; break;
                        case "1266": p1266.BackColor = colorPaid; break;
                        case "1267": p1267.BackColor = colorPaid; break;
                        case "1268": p1268.BackColor = colorPaid; break;
                        case "1269": p1269.BackColor = colorPaid; break;
                        case "1270": p1270.BackColor = colorPaid; break;
                        case "1271": p1271.BackColor = colorPaid; break;
                        case "1272": p1272.BackColor = colorPaid; break;
                        case "1273": p1273.BackColor = colorPaid; break;
                        case "1274": p1274.BackColor = colorPaid; break;
                        case "1275": p1275.BackColor = colorPaid; break;
                        case "1276": p1276.BackColor = colorPaid; break;
                        case "1277": p1277.BackColor = colorPaid; break;
                        case "1278": p1278.BackColor = colorPaid; break;
                        case "1279": p1279.BackColor = colorPaid; break;
                        case "1280": p1280.BackColor = colorPaid; break;
                        case "1281": p1281.BackColor = colorPaid; break;
                        case "1282": p1282.BackColor = colorPaid; break;
                        case "1283": p1283.BackColor = colorPaid; break;
                        case "1284": p1284.BackColor = colorPaid; break;
                        case "1285": p1285.BackColor = colorPaid; break;
                        case "1286": p1286.BackColor = colorPaid; break;
                        case "1287": p1287.BackColor = colorPaid; break;
                        case "1288": p1288.BackColor = colorPaid; break;
                        case "1289": p1289.BackColor = colorPaid; break;
                        case "1290": p1290.BackColor = colorPaid; break;
                        case "1291": p1291.BackColor = colorPaid; break;
                        case "1292": p1292.BackColor = colorPaid; break;
                        case "1293": p1293.BackColor = colorPaid; break;
                        case "1294": p1294.BackColor = colorPaid; break;
                        case "1295": p1295.BackColor = colorPaid; break;
                        case "1296": p1296.BackColor = colorPaid; break;
                        case "1297": p1297.BackColor = colorPaid; break;
                        case "1298": p1298.BackColor = colorPaid; break;
                        case "1299": p1299.BackColor = colorPaid; break;
                        case "1300": p1300.BackColor = colorPaid; break;
                        case "1301": p1301.BackColor = colorPaid; break;
                        case "1302": p1302.BackColor = colorPaid; break;
                        case "1303": p1303.BackColor = colorPaid; break;
                        case "1304": p1304.BackColor = colorPaid; break;
                        case "1305": p1305.BackColor = colorPaid; break;
                        case "1306": p1306.BackColor = colorPaid; break;
                        case "1307": p1307.BackColor = colorPaid; break;
                        case "1308": p1308.BackColor = colorPaid; break;
                        case "1309": p1309.BackColor = colorPaid; break;
                        case "1310": p1310.BackColor = colorPaid; break;
                        case "1311": p1311.BackColor = colorPaid; break;
                        case "1312": p1312.BackColor = colorPaid; break;
                        case "1313": p1313.BackColor = colorPaid; break;
                        case "1314": p1314.BackColor = colorPaid; break;
                        case "1315": p1315.BackColor = colorPaid; break;
                        case "1316": p1316.BackColor = colorPaid; break;
                        case "1317": p1317.BackColor = colorPaid; break;
                        case "1318": p1318.BackColor = colorPaid; break;
                        case "1319": p1319.BackColor = colorPaid; break;
                        case "1320": p1320.BackColor = colorPaid; break;
                        case "1321": p1321.BackColor = colorPaid; break;
                        case "1322": p1322.BackColor = colorPaid; break;
                        case "1323": p1323.BackColor = colorPaid; break;
                        case "1324": p1324.BackColor = colorPaid; break;
                        case "1325": p1325.BackColor = colorPaid; break;
                        case "1326": p1326.BackColor = colorPaid; break;
                        case "1327": p1327.BackColor = colorPaid; break;
                        case "1328": p1328.BackColor = colorPaid; break;
                        case "1329": p1329.BackColor = colorPaid; break;
                        case "1330": p1330.BackColor = colorPaid; break;
                        case "1331": p1331.BackColor = colorPaid; break;
                        case "1332": p1332.BackColor = colorPaid; break;
                        case "1333": p1333.BackColor = colorPaid; break;
                        case "1334": p1334.BackColor = colorPaid; break;
                        case "1335": p1335.BackColor = colorPaid; break;
                        case "1336": p1336.BackColor = colorPaid; break;
                        case "1337": p1337.BackColor = colorPaid; break;
                        case "1338": p1338.BackColor = colorPaid; break;
                        case "1339": p1339.BackColor = colorPaid; break;
                        case "1340": p1340.BackColor = colorPaid; break;
                        case "1341": p1341.BackColor = colorPaid; break;
                        case "1342": p1342.BackColor = colorPaid; break;
                        case "1343": p1343.BackColor = colorPaid; break;
                        case "1344": p1344.BackColor = colorPaid; break;
                        case "1345": p1345.BackColor = colorPaid; break;
                        case "1346": p1346.BackColor = colorPaid; break;
                        case "1347": p1347.BackColor = colorPaid; break;
                        case "1348": p1348.BackColor = colorPaid; break;
                        case "1349": p1349.BackColor = colorPaid; break;
                        case "1350": p1350.BackColor = colorPaid; break;
                        case "1351": p1351.BackColor = colorPaid; break;
                        case "1352": p1352.BackColor = colorPaid; break;
                        case "1353": p1353.BackColor = colorPaid; break;
                        case "1354": p1354.BackColor = colorPaid; break;
                        case "1355": p1355.BackColor = colorPaid; break;
                        case "1356": p1356.BackColor = colorPaid; break;
                        case "1357": p1357.BackColor = colorPaid; break;
                        case "1358": p1358.BackColor = colorPaid; break;
                        case "1359": p1359.BackColor = colorPaid; break;
                        case "1360": p1360.BackColor = colorPaid; break;
                        case "1361": p1361.BackColor = colorPaid; break;
                        case "1362": p1362.BackColor = colorPaid; break;
                        case "1363": p1363.BackColor = colorPaid; break;
                        case "1364": p1364.BackColor = colorPaid; break;
                        case "1365": p1365.BackColor = colorPaid; break;
                        case "1366": p1366.BackColor = colorPaid; break;
                        case "1367": p1367.BackColor = colorPaid; break;
                        case "1368": p1368.BackColor = colorPaid; break;
                        case "1369": p1369.BackColor = colorPaid; break;
                        case "1370": p1370.BackColor = colorPaid; break;
                        case "1371": p1371.BackColor = colorPaid; break;
                        case "1372": p1372.BackColor = colorPaid; break;
                        case "1373": p1373.BackColor = colorPaid; break;
                        case "1374": p1374.BackColor = colorPaid; break;
                        case "1375": p1375.BackColor = colorPaid; break;
                        case "1376": p1376.BackColor = colorPaid; break;
                        case "1377": p1377.BackColor = colorPaid; break;
                        case "1378": p1378.BackColor = colorPaid; break;
                        case "1379": p1379.BackColor = colorPaid; break;
                        case "1380": p1380.BackColor = colorPaid; break;
                        case "1381": p1381.BackColor = colorPaid; break;
                        case "1382": p1382.BackColor = colorPaid; break;
                        case "1383": p1383.BackColor = colorPaid; break;
                        case "1384": p1384.BackColor = colorPaid; break;
                        case "1385": p1385.BackColor = colorPaid; break;
                        case "1386": p1386.BackColor = colorPaid; break;
                        case "1387": p1387.BackColor = colorPaid; break;
                        case "1388": p1388.BackColor = colorPaid; break;
                        case "1389": p1389.BackColor = colorPaid; break;
                        case "1390": p1390.BackColor = colorPaid; break;
                        case "1391": p1391.BackColor = colorPaid; break;
                        case "1392": p1392.BackColor = colorPaid; break;
                        case "1393": p1393.BackColor = colorPaid; break;
                        case "1394": p1394.BackColor = colorPaid; break;
                        case "1395": p1395.BackColor = colorPaid; break;
                        case "1396": p1396.BackColor = colorPaid; break;
                        case "1397": p1397.BackColor = colorPaid; break;
                        case "1398": p1398.BackColor = colorPaid; break;
                        case "1399": p1399.BackColor = colorPaid; break;
                        case "1400": p1400.BackColor = colorPaid; break;
                        case "1401": p1401.BackColor = colorPaid; break;
                        case "1402": p1402.BackColor = colorPaid; break;
                        case "1403": p1403.BackColor = colorPaid; break;
                        case "1404": p1404.BackColor = colorPaid; break;
                        case "1405": p1405.BackColor = colorPaid; break;
                        case "1406": p1406.BackColor = colorPaid; break;
                        case "1407": p1407.BackColor = colorPaid; break;
                        case "1408": p1408.BackColor = colorPaid; break;
                        case "1409": p1409.BackColor = colorPaid; break;
                        case "1410": p1410.BackColor = colorPaid; break;
                        case "1411": p1411.BackColor = colorPaid; break;
                        case "1412": p1412.BackColor = colorPaid; break;
                        case "1413": p1413.BackColor = colorPaid; break;
                        case "1414": p1414.BackColor = colorPaid; break;
                        case "1415": p1415.BackColor = colorPaid; break;
                        case "1416": p1416.BackColor = colorPaid; break;
                        case "1417": p1417.BackColor = colorPaid; break;
                        case "1418": p1418.BackColor = colorPaid; break;
                        case "1419": p1419.BackColor = colorPaid; break;
                        case "1420": p1420.BackColor = colorPaid; break;
                        case "1421": p1421.BackColor = colorPaid; break;
                        case "1422": p1422.BackColor = colorPaid; break;
                        case "1423": p1423.BackColor = colorPaid; break;
                        case "1424": p1424.BackColor = colorPaid; break;
                        case "1425": p1425.BackColor = colorPaid; break;
                        case "1426": p1426.BackColor = colorPaid; break;
                        case "1427": p1427.BackColor = colorPaid; break;
                        case "1428": p1428.BackColor = colorPaid; break;
                        case "1429": p1429.BackColor = colorPaid; break;
                        case "1430": p1430.BackColor = colorPaid; break;
                        case "1431": p1431.BackColor = colorPaid; break;
                        case "1432": p1432.BackColor = colorPaid; break;
                        case "1433": p1433.BackColor = colorPaid; break;
                        case "1434": p1434.BackColor = colorPaid; break;
                        case "1435": p1435.BackColor = colorPaid; break;
                        case "1436": p1436.BackColor = colorPaid; break;
                        case "1437": p1437.BackColor = colorPaid; break;
                        case "1438": p1438.BackColor = colorPaid; break;
                        case "1439": p1439.BackColor = colorPaid; break;
                        case "1440": p1440.BackColor = colorPaid; break;
                        case "1441": p1441.BackColor = colorPaid; break;
                        case "1442": p1442.BackColor = colorPaid; break;
                        case "1443": p1443.BackColor = colorPaid; break;
                        case "1444": p1444.BackColor = colorPaid; break;
                        case "1445": p1445.BackColor = colorPaid; break;
                        case "1446": p1446.BackColor = colorPaid; break;
                        case "1447": p1447.BackColor = colorPaid; break;
                        case "1448": p1448.BackColor = colorPaid; break;
                        case "1449": p1449.BackColor = colorPaid; break;
                        case "1450": p1450.BackColor = colorPaid; break;
                        case "1451": p1451.BackColor = colorPaid; break;
                        case "1452": p1452.BackColor = colorPaid; break;
                        case "1453": p1453.BackColor = colorPaid; break;
                        case "1454": p1454.BackColor = colorPaid; break;
                        case "1455": p1455.BackColor = colorPaid; break;
                        case "1456": p1456.BackColor = colorPaid; break;
                        case "1457": p1457.BackColor = colorPaid; break;
                        case "1458": p1458.BackColor = colorPaid; break;
                        case "1459": p1459.BackColor = colorPaid; break;
                        case "1460": p1460.BackColor = colorPaid; break;
                        case "1461": p1461.BackColor = colorPaid; break;
                        case "1462": p1462.BackColor = colorPaid; break;
                        case "1463": p1463.BackColor = colorPaid; break;
                        case "1464": p1464.BackColor = colorPaid; break;
                        case "1465": p1465.BackColor = colorPaid; break;
                        case "1466": p1466.BackColor = colorPaid; break;
                        case "1467": p1467.BackColor = colorPaid; break;
                        case "1468": p1468.BackColor = colorPaid; break;
                        case "1469": p1469.BackColor = colorPaid; break;
                        case "1470": p1470.BackColor = colorPaid; break;
                        case "1471": p1471.BackColor = colorPaid; break;
                        case "1472": p1472.BackColor = colorPaid; break;
                        case "1473": p1473.BackColor = colorPaid; break;
                        case "1474": p1474.BackColor = colorPaid; break;
                        case "1475": p1475.BackColor = colorPaid; break;
                        case "1476": p1476.BackColor = colorPaid; break;
                        case "1477": p1477.BackColor = colorPaid; break;
                        case "1478": p1478.BackColor = colorPaid; break;
                        case "1479": p1479.BackColor = colorPaid; break;
                        case "1480": p1480.BackColor = colorPaid; break;
                        case "1481": p1481.BackColor = colorPaid; break;
                        case "1482": p1482.BackColor = colorPaid; break;
                        case "1483": p1483.BackColor = colorPaid; break;
                        case "1484": p1484.BackColor = colorPaid; break;
                        case "1485": p1485.BackColor = colorPaid; break;
                        case "1486": p1486.BackColor = colorPaid; break;
                        case "1487": p1487.BackColor = colorPaid; break;
                        case "1488": p1488.BackColor = colorPaid; break;
                        case "1489": p1489.BackColor = colorPaid; break;
                        case "1490": p1490.BackColor = colorPaid; break;
                        case "1491": p1491.BackColor = colorPaid; break;
                        case "1492": p1492.BackColor = colorPaid; break;
                        case "1493": p1493.BackColor = colorPaid; break;
                        case "1494": p1494.BackColor = colorPaid; break;
                        case "1495": p1495.BackColor = colorPaid; break;
                        case "1496": p1496.BackColor = colorPaid; break;
                        case "1497": p1497.BackColor = colorPaid; break;
                        case "1498": p1498.BackColor = colorPaid; break;
                        case "1499": p1499.BackColor = colorPaid; break;
                        case "1500": p1500.BackColor = colorPaid; break;
                        case "1501": p1501.BackColor = colorPaid; break;
                        case "1502": p1502.BackColor = colorPaid; break;
                        case "1503": p1503.BackColor = colorPaid; break;
                        case "1504": p1504.BackColor = colorPaid; break;
                        case "1505": p1505.BackColor = colorPaid; break;
                        case "1506": p1506.BackColor = colorPaid; break;
                        case "1507": p1507.BackColor = colorPaid; break;
                        case "1508": p1508.BackColor = colorPaid; break;
                        case "1509": p1509.BackColor = colorPaid; break;
                        case "1510": p1510.BackColor = colorPaid; break;
                        case "1511": p1511.BackColor = colorPaid; break;
                        case "1512": p1512.BackColor = colorPaid; break;
                        case "1513": p1513.BackColor = colorPaid; break;
                        case "1514": p1514.BackColor = colorPaid; break;
                    }
                }
            }
        }

        void ClearSeat()
        {
            //Button[] btn = new Button[514];

            p1001.BackColor = p1002.BackColor = p1003.BackColor = p1004.BackColor = p1005.BackColor = p1006.BackColor = p1007.BackColor = p1008.BackColor = p1009.BackColor = p1010.BackColor =
                p1011.BackColor = p1012.BackColor = p1013.BackColor = p1014.BackColor = p1015.BackColor = p1016.BackColor = p1017.BackColor = p1018.BackColor = p1019.BackColor = p1020.BackColor =
                    p1021.BackColor = p1022.BackColor = p1023.BackColor = p1024.BackColor = p1025.BackColor = p1026.BackColor = p1027.BackColor = p1028.BackColor = p1029.BackColor = p1030.BackColor =
                        p1031.BackColor = p1032.BackColor = p1033.BackColor = p1034.BackColor = p1035.BackColor = p1036.BackColor = p1037.BackColor = p1038.BackColor = p1039.BackColor = p1040.BackColor =
                            p1041.BackColor = p1042.BackColor = p1043.BackColor = p1044.BackColor = p1045.BackColor = p1046.BackColor = p1047.BackColor = p1048.BackColor = p1049.BackColor = p1050.BackColor =
                                p1051.BackColor = p1052.BackColor = p1053.BackColor = p1054.BackColor = p1055.BackColor = p1056.BackColor = p1057.BackColor = p1058.BackColor = p1059.BackColor = p1060.BackColor =
                                    p1061.BackColor = p1062.BackColor = p1063.BackColor = p1064.BackColor = p1065.BackColor = p1066.BackColor = p1067.BackColor = p1068.BackColor = p1069.BackColor = p1070.BackColor =
                                        p1071.BackColor = p1072.BackColor = p1073.BackColor = p1074.BackColor = p1075.BackColor = p1076.BackColor = p1077.BackColor = p1078.BackColor = p1079.BackColor = p1080.BackColor =
                                            p1081.BackColor = p1082.BackColor = p1083.BackColor = p1084.BackColor = p1085.BackColor = p1086.BackColor = p1087.BackColor = p1088.BackColor = p1089.BackColor = p1090.BackColor =
                                                p1091.BackColor = p1092.BackColor = p1093.BackColor = p1094.BackColor = p1095.BackColor = p1096.BackColor = p1097.BackColor = p1098.BackColor = p1099.BackColor = p1100.BackColor =
                        p1101.BackColor = p1102.BackColor = p1103.BackColor = p1104.BackColor = p1105.BackColor = p1106.BackColor = p1107.BackColor = p1108.BackColor = p1109.BackColor = p1110.BackColor =
                p1111.BackColor = p1112.BackColor = p1113.BackColor = p1114.BackColor = p1115.BackColor = p1116.BackColor = p1117.BackColor = p1118.BackColor = p1119.BackColor = p1120.BackColor =
                    p1121.BackColor = p1122.BackColor = p1123.BackColor = p1124.BackColor = p1125.BackColor = p1126.BackColor = p1127.BackColor = p1128.BackColor = p1129.BackColor = p1130.BackColor =
                        p1131.BackColor = p1132.BackColor = p1133.BackColor = p1134.BackColor = p1135.BackColor = p1136.BackColor = p1137.BackColor = p1138.BackColor = p1139.BackColor = p1140.BackColor =
                            p1141.BackColor = p1142.BackColor = p1143.BackColor = p1144.BackColor = p1145.BackColor = p1146.BackColor = p1147.BackColor = p1148.BackColor = p1149.BackColor = p1150.BackColor =
                                p1151.BackColor = p1152.BackColor = p1153.BackColor = p1154.BackColor = p1155.BackColor = p1156.BackColor = p1157.BackColor = p1158.BackColor = p1159.BackColor = p1160.BackColor =
                                    p1161.BackColor = p1162.BackColor = p1163.BackColor = p1164.BackColor = p1165.BackColor = p1166.BackColor = p1167.BackColor = p1168.BackColor = p1169.BackColor = p1170.BackColor =
                                        p1171.BackColor = p1172.BackColor = p1173.BackColor = p1174.BackColor = p1175.BackColor = p1176.BackColor = p1177.BackColor = p1178.BackColor = p1179.BackColor = p1180.BackColor =
                                            p1181.BackColor = p1182.BackColor = p1183.BackColor = p1184.BackColor = p1185.BackColor = p1186.BackColor = p1187.BackColor = p1188.BackColor = p1189.BackColor = p1190.BackColor =
                                                p1191.BackColor = p1192.BackColor = p1193.BackColor = p1194.BackColor = p1195.BackColor = p1196.BackColor = p1197.BackColor = p1198.BackColor = p1199.BackColor = p1200.BackColor =
                    p1201.BackColor = p1202.BackColor = p1203.BackColor = p1204.BackColor = p1205.BackColor = p1206.BackColor = p1207.BackColor = p1208.BackColor = p1209.BackColor = p1210.BackColor =
                p1211.BackColor = p1212.BackColor = p1213.BackColor = p1214.BackColor = p1215.BackColor = p1216.BackColor = p1217.BackColor = p1218.BackColor = p1219.BackColor = p1220.BackColor =
                    p1221.BackColor = p1222.BackColor = p1223.BackColor = p1224.BackColor = p1225.BackColor = p1226.BackColor = p1227.BackColor = p1228.BackColor = p1229.BackColor = p1230.BackColor =
                        p1231.BackColor = p1232.BackColor = p1233.BackColor = p1234.BackColor = p1235.BackColor = p1236.BackColor = p1237.BackColor = p1238.BackColor = p1239.BackColor = p1240.BackColor =
                            p1241.BackColor = p1242.BackColor = p1243.BackColor = p1244.BackColor = p1245.BackColor = p1246.BackColor = p1247.BackColor = p1248.BackColor = p1249.BackColor = p1250.BackColor =
                                p1251.BackColor = p1252.BackColor = p1253.BackColor = p1254.BackColor = p1255.BackColor = p1256.BackColor = p1257.BackColor = p1258.BackColor = p1259.BackColor = p1260.BackColor =
                                    p1261.BackColor = p1262.BackColor = p1263.BackColor = p1264.BackColor = p1265.BackColor = p1266.BackColor = p1267.BackColor = p1268.BackColor = p1269.BackColor = p1270.BackColor =
                                        p1271.BackColor = p1272.BackColor = p1273.BackColor = p1274.BackColor = p1275.BackColor = p1276.BackColor = p1277.BackColor = p1278.BackColor = p1279.BackColor = p1280.BackColor =
                                            p1281.BackColor = p1282.BackColor = p1283.BackColor = p1284.BackColor = p1285.BackColor = p1286.BackColor = p1287.BackColor = p1288.BackColor = p1289.BackColor = p1290.BackColor =
                                                p1291.BackColor = p1292.BackColor = p1293.BackColor = p1294.BackColor = p1295.BackColor = p1296.BackColor = p1297.BackColor = p1298.BackColor = p1299.BackColor = p1300.BackColor =
                        p1301.BackColor = p1302.BackColor = p1303.BackColor = p1304.BackColor = p1305.BackColor = p1306.BackColor = p1307.BackColor = p1308.BackColor = p1309.BackColor = p1310.BackColor =
                p1311.BackColor = p1312.BackColor = p1313.BackColor = p1314.BackColor = p1315.BackColor = p1316.BackColor = p1317.BackColor = p1318.BackColor = p1319.BackColor = p1320.BackColor =
                    p1321.BackColor = p1322.BackColor = p1323.BackColor = p1324.BackColor = p1325.BackColor = p1326.BackColor = p1327.BackColor = p1328.BackColor = p1329.BackColor = p1330.BackColor =
                        p1331.BackColor = p1332.BackColor = p1333.BackColor = p1334.BackColor = p1335.BackColor = p1336.BackColor = p1337.BackColor = p1338.BackColor = p1339.BackColor = p1340.BackColor =
                            p1341.BackColor = p1342.BackColor = p1343.BackColor = p1344.BackColor = p1345.BackColor = p1346.BackColor = p1347.BackColor = p1348.BackColor = p1349.BackColor = p1350.BackColor =
                                p1351.BackColor = p1352.BackColor = p1353.BackColor = p1354.BackColor = p1355.BackColor = p1356.BackColor = p1357.BackColor = p1358.BackColor = p1359.BackColor = p1360.BackColor =
                                    p1361.BackColor = p1362.BackColor = p1363.BackColor = p1364.BackColor = p1365.BackColor = p1366.BackColor = p1367.BackColor = p1368.BackColor = p1369.BackColor = p1370.BackColor =
                                        p1371.BackColor = p1372.BackColor = p1373.BackColor = p1374.BackColor = p1375.BackColor = p1376.BackColor = p1377.BackColor = p1378.BackColor = p1379.BackColor = p1380.BackColor =
                                            p1381.BackColor = p1382.BackColor = p1383.BackColor = p1384.BackColor = p1385.BackColor = p1386.BackColor = p1387.BackColor = p1388.BackColor = p1389.BackColor = p1390.BackColor =
                                                p1391.BackColor = p1392.BackColor = p1393.BackColor = p1394.BackColor = p1395.BackColor = p1396.BackColor = p1397.BackColor = p1398.BackColor = p1399.BackColor = p1400.BackColor =
                            p1401.BackColor = p1402.BackColor = p1403.BackColor = p1404.BackColor = p1405.BackColor = p1406.BackColor = p1407.BackColor = p1408.BackColor = p1409.BackColor = p1410.BackColor =
                p1411.BackColor = p1412.BackColor = p1413.BackColor = p1414.BackColor = p1415.BackColor = p1416.BackColor = p1417.BackColor = p1418.BackColor = p1419.BackColor = p1420.BackColor =
                    p1421.BackColor = p1422.BackColor = p1423.BackColor = p1424.BackColor = p1425.BackColor = p1426.BackColor = p1427.BackColor = p1428.BackColor = p1429.BackColor = p1430.BackColor =
                        p1431.BackColor = p1432.BackColor = p1433.BackColor = p1434.BackColor = p1435.BackColor = p1436.BackColor = p1437.BackColor = p1438.BackColor = p1439.BackColor = p1440.BackColor =
                            p1441.BackColor = p1442.BackColor = p1443.BackColor = p1444.BackColor = p1445.BackColor = p1446.BackColor = p1447.BackColor = p1448.BackColor = p1449.BackColor = p1450.BackColor =
                                p1451.BackColor = p1452.BackColor = p1453.BackColor = p1454.BackColor = p1455.BackColor = p1456.BackColor = p1457.BackColor = p1458.BackColor = p1459.BackColor = p1460.BackColor =
                                    p1461.BackColor = p1462.BackColor = p1463.BackColor = p1464.BackColor = p1465.BackColor = p1466.BackColor = p1467.BackColor = p1468.BackColor = p1469.BackColor = p1470.BackColor =
                                        p1471.BackColor = p1472.BackColor = p1473.BackColor = p1474.BackColor = p1475.BackColor = p1476.BackColor = p1477.BackColor = p1478.BackColor = p1479.BackColor = p1480.BackColor =
                                            p1481.BackColor = p1482.BackColor = p1483.BackColor = p1484.BackColor = p1485.BackColor = p1486.BackColor = p1487.BackColor = p1488.BackColor = p1489.BackColor = p1490.BackColor =
                                                p1491.BackColor = p1492.BackColor = p1493.BackColor = p1494.BackColor = p1495.BackColor = p1496.BackColor = p1497.BackColor = p1498.BackColor = p1499.BackColor = p1500.BackColor =
                        p1501.BackColor = p1502.BackColor = p1503.BackColor = p1504.BackColor = p1505.BackColor = p1506.BackColor = p1507.BackColor = p1508.BackColor = p1509.BackColor = p1510.BackColor =
                p1511.BackColor = p1512.BackColor = p1513.BackColor = p1514.BackColor =
                 p2001.BackColor = p2002.BackColor = p2003.BackColor = p2004.BackColor = p2005.BackColor = p2006.BackColor = p2007.BackColor = p2008.BackColor = p2009.BackColor = p2010.BackColor =
                p2011.BackColor = p2012.BackColor = p2013.BackColor = p2014.BackColor = p2015.BackColor = p2016.BackColor = p2017.BackColor = p2018.BackColor = p2019.BackColor = p2020.BackColor =
                    p2021.BackColor = p2022.BackColor = p2023.BackColor = p2024.BackColor = p2025.BackColor = p2026.BackColor = p2027.BackColor = p2028.BackColor = p2029.BackColor = p2030.BackColor =
                        p2031.BackColor = p2032.BackColor = p2033.BackColor = p2034.BackColor = p2035.BackColor = p2036.BackColor = p2037.BackColor = p2038.BackColor = p2039.BackColor = p2040.BackColor =
                            p2041.BackColor = p2042.BackColor = p2043.BackColor = p2044.BackColor = p2045.BackColor = p2046.BackColor = p2047.BackColor = p2048.BackColor = p2049.BackColor = p2050.BackColor =
                Control.DefaultBackColor;
        }

        #endregion

        void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            label19.Text = listBox1.Text;
            cn.Open();
            ds = new DataSet();
            da = new SqlDataAdapter();
            da.SelectCommand = cn.CreateCommand();
            if (dt == DateTime.Today && MainForm.blnPass)
            {
                da.SelectCommand.CommandText = @"select distinct Sessions.*  from sessions inner join films on sessions.film=films.id_films
where date_session='" + dt + "'and movies='" + listBox1.Text + "' and time_session>'" + DateTime.Now.ToLongTimeString() + "' order by time_session";
            }
            else if (dt == DateTime.Today && !MainForm.blnPass)
            {
                da.SelectCommand.CommandText = @"select distinct Sessions.*  from sessions inner join films on sessions.film=films.id_films
where date_session='" + dt + "'and movies='" + listBox1.Text + "' and time_session>'" + DateTime.Now.ToLongTimeString() + "' order by time_session";
            }
            else
                da.SelectCommand.CommandText = @"select distinct Sessions.* from Sessions inner join films on sessions.film=films.id_films where date_session='" + dt + "' and movies='" + listBox1.Text + "' order by time_session";
            da.SelectCommand.ExecuteNonQuery();
            da.Fill(ds, "Sessions");
            cn.Close();
            listBox2.DataSource = ds.Tables["Sessions"];
            listBox2.DisplayMember = "time_session";


            pictureBox1.ImageLocation = null;
            cn.Open();
            ds = new DataSet();
            da = new SqlDataAdapter();
            da.SelectCommand = cn.CreateCommand();
            da.SelectCommand.CommandText = @"select * from ViewDateSessionsFilms where date_session='" + dt + "' and movies='" + listBox1.Text + "'";
            da.SelectCommand.ExecuteNonQuery();
            da.Fill(ds, "ViewDateSessionsFilms");
            cn.Close();
            try
            {
                pictureBox1.ImageLocation = ds.Tables["ViewDateSessionsFilms"].Rows[0].ItemArray[3].ToString();
            }
            catch { }

            cn.Open();
            ds = new DataSet();
            da = new SqlDataAdapter();
            da.SelectCommand = cn.CreateCommand();
            da.SelectCommand.CommandText = @"select Halls.* from Sessions inner join films on sessions.film=films.id_films inner join halls on halls.id_hall=sessions.hall where date_session='" + dt + "' and movies='" + listBox1.Text + "' and time_session='" + listBox2.Text + "'";
            da.SelectCommand.ExecuteNonQuery();
            da.Fill(ds, "Halls");
            cn.Close();
            if (ds.Tables["Halls"].Rows.Count != 0)
            {
                label21.Text = listBox2.Text + " / " + ds.Tables["Halls"].Rows[0].ItemArray[1].ToString();
                idHall = ds.Tables["Halls"].Rows[0].ItemArray[0].ToString();
            }
        }

        void Init()
        {
            label12.Visible = false;
            pictureBox2.ImageLocation = MainForm.dbFolder + "Image\\Paid\\assist.png";
            pictureBox3.ImageLocation = MainForm.dbFolder + "Image\\Paid\\webpay.png";
            pictureBox4.ImageLocation = MainForm.dbFolder + "Image\\Paid\\ipay_mts.png";
            pictureBox5.ImageLocation = MainForm.dbFolder + "Image\\Paid\\ipay_velcom.png";
            pictureBox6.ImageLocation = MainForm.dbFolder + "Image\\Paid\\erip.png";
            tel = "";
            colorPaid = Color.Red;
            colorReservation = Color.Blue;
            colorActive = Color.Green;

            splitContainer2.Panel2Collapsed = true;
            splitContainer3.Panel2Collapsed = true;
            dt = DateTime.Today;
            if (MainForm.blnPass)
            {
                service = 0;
                label10.Visible = label8.Visible = false;
            }
            else
            {
                label84.Text = label83.Text = "бронь";
                service = 3000;
            }
            label20.Text = label15.Text = dt.ToLongDateString();
            cn = new SqlConnection(MainForm.connectionString);
            RefreshForm();
        }

        void RefreshForm()
        {
            cn.Open();
            ds = new DataSet();
            da = new SqlDataAdapter();
            da.SelectCommand = cn.CreateCommand();
            if (dt == DateTime.Today && MainForm.blnPass)
            {
                da.SelectCommand.CommandText = @"select distinct movies from sessions inner join films on sessions.film=films.id_films
where date_session='" + dt + "' and time_session>'" + DateTime.Now.ToLongTimeString() + "'";
            }
            else if (dt == DateTime.Today && !MainForm.blnPass)
            {
                da.SelectCommand.CommandText = @"select distinct movies from sessions inner join films on sessions.film=films.id_films
where date_session='" + dt + "' and time_session>'" + DateTime.Now.AddHours(1).ToLongTimeString() + "'";
            }
            else
                da.SelectCommand.CommandText = @"select distinct movies from ViewDateSessionsFilms where date_session='" + dt + "'";
            da.SelectCommand.ExecuteNonQuery();
            da.Fill(ds, "films");
            cn.Close();
            listBox1.DataSource = ds.Tables["films"];
            listBox1.DisplayMember = "movies";
            label19.Text = listBox1.Text;

            cn.Open();
            ds = new DataSet();
            da = new SqlDataAdapter();
            da.SelectCommand = cn.CreateCommand();
            da.SelectCommand.CommandText = @"select * from ViewDateSessionsFilms where date_session='" + dt + "' and movies='" + listBox1.Text + "'";
            da.SelectCommand.ExecuteNonQuery();
            da.Fill(ds, "ViewDateSessionsFilms");
            cn.Close();
            if (ds.Tables["ViewDateSessionsFilms"].Rows.Count != 0)
            {
                try
                {
                    pictureBox1.ImageLocation = ds.Tables["ViewDateSessionsFilms"].Rows[0].ItemArray[3].ToString();
                }
                catch { }
            }

            cn.Open();
            ds = new DataSet();
            da = new SqlDataAdapter();
            da.SelectCommand = cn.CreateCommand();
            if (dt == DateTime.Today && MainForm.blnPass)
            {
                da.SelectCommand.CommandText = @"select distinct Sessions.*  from sessions inner join films on sessions.film=films.id_films
where date_session='" + dt + "'and movies='" + listBox1.Text + "' and time_session>'" + DateTime.Now.ToLongTimeString() + "' order by time_session";
            }
            else if (dt == DateTime.Today && !MainForm.blnPass)
            {
                da.SelectCommand.CommandText = @"select distinct Sessions.*  from sessions inner join films on sessions.film=films.id_films
where date_session='" + dt + "'and movies='" + listBox1.Text + "' and time_session>'" + DateTime.Now.AddHours(1).ToLongTimeString() + "' order by time_session";
            }
            else
                da.SelectCommand.CommandText = @"select distinct Sessions.* from Sessions inner join films on sessions.film=films.id_films where date_session='" + dt + "' and movies='" + listBox1.Text + "' order by time_session";
            da.SelectCommand.ExecuteNonQuery();
            da.Fill(ds, "Sessions");
            cn.Close();
            listBox2.DataSource = ds.Tables["Sessions"];
            listBox2.DisplayMember = "time_session";

            cn.Open();
            ds = new DataSet();
            da = new SqlDataAdapter();
            da.SelectCommand = cn.CreateCommand();
            da.SelectCommand.CommandText = @"select Halls.* from Sessions inner join films on sessions.film=films.id_films inner join halls on halls.id_hall=sessions.hall where date_session='" + dt + "' and movies='" + listBox1.Text + "' and time_session='" + listBox2.Text + "'";
            da.SelectCommand.ExecuteNonQuery();
            da.Fill(ds, "Halls");
            cn.Close();
            if (ds.Tables["Halls"].Rows.Count != 0)
            {
                label21.Text = listBox2.Text + " / " + ds.Tables["Halls"].Rows[0].ItemArray[1].ToString();
                idHall = ds.Tables["Halls"].Rows[0].ItemArray[0].ToString();
            }
        }

    }
}
