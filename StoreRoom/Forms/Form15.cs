using Application.Features.Implementation.Customer_Service;
using Application.Features.Implementation.GoodsIssue_Service;
using Application.Features.Implementation.Warehouse_Service;
using Domain.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ReaLTaiizor.Forms;
using System;
using System.Linq;
using System.Windows.Forms;

namespace StoreRoom.Forms
{
    public partial class Form15 : MaterialForm
    {
        private readonly GoodsIssueService _goodsIssueService;
        private readonly WarehouseService _warehouseService;
        private readonly CustomerService _customerService;
        private readonly UserManager<ApplicationUser> _userManager;

        private Guid _productId;
        private int _issueId = 0;

        // ========== سازنده‌ها ==========

        // درج جدید (بدون شناسه محصول)
        public Form15(GoodsIssueService goodsIssueService)
            : this(goodsIssueService,
                   Program.ServiceProvider.GetRequiredService<WarehouseService>(),
                   Program.ServiceProvider.GetRequiredService<CustomerService>(),
                   Program.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>())
        {
        }

        // درج جدید با شناسه محصول (از Form7)
        public Form15(GoodsIssueService goodsIssueService, Guid productId)
            : this(goodsIssueService,
                   Program.ServiceProvider.GetRequiredService<WarehouseService>(),
                   Program.ServiceProvider.GetRequiredService<CustomerService>(),
                   Program.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>(),
                   productId)
        {
        }

        // سازنده اصلی (با UserManager)
        public Form15(GoodsIssueService goodsIssueService,
                      WarehouseService warehouseService,
                      CustomerService customerService,
                      UserManager<ApplicationUser> userManager)
        {
            InitializeComponent();
            _goodsIssueService = goodsIssueService;
            _warehouseService = warehouseService;
            _customerService = customerService;
            _userManager = userManager;
            _productId = Guid.Empty;
            _issueId = 0;
        }

        // درج جدید با productId
        public Form15(GoodsIssueService goodsIssueService,
                      WarehouseService warehouseService,
                      CustomerService customerService,
                      UserManager<ApplicationUser> userManager,
                      Guid productId)
            : this(goodsIssueService, warehouseService, customerService, userManager)
        {
            _productId = productId;
        }

        // ویرایش (دریافت issueId)
        public Form15(GoodsIssueService goodsIssueService, int issueId)
            : this(goodsIssueService,
                   Program.ServiceProvider.GetRequiredService<WarehouseService>(),
                   Program.ServiceProvider.GetRequiredService<CustomerService>(),
                   Program.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>())
        {
            _issueId = issueId;
        }

        // ========== متدهای کمکی ==========

        private void Clear()
        {
            textBoxEdit1.Text = "";
            textBoxEdit2.Text = "";
            textBoxEdit3.Text = "";
            textBoxEdit4.Text = "";
            comboBoxEdit1.SelectedIndex = -1;
            comboBoxEdit2.SelectedIndex = -1;
            comboBoxEdit3.SelectedIndex = -1;
            comboBoxEdit4.SelectedIndex = -1;
            comboBoxEdit5.SelectedIndex = -1;
            dateTimePicker1.Value = DateTime.Today;
        }

        private string GetIssueTypeDisplayName(IssueType type)
        {
            return type switch
            {
                IssueType.Sale => "فروش",
                IssueType.Transfer => "انتقال",
                IssueType.Donation => "اهدا",
                IssueType.Waste => "ضایعات",
                _ => type.ToString()
            };
        }

        // ========== عملیات اصلی ثبت/ویرایش ==========

