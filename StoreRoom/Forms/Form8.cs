using Application.Features.Implementation.Warehouse_Service;
using Domain.Entity;
using ReaLTaiizor.Forms;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace StoreRoom.Forms
{
    public partial class Form8 : MaterialForm
    {
        private readonly WarehouseService _warehouseService;
        private int _selectedWarehouseId; // تغییر نام برای وضوح

        public Form8(WarehouseService warehouseService)
        {
            InitializeComponent();
            _warehouseService = warehouseService;
        }

        private void Form8_Load(object sender, EventArgs e)
        {
            textBoxEdit1.Text = "جستجو ...";
        }

        private void textBoxEdit1_Enter(object sender, EventArgs e)
        {
            if (textBoxEdit1.Text == "جستجو ...")
                textBoxEdit1.Text = "";
        }

        // ========== تنظیمات دیتاگرید ==========
        private void DgvPersian()
        {
            if (poisonDataGridView1.Columns.Contains("WarehouseId"))
                poisonDataGridView1.Columns["WarehouseId"].Visible = false;

            poisonDataGridView1.Columns["Name"].HeaderText = "نام انبار";
            poisonDataGridView1.Columns["Location"].HeaderText = "موقعیت";
            poisonDataGridView1.Columns["Max"].HeaderText = "حداکثر ظرفیت";
            poisonDataGridView1.Columns["Min"].HeaderText = "حداقل ظرفیت";
            poisonDataGridView1.Columns["Number"].HeaderText = "تعداد قفسه";
            if (poisonDataGridView1.Columns.Contains("Status"))
                poisonDataGridView1.Columns["Status"].HeaderText = "وضعیت";
        }

        private void CustomizeDataGridView()
        {
            poisonDataGridView1.Font = new Font("Tahoma", 12, FontStyle.Bold);
            poisonDataGridView1.DefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Bold);
            poisonDataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Bold);
            poisonDataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            poisonDataGridView1.ReadOnly = true;
        }

        // ========== بارگذاری داده‌ها ==========
        private async Task LoadWarehousesAsync(bool showEmptyMessage = false)
        {
            int count = (int)dungeonNumeric1.Value;
            if (count <= 0) count = 20;

            var warehouses = await _warehouseService.GetTopWarehousesAsync(count);
            poisonDataGridView1.DataSource = warehouses.ToList();

            DgvPersian();
            CustomizeDataGridView();

            if (showEmptyMessage && !warehouses.Any())
                MessageBox.Show("هیچ انباری یافت نشد.", "اطلاع", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                await LoadWarehousesAsync(showEmptyMessage: true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطا: {ex.Message}", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ========== دریافت شناسه ردیف انتخاب‌شده ==========
        private void poisonDataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && poisonDataGridView1.Rows[e.RowIndex].Cells["WarehouseId"].Value != null)
            {
                _selectedWarehouseId = (int)poisonDataGridView1.Rows[e.RowIndex].Cells["WarehouseId"].Value;
            }
        }

        // ========== منوی ویرایش ==========
        private async void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (_selectedWarehouseId == 0)
            {
                MessageBox.Show("لطفاً ابتدا یک سطر را انتخاب کنید.", "اخطار", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // فرض می‌کنیم یک فرم به نام FormWarehouseEdit برای ویرایش انبار دارید
            var editForm = new Form2(_warehouseService, _selectedWarehouseId);
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                await LoadWarehousesAsync(showEmptyMessage: false);
            }
        }

        // ========== منوی حذف ==========
        private async void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (_selectedWarehouseId == 0)
            {
                MessageBox.Show("لطفاً ابتدا یک سطر را انتخاب کنید.", "اخطار", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("آیا از حذف این انبار اطمینان دارید؟", "تأیید حذف",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    await _warehouseService.DeleteAsync(_selectedWarehouseId);
                    MessageBox.Show("انبار با موفقیت حذف شد.", "موفق", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadWarehousesAsync(showEmptyMessage: false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"خطا در حذف: {ex.Message}", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ========== جستجوی زنده ==========
        private async void textBoxEdit1_TextChanged(object sender, EventArgs e)
        {
            string keyword = textBoxEdit1.Text.Trim();

            if (string.IsNullOrWhiteSpace(keyword))
            {
                await LoadWarehousesAsync(showEmptyMessage: false);
                return;
            }

            var searchResults = await _warehouseService.SearchAsync(keyword);
            poisonDataGridView1.DataSource = searchResults.ToList();
            DgvPersian();
            CustomizeDataGridView();

            if (!searchResults.Any())
                MessageBox.Show("هیچ انباری با این عبارت یافت نشد.", "نتیجه جستجو", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void textBoxEdit1_TextChanged_1(object sender, EventArgs e)
        {
            string name = textBoxEdit1.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                await LoadWarehousesAsync(showEmptyMessage: false);
                return;
            }
        }
    }
}