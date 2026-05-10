using Application.Features.Implementation.Warehouse_Service;
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
    public partial class Form2 : MaterialForm
    {
        private readonly WarehouseService _warehouseService;
        public Form2(WarehouseService warehouseService)
        {
            InitializeComponent();
            _warehouseService = warehouseService;
        }

        public void Clear()
        {
            textBoxEdit1.Text = "";
            textBoxEdit2.Text = "";
            textBoxEdit3.Text = "";
            textBoxEdit4.Text = "";
            textBoxEdit5.Text = "";

        }
        private async Task SaveWarehouse() // اصلاح نام تابع
        {
            // 1. اعتبارسنجی فیلدهای متنی (خالی نباشند)
            string name = textBoxEdit1.Text.Trim();
            string location = textBoxEdit2.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("نام انبار نمی‌تواند خالی باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(location))
            {
                MessageBox.Show("موقعیت انبار نمی‌تواند خالی باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. اعتبارسنجی اعداد (و تبدیل آن‌ها)
            if (!int.TryParse(textBoxEdit3.Text.Trim(), out int max))
            {
                MessageBox.Show("حداکثر ظرفیت (Max) باید یک عدد صحیح معتبر باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(textBoxEdit4.Text.Trim(), out int min))
            {
                MessageBox.Show("حداقل ظرفیت (Min) باید یک عدد صحیح معتبر باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(textBoxEdit5.Text.Trim(), out int number))
            {
                MessageBox.Show("تعداد فعلی (Number) باید یک عدد صحیح معتبر باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 3. شرط اصلی: ماکزیمم از مینیمم کمتر نباشد
            if (max < min)
            {
                MessageBox.Show("حداکثر ظرفیت (Max) نمی‌تواند از حداقل ظرفیت (Min) کمتر باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

          
            if (number==null)
            {
                MessageBox.Show($"تعداد قفسه هارا وارد کنید", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 5. ساخت شیء Warehouse
            var warehouse = new Warehouse
            {
                Name = name,
                Location = location,
                Max = max,
                Min = min,
                Number = number,
                Status=comboBoxEdit1.Text
            };

            // 6. ذخیره در دیتابیس
            try
            {
                await _warehouseService.AddAsync(warehouse);

                MessageBox.Show("انبار با موفقیت ذخیره شد.", "موفق", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ;
                Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطا در ذخیره‌سازی در دیتابیس: {ex.Message}", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void foreverButton1_Click(object sender, EventArgs e)
        {
           await SaveWarehouse();
        }
    }
}
