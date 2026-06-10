using Application.Features.Implementation.GoodsIssue_Service;
using Domain.Entity;
using ReaLTaiizor.Forms;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StoreRoom.Forms
{
    public partial class Form22 : MaterialForm
    {
        private readonly GoodsIssueService _goodsIssueService;

        // سازنده با تزریق سرویس
        public Form22(GoodsIssueService goodsIssueService)
        {
            InitializeComponent();
            _goodsIssueService = goodsIssueService;
            this.Load += Form22_Load;
        }

        private async void Form22_Load(object sender, EventArgs e)
        {
            textBoxEdit1.Text = "جستجو ...";
            await LoadIssuesAsync(false);
        }




        // ========== بارگذاری داده‌ها ==========
        private async Task LoadIssuesAsync(bool showEmptyMessage)
        {
            int count = (int)dungeonNumeric1.Value;
            if (count <= 0) count = 20;

            var issues = await _goodsIssueService.GetTopIssuesWithDetailsAsync(count);
            poisonDataGridView1.DataSource = issues.ToList();
            DgvPersian();
            CustomizeDataGridView();

            if (showEmptyMessage && !issues.Any())
                MessageBox.Show("هیچ حواله خروجی یافت نشد.", "اطلاع", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ========== تنظیمات فارسی ستون‌ها (بر اساس خواص GoodsIssueDto) ==========
        private void DgvPersian()
        {
            if (poisonDataGridView1.Columns.Count == 0) return;

            // مخفی کردن IssueId
            if (poisonDataGridView1.Columns.Contains("IssueId"))
                poisonDataGridView1.Columns["IssueId"].Visible = false;

            // عنوان‌های فارسی
            if (poisonDataGridView1.Columns.Contains("ProductName"))
                poisonDataGridView1.Columns["ProductName"].HeaderText = "نام محصول";
            if (poisonDataGridView1.Columns.Contains("WarehouseName"))
                poisonDataGridView1.Columns["WarehouseName"].HeaderText = "انبار";
            if (poisonDataGridView1.Columns.Contains("CustomerName"))
                poisonDataGridView1.Columns["CustomerName"].HeaderText = "مشتری";
            if (poisonDataGridView1.Columns.Contains("UserName"))
                poisonDataGridView1.Columns["UserName"].HeaderText = "ثبت‌کننده";
            if (poisonDataGridView1.Columns.Contains("Quantity"))
                poisonDataGridView1.Columns["Quantity"].HeaderText = "تعداد";
            if (poisonDataGridView1.Columns.Contains("Unit"))
                poisonDataGridView1.Columns["Unit"].HeaderText = "واحد";
            if (poisonDataGridView1.Columns.Contains("UnitSellingPrice"))
                poisonDataGridView1.Columns["UnitSellingPrice"].HeaderText = "قیمت فروش";
            if (poisonDataGridView1.Columns.Contains("TotalPrice"))
                poisonDataGridView1.Columns["TotalPrice"].HeaderText = "قیمت کل";
            if (poisonDataGridView1.Columns.Contains("InvoiceNumber"))
                poisonDataGridView1.Columns["InvoiceNumber"].HeaderText = "شماره فاکتور";
            if (poisonDataGridView1.Columns.Contains("InvoiceDate"))
                poisonDataGridView1.Columns["InvoiceDate"].HeaderText = "تاریخ فاکتور";
            if (poisonDataGridView1.Columns.Contains("IssueDate"))
                poisonDataGridView1.Columns["IssueDate"].HeaderText = "تاریخ خروج";
            if (poisonDataGridView1.Columns.Contains("StatusText"))
                poisonDataGridView1.Columns["StatusText"].HeaderText = "وضعیت";
            if (poisonDataGridView1.Columns.Contains("BatchNumber"))
                poisonDataGridView1.Columns["BatchNumber"].HeaderText = "شماره دسته";
            if (poisonDataGridView1.Columns.Contains("Description"))
                poisonDataGridView1.Columns["Description"].HeaderText = "توضیحات";

            // مرتب‌سازی ستون‌ها (اختیاری)
            int index = 0;
            if (poisonDataGridView1.Columns.Contains("ProductName"))
                poisonDataGridView1.Columns["ProductName"].DisplayIndex = index++;
            if (poisonDataGridView1.Columns.Contains("WarehouseName"))
                poisonDataGridView1.Columns["WarehouseName"].DisplayIndex = index++;
            if (poisonDataGridView1.Columns.Contains("CustomerName"))
                poisonDataGridView1.Columns["CustomerName"].DisplayIndex = index++;
            if (poisonDataGridView1.Columns.Contains("Quantity"))
                poisonDataGridView1.Columns["Quantity"].DisplayIndex = index++;
            if (poisonDataGridView1.Columns.Contains("Unit"))
                poisonDataGridView1.Columns["Unit"].DisplayIndex = index++;
            if (poisonDataGridView1.Columns.Contains("UnitSellingPrice"))
                poisonDataGridView1.Columns["UnitSellingPrice"].DisplayIndex = index++;
            if (poisonDataGridView1.Columns.Contains("InvoiceNumber"))
                poisonDataGridView1.Columns["InvoiceNumber"].DisplayIndex = index++;
            if (poisonDataGridView1.Columns.Contains("IssueDate"))
                poisonDataGridView1.Columns["IssueDate"].DisplayIndex = index++;
            if (poisonDataGridView1.Columns.Contains("StatusText"))
                poisonDataGridView1.Columns["StatusText"].DisplayIndex = index++;
            if (poisonDataGridView1.Columns.Contains("BatchNumber"))
                poisonDataGridView1.Columns["BatchNumber"].DisplayIndex = index++;
            if (poisonDataGridView1.Columns.Contains("Description"))
                poisonDataGridView1.Columns["Description"].DisplayIndex = index++;
        }

        // ========== تنظیمات ظاهری دیتاگرید ==========
        private void CustomizeDataGridView()
        {
            if (poisonDataGridView1.Columns.Count == 0) return;
            poisonDataGridView1.Font = new Font("Tahoma", 12, FontStyle.Bold);
            poisonDataGridView1.DefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Bold);
            poisonDataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Bold);
            poisonDataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            poisonDataGridView1.ReadOnly = true;
        }

        // ========== دکمه نمایش ==========
        private async void foxButton1_Click(object sender, EventArgs e)
        {
            int count = (int)dungeonNumeric1.Value;
            if (count <= 0)
            {
                MessageBox.Show("تعداد باید بزرگتر از صفر باشد", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                await LoadIssuesAsync(showEmptyMessage: true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطا در بارگذاری: {ex.Message}", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ========== جستجوی زنده ==========
        private async void textBoxEdit1_TextChanged_1(object sender, EventArgs e)
        {
            string keyword = textBoxEdit1.Text.Trim();
            if (keyword == "جستجو ...") return;

            if (string.IsNullOrWhiteSpace(keyword))
            {
                await LoadIssuesAsync(false);
                return;
            }

            try
            {
                var results = await _goodsIssueService.SearchIssuesAsync(keyword, (int)dungeonNumeric1.Value);
                poisonDataGridView1.DataSource = results.ToList();
                DgvPersian();
                CustomizeDataGridView();

                if (!results.Any())
                    MessageBox.Show("هیچ حواله خروجی با عبارت مورد نظر یافت نشد.", "نتیجه جستجو", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطا در جستجو: {ex.Message}", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

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
    }
}