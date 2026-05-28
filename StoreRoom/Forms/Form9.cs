using Application.Features.Implementation.Category_Service;
using Application.Features.Implementation.Product_Service;
using Application.Features.Implementation.Supplier_Service;
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
    public partial class Form9 : MaterialForm
    {
        private readonly SupplierService _supplierService;
        public Form9(SupplierService supplierService)
        {
            InitializeComponent();
            _supplierService = supplierService;
        }
        int _supplierId = 0;
        private void Form9_Load(object sender, EventArgs e)
        {
            textBoxEdit1.Text = "جستجو ...";
        }
        private void DgvPersian()
        {
            // مخفی کردن ستون‌های غیرضروری
            if (poisonDataGridView1.Columns.Contains("SupplierId"))
                poisonDataGridView1.Columns["SupplierId"].Visible = false;


            // تنظیم عنوان فارسی ستون‌ها
            poisonDataGridView1.Columns["Name"].HeaderText = "نام";
            poisonDataGridView1.Columns["Address"].HeaderText = "آدرس";
            poisonDataGridView1.Columns["Phone"].HeaderText = "تلفن";
            poisonDataGridView1.Columns["EconomicCode"].HeaderText = "کد اقتصادی";


            // تنظیم ترتیب نمایش ستون‌ها (اختیاری)
            //poisonDataGridView1.Columns["Name"].DisplayIndex = 0;

        }
        // ========== متد کمکی برای بارگذاری داده‌ها (قابل await) ==========
        private async Task LoadSupplierAsync(bool showEmptyMessage = false)
        {
            int count = (int)dungeonNumeric1.Value;
            if (count <= 0) count = 20;

            var categories = await _supplierService.TakeAsync(count);
            poisonDataGridView1.DataSource = categories.ToList();

            DgvPersian();
            CustomizeDataGridView();

            if (showEmptyMessage && !categories.Any())
                MessageBox.Show("هیچ دسته‌بندی یافت نشد.", "اطلاع",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void textBoxEdit1_Enter(object sender, EventArgs e)
        {
            if (textBoxEdit1.Text == "جستجو ...")
            {
                textBoxEdit1.Text = "";
            }
        }
        private void CustomizeDataGridView()
        {
            // تنظیم فونت کل دیتاگرید (Tahoma Bold 12)
            poisonDataGridView1.Font = new Font("Tahoma", 12, FontStyle.Bold);
            // همچنین برای اطمینان، فونت سلول‌ها و هدر را نیز تنظیم کنید
            poisonDataGridView1.DefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Bold);
            poisonDataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Bold);

            // پر کردن کل فضای دیتاگرید توسط ستون‌ها
            poisonDataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private async void foxButton1_Click(object sender, EventArgs e)
        {
            int count = (int)dungeonNumeric1.Value;
            if (count <= 0)
            {
                MessageBox.Show("تعداد باید بزرگتر از صفر باشد", "خطا",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {

                var products = await _supplierService.GetTopSuppliersAsync(count);
                poisonDataGridView1.DataSource = products.ToList();
                DgvPersian();
                CustomizeDataGridView();

                if (!products.Any())
                    MessageBox.Show("هیچ محصولی یافت نشد.", "اطلاع",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطا: {ex.Message}", "خطا",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void poisonDataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && poisonDataGridView1.Rows[e.RowIndex].Cells["SupplierId"].Value != null)
            {
                _supplierId = (int)poisonDataGridView1.Rows[e.RowIndex].Cells["SupplierId"].Value;
            }

        }
        //ویرایش 
        private async void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (_supplierId == 0)
            {
                MessageBox.Show("لطفاً ابتدا یک سطر را انتخاب کنید.", "اخطار",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var editForm = new Form3(_supplierService, _supplierId);
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                await LoadSupplierAsync(showEmptyMessage: false);
            }
        }

        private async void textBoxEdit1_TextChanged(object sender, EventArgs e)
        {
            string name = textBoxEdit1.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                await LoadSupplierAsync(showEmptyMessage: false);
                return;
            }
        }
    }
}
