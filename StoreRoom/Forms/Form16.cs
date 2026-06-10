using Application.Features.Implementation.Usermanagement_Service;
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
    public partial class Form16 : MaterialForm
    {
        private readonly UsermanagementService _usermanagementService;

        public Form16(UsermanagementService usermanagementService)
        {
            InitializeComponent();
            _usermanagementService = usermanagementService;
        }

        private void Clear()
        {
            textBoxEdit1.Text = "";
            textBoxEdit2.Text = "";
        }

        private async Task CreateRoleAsync()
        {
            string name = textBoxEdit1.Text.Trim();
            string description = textBoxEdit2.Text.Trim(); // توجه: املای صحیح Description

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("نام نقش را وارد کنید", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrEmpty(description))
            {
                MessageBox.Show("توضیحات نقش را وارد کنید", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var role = new Role
            {
                Name = name,
                Discription = description   // مطابق با خاصیت در کلاس Role (نه Discription)
            };

            try
            {
                var result = await _usermanagementService.CreateRoleAsync(role);
                if (result.Succeeded)
                {
                    MessageBox.Show("نقش با موفقیت ایجاد شد.", "موفق", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Clear(); // پاک کردن فیلدها برای ثبت بعدی
                    textBoxEdit1.Focus(); // فوکوس روی نام نقش
                }
                else
                {
                    string errors = string.Join(Environment.NewLine, result.Errors.Select(e => e.Description));
                    MessageBox.Show($"خطا در ایجاد نقش:\n{errors}", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطای سیستمی: {ex.Message}", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void foreverButton1_Click(object sender, EventArgs e)
        {
            await CreateRoleAsync();
        }
    }
}
