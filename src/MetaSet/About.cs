﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MetaSet
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
            this.label3.Text = MetaSet.Version;
        }
    }
}
