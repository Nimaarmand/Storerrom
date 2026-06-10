using Application.Features.Implementation.Usermanagement_Service;
using Domain.Entity;
using ReaLTaiizor.Controls;
using ReaLTaiizor.Forms;
using System;
using System.Linq;
using System.Windows.Forms;

namespace StoreRoom.Forms
{
    public partial class Form20 : MaterialForm
    {
        private readonly UsermanagementService _userService;
        private ApplicationUser _foundUser;
        private string _phoneNumber;

        public Form20(UsermanagementService userService)
        {
            InitializeComponent();
            _userService = userService;
            ShowStep(1);
        }

        private void ShowStep(int step)
        {
            panel1.Visible = (step == 1);
            panel2.Visible = (step == 3);
        }

        private bool IsValidPhoneNumber(string phone)
        {
            return phone.Length == 11 && phone.StartsWith("09") && phone.All(char.IsDigit);
        }

        // مرحله 1: بررسی شماره تلفن
        private async void foreverButton1_Click(object sender, EventArgs e)
        {
            string phone = txtPhone.Text.Trim();

            if (string.IsNullOrWhiteSpace(phone))
            {
                MessageBox.Show("شماره تلفن را وارد کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!IsValidPhoneNumber(phone))
            {
                MessageBox.Show("شماره تلفن باید ۱۱ رقم و با ۰۹ شروع شود.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _foundUser = await _userService.FindUserByNameAsync(phone);
            if (_foundUser == null)
            {
                MessageBox.Show("کاربری با این شماره تلفن یافت نشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _phoneNumber = phone;
            string lastFour = phone.Substring(phone.Length - 4);

            DialogResult result = MessageBox.Show(
                $"آیا چهار رقم آخر شماره تلفن شما {lastFour} است؟\nدر صورت تأیید، به مرحله بعد می‌روید.",
                "تأیید هویت",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                txtNewPass.Text = "";
                txtConfirmPass.Text = "";
                ShowStep(3);
            }
        }

        // مرحله 2: تغییر رمز عبور
        private async void foreverButton2_Click(object sender, EventArgs e)
        {
            string newPassword = txtNewPass.Text.Trim();
            string confirmPassword = txtConfirmPass.Text.Trim();

            if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 4)
            {
                MessageBox.Show("رمز عبور جدید باید حداقل ۴ کاراکتر باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("رمز عبور و تکرار آن مطابقت ندارند.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_foundUser == null)
            {
                MessageBox.Show("کاربر یافت نشد. لطفاً فرآیند را از ابتدا شروع کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ShowStep(1);
                return;
            }

            try
            {
                var token = await _userService.GeneratePasswordResetTokenAsync(_foundUser);
                var result = await _userService.ResetPasswordAsync(_foundUser, token, newPassword);

                if (result.Succeeded)
                {
                    MessageBox.Show("رمز عبور با موفقیت تغییر کرد.\nاکنون می‌توانید با رمز جدید وارد شوید.", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    string errors = string.Join("\n", result.Errors.Select(e => e.Description));
                    MessageBox.Show($"خطا در تغییر رمز:\n{errors}", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ShowStep(1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطای سیستمی: {ex.Message}", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ShowStep(1);
            }
        }

       
        private void chkShow_CheckedChanged_1(object sender, EventArgs e)
        {
           
          
            txtNewPass.UseSystemPasswordChar=!chkShow.Checked;
            txtConfirmPass.UseSystemPasswordChar = !chkShow.Checked;
        }
    }
}