using Application.Features.Implementation.Category_Service;
using Application.Features.Implementation.Customer_Service;
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
    public partial class Form10 : MaterialForm
    {
        private readonly CustomerService _customerService;
        public Form10(CustomerService customerService)
        {
            InitializeComponent();
            _customerService = customerService;


        }
        int _customerId;
        private void DgvPersian()
        {
            // مخفی کردن ستون‌های غیرضروری
            if (poisonDataGridView1.Columns.Contains("CustomerId"))
                poisonDataGridView1.Columns["CustomerId"].Visible = false;

            // تنظیم عنوان فارسی ستون‌ها
            poisonDataGridView1.Columns["Name"].HeaderText = "نام";
            poisonDataGridView1.Columns["Address"].HeaderText = "آدرس";
            poisonDataGridView1.Columns["Phone"].HeaderText = "تلفن";
            poisonDataGridView1.Columns["MarketName"].HeaderText = "نام فروشگاه";
            poisonDataGridView1.Columns["Creationdate"].HeaderText = "تاریخ ثبت";

            // تنظیم ترتیب نمایش ستون‌ها (اختیاری)
            poisonDataGridView1.Columns["Name"].DisplayIndex = 0;
            poisonDataGridView1.Columns["Address"].DisplayIndex = 1;
            poisonDataGridView1.Columns["Phone"].DisplayIndex = 2;
            poisonDataGridView1.Columns["MarketName"].DisplayIndex = 3;
            poisonDataGridView1.Columns["Creationdate"].DisplayIndex = 4;


            if (poisonDataGridView1.Columns.Contains("Creationdate"))
            {
                poisonDataGridView1.Columns["Creationdate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            }
        }
        private async Task LoadCustomerAsync(bool showEmptyMessage = false)
        {
            int count = (int)dungeonNumeric1.Value;
            if (count <= 0) count = 20;

            var categories = await _customerService.TakeAsync(count);
            poisonDataGridView1.DataSource = categories.ToList();

            DgvPersian();
            CustomizeDataGridView();

            if (showEmptyMessage && !categories.Any())
                MessageBox.Show("هیچ مشتری یافت نشد.", "اطلاع",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        private void Form10_Load(object sender, EventArgs e)
        {
            textBoxEdit1.Text = "جستجو ...";
        }

        private void textBoxEdit1_Enter(object sender, EventArgs e)
        {
            if (textBoxEdit1.Text == "جستجو ...")
            {
                textBoxEdit1.Text = "";
            }
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

                var customer = await _customerService.GetTopSuppliersAsync(count);
                poisonDataGridView1.DataSource = customer.ToList();
                DgvPersian();
                CustomizeDataGridView();

                if (!customer.Any())
                    MessageBox.Show("هیچ محصولی یافت نشد.", "اطلاع",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطا: {ex.Message}", "خطا",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //دکمه حذف
        private async void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (_customerId == 0)
            {
                MessageBox.Show("لطفاً ابتدا یک سطر را انتخاب کنید.", "اخطار",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("آیا از حذف این دسته‌بندی اطمینان دارید؟", "تأیید حذف",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var result = await _customerService.DeleteCustomerAsync(_customerId);
                if (result.Success)
                {
                    MessageBox.Show(result.Message, "موفق", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadCustomerAsync(showEmptyMessage: false);
                }
                else
                {
                    MessageBox.Show(result.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        //دریافت ایدی از دیتا گرید
        private void poisonDataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && poisonDataGridView1.Rows[e.RowIndex].Cells["CustomerId"].Value != null)
            {
                _customerId = (int)poisonDataGridView1.Rows[e.RowIndex].Cells["CustomerId"].Value;
            }

        }
        //دکمه ویرایش
        private async void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (_customerId == 0)
            {
                MessageBox.Show("لطفاً ابتدا یک سطر را انتخاب کنید.", "اخطار",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var editform = new Form4(_customerService, _customerId);
            if (editform.ShowDialog() == DialogResult.OK)
            {
                await LoadCustomerAsync(showEmptyMessage: false);
            }
        }

        private async void textBoxEdit1_TextChanged(object sender, EventArgs e)
        {
            string name = textBoxEdit1.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                await LoadCustomerAsync(showEmptyMessage: false);
                return;
            }
        }
    }
}
