using Microsoft.Extensions.DependencyInjection;
using ReaLTaiizor.Forms;
using StoreRoom.Forms;
using System.Linq.Expressions;

namespace StoreRoom
{
    //public partial class Form1 : MaterialForm
    //{
    //    public Form1()
    //    {
    //        InitializeComponent();
    //    }

    //    private void hopeButton1_Click(object sender, EventArgs e)
    //    {
    //        var form2 = Program.ServiceProvider.GetRequiredService<Form2>();
    //        form2.ShowDialog();
    //    }

    //    private void hopeButton5_Click(object sender, EventArgs e)
    //    {
    //        var form3 = Program.ServiceProvider.GetRequiredService<Form3>();
    //        form3.ShowDialog();
    //    }

    //    private void hopeButton8_Click(object sender, EventArgs e)
    //    {
    //        var form4 = Program.ServiceProvider.GetRequiredService<Form4>();
    //        form4.ShowDialog();
    //    }

    //    private void hopeButton3_Click(object sender, EventArgs e)
    //    {
    //        var form5 = Program.ServiceProvider.GetRequiredService<Form5>();
    //        form5.ShowDialog();
    //    }

    //    private void hopeButton10_Click(object sender, EventArgs e)
    //    {
    //        var form6 = Program.ServiceProvider.GetRequiredService<Form6>();
    //        form6.ShowDialog();
    //    }

    //    private void hopeButton4_Click(object sender, EventArgs e)
    //    {
    //        var form7 = Program.ServiceProvider.GetRequiredService<Form7>();
    //        form7.ShowDialog();

    //    }

    //    private void hopeButton2_Click(object sender, EventArgs e)
    //    {
    //        var form8 = Program.ServiceProvider.GetRequiredService<Form8>();
    //        form8.ShowDialog();
    //    }

    //    private void hopeButton6_Click(object sender, EventArgs e)
    //    {
    //        var form9 = Program.ServiceProvider.GetRequiredService<Form9>();
    //        form9.ShowDialog();
    //    }

    //    private void hopeButton7_Click(object sender, EventArgs e)
    //    {
    //        var form10 = Program.ServiceProvider.GetRequiredService<Form10>();
    //        form10.ShowDialog();
    //    }

    //    private void hopeButton9_Click(object sender, EventArgs e)
    //    {
    //        var form11 = Program.ServiceProvider.GetRequiredService<Form11>();
    //        form11.ShowDialog();

    //    }

    //    private void hopeButton14_Click(object sender, EventArgs e)
    //    {
    //        var form13 = Program.ServiceProvider.GetRequiredService<Form13>();
    //        form13.ShowDialog();
    //    }

    //    private void hopeButton13_Click(object sender, EventArgs e)
    //    {
    //        var form17 = Program.ServiceProvider.GetRequiredService<Form17>();
    //        form17.ShowDialog();
    //    }

    //    private void hopeButton15_Click(object sender, EventArgs e)
    //    {
    //        var form16 = Program.ServiceProvider.GetRequiredService<Form16>();
    //        form16.ShowDialog();
    //    }

    //    private void hopeButton16_Click(object sender, EventArgs e)
    //    {
    //        var form19 = Program.ServiceProvider.GetRequiredService<Form19>();
    //        form19.ShowDialog();
    //    }

    //    private void hopeButton12_Click(object sender, EventArgs e)
    //    {
    //        var form21 = Program.ServiceProvider.GetRequiredService<Form21>();
    //        form21.ShowDialog();
    //    }

    //    private void hopeButton11_Click(object sender, EventArgs e)
    //    {
    //        var form22 = Program.ServiceProvider.GetRequiredService<Form22>();
    //        form22.ShowDialog();
    //    }

    //    private void hopeButton18_Click(object sender, EventArgs e)
    //    {
    //        var form23= Program.ServiceProvider.GetRequiredService<Form23>();
    //        form23.ShowDialog();
    //    }
    //}
    public partial class Form1 : MaterialForm
    {
        public Form1()
        {
            InitializeComponent();
            // اطمینان از اینکه panel1 فضای باقی‌مانده را پر کند
            panel1.Dock = DockStyle.Fill;
        }

