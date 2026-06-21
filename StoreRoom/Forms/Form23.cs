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
    public partial class Form23 : MaterialForm
    {
        private readonly GoodsReceiptService _goodsReceiptService;
        public Form23(GoodsReceiptService goodsReceiptService)
        {
            InitializeComponent();
            _goodsReceiptService = goodsReceiptService;
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
                poisonDataGridView1.Columns["StatusText"].DisplayIndex = 10;
            }



            if (poisonDataGridView1.Columns.Contains("FullName"))
            {
                poisonDataGridView1.Columns["FullName"].Visible = true;
                poisonDataGridView1.Columns["FullName"].HeaderText = "‌تحویل گیرنده";
                poisonDataGridView1.Columns["FullName"].DisplayIndex = 11;
            }

        }


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

        private async void Form23_Load(object sender, EventArgs e)
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

        private async void foxButton1_Click(object sender, EventArgs e)
        {
            int count = (int)dungeonNumeric1.Value;
            if (count <= 0)
            {
                MessageBox.Show("تعداد باید بزرگتر از صفر باشد");
                return;
            }

            try
            {
                // فراخوانی متد جدید برای رسیدهای تأیید شده
                var receipts = await _goodsReceiptService.GetTopApprovedReceiptsAsync(count);

                // دیباگ (اختیاری)
                MessageBox.Show($"تعداد رکورد دریافتی: {receipts?.Count() ?? 0}");

                poisonDataGridView1.DataSource = receipts?.ToList();
                DgvPersian();         
                CustomizeDataGridView();

                if (receipts == null || !receipts.Any())
                    MessageBox.Show("هیچ رسید تأیید شده‌ای یافت نشد.", "اطلاع");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطا: {ex.Message}");
            }
        }
    }
}
