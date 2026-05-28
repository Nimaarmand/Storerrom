using Application.Features.Implementation.Supplier_Service;
using Domain.Entity;
using ReaLTaiizor.Forms;
using System;
using System.Linq;
using System.Windows.Forms;

namespace StoreRoom.Forms
{
    public partial class Form3 : MaterialForm
    {
        private readonly SupplierService _supplierService;
        private int _supplierId = 0;

        public Form3(SupplierService supplierService, int supplierId = 0)
        {
            InitializeComponent();
            _supplierService = supplierService;
            _supplierId = supplierId;
            this.Load += Form3_Load; 
        }

        // پاک کردن فیلدها
        private void Clear()
        {
            textBoxEdit1.Text = "";
            textBoxEdit2.Text = "";
            textBoxEdit3.Text = "";
            textBoxEdit4.Text = "";
        }

        // بارگذاری اطلاعات در حالت ویرایش
        private async void Form3_Load(object sender, EventArgs e)
        {
            if (_supplierId != 0)
            {
                var supplier = await _supplierService.GetByIdAsync(_supplierId);
                if (supplier != null)
                {
                    textBoxEdit1.Text = supplier.Name;
                    textBoxEdit2.Text = supplier.Phone;
                    textBoxEdit3.Text = supplier.EconomicCode;
                    textBoxEdit4.Text = supplier.Address;
                    foreverButton1.Text = "بروزرسانی";
                }
                else
                {
                    MessageBox.Show("تأمین‌کننده یافت نشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }
            }
            else
            {
                foreverButton1.Text = "ذخیره";
            }
        }

        // عملیات ذخیره (درج جدید یا ویرایش)
        private async Task SaveSupplier()
        {
            string name = textBoxEdit1.Text.Trim();
            string phone = textBoxEdit2.Text.Trim();
            string economicCode = textBoxEdit3.Text.Trim();
            string address = textBoxEdit4.Text.Trim();

            // اعتبارسنجی
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("نام تأمین‌کننده نمی‌تواند خالی باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(phone))
            {
                MessageBox.Show("شماره تلفن نمی‌تواند خالی باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!string.IsNullOrWhiteSpace(economicCode) && !economicCode.All(char.IsDigit))
            {
                MessageBox.Show("کد اقتصادی باید فقط شامل اعداد باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (_supplierId == 0) // درج جدید
                {
                    var newSupplier = new Supplier
                    {
                        Name = name,
                        Phone = phone,
                        EconomicCode = string.IsNullOrWhiteSpace(economicCode) ? null : economicCode,
                        Address = string.IsNullOrWhiteSpace(address) ? null : address
                    };
                    await _supplierService.AddAsync(newSupplier);
                    MessageBox.Show("تأمین‌کننده با موفقیت ذخیره شد.", "موفق", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else // ویرایش
                {
                    var existing = await _supplierService.GetByIdAsync(_supplierId);
                    if (existing == null)
                    {
                        MessageBox.Show("تأمین‌کننده یافت نشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    existing.Name = name;
                    existing.Phone = phone;
                    existing.EconomicCode = string.IsNullOrWhiteSpace(economicCode) ? null : economicCode;
                    existing.Address = string.IsNullOrWhiteSpace(address) ? null : address;
                    await _supplierService.UpdateAsync(existing);
                    MessageBox.Show("تأمین‌کننده با موفقیت بروزرسانی شد.", "موفق", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                Clear();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطا در ذخیره‌سازی: {ex.Message}", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // رویداد کلیک دکمه
        private async void foreverButton1_Click(object sender, EventArgs e)
        {
            await SaveSupplier();
        }
    }
}