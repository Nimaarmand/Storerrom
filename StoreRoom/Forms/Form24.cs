using Application.Dto;
using Application.Features.Implementation.GoodsIssue_Service;
using Application.Dto;
using Application.Features.Implementation.GoodsIssue_Service;
using ReaLTaiizor.Forms;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StoreRoom.Forms
{
    public partial class Form24 : MaterialForm
    {
        private readonly GoodsIssueService _goodsIssueService;

        public Form24(GoodsIssueService goodsIssueService)
        {
            InitializeComponent();
            _goodsIssueService = goodsIssueService;

            // اتصال رویداد CellFormatting برای فرمت قیمت و تاریخ
            poisonDataGridView1.CellFormatting += PoisonDataGridView1_CellFormatting;
        }

        // ========== بارگذاری حواله‌های تأیید شده (Status = 1) ==========
        private async Task LoadApprovedIssuesAsync(bool showEmptyMessage)
        {
            int count = (int)dungeonNumeric1.Value;
            if (count <= 0) count = 20;

            try
            {
                var allIssues = await _goodsIssueService.GetApprovedIssuesDtoAsync(count);

                var approvedIssues = allIssues?
                    .Where(i => i != null && i.StatusText == "تأیید شده")
                    .ToList() ?? new List<GoodsIssueDto>();

                poisonDataGridView1.DataSource = approvedIssues;
                DgvPersian();
                CustomizeDataGridView();

                if (showEmptyMessage && approvedIssues.Count == 0)
                    MessageBox.Show("هیچ حواله خروج تأیید شده‌ای یافت نشد.", "اطلاع", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطا در بارگذاری داده‌ها: {ex.Message}", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ========== تنظیمات فارسی ستون‌ها ==========
        private void DgvPersian()
        {
            if (poisonDataGridView1.Columns.Count == 0) return;

            // مخفی کردن ستون‌های غیرضروری
            if (poisonDataGridView1.Columns.Contains("IssueId"))
                poisonDataGridView1.Columns["IssueId"].Visible = false;
            if (poisonDataGridView1.Columns.Contains("BatchNumber"))
                poisonDataGridView1.Columns["BatchNumber"].Visible = false;
            if (poisonDataGridView1.Columns.Contains("TypeText"))
                poisonDataGridView1.Columns["TypeText"].Visible = false;
            if (poisonDataGridView1.Columns.Contains("InvoiceDate"))
                poisonDataGridView1.Columns["InvoiceDate"].Visible = false;

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
            if (poisonDataGridView1.Columns.Contains("CreatedAt"))
                poisonDataGridView1.Columns["CreatedAt"].HeaderText = "تاریخ ثبت حواله";
            if (poisonDataGridView1.Columns.Contains("IssueDate"))
                poisonDataGridView1.Columns["IssueDate"].HeaderText = "تاریخ خروج";
            if (poisonDataGridView1.Columns.Contains("StatusText"))
                poisonDataGridView1.Columns["StatusText"].HeaderText = "وضعیت";
            if (poisonDataGridView1.Columns.Contains("Description"))
                poisonDataGridView1.Columns["Description"].HeaderText = "توضیحات";
            if (poisonDataGridView1.Columns.Contains("ApprovedByUserName"))
                poisonDataGridView1.Columns["ApprovedByUserName"].HeaderText = "تحویل‌دهنده";

            // تنظیم ترتیب نمایش ستون‌ها (از 0 تا آخر)
            int index = 0;
            if (poisonDataGridView1.Columns.Contains("ProductName"))
                poisonDataGridView1.Columns["ProductName"].DisplayIndex = index++;
            if (poisonDataGridView1.Columns.Contains("Description"))
                poisonDataGridView1.Columns["Description"].DisplayIndex = index++;
            if (poisonDataGridView1.Columns.Contains("Quantity"))
                poisonDataGridView1.Columns["Quantity"].DisplayIndex = index++;
            if (poisonDataGridView1.Columns.Contains("Unit"))
                poisonDataGridView1.Columns["Unit"].DisplayIndex = index++;
            if (poisonDataGridView1.Columns.Contains("UnitSellingPrice"))
                poisonDataGridView1.Columns["UnitSellingPrice"].DisplayIndex = index++;
            if (poisonDataGridView1.Columns.Contains("TotalPrice"))
                poisonDataGridView1.Columns["TotalPrice"].DisplayIndex = index++;
            if (poisonDataGridView1.Columns.Contains("StatusText"))
                poisonDataGridView1.Columns["StatusText"].DisplayIndex = index++;
            if (poisonDataGridView1.Columns.Contains("IssueDate"))
                poisonDataGridView1.Columns["IssueDate"].DisplayIndex = index++;
            if (poisonDataGridView1.Columns.Contains("CreatedAt"))
                poisonDataGridView1.Columns["CreatedAt"].DisplayIndex = index++;
            if (poisonDataGridView1.Columns.Contains("CustomerName"))
                poisonDataGridView1.Columns["CustomerName"].DisplayIndex = index++;
            if (poisonDataGridView1.Columns.Contains("WarehouseName"))
                poisonDataGridView1.Columns["WarehouseName"].DisplayIndex = index++;
            if (poisonDataGridView1.Columns.Contains("InvoiceNumber"))
                poisonDataGridView1.Columns["InvoiceNumber"].DisplayIndex = index++;
            if (poisonDataGridView1.Columns.Contains("ApprovedByUserName"))
                poisonDataGridView1.Columns["ApprovedByUserName"].DisplayIndex = index++;

            // تنظیم تراز تاریخ‌ها به چپ
            if (poisonDataGridView1.Columns.Contains("CreatedAt"))
                poisonDataGridView1.Columns["CreatedAt"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            if (poisonDataGridView1.Columns.Contains("IssueDate"))
                poisonDataGridView1.Columns["IssueDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
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

        // ========== رویداد CellFormatting برای فرمت قیمت‌ها ==========
        private void PoisonDataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value == null || e.Value == DBNull.Value) return;

            // فرمت ستون‌های قیمت (UnitSellingPrice و TotalPrice)
            if (poisonDataGridView1.Columns[e.ColumnIndex].Name == "UnitSellingPrice" ||
                poisonDataGridView1.Columns[e.ColumnIndex].Name == "TotalPrice")
            {
                if (decimal.TryParse(e.Value.ToString(), out decimal price))
                {
                    e.Value = price.ToString("#,0");
                    e.FormattingApplied = true;
                }
            }
        }

        // ========== رویدادهای فرم ==========
        private async void Form24_Load(object sender, EventArgs e)
        {
            textBoxEdit1.Text = "جستجو ...";
            poisonDataGridView1.AutoGenerateColumns = true;
            await LoadApprovedIssuesAsync(false);
        }

        private async void textBoxEdit1_TextChanged(object sender, EventArgs e)
        {
            string keyword = textBoxEdit1.Text.Trim();
            if (keyword == "جستجو ...") return;

            if (string.IsNullOrWhiteSpace(keyword))
            {
                await LoadApprovedIssuesAsync(false);
                return;
            }

            try
            {
                var results = await _goodsIssueService.SearchIssuesAsync(keyword, (int)dungeonNumeric1.Value);
                var approvedResults = results?
                    .Where(r => r != null && r.StatusText == "تأیید شده")
                    .ToList() ?? new List<GoodsIssueDto>();

                poisonDataGridView1.DataSource = approvedResults;
                DgvPersian();
                CustomizeDataGridView();

                if (!approvedResults.Any())
                    MessageBox.Show("هیچ حواله خروج تأیید شده با عبارت مورد نظر یافت نشد.", "نتیجه جستجو", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطا در جستجو: {ex.Message}", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

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
                await LoadApprovedIssuesAsync(showEmptyMessage: true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطا در بارگذاری: {ex.Message}", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}