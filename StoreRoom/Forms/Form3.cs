using Application.Features.Implementation.Supplier_Service;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
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
    public partial class Form3 : MaterialForm
    {
        private readonly SupplierService _supplierService;
        public Form3(SupplierService supplierService)
        {
            InitializeComponent();
            _supplierService = supplierService;
        }
        public void Clear()
        {
            textBoxEdit1.Text = "";
            textBoxEdit2.Text = "";
            textBoxEdit3.Text = "";
            textBoxEdit4.Text = "";
        }
        private async Task SaveSupplier()
        {
            // 1. اعتبارسنجی فیلدهای اجباری
            string name = textBoxEdit1.Text.Trim();
            string phone = textBoxEdit2.Text.Trim();
            string economicCode = textBoxEdit3.Text.Trim();
            string address = textBoxEdit4.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("نام تامین‌کننده نمی‌تواند خالی باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            if (string.IsNullOrWhiteSpace(phone))
            {
                MessageBox.Show("شماره تلفن نمی‌تواند خالی باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. (اختیاری) اعتبارسنجی ساده برای کد اقتصادی (مثلاً فقط عدد)
            if (!string.IsNullOrWhiteSpace(economicCode) && !economicCode.All(char.IsDigit))
            {
                MessageBox.Show("کد اقتصادی باید فقط شامل اعداد باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 3. ساخت شیء Supplier
            var supplier = new Supplier
            {
                Name = name,
                Phone = string.IsNullOrWhiteSpace(phone) ? null : phone,  // اگر خالی بود null ذخیره کن
                EconomicCode = string.IsNullOrWhiteSpace(economicCode) ? null : economicCode,
                Address = string.IsNullOrWhiteSpace(address) ? null : address
            };

            // 4. ذخیره در دیتابیس
            try
            {
                await _supplierService.AddAsync(supplier);

                MessageBox.Show("تامین‌کننده با موفقیت ذخیره شد.", "موفق", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // (اختیاری) پاک کردن فیلدها
                textBoxEdit1.Text = string.Empty;
                textBoxEdit2.Text = string.Empty;
                textBoxEdit3.Text = string.Empty;
                textBoxEdit4.Text = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطا در ذخیره‌سازی: {ex.Message}", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void foreverButton1_Click(object sender, EventArgs e)
        {
            await SaveSupplier();
        }
    }
}
