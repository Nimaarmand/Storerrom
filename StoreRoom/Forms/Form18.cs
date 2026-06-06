using Application.Features.Implementation.Usermanagement_Service;
using Domain.Entity;
using Microsoft.Extensions.DependencyInjection;
using ReaLTaiizor.Forms;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StoreRoom.Forms
{
    public partial class Form18 : MaterialForm
    {
        private readonly string _userId;

        public Form18(string userId)
        {
            InitializeComponent();
            _userId = userId;
            this.Load += Form18_Load;
        }

        // متد کمکی برای ایجاد Scope جدید و گرفتن سرویس
        private async Task<T> WithScopeAsync<T>(Func<UsermanagementService, Task<T>> action)
        {
            using (var scope = Program.ServiceProvider.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<UsermanagementService>();
                return await action(service);
            }
        }

        private async void Form18_Load(object sender, EventArgs e)
        {
            await WithScopeAsync(async service =>
            {
                var user = await service.FindUserByIdAsync(_userId);
                if (user == null)
                {
                    MessageBox.Show("کاربر یافت نشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return null;
                }

                // به‌روزرسانی UI باید در همان Context انجام شود
                this.Invoke(new Action(() =>
                {
                    txtUsername.Text = user.UserName;
                    txtUsername.Enabled = false;
                }));

                var allRoles = await service.GetAllRolesAsync();
                this.Invoke(new Action(() =>
                {
                    comboBoxRoles.DataSource = allRoles.ToList();
                    comboBoxRoles.DisplayMember = "Name";
                    comboBoxRoles.ValueMember = "Name";
                    comboBoxRoles.SelectedIndex = -1;
                }));
                return user;
            });
        }

        private async void foreverButton1_Click(object sender, EventArgs e) // افزودن نقش
        {
            string role = comboBoxRoles.SelectedValue?.ToString();
            if (string.IsNullOrEmpty(role))
            {
                MessageBox.Show("لطفاً یک نقش را انتخاب کنید.", "اخطار", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            await WithScopeAsync(async service =>
            {
                var user = await service.FindUserByIdAsync(_userId);
                if (user == null)
                {
                    MessageBox.Show("کاربر یافت نشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                var result = await service.AddToRoleAsync(user, role);
                if (result.Succeeded)
                {
                    MessageBox.Show($"نقش '{role}' با موفقیت به کاربر اضافه شد.", "موفق", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // به‌روزرسانی ComboBox (در صورت نیاز)
                }
                else
                {
                    string errors = string.Join(Environment.NewLine, result.Errors.Select(e => e.Description));
                    MessageBox.Show($"خطا: {errors}", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return true;
            });
        }

        private async void foreverButton2_Click(object sender, EventArgs e) // حذف نقش
        {
            string role = comboBoxRoles.SelectedValue?.ToString();
            if (string.IsNullOrEmpty(role))
            {
                MessageBox.Show("لطفاً یک نقش را انتخاب کنید.", "اخطار", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            await WithScopeAsync(async service =>
            {
                var user = await service.FindUserByIdAsync(_userId);
                if (user == null)
                {
                    MessageBox.Show("کاربر یافت نشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                var result = await service.RemoveFromRoleAsync(user, role);
                if (result.Succeeded)
                {
                    MessageBox.Show($"نقش '{role}' با موفقیت از کاربر حذف شد.", "موفق", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    string errors = string.Join(Environment.NewLine, result.Errors.Select(e => e.Description));
                    MessageBox.Show($"خطا: {errors}", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return true;
            });
        }
    }
}