using Application.Features.Implementation.Warehouse_Service;
using Domain.Entity;
using ReaLTaiizor.Forms;
using System;
using System.Windows.Forms;

namespace StoreRoom.Forms
{
    public partial class Form2 : MaterialForm
    {
        private readonly WarehouseService _warehouseService;
        private int _warehouseId = 0; // 0 = حالت درج جدید

        // سازنده برای درج جدید
        public Form2(WarehouseService warehouseService)
        {
            InitializeComponent();
            _warehouseService = warehouseService;
        }

        // سازنده برای ویرایش (دریافت شناسه انبار)
        public Form2(WarehouseService warehouseService, int warehouseId) : this(warehouseService)
        {
            _warehouseId = warehouseId;
        }

        private async void Form2_Load(object sender, EventArgs e)
        {
            if (_warehouseId != 0)
            {
                // حالت ویرایش: بارگذاری اطلاعات
                var warehouse = await _warehouseService.GetByIdAsync(_warehouseId);
                if (warehouse != null)
                {
                    textBoxEdit1.Text = warehouse.Name;
                    textBoxEdit2.Text = warehouse.Location;
                    textBoxEdit3.Text = warehouse.Max.ToString();
                    textBoxEdit4.Text = warehouse.Min.ToString();
                    textBoxEdit5.Text = warehouse.Number.ToString();
                    comboBoxEdit1.Text = warehouse.Status;
                    foreverButton1.Text = "بروزرسانی";
                }
                else
                {
                    MessageBox.Show("انبار یافت نشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }
            }
            else
            {
                foreverButton1.Text = "ذخیره";
            }
        }

        public void Clear()
        {
            textBoxEdit1.Text = "";
            textBoxEdit2.Text = "";
            textBoxEdit3.Text = "";
            textBoxEdit4.Text = "";
            textBoxEdit5.Text = "";
            comboBoxEdit1.Text = "";
        }

        private async Task SaveWarehouse()
        {
            string name = textBoxEdit1.Text.Trim();
            string location = textBoxEdit2.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("نام انبار نمی‌تواند خالی باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(location))
            {
                MessageBox.Show("موقعیت انبار نمی‌تواند خالی باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(textBoxEdit3.Text.Trim(), out int max))
            {
                MessageBox.Show("حداکثر ظرفیت (Max) باید یک عدد صحیح معتبر باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(textBoxEdit4.Text.Trim(), out int min))
            {
                MessageBox.Show("حداقل ظرفیت (Min) باید یک عدد صحیح معتبر باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(textBoxEdit5.Text.Trim(), out int number))
            {
                MessageBox.Show("تعداد فعلی (Number) باید یک عدد صحیح معتبر باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (max < min)
            {
                MessageBox.Show("حداکثر ظرفیت (Max) نمی‌تواند از حداقل ظرفیت (Min) کمتر باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // اگر وضعیت (Status) خالی بود، می‌توانید مقدار پیش‌فرض بدهید
            string status = comboBoxEdit1.Text.Trim();
            if (string.IsNullOrWhiteSpace(status))
                status = "فعال"; // یا هر مقدار پیش‌فرض دیگر

            try
            {
                if (_warehouseId == 0) // درج جدید
                {
                    var warehouse = new Warehouse
                    {
                        Name = name,
                        Location = location,
                        Max = max,
                        Min = min,
                        Number = number,
                        Status = status
                    };
                    await _warehouseService.AddAsync(warehouse);
                    MessageBox.Show("انبار با موفقیت ذخیره شد.", "موفق", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else // ویرایش
                {
                    var existing = await _warehouseService.GetByIdAsync(_warehouseId);
                    if (existing == null)
                    {
                        MessageBox.Show("انبار یافت نشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    existing.Name = name;
                    existing.Location = location;
                    existing.Max = max;
                    existing.Min = min;
                    existing.Number = number;
                    existing.Status = status;
                    await _warehouseService.UpdateAsync(existing);
                    MessageBox.Show("انبار با موفقیت بروزرسانی شد.", "موفق", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                Clear();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطا در ذخیره‌سازی: {ex.Message}", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void foreverButton1_Click(object sender, EventArgs e)
        {
            await SaveWarehouse();
        }
    }
}