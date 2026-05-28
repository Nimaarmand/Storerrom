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

            if (_editingCategoryId != 0)
            {
                foreverButton1.Text = "بروزرسانی";
                LoadCategoryData();
            }
            else
            {
                foreverButton1.Text = "ذخیره";
            }
        }

        private async void LoadCategoryData()
        {
            _currentCategory = await _categoryService.GetByIdAsync(_editingCategoryId);
            if (_currentCategory != null)
            {
                textBoxEdit1.Text = _currentCategory.Name;
                textBoxEdit2.Text = _currentCategory.Description;
            }
        }

        private async void foreverButton1_Click(object sender, EventArgs e)
        {
            string name = textBoxEdit1.Text.Trim();
            string description = textBoxEdit2.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("نام دسته‌بندی نمی‌تواند خالی باشد.", "خطا");
                return;
            }

            if (_editingCategoryId == 0)  
            {
                var newCategory = new Category
                {
                    Name = name,
                    Description = string.IsNullOrWhiteSpace(description) ? null : description,
                    IsActive = true
                };
                var result = await _categoryService.CreateCategoryAsync(newCategory);
                if (result.Success)
                    MessageBox.Show("دسته‌بندی با موفقیت ثبت شد.", "موفق");
                else
                    MessageBox.Show(result.Message, "خطا");
            }
            else  
            {
                if (_currentCategory == null)
                    _currentCategory = await _categoryService.GetByIdAsync(_editingCategoryId);

                _currentCategory.Name = name;
                _currentCategory.Description = description;

                var result = await _categoryService.UpdateCategoryAsync(_currentCategory);
                if (result.Success)
                    MessageBox.Show("دسته‌بندی با موفقیت بروزرسانی شد.", "موفق");
                else
                    MessageBox.Show(result.Message, "خطا");
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
