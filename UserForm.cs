using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kino
{
    public partial class UserForm : Form
    {
        Timer tm;
        string s;
        public UserForm()
        {
            InitializeComponent();
            tm = new Timer();
            tm.Interval = 500;
            maskedTextBox1.KeyDown += maskedTextBox1_KeyDown;
        }

        void maskedTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                this.Close();
            }
            else if (e.KeyData == Keys.Enter)
            {
                s = maskedTextBox1.Text;
                s = s.Replace(" ", "");
                if (s.Length == 16)
                {
                    ReservationForm.tel = maskedTextBox1.Text;

                    Close();
                }
                else
                {
                    maskedTextBox1.Text = "";
                    maskedTextBox1.BackColor = Color.Red;
                    tm.Start();
                    tm.Tick += tm_Tick;
                }
            }
        }

        void tm_Tick(object sender, EventArgs e)
        {
            maskedTextBox1.BackColor = Color.White;
            tm.Stop();
        }

    }
}
