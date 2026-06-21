using Microsoft.Extensions.DependencyInjection;
using ReaLTaiizor.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace StoreRoom.Forms
{
    public partial class Form25 : MaterialForm
    {
        public Form25()
        {
            InitializeComponent();
        }

        private async void Form25_Load(object sender, EventArgs e)
        {
            label1.Text = "در حال بارگذاری کمی صبر کنید ....";
            for (int i = 0; i <= 100; i++)
            {
                cyberProgressBar1.Value = i;
                await Task.Delay(50);
            }
            
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
