using Application.Features.Implementation.Category_Service;
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
    public partial class Form6 : MaterialForm
    {
        private readonly CategoryService _categoryService;
        private int _editingCategoryId = 0;
        private Category _currentCategory = null;

        public Form6(CategoryService categoryService, int categoryId = 0)
        {
            InitializeComponent();
            _categoryService = categoryService;
            _editingCategoryId = categoryId;
            this.Load += Form6_Load;
        }

        private async void Form6_Load(object sender, EventArgs e)
        {
            if (_editingCategoryId != 0)
            {
                foreverButton1.Text = "بروزرسانی";
                await LoadCategoryData();
            }
            else
            {
                foreverButton1.Text = "ذخیره";
            }
        }

        private async Task LoadCategoryData()
        {
            _currentCategory = await _categoryService.GetByIdAsync(_editingCategoryId);
            if (_currentCategory != null)
            {
                textBoxEdit1.Text = _currentCategory.Name;
                textBoxEdit2.Text = _currentCategory.Description;
            }
            else
            {
                MessageBox.Show("دسته‌بندی یافت نشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private void Clear()
        {
            textBoxEdit1.Text = "";
            textBoxEdit2.Text = "";
        }

        private void ResetToAddMode()
        {
            _editingCategoryId = 0;
            _currentCategory = null;
            foreverButton1.Text = "ذخیره";
            Clear();
        }

        private async void foreverButton1_Click(object sender, EventArgs e)
        {
            string name = textBoxEdit1.Text.Trim();
            string description = textBoxEdit2.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("نام دسته‌بندی نمی‌تواند خالی باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (_editingCategoryId == 0) // درج جدید
                {
                    var newCategory = new Category
                    {
                        Name = name,
                        Description = string.IsNullOrWhiteSpace(description) ? null : description,
                        IsActive = true
                    };
                    // فرض می‌کنیم CategoryService متد CreateCategoryAsync دارد
                    var result = await _categoryService.CreateCategoryAsync(newCategory);
                    if (result.Success)
                    {
                        MessageBox.Show("دسته‌بندی با موفقیت ثبت شد.", "موفق", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ResetToAddMode(); // بازنشانی به حالت درج جدید
                    }
                    else
                    {
                        MessageBox.Show(result.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else // ویرایش
                {
                    if (_currentCategory == null)
                        _currentCategory = await _categoryService.GetByIdAsync(_editingCategoryId);

                    if (_currentCategory == null)
                    {
                        MessageBox.Show("دسته‌بندی یافت نشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    _currentCategory.Name = name;
                    _currentCategory.Description = description;

                    var result = await _categoryService.UpdateCategoryAsync(_currentCategory);
                    if (result.Success)
                    {
                        MessageBox.Show("دسته‌بندی با موفقیت بروزرسانی شد.", "موفق", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
    }
}
