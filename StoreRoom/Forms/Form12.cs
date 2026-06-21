using Application.Features.Implementation.GoodsReceipt_Service;
using Application.Features.Implementation.Supplier_Service;
using Application.Features.Implementation.Warehouse_Service;
using Domain.Entity;
using Microsoft.Extensions.DependencyInjection;
using ReaLTaiizor.Forms;
using System;
using System.Linq;
using System.Windows.Forms;

namespace StoreRoom.Forms
{
    public partial class Form12 : MaterialForm
    {
        private readonly GoodsReceiptService _goodsReceiptService;
        private readonly WarehouseService _warehouseService;
        private readonly SupplierService _supplierService;
        private Guid _productId;
        private int _receiptId = 0;   // برای ویرایش

        // ========== سازنده‌ها ==========

        // سازنده برای ویرایش (فقط GoodsReceiptService + receiptId)
        public Form12(GoodsReceiptService goodsReceiptService, int receiptId)
            : this(goodsReceiptService,
                   Program.ServiceProvider.GetRequiredService<WarehouseService>(),
                   Program.ServiceProvider.GetRequiredService<SupplierService>(),
                   Guid.Empty)
        {
            _receiptId = receiptId;
        }

        // سازنده برای درج جدید با productId (از Form7)
        public Form12(GoodsReceiptService goodsReceiptService, Guid productId)
            : this(goodsReceiptService,
                   Program.ServiceProvider.GetRequiredService<WarehouseService>(),
                   Program.ServiceProvider.GetRequiredService<SupplierService>(),
                   productId)
        {
        }

        // سازنده اصلی (با همه سرویس‌ها و productId)
        public Form12(
            GoodsReceiptService goodsReceiptService,
            WarehouseService warehouseService,
            SupplierService supplierService,
            Guid productId)
        {
            InitializeComponent();
            _goodsReceiptService = goodsReceiptService;
            _warehouseService = warehouseService;
            _supplierService = supplierService;
            _productId = productId;
            this.Load += Form12_Load;
        }

        // پاک کردن همه فیلدها
        private void Clear()
        {
            textBoxEdit1.Text = "";
            textBoxEdit2.Text = "";
            textBoxEdit3.Text = "";
            textBoxEdit4.Text = "";
            textBoxEdit5.Text = "";
            textBoxEdit6.Text = "";
            textBoxEdit7.Text = "";
            comboBoxEdit1.SelectedIndex = -1;
            comboBoxEdit2.SelectedIndex = -1;
            comboBoxEdit3.SelectedIndex = -1;
            dateTimePicker1.Value = DateTime.Now;
        }

