using ReaLTaiizor.Forms;
using StoreRoom.Forms;

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
            Form2 frm = new Form2(Program.WarehouseService);
            frm.ShowDialog();
        }

        private void hopeButton5_Click(object sender, EventArgs e)
        {
            Form3 frm = new Form3(Program.SupplierService);
            frm.ShowDialog();
        }

        private void hopeButton8_Click(object sender, EventArgs e)
        {
            Form4 frm = new Form4(Program.CustomerService);
            frm.ShowDialog();
        }
    }
}
