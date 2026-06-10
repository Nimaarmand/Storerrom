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

        // سازنده اول (فقط سرویس)
        public Form15(GoodsIssueService goodsIssueService)
        {
            InitializeComponent();
            _goodsIssueService = goodsIssueService;
        }

        // سازنده دوم (سرویس + شناسه محصول) - برای باز شدن از Form7
        public Form15(GoodsIssueService goodsIssueService, Guid productId) : this(goodsIssueService)
        {
            _productId = productId;
        }

        // پاک کردن فیلدهای متنی (و در صورت نیاز کامبوباکس‌ها)
        private void Clear()
        {
            textBoxEdit1.Text = "";
            textBoxEdit2.Text = "";
            textBoxEdit3.Text = "";
            textBoxEdit4.Text = "";
            // در صورت تمایل می‌توانید کامبوباکس‌ها و دیتاپیکرها را هم ریست کنید:
            comboBoxEdit1.SelectedIndex = -1;
            comboBoxEdit2.SelectedIndex = -1;
            comboBoxEdit3.SelectedIndex = -1;
            comboBoxEdit4.SelectedIndex = -1;
            comboBoxEdit5.SelectedIndex = -1;
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
        }

        private async Task CreateGoodsReceipt()
        {
            // 0. بررسی شناسه محصول (از قبل در فرم ذخیره شده، مثلاً _productId)
            if (_productId == Guid.Empty)
            {
                MessageBox.Show("شناسه محصول معتبر نیست. لطفاً یک محصول را انتخاب کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 1. تعداد خروج
            if (!decimal.TryParse(textBoxEdit1.Text.Trim(), out decimal quantity) || quantity <= 0)
            {
                MessageBox.Show("تعداد خروجی (Quantity) باید یک عدد معتبر و بزرگتر از صفر باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. قیمت فروش واحد
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

            // 3. واحد
            string unit = comboBoxEdit1.Text.Trim();
            if (string.IsNullOrWhiteSpace(unit))
            {
                MessageBox.Show("واحد اندازه‌گیری را انتخاب کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 4. نوع حواله
            if (comboBoxEdit3.SelectedValue == null)
            {
                MessageBox.Show("لطفاً نوع حواله را انتخاب کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            IssueType issueType = (IssueType)comboBoxEdit3.SelectedValue;

            // اعتبارسنجی وضعیت (در صورت وجود کامبوباکس وضعیت)
            if (comboBoxEdit2.SelectedValue == null)
            {
                MessageBox.Show("لطفاً وضعیت را انتخاب کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 5. انبار مبدأ
            if (comboBoxEdit5.SelectedValue == null)
            {
                MessageBox.Show("لطفاً انبار مبدأ را انتخاب کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int warehouseId = (int)comboBoxEdit5.SelectedValue;

            // 6. مشتری (در صورت فروش)
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

            // 7. شماره فاکتور
            string invoiceNumber = textBoxEdit4.Text.Trim();

            // 8. تاریخ فاکتور
            DateTime? invoiceDate = null;
            if (dateTimePicker1.Checked)
            {
                invoiceDate = dateTimePicker1.Value;
                if (invoiceDate > DateTime.Today)
                {
                    MessageBox.Show("تاریخ فاکتور نمی‌تواند در آینده باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            // 9. توضیحات
            string description = textBoxEdit3.Text.Trim();

            // 10. تاریخ صدور حواله
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
                Description = description,
                IssueDate = issueDate,
                CreatedAt = DateTime.Now,
                Status = 0,
                UserId = Program.CurrentUserId
            };

            try
            {
                await _goodsIssueService.AddAsync(goodsIssue);
                MessageBox.Show("حواله خروج کالا با موفقیت ثبت شد.", "موفق", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطا در ثبت حواله خروج: {ex.Message}", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // رویداد کلیک دکمه (فرض می‌شود دکمه‌ای با نام foreverButton1 در فرم وجود دارد
        private async void foreverButton1_Click_1(object sender, EventArgs e)
        {
            await CreateGoodsReceipt();
        }
    }
}