        private async Task GoodsIssue()
        {
            try
            {
                // 1. اگر در حالت ویرایش هستیم، productId را از دیتابیس می‌خوانیم
                if (_issueId > 0 && _productId == Guid.Empty)
                {
                    var issue = await _goodsIssueService.GetByIdAsync(_issueId);
                    if (issue != null)
                        _productId = issue.ProductId;
                }

                // 2. اعتبارسنجی شناسه محصول
                if (_productId == Guid.Empty)
                {
                    MessageBox.Show("شناسه محصول معتبر نیست. لطفاً یک محصول را انتخاب کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 3. اعتبارسنجی تعداد خروج
                // 3. اعتبارسنجی تعداد خروج
                if (!int.TryParse(textBoxEdit1.Text.Trim(), out int quantity) || quantity <= 0)
                {
                    MessageBox.Show("تعداد خروجی باید یک عدد صحیح و بزرگتر از صفر باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 4. اعتبارسنجی قیمت فروش (اختیاری)
                decimal? unitSellingPrice = null;
                if (!string.IsNullOrWhiteSpace(textBoxEdit2.Text.Trim()))
                {
                    string rawPrice = textBoxEdit2.Text.Replace(",", "");
                    if (!decimal.TryParse(rawPrice, out decimal price) || price < 0)
                    {
                        MessageBox.Show("قیمت فروش واحد باید یک عدد معتبر (غیرمنفی) باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    unitSellingPrice = price;
                }

                // 5. واحد اندازه‌گیری
                string unit = comboBoxEdit1.Text.Trim();
                if (string.IsNullOrWhiteSpace(unit))
                {
                    MessageBox.Show("واحد اندازه‌گیری را انتخاب کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 6. نوع حواله
                if (comboBoxEdit3.SelectedValue == null)
                {
                    MessageBox.Show("لطفاً نوع حواله را انتخاب کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                IssueType issueType = (IssueType)comboBoxEdit3.SelectedValue;

                // 7. وضعیت (فقط در انتظار تأیید)
                if (comboBoxEdit2.SelectedValue == null)
                {
                    MessageBox.Show("لطفاً وضعیت را انتخاب کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 8. انبار مبدأ
                if (comboBoxEdit5.SelectedValue == null)
                {
                    MessageBox.Show("لطفاً انبار مبدأ را انتخاب کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                int warehouseId = (int)comboBoxEdit5.SelectedValue;

                // 9. مشتری (فقط در صورت فروش)
                int? customerId = null;
                if (issueType == IssueType.Sale)
                {
                    if (comboBoxEdit4.SelectedValue == null)
                    {
                        MessageBox.Show("برای حواله فروش، لطفاً مشتری را انتخاب کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    customerId = (int)comboBoxEdit4.SelectedValue;
                }

                string invoiceNumber = textBoxEdit4.Text.Trim();
                string description = textBoxEdit3.Text.Trim();

                // 10. اگر کاربر لاگین نیست، خطا بده
                if (string.IsNullOrEmpty(Program.CurrentUserId))
                {
                    MessageBox.Show("لطفاً ابتدا وارد سیستم شوید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 11. دریافت نام کامل کاربر
                var user = await _userManager.FindByIdAsync(Program.CurrentUserId);
                string userFullName = user?.FullName ?? "نامشخص";

                // 12. عملیات اصلی
                if (_issueId == 0)   // درج جدید
                {
                    var goodsIssue = new GoodsIssue
                    {
                        ProductId = _productId,
                        Quantity = quantity,
                        Unit = unit,
                        UnitSellingPrice = unitSellingPrice,
                        Type = issueType,
                        CustomerId = customerId,
                        InvoiceNumber = invoiceNumber,
                        WarehouseId = warehouseId,
                        Description = description,
                        CreatedAt = DateTime.Now,
                        Status = 0,
                        UserId = Program.CurrentUserId,
                        UserFullName = userFullName
                    };
                    await _goodsIssueService.AddAsync(goodsIssue);
                    MessageBox.Show("حواله خروج کالا با موفقیت ثبت شد.", "موفق", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else   // ویرایش
                {
                    var existing = await _goodsIssueService.GetByIdAsync(_issueId);
                    if (existing == null)
                    {
                        MessageBox.Show("حواله یافت نشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    existing.Quantity = quantity;
                    existing.Unit = unit;
                    existing.UnitSellingPrice = unitSellingPrice;
                    existing.Type = issueType;
                    existing.CustomerId = customerId;
                    existing.InvoiceNumber = invoiceNumber;
                    existing.WarehouseId = warehouseId;
                    existing.Description = description;

                    await _goodsIssueService.UpdateAsync(existing);
                    MessageBox.Show("حواله خروج کالا با موفقیت ویرایش شد.", "موفق", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                this.DialogResult = DialogResult.OK;
                Clear();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطا در ثبت/ویرایش حواله خروج: {ex.Message}", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ========== رویدادها ==========

        private async void foreverButton1_Click(object sender, EventArgs e)
        {
            await GoodsIssue();
        }

        private async void Test_Load(object sender, EventArgs e)
        {
            // بارگذاری انبارها
            var warehouses = await _warehouseService.GetAllAsync();
            comboBoxEdit5.DataSource = warehouses.ToList();
            comboBoxEdit5.DisplayMember = "Name";
            comboBoxEdit5.ValueMember = "WarehouseId";
            comboBoxEdit5.SelectedIndex = -1;

            // بارگذاری مشتریان
            var customers = await _customerService.GetAllAsync();
            comboBoxEdit4.DataSource = customers.ToList();
            comboBoxEdit4.DisplayMember = "Name";
            comboBoxEdit4.ValueMember = "CustomerId";
            comboBoxEdit4.SelectedIndex = -1;

            // مقداردهی کامبوباکس نوع حواله
            var issueTypes = Enum.GetValues(typeof(IssueType))
                                 .Cast<IssueType>()
                                 .Select(e => new { Value = e, Name = GetIssueTypeDisplayName(e) })
                                 .ToList();
            comboBoxEdit3.DataSource = issueTypes;
            comboBoxEdit3.DisplayMember = "Name";
            comboBoxEdit3.ValueMember = "Value";
            comboBoxEdit3.SelectedIndex = -1;

            // مقداردهی کامبوباکس وضعیت (فقط در انتظار تأیید)
            var statuses = new[]
            {
                new { Value = 0, Name = "در انتظار تأیید" }
            };
            comboBoxEdit2.DataSource = statuses;
            comboBoxEdit2.DisplayMember = "Name";
            comboBoxEdit2.ValueMember = "Value";
            comboBoxEdit2.SelectedIndex = -1;

            // در صورت ویرایش، اطلاعات حواله را بارگذاری کن
            if (_issueId > 0)
            {
                var issue = await _goodsIssueService.GetByIdAsync(_issueId);
                if (issue != null)
                {
                    _productId = issue.ProductId;
                    textBoxEdit1.Text = issue.Quantity.ToString();
                    textBoxEdit2.Text = issue.UnitSellingPrice?.ToString() ?? "";
                    textBoxEdit3.Text = issue.Description;
                    textBoxEdit4.Text = issue.InvoiceNumber;
                    comboBoxEdit1.Text = issue.Unit;
                    comboBoxEdit3.SelectedValue = issue.Type;
                    comboBoxEdit5.SelectedValue = issue.WarehouseId;
                    if (issue.CustomerId.HasValue)
                        comboBoxEdit4.SelectedValue = issue.CustomerId.Value;

                    dateTimePicker1.Value = DateTime.Today;
                    dateTimePicker1.Enabled = false;

                    foreverButton1.Text = "ویرایش";
                }
                else
                {
                    MessageBox.Show("حواله مورد نظر یافت نشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
            else
            {
                dateTimePicker1.Enabled = false;
                dateTimePicker1.Value = DateTime.Today;
                foreverButton1.Text = "ذخیره";
            }
        }

        // ========== فرمت‌کردن قیمت ==========

        private void textBoxEdit2_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxEdit2.Text))
                return;

            string raw = textBoxEdit2.Text.Replace(",", "");
            if (decimal.TryParse(raw, out decimal price))
            {
                textBoxEdit2.Text = price.ToString("#,0");
            }
        }
    }
}