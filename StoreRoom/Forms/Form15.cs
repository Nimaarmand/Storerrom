using Application.Features.Implementation.GoodsIssue_Service;
using Application.Features.Implementation.GoodsReceipt_Service;
using Domain.Entity;
using ReaLTaiizor.Controls;
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
    public partial class Form15 : MaterialForm
    {
        private readonly GoodsIssueService _goodsIssueService;
        private Guid _productId;
        public Form15(GoodsIssueService goodsIssueService)
        {
            InitializeComponent();
            _goodsIssueService = goodsIssueService;
        }
        private void Clear()
        {
            textBoxEdit1.Text = "";
            textBoxEdit2.Text = "";
            textBoxEdit3.Text = "";
            textBoxEdit4.Text = "";     
        }
        private async Task CreateGoodsReceipt()
        {
            // 0. بررسی شناسه محصول (از قبل در فرم ذخیره شده، مثلاً _productId)
            if (_productId == Guid.Empty)
            {
                MessageBox.Show("شناسه محصول معتبر نیست. لطفاً یک محصول را انتخاب کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ==============================
            // 1. تعداد خروج (Quantity) - اعتبارسنجی
            // ==============================
            if (!decimal.TryParse(textBoxEdit1.Text.Trim(), out decimal quantity) || quantity <= 0)
            {
                MessageBox.Show("تعداد خروجی (Quantity) باید یک عدد معتبر و بزرگتر از صفر باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ==============================
            // 2. قیمت فروش واحد (UnitSellingPrice) - اختیاری اما اگر وارد شود باید >=0 باشد
            // ==============================
            decimal? unitSellingPrice = null;
            if (!string.IsNullOrWhiteSpace(textBoxEdit2.Text.Trim()))
            {
                if (!decimal.TryParse(textBoxEdit2.Text.Trim(), out decimal price) || price < 0)
                {
                    MessageBox.Show("قیمت فروش واحد باید یک عدد معتبر (غیرمنفی) باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                unitSellingPrice = price;
            }

            // ==============================
            // 3. واحد (Unit) - اجباری
            // ==============================
            string unit = comboBoxEdit1.Text.Trim();
            if (string.IsNullOrWhiteSpace(unit))
            {
                MessageBox.Show("واحد اندازه‌گیری را انتخاب کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ==============================
            // 4. نوع حواله (Type) - اجباری (از یک ComboBox یا RadioButton)
            // ==============================
            if (comboBoxEdit3.SelectedValue == null)
            {
                MessageBox.Show("لطفاً نوع حواله را انتخاب کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            IssueType issueType = (IssueType)comboBoxEdit3.SelectedValue;

            if(comboBoxEdit2.SelectedValue == null)
            {
                MessageBox.Show("لطفاً وضعیت را انتخاب کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // ==============================
            // 5. انبار مبدأ (WarehouseId) - اجباری
            // ==============================
            if (comboBoxEdit5.SelectedValue == null)
            {
                MessageBox.Show("لطفاً انبار مبدأ را انتخاب کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int warehouseId = (int)comboBoxEdit5.SelectedValue;

            // ==============================
            // 6. مشتری (CustomerId) - در صورت نیاز (مثلاً نوع حواله فروش)
            // ==============================
            int? customerId = null;
            if (issueType == IssueType.Sale)
            {
                if (comboBoxEdit4.SelectedValue == null)
                {
                    MessageBox.Show("برای حواله فروش، لطفاً مشتری را انتخاب کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                customerId = (int)comboBoxEdit4.SelectedValue;
            }

            // ==============================
            // 7. شماره فاکتور (InvoiceNumber) - اختیاری (اما اگر وارد شد اعتبارسنجی نشانی)
            // ==============================
            string invoiceNumber = textBoxEdit4.Text.Trim();
            // در صورت نیاز می‌توانید الزامی کنید – ولی در مدل nullable است.

            // ==============================
            // 8. تاریخ فاکتور (InvoiceDate) - اختیاری
            // ==============================
            DateTime? invoiceDate = null;
            if (dateTimePicker1.Checked || dateTimePicker1.Enabled) // فرض کنید یک DateTimePicker دارید
            {
                invoiceDate = dateTimePicker1.Value;
                if (invoiceDate > DateTime.Today)
                {
                    MessageBox.Show("تاریخ فاکتور نمی‌تواند در آینده باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            // ==============================
            // 9. موقعیت قفسه (ShelfLocation) - اختیاری
            // ==============================
            //string shelfLocation = txtShelfLocation.Text.Trim();

            // ==============================
            // 10. شماره سری/دسته (BatchNumber) - اختیاری
            // ==============================
            //string batchNumber = txtBatchNumber.Text.Trim();

            // ==============================
            // 11. توضیحات (Description) - اختیاری
            // ==============================
            string description = textBoxEdit3.Text.Trim();

            // ==============================
            // 12. تاریخ صدور حواله (IssueDate) - اجباری (پیش‌فرض امروز)
            // ==============================
            DateTime issueDate = dateTimePicker2.Value;
            if (issueDate > DateTime.Today)
            {
                MessageBox.Show("تاریخ صدور حواله نمی‌تواند در آینده باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ساخت شیء GoodsIssue
            var goodsIssue = new GoodsIssue
            {
                ProductId = _productId,
                Quantity = quantity,
                Unit = unit,
                UnitSellingPrice = unitSellingPrice,
                Type = issueType,
                CustomerId = customerId,
                InvoiceNumber = string.IsNullOrWhiteSpace(invoiceNumber) ? null : invoiceNumber,
                InvoiceDate = invoiceDate,
                WarehouseId = warehouseId,
                //ShelfLocation = shelfLocation,
                //BatchNumber = batchNumber,
                Description = description,
                IssueDate = issueDate,
                CreatedAt = DateTime.Now,
                Status = 0,                       // وضعیت پیش‌فرض (مثلاً 0 = ثبت شده)
                UserId = Program.CurrentUserId    // شناسه کاربر جاری (از جایی که ذخیره کرده‌اید)
            };

            try
            {
                await _goodsIssueService.AddAsync(goodsIssue);
                MessageBox.Show("حواله خروج کالا با موفقیت ثبت شد.", "موفق", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                Clear();   // متد پاک کردن فرم خود را صدا بزنید
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطا در ثبت حواله خروج: {ex.Message}", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
