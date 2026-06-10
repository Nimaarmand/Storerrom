using Application.Features.Implementation.GoodsReceipt_Service;
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
    public partial class Form21 : MaterialForm
    {
        private readonly GoodsReceiptService _goodsReceiptService;

        public Form21(GoodsReceiptService goodsReceiptService)
        {
            InitializeComponent();
            _goodsReceiptService = goodsReceiptService;
            this.Load += Form21_Load;
        }

        private async void Form21_Load(object sender, EventArgs e)
        {
            // تنظیم placeholder برای تکست باکس جستجو (در صورت وجود)
            textBoxEdit1.Text = "جستجو ...";
            // تنظیمات اولیه دیتاگرید
            CustomizeDataGridView();
            // بارگذاری داده‌ها با مقدار پیش‌فرض NumericUpDown (مثلاً 20)
            await LoadReceiptsAsync(showEmptyMessage: false);
        }

        private void textBoxEdit1_Enter(object sender, EventArgs e)
        {
            if (textBoxEdit1.Text == "جستجو ...")
                textBoxEdit1.Text = "";
        }

        private void textBoxEdit1_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxEdit1.Text))
                textBoxEdit1.Text = "جستجو ...";
        }

        // ========== بارگذاری داده‌ها ==========
        private async Task LoadReceiptsAsync(bool showEmptyMessage = false)
        {
            // دریافت تعداد از NumericUpDown (نام کنترل را متناسب با پروژه خود تنظیم کنید)
            int count = (int)dungeonNumeric1.Value;  // فرض می‌کنیم NumericUpDown با همین نام وجود دارد
            if (count <= 0) count = 20;

            // فراخوانی سرویس برای دریافت DTOها
            var receipts = await _goodsReceiptService.GetTopReceiptsWithDetailsAsync(count);
            poisonDataGridView1.DataSource = receipts.ToList();

            // اعمال تنظیمات فارسی و ظاهری
            DgvPersian();          // این متد باید بر اساس خواص DTO تنظیم شود
            CustomizeDataGridView();

            if (showEmptyMessage && !receipts.Any())
                MessageBox.Show("هیچ رسیدی یافت نشد.", "اطلاع", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ========== تنظیمات دیتاگرید (فارسی‌سازی بر اساس GoodsReceiptDto) ==========
        private void DgvPersian()
        {
            // 1. مخفی کردن تمام ستون‌ها
            foreach (DataGridViewColumn col in poisonDataGridView1.Columns)
            {
                col.Visible = false;
            }

            

            // ستون نام محصول
            if (poisonDataGridView1.Columns.Contains("ProductName"))
            {
                poisonDataGridView1.Columns["ProductName"].Visible = true;
                poisonDataGridView1.Columns["ProductName"].HeaderText = "نام محصول";
                poisonDataGridView1.Columns["ProductName"].DisplayIndex = 0;
            }

            if (poisonDataGridView1.Columns.Contains("Description"))
            {
                poisonDataGridView1.Columns["Description"].Visible = true;
                poisonDataGridView1.Columns["Description"].HeaderText = "توضیحات";
                poisonDataGridView1.Columns["Description"].DisplayIndex = 1;
            }

            // ستون تعداد
            if (poisonDataGridView1.Columns.Contains("Quantity"))
            {
                poisonDataGridView1.Columns["Quantity"].Visible = true;
                poisonDataGridView1.Columns["Quantity"].HeaderText = "تعداد";
                poisonDataGridView1.Columns["Quantity"].DisplayIndex = 2;
            }

            // ستون واحد
            if (poisonDataGridView1.Columns.Contains("Unit"))
            {
                poisonDataGridView1.Columns["Unit"].Visible = true;
                poisonDataGridView1.Columns["Unit"].HeaderText = "واحد";
                poisonDataGridView1.Columns["Unit"].DisplayIndex = 3;
            }
            //قیمت
            if (poisonDataGridView1.Columns.Contains("UnitPrice"))
            {
                poisonDataGridView1.Columns["UnitPrice"].Visible = true;
                poisonDataGridView1.Columns["UnitPrice"].HeaderText = "قیمت";
                poisonDataGridView1.Columns["UnitPrice"].DisplayIndex = 4;
            }

            //قیمت کل 
            if (poisonDataGridView1.Columns.Contains("TotalPrice"))
            {
                poisonDataGridView1.Columns["TotalPrice"].Visible = true;
                poisonDataGridView1.Columns["TotalPrice"].HeaderText = "قیمت کل";
                poisonDataGridView1.Columns["TotalPrice"].DisplayIndex = 5;
            }
            // ستون تاریخ ورود
            if (poisonDataGridView1.Columns.Contains("ReceiptDate"))
            {
                poisonDataGridView1.Columns["ReceiptDate"].Visible = true;
                poisonDataGridView1.Columns["ReceiptDate"].HeaderText = "تاریخ ورود";
                poisonDataGridView1.Columns["ReceiptDate"].DisplayIndex = 6;
            }

            //انبار 
            if (poisonDataGridView1.Columns.Contains("WarehouseName"))
            {
                poisonDataGridView1.Columns["WarehouseName"].Visible = true;
                poisonDataGridView1.Columns["WarehouseName"].HeaderText = "انبار";
                poisonDataGridView1.Columns["WarehouseName"].DisplayIndex = 7;
            }
            //تامین کننده 
            if (poisonDataGridView1.Columns.Contains("SupplierName"))
            {
                poisonDataGridView1.Columns["SupplierName"].Visible = true;
                poisonDataGridView1.Columns["SupplierName"].HeaderText = " تأمین‌کننده";
                poisonDataGridView1.Columns["SupplierName"].DisplayIndex = 8;
            }
            // ستون شماره فاکتور
            if (poisonDataGridView1.Columns.Contains("InvoiceNumber"))
            {
                poisonDataGridView1.Columns["InvoiceNumber"].Visible = true;
                poisonDataGridView1.Columns["InvoiceNumber"].HeaderText = "شماره فاکتور";
                poisonDataGridView1.Columns["InvoiceNumber"].DisplayIndex = 9;
            }

            // ستون وضعیت (تأیید شده، در انتظار، لغو شده)
            if (poisonDataGridView1.Columns.Contains("StatusText"))
            {
                poisonDataGridView1.Columns["StatusText"].Visible = true;
                poisonDataGridView1.Columns["StatusText"].HeaderText = "وضعیت";
                poisonDataGridView1.Columns["StatusText"].DisplayIndex =10;
            }

           
            
            if (poisonDataGridView1.Columns.Contains("FullName"))
            {
                poisonDataGridView1.Columns["FullName"].Visible = true;
                poisonDataGridView1.Columns["FullName"].HeaderText = "‌تحویل گیرنده";
                poisonDataGridView1.Columns["FullName"].DisplayIndex = 11;
            }
            
        }
        //private void DgvPersian()
        //{
        //    // مخفی کردن ستون ReceiptId (در صورت وجود)
        //    if (poisonDataGridView1.Columns.Contains("ReceiptId"))
        //        poisonDataGridView1.Columns["ReceiptId"].Visible = false;
        //    if (poisonDataGridView1.Columns.Contains("BatchNumber"))
        //        poisonDataGridView1.Columns["BatchNumber"].Visible = false;

        //    // عنوان‌های فارسی برای ستون‌های DTO
        //    if (poisonDataGridView1.Columns.Contains("ProductName"))
        //        poisonDataGridView1.Columns["ProductName"].HeaderText = "نام محصول";
        //    if (poisonDataGridView1.Columns.Contains("SupplierName"))
        //        poisonDataGridView1.Columns["SupplierName"].HeaderText = "تأمین‌کننده";
        //    if (poisonDataGridView1.Columns.Contains("WarehouseName"))
        //        poisonDataGridView1.Columns["WarehouseName"].HeaderText = "انبار";
        //    if (poisonDataGridView1.Columns.Contains("UserName"))
        //        poisonDataGridView1.Columns["UserName"].HeaderText = "ثبت‌کننده";
        //    if (poisonDataGridView1.Columns.Contains("Quantity"))
        //        poisonDataGridView1.Columns["Quantity"].HeaderText = "تعداد";
        //    if (poisonDataGridView1.Columns.Contains("Unit"))
        //        poisonDataGridView1.Columns["Unit"].HeaderText = "واحد";
        //    if (poisonDataGridView1.Columns.Contains("UnitPrice"))
        //        poisonDataGridView1.Columns["UnitPrice"].HeaderText = "قیمت واحد";
        //    if (poisonDataGridView1.Columns.Contains("TotalPrice"))
        //        poisonDataGridView1.Columns["TotalPrice"].HeaderText = "قیمت کل";
        //    if (poisonDataGridView1.Columns.Contains("InvoiceNumber"))
        //        poisonDataGridView1.Columns["InvoiceNumber"].HeaderText = "شماره فاکتور";
        //    if (poisonDataGridView1.Columns.Contains("InvoiceDate"))
        //        poisonDataGridView1.Columns["InvoiceDate"].HeaderText = "تاریخ فاکتور";
        //    if (poisonDataGridView1.Columns.Contains("ReceiptDate"))
        //        poisonDataGridView1.Columns["ReceiptDate"].HeaderText = "تاریخ ورود";
        //    if (poisonDataGridView1.Columns.Contains("StatusText"))
        //        poisonDataGridView1.Columns["StatusText"].HeaderText = "وضعیت";     
        //    if (poisonDataGridView1.Columns.Contains("Description"))
        //        poisonDataGridView1.Columns["Description"].HeaderText = "توضیحات";

        //    // تنظیم ترتیب نمایش (اختیاری)
        //    int index = 0;
        //    if (poisonDataGridView1.Columns.Contains("ProductName"))
        //        poisonDataGridView1.Columns["ProductName"].DisplayIndex =0;
        //    if (poisonDataGridView1.Columns.Contains("Description"))
        //        poisonDataGridView1.Columns["Description"].DisplayIndex = 1;
        //    if (poisonDataGridView1.Columns.Contains("Quantity"))
        //        poisonDataGridView1.Columns["Quantity"].DisplayIndex =2;
        //    if (poisonDataGridView1.Columns.Contains("Unit"))
        //        poisonDataGridView1.Columns["Unit"].DisplayIndex = 3;
        //    if (poisonDataGridView1.Columns.Contains("UnitPrice"))
        //        poisonDataGridView1.Columns["UnitPrice"].DisplayIndex = 4;
        //    if (poisonDataGridView1.Columns.Contains("WarehouseName"))
        //        poisonDataGridView1.Columns["WarehouseName"].DisplayIndex = 5;
        //    if (poisonDataGridView1.Columns.Contains("SupplierName"))
        //        poisonDataGridView1.Columns["SupplierName"].DisplayIndex = 6;

        //    if (poisonDataGridView1.Columns.Contains("InvoiceNumber"))
        //        poisonDataGridView1.Columns["InvoiceNumber"].DisplayIndex = 7;
        //    if (poisonDataGridView1.Columns.Contains("ReceiptDate"))
        //        poisonDataGridView1.Columns["ReceiptDate"].DisplayIndex =8;
        //    if (poisonDataGridView1.Columns.Contains("StatusText"))
        //        poisonDataGridView1.Columns["StatusText"].DisplayIndex =9;
        //    if (poisonDataGridView1.Columns.Contains("BatchNumber"))
        //        poisonDataGridView1.Columns["BatchNumber"].DisplayIndex = index++;

        //}

        // ========== تنظیمات ظاهری دیتاگرید ==========
        private void CustomizeDataGridView()
        {
            if (poisonDataGridView1.Columns.Count == 0) return;
            poisonDataGridView1.Font = new Font("Tahoma", 12, FontStyle.Bold);
            poisonDataGridView1.DefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Bold);
            poisonDataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Bold);
            poisonDataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            poisonDataGridView1.ReadOnly = true;
        }

        // ========== دکمه نمایش ==========
        private async void foxButton1_Click(object sender, EventArgs e)
        {
            int count = (int)dungeonNumeric1.Value;
            if (count <= 0)
            {
                MessageBox.Show("تعداد باید بزرگتر از صفر باشد", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                await LoadReceiptsAsync(showEmptyMessage: true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطا در بارگذاری داده‌ها: {ex.Message}", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

       


        // ========== رویداد کلیک روی سطر برای نمایش جزئیات (اختیاری) ==========
        private void poisonDataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var receiptId = (int)poisonDataGridView1.Rows[e.RowIndex].Cells["ReceiptId"].Value;
                // باز کردن فرم جزئیات رسید (در صورت وجود)
                // var detailsForm = new FormReceiptDetails(_goodsReceiptService, receiptId);
                // detailsForm.ShowDialog();
            }
        }
        // ========== جستجوی زنده (در صورت وجود تکست باکس جستجو) ==========
        private async void textBoxEdit1_TextChanged(object sender, EventArgs e)
        {
            string keyword = textBoxEdit1.Text.Trim();
            if (keyword == "جستجو ...") return;

            if (string.IsNullOrWhiteSpace(keyword))
            {
                await LoadReceiptsAsync(false);
                return;
            }

            try
            {
                // فرض می‌کنیم متد SearchReceiptsAsync در سرویس وجود دارد
                var searchResults = await _goodsReceiptService.SearchReceiptsAsync(keyword, (int)dungeonNumeric1.Value);
                poisonDataGridView1.DataSource = searchResults.ToList();
                DgvPersian();
                CustomizeDataGridView();

                if (!searchResults.Any())
                    MessageBox.Show("هیچ رسیدی با عبارت مورد نظر یافت نشد.", "نتیجه جستجو", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطا در جستجو: {ex.Message}", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