        /// <summary>
        /// نمایش فرم درون panel1
        /// </summary>
        /// <param name="form">فرم مورد نظر</param>
        /// <param name="fillPanel">آیا فرم کل پنل را پر کند؟ (true = پر کردن، false = اندازه اصلی)</param>
        private void ShowFormInPanel(Form form, bool fillPanel = true)
        {
            // حذف فرم قبلی
            panel1.Controls.Clear();

            // تنظیم فرم برای نمایش به عنوان کنترل فرزند
            form.TopLevel = false;
            form.Visible = true;

            if (fillPanel)
            {
                // حالت پر کردن کل پنل
                form.FormBorderStyle = FormBorderStyle.None;
                form.Dock = DockStyle.Fill;
            }
            else
            {
                // حالت عدم پر کردن – اندازه اصلی و قابلیت جابجایی (اختیاری)
                form.FormBorderStyle = FormBorderStyle.Sizable;
                form.Dock = DockStyle.None;
                form.StartPosition = FormStartPosition.CenterParent;
                // در صورت تمایل می‌توانید اندازه خاصی نیز تعیین کنید:
                // form.Size = new Size(600, 400);
            }

            panel1.Controls.Add(form);
            form.Show();

            // هنگام بسته شدن فرم، آن را از پنل پاک کن
            form.FormClosed += (s, args) =>
            {
                panel1.Controls.Remove(form);
                form.Dispose();
            };
        }

        private void hopeButton1_Click(object sender, EventArgs e)
        {
            var form2 = Program.ServiceProvider.GetRequiredService<Form2>();
            ShowFormInPanel(form2, fillPanel: false);
        }

        private void hopeButton5_Click(object sender, EventArgs e)
        {
            var form3 = Program.ServiceProvider.GetRequiredService<Form3>();
            ShowFormInPanel(form3, fillPanel: false);
        }

        private void hopeButton8_Click(object sender, EventArgs e)
        {
            var form4 = Program.ServiceProvider.GetRequiredService<Form4>();
            ShowFormInPanel(form4, fillPanel: false);
        }

        private void hopeButton3_Click(object sender, EventArgs e)
        {
            var form5 = Program.ServiceProvider.GetRequiredService<Form5>();
            ShowFormInPanel(form5, fillPanel: false);
        }

        private void hopeButton10_Click(object sender, EventArgs e)
        {
            var form6 = Program.ServiceProvider.GetRequiredService<Form6>();
            ShowFormInPanel(form6, fillPanel: false);
        }

        private void hopeButton4_Click(object sender, EventArgs e)
        {
            var form7 = Program.ServiceProvider.GetRequiredService<Form7>();
            ShowFormInPanel(form7);
        }

        private void hopeButton2_Click(object sender, EventArgs e)
        {
            var form8 = Program.ServiceProvider.GetRequiredService<Form8>();
            ShowFormInPanel(form8);
        }

        private void hopeButton6_Click(object sender, EventArgs e)
        {
            var form9 = Program.ServiceProvider.GetRequiredService<Form9>();
            ShowFormInPanel(form9);
        }

        private void hopeButton7_Click(object sender, EventArgs e)
        {
            var form10 = Program.ServiceProvider.GetRequiredService<Form10>();
            ShowFormInPanel(form10);
        }

        private void hopeButton9_Click(object sender, EventArgs e)
        {
            var form11 = Program.ServiceProvider.GetRequiredService<Form11>();
            ShowFormInPanel(form11);
        }

        private void hopeButton14_Click(object sender, EventArgs e)
        {
            var form13 = Program.ServiceProvider.GetRequiredService<Form13>();
            ShowFormInPanel(form13, fillPanel: false);
        }

        private void hopeButton13_Click(object sender, EventArgs e)
        {
            var form17 = Program.ServiceProvider.GetRequiredService<Form17>();
            ShowFormInPanel(form17);
        }

        private void hopeButton15_Click(object sender, EventArgs e)
        {
            var form16 = Program.ServiceProvider.GetRequiredService<Form16>();
            ShowFormInPanel(form16, fillPanel: false);
        }

        private void hopeButton16_Click(object sender, EventArgs e)
        {
            var form19 = Program.ServiceProvider.GetRequiredService<Form19>();
            ShowFormInPanel(form19);
        }

        private void hopeButton12_Click(object sender, EventArgs e)
        {
            var form21 = Program.ServiceProvider.GetRequiredService<Form21>();
            ShowFormInPanel(form21);
        }

        private void hopeButton11_Click(object sender, EventArgs e)
        {
            var form22 = Program.ServiceProvider.GetRequiredService<Form22>();
            ShowFormInPanel(form22);
        }

        private void hopeButton18_Click(object sender, EventArgs e)
        {
            var form23 = Program.ServiceProvider.GetRequiredService<Form23>();
            ShowFormInPanel(form23);
        }

        private void hopeButton17_Click(object sender, EventArgs e)
        {
            var form24 = Program.ServiceProvider.GetRequiredService<Form24>();
            ShowFormInPanel(form24);
        }
    }
}
