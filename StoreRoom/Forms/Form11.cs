using Application.Features.Implementation.Category_Service;
using Domain.Entity;
using ReaLTaiizor.Forms;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace StoreRoom.Forms
{
    public partial class Form11 : MaterialForm
    {
        private readonly CategoryService _categoryService;
        private int _selectedCategoryId;

        public Form11(CategoryService categoryService)
        {
            InitializeComponent();
            _categoryService = categoryService;
        }

        // ========== متد کمکی برای بارگذاری داده‌ها (قابل await) ==========
        private async Task LoadCategoriesAsync(bool showEmptyMessage = false)
        {
            int count = (int)dungeonNumeric1.Value;
            if (count <= 0) count = 20; 

            var categories = await _categoryService.GetTopCategoryAsync(count);
            poisonDataGridView1.DataSource = categories.ToList();

            DgvPersian();
            CustomizeDataGridView();

            if (showEmptyMessage && !categories.Any())
                MessageBox.Show("هیچ دسته‌بندی یافت نشد.", "اطلاع",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ========== تنظیمات دیتاگرید (فارسی‌سازی، مخفی‌سازی ستون‌ها) ==========
        private void DgvPersian()
        {
            // مخفی کردن ستون‌های غیرضروری (در صورت وجود)
            if (poisonDataGridView1.Columns.Contains("CategoryId"))
                poisonDataGridView1.Columns["CategoryId"].Visible = false;

            if (poisonDataGridView1.Columns.Contains("ParentId"))
                poisonDataGridView1.Columns["ParentId"].Visible = false;

            if (poisonDataGridView1.Columns.Contains("IsActive"))
                poisonDataGridView1.Columns["IsActive"].Visible = false;

            // تنظیم عنوان فارسی ستون‌ها
            if (poisonDataGridView1.Columns.Contains("Name"))
                poisonDataGridView1.Columns["Name"].HeaderText = "نام";

            if (poisonDataGridView1.Columns.Contains("Description"))
                poisonDataGridView1.Columns["Description"].HeaderText = "توضیحات";

            if (poisonDataGridView1.Columns.Contains("Parent"))
                poisonDataGridView1.Columns["Parent"].HeaderText = "دسته والد";

            if (poisonDataGridView1.Columns.Contains("SubCategories"))
                poisonDataGridView1.Columns["SubCategories"].HeaderText = "زیرمجموعه‌ها";

            if (poisonDataGridView1.Columns.Contains("CreatedAt"))
                poisonDataGridView1.Columns["CreatedAt"].HeaderText = "تاریخ ثبت";

            // تنظیم ترتیب نمایش ستون‌ها
            if (poisonDataGridView1.Columns.Contains("Name"))
                poisonDataGridView1.Columns["Name"].DisplayIndex = 0;
            if (poisonDataGridView1.Columns.Contains("Description"))
                poisonDataGridView1.Columns["Description"].DisplayIndex = 1;
            if (poisonDataGridView1.Columns.Contains("Parent"))
                poisonDataGridView1.Columns["Parent"].DisplayIndex = 2;
            if (poisonDataGridView1.Columns.Contains("SubCategories"))
                poisonDataGridView1.Columns["SubCategories"].DisplayIndex = 3;
            if (poisonDataGridView1.Columns.Contains("CreatedAt"))
                poisonDataGridView1.Columns["CreatedAt"].DisplayIndex = 4;
        }

        // ========== تنظیمات ظاهری دیتاگرید ==========
        private void CustomizeDataGridView()
        {
            poisonDataGridView1.Font = new Font("Tahoma", 12, FontStyle.Bold);
            poisonDataGridView1.DefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Bold);
            poisonDataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Bold);
            poisonDataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            poisonDataGridView1.ReadOnly = true; // غیرقابل ویرایش مستقیم
        }

        // ========== رویدادهای فرم ==========
        private async void Form11_Load(object sender, EventArgs e)
        {
            textBoxEdit1.Text = "جستجو ...";
            await LoadCategoriesAsync(showEmptyMessage: false); // بارگذاری خودکار بدون پیغام
        }

        private void textBoxEdit1_Enter(object sender, EventArgs e)
        {
            if (textBoxEdit1.Text == "جستجو ...")
                textBoxEdit1.Text = "";
        }

        // رویداد کلیک دکمه نمایش (با قابلیت نمایش پیغام در صورت خالی بودن)
        private async void foxButton1_Click(object sender, EventArgs e)
        {
            try
            {
                await LoadCategoriesAsync(showEmptyMessage: true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطا در بارگذاری: {ex.Message}", "خطا",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // جستجوی زنده هنگام تایپ در تکست‌باکس
        private async void textBoxEdit1_TextChanged(object sender, EventArgs e)
        {
            string name = textBoxEdit1.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                await LoadCategoriesAsync(showEmptyMessage: false);
                return;
            }

            var searchResults = await _categoryService.SearchByNameAsync(name);
            poisonDataGridView1.DataSource = searchResults.ToList();
            DgvPersian();
            CustomizeDataGridView();

            if (!searchResults.Any())
                MessageBox.Show("هیچ دسته‌بندی با این نام یافت نشد.", "نتیجه جستجو",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // دریافت شناسه دسته‌بندی انتخاب‌شده با کلیک روی سطر
        private void poisonDataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && poisonDataGridView1.Rows[e.RowIndex].Cells["CategoryId"].Value != null)
            {
                _selectedCategoryId = (int)poisonDataGridView1.Rows[e.RowIndex].Cells["CategoryId"].Value;
            }
        }

        // منوی کلیک راست – ویرایش
        private async void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (_selectedCategoryId == 0)
            {
                MessageBox.Show("لطفاً ابتدا یک سطر را انتخاب کنید.", "اخطار",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var editForm = new Form6(_categoryService, _selectedCategoryId);
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                await LoadCategoriesAsync(showEmptyMessage: false);
            }
        }

        // منوی کلیک راست – حذف
        private async void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (_selectedCategoryId == 0)
            {
                MessageBox.Show("لطفاً ابتدا یک سطر را انتخاب کنید.", "اخطار",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("آیا از حذف این دسته‌بندی اطمینان دارید؟", "تأیید حذف",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var result = await _categoryService.DeleteCategoryAsync(_selectedCategoryId);
                if (result.Success)
                {
                    MessageBox.Show(result.Message, "موفق", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadCategoriesAsync(showEmptyMessage: false);
                }
                else
                {
                    MessageBox.Show(result.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}