using Application.Features.Implementation.GoodsReceipt_Service;
using Domain.Entity;
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
    public partial class Form12 : MaterialForm
    {
        private readonly GoodsReceiptService _goodsReceiptService;
        private Guid _productId;
        public Form12(GoodsReceiptService goodsReceiptService, Guid productId)
        {
            InitializeComponent();
            _goodsReceiptService = goodsReceiptService;
            _productId = productId;
        }
        
        private void Clear()
        {
            textBoxEdit1.Text = "";
            textBoxEdit2.Text = "";
            textBoxEdit3.Text = "";
            textBoxEdit4.Text = "";
            textBoxEdit5.Text = "";
            textBoxEdit6.Text = "";
            textBoxEdit7.Text = "";
        }
        private async Task CreateGoodsReceipt()
        {
            // 0. بررسی وجود شناسه محصول
            if (_productId == Guid.Empty)
            {
                MessageBox.Show("شناسه محصول معتبر نیست. لطفاً یک محصول را انتخاب کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 1. اعتبارسنجی تعداد
            if (!decimal.TryParse(textBoxEdit1.Text.Trim(), out decimal quantity) || quantity <= 0)
            {
                MessageBox.Show("تعداد (Quantity) باید یک عدد معتبر و بزرگتر از صفر باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. اعتبارسنجی قیمت واحد
            if (!decimal.TryParse(textBoxEdit2.Text.Trim(), out decimal unitPrice) || unitPrice < 0)
            {
                MessageBox.Show("قیمت واحد باید یک عدد معتبر (غیرمنفی) باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 3. اعتبارسنجی واحد
            string unit = comboBoxEdit1.Text.Trim();
            if (string.IsNullOrWhiteSpace(unit))
            {
                MessageBox.Show("واحد اندازه‌گیری را انتخاب کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 4. اعتبارسنجی انبار
            if (comboBoxEdit2.SelectedValue == null)
            {
                MessageBox.Show("لطفاً انبار مقصد را انتخاب کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int warehouseId = (int)comboBoxEdit2.SelectedValue;

            // 5. اعتبارسنجی تأمین‌کننده
            if (comboBoxEdit3.SelectedValue == null)
            {
                MessageBox.Show("لطفاً تأمین‌کننده را انتخاب کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int supplierId = (int)comboBoxEdit3.SelectedValue;

            // 6. اعتبارسنجی شماره فاکتور
            string invoiceNumber = textBoxEdit5.Text.Trim();
            if (string.IsNullOrWhiteSpace(invoiceNumber))
            {
                MessageBox.Show("شماره فاکتور الزامی است.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 7. اعتبارسنجی تاریخ فاکتور (اختیاری - در صورت نیاز)
            DateTime invoiceDate = dateTimePicker1.Value;
            if (invoiceDate > DateTime.Today)
            {
                MessageBox.Show("تاریخ فاکتور نمی‌تواند در آینده باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 8. مقداردهی فیلدهای اختیاری (در صورت خالی بودن، null ارسال شود)
            string shelfLocation = string.IsNullOrWhiteSpace(textBoxEdit6.Text.Trim()) ? null : textBoxEdit6.Text.Trim();
            string batchNumber = string.IsNullOrWhiteSpace(textBoxEdit7.Text.Trim()) ? null : textBoxEdit7.Text.Trim();
            string description = string.IsNullOrWhiteSpace(textBoxEdit4.Text.Trim()) ? null : textBoxEdit4.Text.Trim();

            // ساخت شیء
            var receipt = new GoodsReceipt
            {
                ProductId = _productId,
                Quantity = quantity,
                UnitPrice = unitPrice,
                Unit = unit,
                WarehouseId = warehouseId,
                SupplierId = supplierId,
                InvoiceNumber = invoiceNumber,
                InvoiceDate = invoiceDate,
                ShelfLocation = shelfLocation,
                BatchNumber = batchNumber,
                Description = description,
                ScannedBarcode = textBoxEdit3.Text,
                TaxRate = 0,
                ReceiptDate = DateTime.Today,
                CreatedAt = DateTime.Now,
                Status = 0,
                // UserId = Program.CurrentUserId // در صورت وجود کاربر جاری
            };

            try
            {
                await _goodsReceiptService.AddAsync(receipt);
                MessageBox.Show("رسید انبار با موفقیت ثبت شد.", "موفق", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطا در ثبت رسید: {ex.Message}", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void foreverButton1_Click(object sender, EventArgs e)
        {
            await CreateGoodsReceipt();
        }
    }
}
