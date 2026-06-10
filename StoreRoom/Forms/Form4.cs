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

        public Form4(CustomerService customerService, int customerId = 0)
        {
            InitializeComponent();
            _customerService = customerService;
            _customerId = customerId;
            this.Load += Form4_Load;
        }

        private async void Form4_Load(object sender, EventArgs e)
        {
            if (_customerId != 0)
            {
                foreverButton1.Text = "بروزرسانی";
                await LoadCustomerData();
            }
            else
            {
                foreverButton1.Text = "ذخیره";
            }
        }

        private async Task LoadCustomerData()
        {
            _customer = await _customerService.GetByIdAsync(_customerId);
            if (_customer != null)
            {
                textBoxEdit1.Text = _customer.Name;
                textBoxEdit2.Text = _customer.Phone;
                textBoxEdit3.Text = _customer.Address;
                textBoxEdit4.Text = _customer.MarketName;
            }
            else
            {
                MessageBox.Show("مشتری یافت نشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private void Clear()
        {
            textBoxEdit1.Text = "";
            textBoxEdit2.Text = "";
            textBoxEdit3.Text = "";
            textBoxEdit4.Text = "";
        }

        private void ResetToAddMode()
        {
            _customerId = 0;
            _customer = null;
            foreverButton1.Text = "ذخیره";
            Clear();
        }

        private async Task SaveCustomer()
        {
            // 1. دریافت مقادیر از فیلدها
            string name = textBoxEdit1.Text.Trim();
            string phone = textBoxEdit2.Text.Trim();
            string address = textBoxEdit3.Text.Trim();
            string marketName = textBoxEdit4.Text.Trim();

            // 2. اعتبارسنجی
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

            // اعتبارسنجی شماره تلفن (اختیاری اما مفید)
            if (!string.IsNullOrWhiteSpace(phone))
            {
                string cleanedPhone = phone.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");
                if (!cleanedPhone.All(char.IsDigit))
                {
                    MessageBox.Show("شماره تلفن فقط باید شامل اعداد باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (cleanedPhone.Length != 11)
                {
                    MessageBox.Show("شماره تلفن باید 11 رقم باشد (مثلاً 09123456789).", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!cleanedPhone.StartsWith("09"))
                {
                    MessageBox.Show("شماره موبایل باید با 09 شروع شود.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                phone = cleanedPhone; // ذخیره نسخه پاک شده
            }

            // 3. ساخت شیء Customer
            var customer = new Customer
            {
                Name = name,
                Phone = string.IsNullOrWhiteSpace(phone) ? null : phone,
                Address = string.IsNullOrWhiteSpace(address) ? null : address,
                MarketName = string.IsNullOrWhiteSpace(marketName) ? null : marketName
            };

            // 4. عملیات ذخیره یا بروزرسانی
            try
            {
                if (_customerId == 0) // درج جدید
                {
                    var result = await _customerService.CreateCustomerAsync(customer);
                    if (result.Success)
                    {
                        MessageBox.Show("مشتری با موفقیت ذخیره شد.", "موفق", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ResetToAddMode(); // بازنشانی به حالت درج جدید
                    }
                    else
                    {
                        MessageBox.Show(result.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else // ویرایش
                {
                    if (_customer == null)
                        _customer = await _customerService.GetByIdAsync(_customerId);

                    if (_customer == null)
                    {
                        MessageBox.Show("مشتری یافت نشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    _customer.Name = name;
                    _customer.Phone = string.IsNullOrWhiteSpace(phone) ? null : phone;
                    _customer.Address = string.IsNullOrWhiteSpace(address) ? null : address;
                    _customer.MarketName = string.IsNullOrWhiteSpace(marketName) ? null : marketName;

                    var result = await _customerService.UpdateCustomerAsync(_customer);
                    if (result.Success)
                    {
                        MessageBox.Show("مشتری با موفقیت بروزرسانی شد.", "موفق", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ResetToAddMode(); // پس از بروزرسانی، فرم به حالت درج جدید بازمی‌گردد
                    }
                    else
                    {
                        MessageBox.Show(result.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطا در ذخیره‌سازی: {ex.Message}", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void foreverButton1_Click(object sender, EventArgs e)
        {
            await SaveCustomer();
        }
    }
}
