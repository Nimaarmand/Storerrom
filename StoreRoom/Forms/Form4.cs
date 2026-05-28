using Application.Features.Implementation.Category_Service;
using Application.Features.Implementation.Customer_Service;
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
    public partial class Form4 : MaterialForm
    {
        private readonly CustomerService _customerService;
        private int _customerId = 0;
        private Customer _customer;
        public Form4(CustomerService customerService, int custumerid=0)
        {
            InitializeComponent();
            _customerService = customerService;
            _customerId = custumerid;
            if(_customerId!=0)
            {
                foreverButton1.Text = "بروزرسانی";
                LoadCstumerData();
            }
        }
        private async void LoadCstumerData()
        {
            _customer = await _customerService.GetByIdAsync(_customerId);
            if (_customer != null)
            {
                textBoxEdit1.Text = _customer.Name;
                textBoxEdit2.Text = _customer.Address;
                textBoxEdit2.Text = _customer.Phone;
                textBoxEdit2.Text = _customer.MarketName;
            }
        }
        private void Clear()
        {
            textBoxEdit1.Text = "";
            textBoxEdit2.Text = "";
            textBoxEdit3.Text = "";
            textBoxEdit4.Text = "";
        }
        private async Task Sevecustomer()  // نام متد: SaveCustomer
        {
            // 1. اعتبارسنجی میدان‌های ورودی
            string name = textBoxEdit1.Text.Trim();
            string phone = textBoxEdit2.Text.Trim();
            string address = textBoxEdit3.Text.Trim();
            string marketName = textBoxEdit4.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("نام مشتری نمی‌تواند خالی باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(address))
            {
                MessageBox.Show("آدرس مشتری نمی‌تواند خالی باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // (اختیاری) اگر می‌خواهید شماره تلفن را اعتبارسنجی کنید
            if (!string.IsNullOrWhiteSpace(phone) && !phone.All(char.IsDigit))
            {
                MessageBox.Show("شماره تلفن فقط باید شامل اعداد باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // بررسی تعداد ارقام (مثال: 11 رقم برای موبایل ایران)
            if (phone.Length != 11)
            {
                MessageBox.Show("شماره تلفن باید 11 رقم باشد (مثلاً 09123456789).", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // بررسی اینکه با '09' شروع شود (اختیاری ولی توصیه می‌شود)
            if (!phone.StartsWith("09"))
            {
                MessageBox.Show("شماره موبایل باید با 09 شروع شود.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!string.IsNullOrWhiteSpace(phone))
            {
                // حذف فاصله، خط تیره و پرانتز (در صورت وجود)
                string cleanedPhone = phone.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");
            }

                // 2. ساخت شیء Customer
                var customer = new Customer
            {
                Name = name,
                Phone = string.IsNullOrWhiteSpace(phone) ? null : phone,
                Address = string.IsNullOrWhiteSpace(address) ? null : address,
                MarketName = string.IsNullOrWhiteSpace(marketName) ? null : marketName
                // Creationdate به صورت خودکار در سرویس یا مدل مقداردهی می‌شود
            };

            // 3. فراخوانی سرویس برای ذخیره
            try
            {
                // فرض می‌کنیم یک فیلد _customerService از نوع CustomerService وجود دارد
                var result = await _customerService.CreateCustomerAsync(customer);
                if (result.Success)
                {
                    MessageBox.Show("مشتری با موفقیت ذخیره شد.", "موفق", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // پاک کردن فیلدها پس از ذخیره (اختیاری)
                    Clear();
                }
                else
                {
                    MessageBox.Show(result.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطا در ذخیره‌سازی: {ex.Message}", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private async void foreverButton1_Click(object sender, EventArgs e)
        {
            await Sevecustomer();
        }
    }
}
