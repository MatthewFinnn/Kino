﻿using System;
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
    public partial class WebForm : Form
    {
        public WebForm()
        {
            InitializeComponent();
            try
            {
                webBrowser1.Url = new Uri(MainForm.url);
            }
            catch{}
            
        }
    }
}
