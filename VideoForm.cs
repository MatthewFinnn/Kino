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
    public partial class VideoForm : Form
    {
        public VideoForm()
        {
            InitializeComponent();
            try
            {
                axWindowsMediaPlayer1.URL = FilmsForm.trailer;
            }
            catch { }
            this.FormClosing += VideoForm_FormClosing;
        }

        void VideoForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            axWindowsMediaPlayer1.close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
