using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Bondesjakk
{
    public partial class canvasForm : Form
    {
        public canvasForm()
        {
            InitializeComponent();
        }

       private void OnResize(object sender, EventArgs e)
       {
            Invalidate(true);
       }

        private void OnStart(object sender, EventArgs e)
        {
            brettControl.Start();
        }

        private void OnClear(object sender, EventArgs e)
        {
            brettControl.Clear();
        }
    }
}