        private async Task CreateGoodsReceipt()
        {
            // اگر در حالت ویرایش هستیم و productId هنوز مشخص نیست، از رسید موجود بخوان
            if (_receiptId > 0 && _productId == Guid.Empty)
            {
                var existingReceipt = await _goodsReceiptService.GetByIdAsync(_receiptId);
                if (existingReceipt != null)
                    _productId = existingReceipt.ProductId;
                else
                {
                    MessageBox.Show("رسید مورد نظر یافت نشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (_productId == Guid.Empty)
            {
                MessageBox.Show("شناسه محصول معتبر نیست. لطفاً یک محصول را انتخاب کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(textBoxEdit1.Text.Trim(), out decimal quantity) || quantity <= 0)
            {
                MessageBox.Show("تعداد (Quantity) باید یک عدد معتبر و بزرگتر از صفر باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(textBoxEdit2.Text.Trim(), out decimal unitPrice) || unitPrice < 0)
            {
                MessageBox.Show("قیمت واحد باید یک عدد معتبر (غیرمنفی) باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string unit = comboBoxEdit1.Text.Trim();
            if (string.IsNullOrWhiteSpace(unit))
            {
                MessageBox.Show("واحد اندازه‌گیری را انتخاب کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (comboBoxEdit2.SelectedValue == null)
            {
                MessageBox.Show("لطفاً انبار مقصد را انتخاب کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int warehouseId = (int)comboBoxEdit2.SelectedValue;

            if (comboBoxEdit3.SelectedValue == null)
            {
                MessageBox.Show("لطفاً تأمین‌کننده را انتخاب کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int supplierId = (int)comboBoxEdit3.SelectedValue;

            string invoiceNumber = textBoxEdit5.Text.Trim();
            if (string.IsNullOrWhiteSpace(invoiceNumber))
            {
                MessageBox.Show("شماره فاکتور الزامی است.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime invoiceDate = dateTimePicker1.Value.Date;
            if (invoiceDate > DateTime.Today)
            {
                MessageBox.Show("تاریخ فاکتور نمی‌تواند در آینده باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string shelfLocation = string.IsNullOrWhiteSpace(textBoxEdit6.Text.Trim()) ? null : textBoxEdit6.Text.Trim();
            string batchNumber = string.IsNullOrWhiteSpace(textBoxEdit7.Text.Trim()) ? null : textBoxEdit7.Text.Trim();
            string description = string.IsNullOrWhiteSpace(textBoxEdit4.Text.Trim()) ? null : textBoxEdit4.Text.Trim();
            string scannedBarcode = string.IsNullOrWhiteSpace(textBoxEdit3.Text.Trim()) ? null : textBoxEdit3.Text.Trim();

            try
            {
                if (_receiptId == 0)   // درج جدید
                {
                    var receipt = new GoodsReceipt
                    {
                        ProductId = _productId,
                        Quantity = quantity,
                        UnitPrice = unitPrice,
                        Unit = unit,
                        WarehouseId = warehouseId,
                        SupplierId = supplierId,
                        InvoiceNumber = invoiceNumber,
                        InvoiceDate = invoiceDate,
                        ShelfLocation = shelfLocation,
                        BatchNumber = batchNumber,
                        Description = description,
                        ScannedBarcode = scannedBarcode,
                        TaxRate = 0,
                        ReceiptDate = DateTime.Today,
                        CreatedAt = DateTime.Now,
                        Status = 0,
                        UserId = Program.CurrentUserId
                    };
                    await _goodsReceiptService.AddAsync(receipt);
                    MessageBox.Show("رسید انبار با موفقیت ثبت شد.", "موفق", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else   // ویرایش
                {
                    var existing = await _goodsReceiptService.GetByIdAsync(_receiptId);
                    if (existing == null)
                    {
                        MessageBox.Show("رسید یافت نشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    // به‌روزرسانی فیلدها
                    existing.Quantity = quantity;
                    existing.UnitPrice = unitPrice;
                    existing.Unit = unit;
                    existing.WarehouseId = warehouseId;
                    existing.SupplierId = supplierId;
                    existing.InvoiceNumber = invoiceNumber;
                    existing.InvoiceDate = invoiceDate;
                    existing.ShelfLocation = shelfLocation;
                    existing.BatchNumber = batchNumber;
                    existing.Description = description;
                    existing.ScannedBarcode = scannedBarcode;
                    // موجودیت‌های دیگر (مانند TaxRate, ReceiptDate, CreatedAt, Status, UserId) معمولاً نباید تغییر کنند
                    await _goodsReceiptService.UpdateAsync(existing);
                    MessageBox.Show("رسید انبار با موفقیت ویرایش شد.", "موفق", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                Clear();
                textBoxEdit1.Focus();
                // در صورت تمایل، فرم بسته نشود (برای ویرایش پیاپی) ولی معمولاً بسته می‌شود
                // اینجا فقط فیلدها پاک می‌شوند؛ برای بستن می‌توانید this.DialogResult = DialogResult.OK قرار دهید
                if (_receiptId == 0)
                    this.DialogResult = DialogResult.OK;  // برای درج جدید، فرم بسته شود
                // برای ویرایش، فرم بسته نمی‌شود تا کاربر بتواند ویرایش دیگری انجام دهد (یا می‌توانید ببندید)
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطا در ثبت/ویرایش رسید: {ex.Message}", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void foreverButton1_Click(object sender, EventArgs e)
        {
            await CreateGoodsReceipt();
        }

        private async void Form12_Load(object sender, EventArgs e)
        {
            // بارگذاری انبارها
            var warehouses = await _warehouseService.GetAllAsync();
            comboBoxEdit2.DataSource = warehouses.ToList();
            comboBoxEdit2.DisplayMember = "Name";
            comboBoxEdit2.ValueMember = "WarehouseId";
            comboBoxEdit2.SelectedIndex = -1;

            // بارگذاری تأمین‌کنندگان
            var suppliers = await _supplierService.GetAllAsync();
            comboBoxEdit3.DataSource = suppliers.ToList();
            comboBoxEdit3.DisplayMember = "Name";
            comboBoxEdit3.ValueMember = "SupplierId";
            comboBoxEdit3.SelectedIndex = -1;

            // ========== در صورت ویرایش، اطلاعات رسید را بارگذاری کن ==========
            if (_receiptId > 0)
            {
                var receipt = await _goodsReceiptService.GetByIdAsync(_receiptId);
                if (receipt != null)
                {
                    _productId = receipt.ProductId;
                    textBoxEdit1.Text = receipt.Quantity.ToString();
                    textBoxEdit2.Text = receipt.UnitPrice.ToString();
                    textBoxEdit3.Text = receipt.ScannedBarcode;
                    textBoxEdit4.Text = receipt.Description;
                    textBoxEdit5.Text = receipt.InvoiceNumber;
                    textBoxEdit6.Text = receipt.ShelfLocation;
                    textBoxEdit7.Text = receipt.BatchNumber;
                    comboBoxEdit1.Text = receipt.Unit;
                    comboBoxEdit2.SelectedValue = receipt.WarehouseId;
                    comboBoxEdit3.SelectedValue = receipt.SupplierId;
                    dateTimePicker1.Value = receipt.InvoiceDate;
                    foreverButton1.Text = "ویرایش";
                    this.Text = "ویرایش رسید انبار";
                }
                else
                {
                    MessageBox.Show("رسید مورد نظر یافت نشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
            else
            {
                foreverButton1.Text = "ذخیره";
                this.Text = "ثبت رسید جدید";
            }
        }
    }
}