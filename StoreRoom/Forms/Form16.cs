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

        private async void CreateRole()
        {
            string name = textBoxEdit1.Text.Trim();
            string discription = textBoxEdit2.Text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("نام دسترسی را وارد کنید", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrEmpty(discription))
            {
                MessageBox.Show("توضیحات نقش را وارد کنید", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var role = new Role
            {
                Name = name,
                Discription = discription
            };

            try
            {
                var result = await _usermanagementService.CreateRoleAsync(role);
                if (result.Succeeded)
                {
                    MessageBox.Show("نقش با موفقیت ایجاد شد.", "موفق", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Clear();
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
            CreateRole();
        }
    }
}
