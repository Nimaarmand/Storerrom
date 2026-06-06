using Application.Features.Implementation.Usermanagement_Service;
using Domain.Entity;
using ReaLTaiizor.Forms;
using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StoreRoom.Forms
{
    public partial class Form17 : MaterialForm
    {
        private readonly UsermanagementService _usermanagementService;
        private string _userId;
        private CancellationTokenSource _searchCts;
        private readonly SemaphoreSlim _serviceLock = new SemaphoreSlim(1, 1);

        public Form17(UsermanagementService usermanagementService)
        {
            InitializeComponent();
            _usermanagementService = usermanagementService;
            poisonDataGridView1.CellFormatting += PoisonDataGridView1_CellFormatting;
            poisonDataGridView1.DataError += PoisonDataGridView1_DataError;
        }

        private async void Form17_Load(object sender, EventArgs e)
        {
            textBoxEdit1.Text = "جستجو ...";
            await LoadUsersAsync(false);
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

        private async Task LoadUsersAsync(bool showEmptyMessage)
        {
            await _serviceLock.WaitAsync();
            try
            {
                var users = await _usermanagementService.GetAllUsersAsync();
                poisonDataGridView1.DataSource = users.ToList();
                ApplyDataGridViewSettings();

                if (showEmptyMessage && !users.Any())
                    MessageBox.Show("هیچ کاربری یافت نشد.", "اطلاع", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                _serviceLock.Release();
            }
        }

        private void ApplyDataGridViewSettings()
        {
            var hiddenColumns = new[]
            {
                "Id", "Email", "EmailConfirmed", "PasswordHash", "SecurityStamp",
                "ConcurrencyStamp", "PhoneNumber", "PhoneNumberConfirmed",
                "TwoFactorEnabled", "LockoutEnd", "LockoutEnabled", "AccessFailedCount",
                "NormalizedUserName", "NormalizedEmail"
            };
            foreach (string colName in hiddenColumns)
                if (poisonDataGridView1.Columns.Contains(colName))
                    poisonDataGridView1.Columns[colName].Visible = false;

            if (poisonDataGridView1.Columns.Contains("UserName"))
                poisonDataGridView1.Columns["UserName"].HeaderText = "نام کاربری";
            if (poisonDataGridView1.Columns.Contains("FullName"))
                poisonDataGridView1.Columns["FullName"].HeaderText = "نام و نام خانوادگی";
            if (poisonDataGridView1.Columns.Contains("IsActive"))
                poisonDataGridView1.Columns["IsActive"].HeaderText = "وضعیت";

            poisonDataGridView1.Font = new Font("Tahoma", 12, FontStyle.Bold);
            poisonDataGridView1.DefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Bold);
            poisonDataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Bold);
            poisonDataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            poisonDataGridView1.ReadOnly = true;
        }

        private void PoisonDataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (poisonDataGridView1.Columns[e.ColumnIndex].Name == "IsActive" && e.Value != null)
            {
                bool isActive = false;
                try
                {
                    if (e.Value is bool boolValue) isActive = boolValue;
                    else if (e.Value is string strValue) isActive = (strValue == "true" || strValue == "True" || strValue == "1" || strValue == "فعال");
                    else isActive = Convert.ToBoolean(e.Value);
                }
                catch { isActive = false; }

                e.Value = isActive ? "فعال" : "غیرفعال";
                e.FormattingApplied = true;
            }
        }

        private void PoisonDataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private async void foxButton1_Click(object sender, EventArgs e)
        {
            await LoadUsersAsync(true);
        }

        private async void textBoxEdit1_TextChanged(object sender, EventArgs e)
        {
            string keyword = textBoxEdit1.Text.Trim();
            if (keyword == "جستجو ...") return;

            _searchCts?.Cancel();
            _searchCts = new CancellationTokenSource();
            var token = _searchCts.Token;

            await _serviceLock.WaitAsync();
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                {
                    await LoadUsersAsync(false);
                    return;
                }

                var searchResults = await _usermanagementService.SearchUsersAsync(keyword);
                if (token.IsCancellationRequested) return;

                poisonDataGridView1.DataSource = searchResults.ToList();
                ApplyDataGridViewSettings();

                if (!searchResults.Any())
                    MessageBox.Show("هیچ کاربری با عبارت مورد نظر یافت نشد.", "نتیجه جستجو", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (OperationCanceledException) { }
            finally
            {
                _serviceLock.Release();
            }
        }

        private void poisonDataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && poisonDataGridView1.Rows[e.RowIndex].Cells["Id"].Value != null)
                _userId = poisonDataGridView1.Rows[e.RowIndex].Cells["Id"].Value.ToString();
        }

        // دکمه تعیین دسترسی – بدون ارسال سرویس
        private async void تعیندسترسیToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_userId))
            {
                MessageBox.Show("لطفاً ابتدا یک کاربر را انتخاب کنید.");
                return;
            }

            _searchCts?.Cancel();
            await _serviceLock.WaitAsync();
            _serviceLock.Release();

            // فقط userId ارسال می‌شود (Form18 خودش Scope می‌سازد)
            var form18 = new Form18(_userId);
            form18.ShowDialog();
        }
    }
}