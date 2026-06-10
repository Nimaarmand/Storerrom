using Application.Features.Implementation.Usermanagement_Service;
using Domain.Entity;
using ReaLTaiizor.Forms;
using System;
using System.Linq;
using System.Windows.Forms;

namespace StoreRoom.Forms
{
    public partial class Form13 : MaterialForm
    {
        private readonly UsermanagementService _usermanagementService;

        public Form13(UsermanagementService usermanagementService)
        {
            InitializeComponent();
            _usermanagementService = usermanagementService;
            this.Load += Form13_Load;
        }

        private void Form13_Load(object sender, EventArgs e)
        {
            textBoxEdit1.UseSystemPasswordChar = true;
            textBoxEdit2.UseSystemPasswordChar = true;
            textBoxEdit3.KeyPress += TextBoxPhone_KeyPress;
        }

        private void TextBoxPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private string NormalizeString(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            var cleanString = new string(input.Where(c => !char.IsControl(c) && c != '\u200E' && c != '\u200F' && c != '\u202A' && c != '\u202B' && c != '\u202C').ToArray());
            return cleanString.Trim();
        }

        private string ConvertToEnglishDigits(string input)
        {
            var map = new (char Persian, char English)[]
            {
            ('۰','0'), ('۱','1'), ('۲','2'), ('۳','3'), ('۴','4'),
            ('۵','5'), ('۶','6'), ('۷','7'), ('۸','8'), ('۹','9'),
            ('٠','0'), ('١','1'), ('٢','2'), ('٣','3'), ('٤','4'),
            ('٥','5'), ('٦','6'), ('٧','7'), ('٨','8'), ('٩','9')
            };
            var result = input;
            foreach (var item in map)
                result = result.Replace(item.Persian, item.English);
            return result;
        }

        private bool IsValidIranianPhoneNumber(string phoneNumber)
        {
            if (phoneNumber.Length != 11) return false;
            if (!phoneNumber.StartsWith("09")) return false;
            return phoneNumber.All(char.IsDigit);
        }

        private async Task CreateUserAsync()
        {
            string rawPhone = textBoxEdit3.Text.Trim();
            string fullname = textBoxEdit4.Text.Trim();
            string password = NormalizeString(textBoxEdit1.Text);
            string confirmPassword = NormalizeString(textBoxEdit2.Text);

            if (string.IsNullOrWhiteSpace(fullname))
            {
                MessageBox.Show("نام و نام خانوادگی نمی‌تواند خالی باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(rawPhone))
            {
                MessageBox.Show("شماره تلفن نمی‌تواند خالی باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string englishPhone = ConvertToEnglishDigits(rawPhone);
            string cleanedPhone = new string(englishPhone.Where(char.IsDigit).ToArray());

            if (!IsValidIranianPhoneNumber(cleanedPhone))
            {
                MessageBox.Show("شماره تلفن وارد شده معتبر نیست.\nفرمت صحیح: 09123456789 (11 رقم، شروع با 09)", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("رمز عبور را وارد کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (password != confirmPassword)
            {
                MessageBox.Show("رمز عبور و تکرار آن برابر نیستند.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var applicationUser = new ApplicationUser
            {
                UserName = cleanedPhone,
                FullName = fullname,
                IsActive = true
            };

            try
            {
                var result = await _usermanagementService.RegisterUserAsync(applicationUser, password);
                if (result.Succeeded)
                {
                    MessageBox.Show("ثبت‌نام با موفقیت انجام شد.", "موفقیت", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // پاک کردن فیلدها و آماده‌سازی برای ثبت بعدی
                    textBoxEdit1.Text = "";
                    textBoxEdit2.Text = "";
                    textBoxEdit3.Text = "";
                    textBoxEdit4.Text = "";
                    textBoxEdit4.Focus(); // فوکوس روی نام کاربر جدید
                }
                else
                {
                    string errors = string.Join(Environment.NewLine, result.Errors.Select(e => e.Description));
                    MessageBox.Show($"ثبت‌نام ناموفق بود:\n{errors}", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطای سیستمی:\n{ex.Message}", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void foreverButton1_Click(object sender, EventArgs e)
        {
            await CreateUserAsync();
        }

        private void materialCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBoxEdit1.UseSystemPasswordChar = !materialCheckBox1.Checked;
            textBoxEdit2.UseSystemPasswordChar = !materialCheckBox1.Checked;
        }
    }
}