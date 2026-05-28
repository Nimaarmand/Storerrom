using Microsoft.Extensions.DependencyInjection;
using ReaLTaiizor.Forms;
using StoreRoom.Forms;
using System.Linq.Expressions;

namespace StoreRoom
{
    public partial class Form1 : MaterialForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void hopeButton1_Click(object sender, EventArgs e)
        {
            var form2 = Program.ServiceProvider.GetRequiredService<Form2>();
            form2.ShowDialog();
        }

        private void hopeButton5_Click(object sender, EventArgs e)
        {
            var form3 = Program.ServiceProvider.GetRequiredService<Form3>();
            form3.ShowDialog();
        }

        private void hopeButton8_Click(object sender, EventArgs e)
        {
            var form4 = Program.ServiceProvider.GetRequiredService<Form4>();
            form4.ShowDialog();
        }

        private void hopeButton3_Click(object sender, EventArgs e)
        {
            var form5 = Program.ServiceProvider.GetRequiredService<Form5>();
            form5.ShowDialog();
        }

        private void hopeButton10_Click(object sender, EventArgs e)
        {
            var form6 = Program.ServiceProvider.GetRequiredService<Form6>();
            form6.ShowDialog();
        }

        private void hopeButton4_Click(object sender, EventArgs e)
        {
            var form7 = Program.ServiceProvider.GetRequiredService<Form7>();
            form7.ShowDialog();

        }

        private void hopeButton2_Click(object sender, EventArgs e)
        {
            var form8 = Program.ServiceProvider.GetRequiredService<Form8>();
            form8.ShowDialog();
        }

        private void hopeButton6_Click(object sender, EventArgs e)
        {
            var form9 = Program.ServiceProvider.GetRequiredService<Form9>();
            form9.ShowDialog();
        }

        private void hopeButton7_Click(object sender, EventArgs e)
        {
            var form10 = Program.ServiceProvider.GetRequiredService<Form10>();
            form10.ShowDialog();
        }

        private void hopeButton9_Click(object sender, EventArgs e)
        {
            var form11 = Program.ServiceProvider.GetRequiredService<Form11>();
            form11.ShowDialog();

        }
    }
}
