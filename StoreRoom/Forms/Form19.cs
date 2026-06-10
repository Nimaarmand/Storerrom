using Application.Features.Implementation.Usermanagement_Service;
using Domain.Entity;
using ReaLTaiizor.Forms;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace StoreRoom.Forms
{
    public partial class Form19 : MaterialForm
    {
        private readonly UsermanagementService _usermanagementService;
        private List<Role> _allRoles; 

        public Form19(UsermanagementService usermanagementService)
        {
            InitializeComponent();
            _usermanagementService = usermanagementService;
        }

        // ========== بارگذاری نقش‌ها ==========
        private async Task LoadRolesAsync(bool showEmptyMessage = false)
        {
            var roles = await _usermanagementService.GetAllRolesAsync();
            _allRoles = roles.ToList();
            poisonDataGridView1.DataSource = _allRoles;
            ApplyDataGridViewSettings();

            if (showEmptyMessage && !_allRoles.Any())
                MessageBox.Show("هیچ نقشی یافت نشد.", "اطلاع", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ========== اعمال تنظیمات دیتاگرید ==========
        private void ApplyDataGridViewSettings()
        {
            // مخفی کردن ستون Id
            if (poisonDataGridView1.Columns.Contains("Id"))
                poisonDataGridView1.Columns["Id"].Visible = false;

            if (poisonDataGridView1.Columns.Contains("NormalizedName"))
                poisonDataGridView1.Columns["NormalizedName"].Visible = false;

            if (poisonDataGridView1.Columns.Contains("ConcurrencyStamp"))
                poisonDataGridView1.Columns["ConcurrencyStamp"].Visible = false;

            // تنظیم عنوان فارسی ستون‌ها (اختیاری)
            if (poisonDataGridView1.Columns.Contains("Name"))
                poisonDataGridView1.Columns["Name"].HeaderText = "نام نقش";
            if (poisonDataGridView1.Columns.Contains("Discription"))
                poisonDataGridView1.Columns["Discription"].HeaderText = "توضیحات";

            // تنظیم ترتیب نمایش ستون‌ها
            if (poisonDataGridView1.Columns.Contains("Name"))
                poisonDataGridView1.Columns["Name"].DisplayIndex = 0;
            if (poisonDataGridView1.Columns.Contains("Discription"))
                poisonDataGridView1.Columns["Discription"].DisplayIndex = 1;

            // تنظیم فونت و پر کردن فضا
            poisonDataGridView1.Font = new Font("Tahoma", 12, FontStyle.Bold);
            poisonDataGridView1.DefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Bold);
            poisonDataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Bold);
            poisonDataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            poisonDataGridView1.ReadOnly = true;
        }

        // ========== رویداد بارگذاری فرم ==========
        private async void Form19_Load(object sender, EventArgs e)
        {
            textBoxEdit1.Text = "جستجو ...";
            await LoadRolesAsync(showEmptyMessage: false);
        }

        // ========== Placeholder برای تکست‌باکس جستجو ==========
        private void textBoxEdit1_Enter(object sender, EventArgs e)
        {
            if (textBoxEdit1.Text == "جستجو ...")
                textBoxEdit1.Text = "";
        }

        private void textBoxEdit1_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxEdit1.Text))
                textBoxEdit1.Text = "جستجو ...";
        }

        // ========== جستجوی زنده (روی نقش‌ها) ==========
        private async void textBoxEdit1_TextChanged(object sender, EventArgs e)
        {
            string keyword = textBoxEdit1.Text.Trim();
            if (keyword == "جستجو ...") return;

            if (string.IsNullOrWhiteSpace(keyword))
            {
                await LoadRolesAsync(showEmptyMessage: false);
                return;
            }

            // جستجو در حافظه (با LINQ) بر اساس Name و Discription
            var filtered = _allRoles?
                .Where(r => r.Name.Contains(keyword) ||
                            (r.Discription != null && r.Discription.Contains(keyword)))
                .ToList();

            poisonDataGridView1.DataSource = filtered;
            ApplyDataGridViewSettings();

            if (filtered == null || !filtered.Any())
                MessageBox.Show("هیچ نقشی با عبارت مورد نظر یافت نشد.", "نتیجه جستجو",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ========== دکمه نمایش همه نقش‌ها ==========
        private async void foxButton1_Click(object sender, EventArgs e)
        {
            await LoadRolesAsync(showEmptyMessage: true);
        }
    }
}