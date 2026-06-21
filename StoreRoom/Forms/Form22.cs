using Application.Dto;
using Application.Features.Implementation.GoodsIssue_Service;
using Application.Features.Implementation.Warehouse_Service;
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
        private int _IssueId;

        public Form22(GoodsIssueService goodsIssueService)
        {
            InitializeComponent();
            _goodsIssueService = goodsIssueService;
            this.Load += Form22_Load;
            poisonDataGridView1.CellFormatting += poisonDataGridView1_CellFormatting;
        }

        private async void Form22_Load(object sender, EventArgs e)
        {
            textBoxEdit1.Text = "جستجو ...";
            poisonDataGridView1.AutoGenerateColumns = true;
            await LoadIssuesAsync(false);
        }

        // ========== بارگذاری داده‌ها ==========
        private async Task LoadIssuesAsync(bool showEmptyMessage)
        {
            int count = (int)dungeonNumeric1.Value;
            if (count <= 0) count = 20;

            try
            {
                var issues = await _goodsIssueService.GetTopPendingIssuesAsync(count);
                var list = issues?.ToList() ?? new System.Collections.Generic.List<GoodsIssueDto>();
                poisonDataGridView1.DataSource = list;
                DgvPersian();
                CustomizeDataGridView();

                if (showEmptyMessage && list.Count == 0)
                    MessageBox.Show("هیچ حواله خروجی یافت نشد.", "اطلاع", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            if (poisonDataGridView1.Columns.Contains("IssueDate"))
                poisonDataGridView1.Columns["IssueDate"].Visible = false;

            if (poisonDataGridView1.Columns.Contains("ApprovedByUserName"))
                poisonDataGridView1.Columns["ApprovedByUserName"].Visible = false;

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
            

            // ✅ تنظیم ستون تاریخ ثبت (CreatedAt)
            if (poisonDataGridView1.Columns.Contains("CreatedAt"))
            {
                poisonDataGridView1.Columns["CreatedAt"].HeaderText = "تاریخ ثبت حواله";
                poisonDataGridView1.Columns["CreatedAt"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            }

            if (poisonDataGridView1.Columns.Contains("StatusText"))
                poisonDataGridView1.Columns["StatusText"].HeaderText = "وضعیت";
            if (poisonDataGridView1.Columns.Contains("Description"))
                poisonDataGridView1.Columns["Description"].HeaderText = "توضیحات";

            // مرتب‌سازی ستون‌ها (DisplayIndex)
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
            if (poisonDataGridView1.Columns.Contains("StatusText"))
                poisonDataGridView1.Columns["StatusText"].DisplayIndex = index++;
            if (poisonDataGridView1.Columns.Contains("CreatedAt"))
                poisonDataGridView1.Columns["CreatedAt"].DisplayIndex = index++;
            if (poisonDataGridView1.Columns.Contains("CustomerName"))
                poisonDataGridView1.Columns["CustomerName"].DisplayIndex = index++;
            if (poisonDataGridView1.Columns.Contains("WarehouseName"))
                poisonDataGridView1.Columns["WarehouseName"].DisplayIndex = index++;
            if (poisonDataGridView1.Columns.Contains("InvoiceNumber"))
                poisonDataGridView1.Columns["InvoiceNumber"].DisplayIndex = index++;
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

        // ========== فرمت‌سازی قیمت‌ها و تاریخ‌ها ==========
        private void poisonDataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value == null || e.Value == DBNull.Value) return;

            if (poisonDataGridView1.Columns[e.ColumnIndex].Name == "UnitSellingPrice" ||
                poisonDataGridView1.Columns[e.ColumnIndex].Name == "TotalPrice")
            {
                if (decimal.TryParse(e.Value.ToString(), out decimal price))
                {
                    e.Value = price.ToString("#,0").Replace(",", "/");
                    e.FormattingApplied = true;
                }
            }
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
                poisonDataGridView1.DataSource = results?.ToList() ?? new System.Collections.Generic.List<GoodsIssueDto>();
                DgvPersian();
                CustomizeDataGridView();

                if (results == null || !results.Any())
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

        private void poisonDataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && poisonDataGridView1.Rows[e.RowIndex].Cells["IssueId"].Value != null)
            {
                _IssueId = (int)poisonDataGridView1.Rows[e.RowIndex].Cells["IssueId"].Value;
            }
        }

        // ========== تأیید حواله ==========
        private async void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (_IssueId == 0)
            {
                MessageBox.Show("لطفاً یک سطر را انتخاب کنید.", "اخطار", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // بررسی وجود کاربر جاری
            if (string.IsNullOrEmpty(Program.CurrentUserId))
            {
                MessageBox.Show("لطفاً ابتدا وارد سیستم شوید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // ارسال شناسه کاربر جاری به متد تأیید
                var result = await _goodsIssueService.ApproveIssueAsync(_IssueId, Program.CurrentUserId);
                if (result.Success)
                {
                    MessageBox.Show(result.Message, "موفق", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadIssuesAsync(false);
                }
                else
                {
                    MessageBox.Show(result.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطای سیستمی: {ex.Message}", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ========== ویرایش حواله ==========
        private async void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (_IssueId == 0)
            {
                MessageBox.Show("لطفاً ابتدا یک سطر را انتخاب کنید.", "اخطار", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var editForm = new Form15(_goodsIssueService, _IssueId);
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                await LoadIssuesAsync(showEmptyMessage: false);
            }
        }

        // ========== حذف حواله ==========
        private async void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (_IssueId == 0)
            {
                MessageBox.Show("لطفاً ابتدا یک سطر را انتخاب کنید.", "اخطار", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("آیا از حذف این حواله خروج اطمینان دارید؟", "تأیید حذف",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    await _goodsIssueService.RemoveByIdAsync(_IssueId);
                    MessageBox.Show("حواله خروج با موفقیت حذف شد.", "موفق", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadIssuesAsync(false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"خطا در حذف حواله: {ex.Message}", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Form22_Load_1(object sender, EventArgs e) { }
    }
}