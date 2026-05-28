using Application.Features.Implementation.Product_Service;
using Domain.Entity;
using ReaLTaiizor.Forms;
using System;
using System.Windows.Forms;

namespace StoreRoom.Forms
{
    public partial class Form5 : MaterialForm
    {
        private readonly ProductService _productService;
        private Guid _productId;

        // سازنده اصلی (برای ویرایش با شناسه)
        public Form5(ProductService productService, Guid productId)
        {
            InitializeComponent();
            _productService = productService;
            _productId = productId;
        }

        // سازنده اضافی برای درج جدید (بدون نیاز به ارسال شناسه)
        public Form5(ProductService productService) : this(productService, Guid.Empty)
        {
        }

        private void Clear()
        {
            textBoxEdit1.Text = "";
            textBoxEdit2.Text = "";
            textBoxEdit3.Text = "";
            textBoxEdit4.Text = "";
            textBoxEdit5.Text = "";
            textBoxEdit6.Text = "";
            textBoxEdit7.Text = "";
            comboBoxEdit1.Text = "";
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
        }

        private async Task SaveProduct()
        {
            try
            {
                string barcode = textBoxEdit1.Text.Trim();
                string name = textBoxEdit2.Text.Trim();
                string description = textBoxEdit5.Text.Trim();
                string baseUnit = comboBoxEdit1.Text.Trim();
                DateTime productionDate = dateTimePicker1.Value;
                DateTime expirationDate = dateTimePicker2.Value;

                int? number = null;
                if (!string.IsNullOrWhiteSpace(textBoxEdit6.Text) && int.TryParse(textBoxEdit6.Text, out int num))
                    number = num;

                decimal? minStock = null;
                if (!string.IsNullOrWhiteSpace(textBoxEdit4.Text) && decimal.TryParse(textBoxEdit4.Text, out decimal min))
                    minStock = min;

               

                decimal? maxStock = null;
                if (!string.IsNullOrWhiteSpace(textBoxEdit3.Text) && decimal.TryParse(textBoxEdit3.Text, out decimal max))
                    maxStock = max;

                decimal? weight = null;
                if (!string.IsNullOrWhiteSpace(textBoxEdit7.Text) && decimal.TryParse(textBoxEdit7.Text, out decimal w))
                    weight = w;

                if (string.IsNullOrWhiteSpace(name))
                {
                    MessageBox.Show("نام محصول نمی‌تواند خالی باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (string.IsNullOrWhiteSpace(baseUnit))
                {
                    MessageBox.Show("واحد پایه نمی‌تواند خالی باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (productionDate > expirationDate)
                {
                    MessageBox.Show("تاریخ تولید نمی‌تواند بعد از تاریخ انقضا باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (number.HasValue && number < 0)
                {
                    MessageBox.Show("تعداد موجودی نمی‌تواند منفی باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_productId == Guid.Empty) // درج جدید
                {
                    var newProduct = new Product
                    {
                        Barcode = string.IsNullOrWhiteSpace(barcode) ? null : barcode,
                        Name = name,
                        Description = string.IsNullOrWhiteSpace(description) ? null : description,
                        BaseUnit = baseUnit,
                        ProductionDate = productionDate,
                        ExpirationDate = expirationDate,
                        Number = number,
                        MinStockLevel = minStock,
                        MaxStockLevel = maxStock,
                        Weight = weight,
                        IsActive = true,
                        CreatedAt = DateTime.Now
                    };
                    var result = await _productService.CreateProductAsync(newProduct);
                    if (result.Success)
                    {
                        MessageBox.Show(result.Message, "موفق", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Clear();
                    }
                    else
                        MessageBox.Show(result.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else // ویرایش
                {
                    var existing = await _productService.GetByIdAsync(_productId);
                    if (existing == null)
                    {
                        MessageBox.Show("محصول یافت نشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    existing.Barcode = string.IsNullOrWhiteSpace(barcode) ? null : barcode;
                    existing.Name = name;
                    existing.Description = string.IsNullOrWhiteSpace(description) ? null : description;
                    existing.BaseUnit = baseUnit;
                    existing.ProductionDate = productionDate;
                    existing.ExpirationDate = expirationDate;
                    existing.Number = number;
                    existing.MinStockLevel = minStock;
                    existing.MaxStockLevel = maxStock;
                    existing.Weight = weight;
                    // IsActive و CreatedAt تغییر نمی‌کنند
                    var result = await _productService.UpdateProductAsync(existing);
                    if (result.Success)
                    {
                        MessageBox.Show(result.Message, "موفق", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Clear();
                    }
                    else
                        MessageBox.Show(result.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

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
            await SaveProduct();
        }

        private async void Form5_Load(object sender, EventArgs e)
        {
            if (_productId != Guid.Empty)
            {
                var product = await _productService.GetByIdAsync(_productId);
                if (product != null)
                {
                    textBoxEdit1.Text = product.Barcode;
                    textBoxEdit2.Text = product.Name;
                    textBoxEdit3.Text = product.MaxStockLevel?.ToString();
                    textBoxEdit4.Text = product.MinStockLevel?.ToString();
                    textBoxEdit5.Text = product.Description;
                    textBoxEdit6.Text = product.Number?.ToString();
                    textBoxEdit7.Text = product.Weight?.ToString();
                    comboBoxEdit1.Text = product.BaseUnit;

                    if (product.ProductionDate.HasValue)
                        dateTimePicker1.Value = product.ProductionDate.Value;
                    else
                        dateTimePicker1.Checked = false;

                    if (product.ExpirationDate.HasValue)
                        dateTimePicker2.Value = product.ExpirationDate.Value;
                    else
                        dateTimePicker2.Checked = false;

                    foreverButton1.Text = "بروزرسانی";
                }
            }
            else
            {
                foreverButton1.Text = "ذخیره";
            }
        }
    }
}