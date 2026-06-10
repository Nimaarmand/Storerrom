namespace StoreRoom.Forms
{
    partial class Form20
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
            panel1 = new Panel();
            foreverButton1 = new ReaLTaiizor.Controls.ForeverButton();
            label3 = new Label();
            txtPhone = new ReaLTaiizor.Controls.TextBoxEdit();
            panel2 = new Panel();
            foreverButton2 = new ReaLTaiizor.Controls.ForeverButton();
            chkShow = new ReaLTaiizor.Controls.MaterialCheckBox();
            label2 = new Label();
            txtConfirmPass = new ReaLTaiizor.Controls.TextBoxEdit();
            label1 = new Label();
            txtNewPass = new ReaLTaiizor.Controls.TextBoxEdit();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(foreverButton1);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(txtPhone);
            panel1.Location = new Point(7, 68);
            panel1.Margin = new Padding(4);
            panel1.Name = "panel1";
            panel1.Size = new Size(472, 455);
            panel1.TabIndex = 0;
            // 
            // foreverButton1
            // 
            foreverButton1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            foreverButton1.BackColor = Color.Transparent;
            foreverButton1.BaseColor = Color.FromArgb(35, 168, 109);
            foreverButton1.Font = new Font("Tahoma", 12F, FontStyle.Bold);
            foreverButton1.Location = new Point(103, 117);
            foreverButton1.Margin = new Padding(6, 5, 6, 5);
            foreverButton1.Name = "foreverButton1";
            foreverButton1.Rounded = false;
            foreverButton1.Size = new Size(184, 70);
            foreverButton1.TabIndex = 26;
            foreverButton1.Text = "تایید شماره";
            foreverButton1.TextColor = Color.FromArgb(243, 243, 243);
            foreverButton1.Click += foreverButton1_Click;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Location = new Point(370, 20);
            label3.Margin = new Padding(6, 0, 6, 0);
            label3.Name = "label3";
            label3.Size = new Size(96, 19);
            label3.TabIndex = 25;
            label3.Text = "نام کاربری :";
            // 
            // txtPhone
            // 
            txtPhone.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtPhone.BackColor = Color.Transparent;
            txtPhone.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtPhone.ForeColor = Color.White;
            txtPhone.Image = null;
            txtPhone.Location = new Point(22, 5);
            txtPhone.Margin = new Padding(6, 5, 6, 5);
            txtPhone.MaxLength = 32767;
            txtPhone.Multiline = false;
            txtPhone.Name = "txtPhone";
            txtPhone.ReadOnly = false;
            txtPhone.Size = new Size(336, 43);
            txtPhone.TabIndex = 24;
            txtPhone.TextAlignment = HorizontalAlignment.Left;
            txtPhone.UseSystemPasswordChar = false;
            // 
            // panel2
            // 
            panel2.Controls.Add(foreverButton2);
            panel2.Controls.Add(chkShow);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(txtConfirmPass);
            panel2.Controls.Add(label1);
            panel2.Controls.Add(txtNewPass);
            panel2.Location = new Point(7, 68);
            panel2.Margin = new Padding(4);
            panel2.Name = "panel2";
            panel2.Size = new Size(472, 455);
            panel2.TabIndex = 1;
            // 
            // foreverButton2
            // 
            foreverButton2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            foreverButton2.BackColor = Color.Transparent;
            foreverButton2.BaseColor = Color.FromArgb(35, 168, 109);
            foreverButton2.Font = new Font("Tahoma", 12F, FontStyle.Bold);
            foreverButton2.Location = new Point(130, 200);
            foreverButton2.Margin = new Padding(6, 5, 6, 5);
            foreverButton2.Name = "foreverButton2";
            foreverButton2.Rounded = false;
            foreverButton2.Size = new Size(184, 70);
            foreverButton2.TabIndex = 41;
            foreverButton2.Text = "تایید شماره";
            foreverButton2.TextColor = Color.FromArgb(243, 243, 243);
            foreverButton2.Click += foreverButton2_Click;
            // 
            // chkShow
            // 
            chkShow.AutoSize = true;
            chkShow.Depth = 0;
            chkShow.Location = new Point(363, 150);
            chkShow.Margin = new Padding(0);
            chkShow.MouseLocation = new Point(-1, -1);
            chkShow.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            chkShow.Name = "chkShow";
            chkShow.ReadOnly = false;
            chkShow.RightToLeft = RightToLeft.Yes;
            chkShow.Ripple = true;
            chkShow.Size = new Size(79, 37);
            chkShow.TabIndex = 40;
            chkShow.Text = "نمایش رمز";
            chkShow.UseAccentColor = false;
            chkShow.UseVisualStyleBackColor = true;
            chkShow.CheckedChanged += chkShow_CheckedChanged_1;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Location = new Point(363, 85);
            label2.Margin = new Padding(6, 0, 6, 0);
            label2.Name = "label2";
            label2.Size = new Size(96, 19);
            label2.TabIndex = 29;
            label2.Text = "نام کاربری :";
            // 
            // txtConfirmPass
            // 
            txtConfirmPass.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtConfirmPass.BackColor = Color.Transparent;
            txtConfirmPass.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtConfirmPass.ForeColor = Color.White;
            txtConfirmPass.Image = null;
            txtConfirmPass.Location = new Point(22, 85);
            txtConfirmPass.Margin = new Padding(6, 5, 6, 5);
            txtConfirmPass.MaxLength = 32767;
            txtConfirmPass.Multiline = false;
            txtConfirmPass.Name = "txtConfirmPass";
            txtConfirmPass.ReadOnly = false;
            txtConfirmPass.Size = new Size(338, 43);
            txtConfirmPass.TabIndex = 28;
            txtConfirmPass.TextAlignment = HorizontalAlignment.Left;
            txtConfirmPass.UseSystemPasswordChar = false;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(363, 5);
            label1.Margin = new Padding(6, 0, 6, 0);
            label1.Name = "label1";
            label1.Size = new Size(96, 19);
            label1.TabIndex = 27;
            label1.Text = "نام کاربری :";
            // 
            // txtNewPass
            // 
            txtNewPass.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtNewPass.BackColor = Color.Transparent;
            txtNewPass.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtNewPass.ForeColor = Color.White;
            txtNewPass.Image = null;
            txtNewPass.Location = new Point(22, 5);
            txtNewPass.Margin = new Padding(6, 5, 6, 5);
            txtNewPass.MaxLength = 32767;
            txtNewPass.Multiline = false;
            txtNewPass.Name = "txtNewPass";
            txtNewPass.ReadOnly = false;
            txtNewPass.Size = new Size(338, 43);
            txtNewPass.TabIndex = 26;
            txtNewPass.TextAlignment = HorizontalAlignment.Left;
            txtNewPass.UseSystemPasswordChar = false;
            // 
            // Form20
            // 
            AutoScaleDimensions = new SizeF(10F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(487, 525);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Margin = new Padding(4);
            Name = "Form20";
            Padding = new Padding(3, 64, 4, 4);
            RightToLeft = RightToLeft.Yes;
            Text = "Form20";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private ReaLTaiizor.Controls.TextBoxEdit txtPhone;
        private Label label3;
        private ReaLTaiizor.Controls.ForeverButton foreverButton1;
        private Label label2;
        private ReaLTaiizor.Controls.TextBoxEdit txtConfirmPass;
        private Label label1;
        private ReaLTaiizor.Controls.TextBoxEdit txtNewPass;
        private ReaLTaiizor.Controls.MaterialCheckBox chkShow;
        private ReaLTaiizor.Controls.ForeverButton foreverButton2;
    }
}