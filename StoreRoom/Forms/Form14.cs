using Application.Features.Implementation.Usermanagement_Service;
using Microsoft.Extensions.DependencyInjection;
using ReaLTaiizor.Controls;
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
    public partial class Form14 : MaterialForm
    {
        private readonly UsermanagementService _userManagementService;
        public Form14(UsermanagementService userManagementService)
        {
            InitializeComponent();
            _userManagementService = userManagementService;

        }

        private async void foreverButton1_Click(object sender, EventArgs e)
        {
            string username = textBoxEdit1.Text.Trim();
            string password = textBoxEdit2.Text.Trim();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("نام کاربری و رمز عبور را وارد کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                bool isValid = await _userManagementService.LoginAsync(username, password);

                if (isValid)
                {
                    var user = await _userManagementService.FindUserByNameAsync(username);
                    if (user != null)
                    {
                        Program.CurrentUserId = user.Id;
                    }

                   

                    var mainForm = Program.ServiceProvider.GetRequiredService<Form1>();
                    mainForm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("نام کاربری یا رمز عبور اشتباه است.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطا در اتصال: {ex.Message}", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}


