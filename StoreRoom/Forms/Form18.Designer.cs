namespace StoreRoom.Forms
{
    partial class Form18
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            txtUsername = new ReaLTaiizor.Controls.TextBoxEdit();
            label6 = new Label();
            comboBoxRoles = new ReaLTaiizor.Controls.ComboBoxEdit();
            foreverButton1 = new ReaLTaiizor.Controls.ForeverButton();
            foreverButton2 = new ReaLTaiizor.Controls.ForeverButton();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(279, 77);
            label1.Name = "label1";
            label1.Size = new Size(54, 19);
            label1.TabIndex = 5;
            label1.Text = "کاربر :";
            // 
            // txtUsername
            // 
            txtUsername.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtUsername.BackColor = Color.Transparent;
            txtUsername.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtUsername.ForeColor = Color.White;
            txtUsername.Image = null;
            txtUsername.Location = new Point(13, 77);
            txtUsername.MaxLength = 32767;
            txtUsername.Multiline = false;
            txtUsername.Name = "txtUsername";
            txtUsername.ReadOnly = false;
            txtUsername.Size = new Size(261, 43);
            txtUsername.TabIndex = 4;
            txtUsername.TextAlignment = HorizontalAlignment.Left;
            txtUsername.UseSystemPasswordChar = false;
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label6.AutoSize = true;
            label6.Location = new Point(281, 161);
            label6.Name = "label6";
            label6.Size = new Size(86, 19);
            label6.TabIndex = 17;
            label6.Text = "دسترسی";
            // 
            // comboBoxRoles
            // 
            comboBoxRoles.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            comboBoxRoles.BackColor = Color.FromArgb(246, 246, 246);
            comboBoxRoles.DrawMode = DrawMode.OwnerDrawFixed;
            comboBoxRoles.DropDownHeight = 100;
            comboBoxRoles.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxRoles.Font = new Font("Tahoma", 12F, FontStyle.Bold);
            comboBoxRoles.ForeColor = Color.Black;
            comboBoxRoles.FormattingEnabled = true;
            comboBoxRoles.HoverSelectionColor = Color.FromArgb(241, 241, 241);
            comboBoxRoles.IntegralHeight = false;
            comboBoxRoles.ItemHeight = 20;
            comboBoxRoles.Location = new Point(13, 161);
            comboBoxRoles.Name = "comboBoxRoles";
            comboBoxRoles.Size = new Size(261, 26);
            comboBoxRoles.StartIndex = 0;
            comboBoxRoles.TabIndex = 16;
            // 
            // foreverButton1
            // 
            foreverButton1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            foreverButton1.BackColor = Color.Transparent;
            foreverButton1.BaseColor = Color.FromArgb(35, 168, 109);
            foreverButton1.Font = new Font("Tahoma", 12F, FontStyle.Bold);
            foreverButton1.Location = new Point(193, 242);
            foreverButton1.Name = "foreverButton1";
            foreverButton1.Rounded = false;
            foreverButton1.Size = new Size(164, 62);
            foreverButton1.TabIndex = 18;
            foreverButton1.Text = "ذخیره";
            foreverButton1.TextColor = Color.FromArgb(243, 243, 243);
            foreverButton1.Click += foreverButton1_Click;
            // 
            // foreverButton2
            // 
            foreverButton2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            foreverButton2.BackColor = Color.Transparent;
            foreverButton2.BaseColor = Color.Red;
            foreverButton2.Font = new Font("Tahoma", 12F, FontStyle.Bold);
            foreverButton2.Location = new Point(13, 242);
            foreverButton2.Name = "foreverButton2";
            foreverButton2.Rounded = false;
            foreverButton2.Size = new Size(164, 62);
            foreverButton2.TabIndex = 19;
            foreverButton2.Text = "حذف";
            foreverButton2.TextColor = Color.FromArgb(243, 243, 243);
            foreverButton2.Click += foreverButton2_Click;
            // 
            // Form18
            // 
            AutoScaleDimensions = new SizeF(10F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(364, 311);
            Controls.Add(foreverButton2);
            Controls.Add(foreverButton1);
            Controls.Add(label6);
            Controls.Add(comboBoxRoles);
            Controls.Add(label1);
            Controls.Add(txtUsername);
            Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Margin = new Padding(4);
            Name = "Form18";
            Padding = new Padding(3, 64, 4, 4);
            RightToLeft = RightToLeft.Yes;
            Text = "تعین دسترسی";
            Load += Form18_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private ReaLTaiizor.Controls.TextBoxEdit txtUsername;
        private Label label6;
        private ReaLTaiizor.Controls.ComboBoxEdit comboBoxRoles;
        private ReaLTaiizor.Controls.ForeverButton foreverButton1;
        private ReaLTaiizor.Controls.ForeverButton foreverButton2;
    }
}