namespace StoreRoom.Forms
{
    partial class Form2
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
            label6 = new Label();
            textBoxEdit5 = new ReaLTaiizor.Controls.TextBoxEdit();
            label5 = new Label();
            comboBoxEdit1 = new ReaLTaiizor.Controls.ComboBoxEdit();
            label4 = new Label();
            textBoxEdit4 = new ReaLTaiizor.Controls.TextBoxEdit();
            label3 = new Label();
            textBoxEdit3 = new ReaLTaiizor.Controls.TextBoxEdit();
            label2 = new Label();
            textBoxEdit2 = new ReaLTaiizor.Controls.TextBoxEdit();
            label1 = new Label();
            textBoxEdit1 = new ReaLTaiizor.Controls.TextBoxEdit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(foreverButton1);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(textBoxEdit5);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(comboBoxEdit1);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(textBoxEdit4);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(textBoxEdit3);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(textBoxEdit2);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(textBoxEdit1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 64);
            panel1.Name = "panel1";
            panel1.Size = new Size(858, 464);
            panel1.TabIndex = 1;
            // 
            // foreverButton1
            // 
            foreverButton1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            foreverButton1.BackColor = Color.Transparent;
            foreverButton1.BaseColor = Color.FromArgb(35, 168, 109);
            foreverButton1.Font = new Font("Tahoma", 12F, FontStyle.Bold);
            foreverButton1.Location = new Point(518, 341);
            foreverButton1.Name = "foreverButton1";
            foreverButton1.Rounded = false;
            foreverButton1.Size = new Size(233, 70);
            foreverButton1.TabIndex = 12;
            foreverButton1.Text = "ذخیره";
            foreverButton1.TextColor = Color.FromArgb(243, 243, 243);
            foreverButton1.Click += foreverButton1_Click;
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label6.AutoSize = true;
            label6.Location = new Point(287, 220);
            label6.Name = "label6";
            label6.Size = new Size(108, 19);
            label6.TabIndex = 11;
            label6.Text = "تعداد قفسه :";
            // 
            // textBoxEdit5
            // 
            textBoxEdit5.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBoxEdit5.BackColor = Color.Transparent;
            textBoxEdit5.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            textBoxEdit5.ForeColor = Color.White;
            textBoxEdit5.Image = null;
            textBoxEdit5.Location = new Point(21, 220);
            textBoxEdit5.MaxLength = 32767;
            textBoxEdit5.Multiline = false;
            textBoxEdit5.Name = "textBoxEdit5";
            textBoxEdit5.ReadOnly = false;
            textBoxEdit5.Size = new Size(261, 43);
            textBoxEdit5.TabIndex = 10;
            textBoxEdit5.TextAlignment = HorizontalAlignment.Left;
            textBoxEdit5.UseSystemPasswordChar = false;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label5.AutoSize = true;
            label5.Location = new Point(757, 252);
            label5.Name = "label5";
            label5.Size = new Size(74, 19);
            label5.TabIndex = 9;
            label5.Text = "وضعیت :";
            // 
            // comboBoxEdit1
            // 
            comboBoxEdit1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            comboBoxEdit1.BackColor = Color.FromArgb(246, 246, 246);
            comboBoxEdit1.DrawMode = DrawMode.OwnerDrawFixed;
            comboBoxEdit1.DropDownHeight = 100;
            comboBoxEdit1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxEdit1.Font = new Font("Tahoma", 12F, FontStyle.Bold);
            comboBoxEdit1.ForeColor = Color.Black;
            comboBoxEdit1.FormattingEnabled = true;
            comboBoxEdit1.HoverSelectionColor = Color.FromArgb(241, 241, 241);
            comboBoxEdit1.IntegralHeight = false;
            comboBoxEdit1.ItemHeight = 20;
            comboBoxEdit1.Items.AddRange(new object[] { "تکمیل", "نیمه", "خالی" });
            comboBoxEdit1.Location = new Point(490, 249);
            comboBoxEdit1.Name = "comboBoxEdit1";
            comboBoxEdit1.Size = new Size(261, 26);
            comboBoxEdit1.StartIndex = 0;
            comboBoxEdit1.TabIndex = 8;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label4.AutoSize = true;
            label4.Location = new Point(287, 133);
            label4.Name = "label4";
            label4.Size = new Size(72, 19);
            label4.TabIndex = 7;
            label4.Text = "حد اقل :";
            // 
            // textBoxEdit4
            // 
            textBoxEdit4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBoxEdit4.BackColor = Color.Transparent;
            textBoxEdit4.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            textBoxEdit4.ForeColor = Color.White;
            textBoxEdit4.Image = null;
            textBoxEdit4.Location = new Point(21, 133);
            textBoxEdit4.MaxLength = 32767;
            textBoxEdit4.Multiline = false;
            textBoxEdit4.Name = "textBoxEdit4";
            textBoxEdit4.ReadOnly = false;
            textBoxEdit4.Size = new Size(261, 43);
            textBoxEdit4.TabIndex = 6;
            textBoxEdit4.TextAlignment = HorizontalAlignment.Left;
            textBoxEdit4.UseSystemPasswordChar = false;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Location = new Point(287, 42);
            label3.Name = "label3";
            label3.Size = new Size(73, 19);
            label3.TabIndex = 5;
            label3.Text = "حد اکثر :";
            // 
            // textBoxEdit3
            // 
            textBoxEdit3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBoxEdit3.BackColor = Color.Transparent;
            textBoxEdit3.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            textBoxEdit3.ForeColor = Color.White;
            textBoxEdit3.Image = null;
            textBoxEdit3.Location = new Point(21, 42);
            textBoxEdit3.MaxLength = 32767;
            textBoxEdit3.Multiline = false;
            textBoxEdit3.Name = "textBoxEdit3";
            textBoxEdit3.ReadOnly = false;
            textBoxEdit3.Size = new Size(261, 43);
            textBoxEdit3.TabIndex = 4;
            textBoxEdit3.TextAlignment = HorizontalAlignment.Left;
            textBoxEdit3.UseSystemPasswordChar = false;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Location = new Point(757, 145);
            label2.Name = "label2";
            label2.Size = new Size(62, 19);
            label2.TabIndex = 3;
            label2.Text = "آدرس :";
            // 
            // textBoxEdit2
            // 
            textBoxEdit2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBoxEdit2.BackColor = Color.Transparent;
            textBoxEdit2.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            textBoxEdit2.ForeColor = Color.White;
            textBoxEdit2.Image = null;
            textBoxEdit2.Location = new Point(491, 145);
            textBoxEdit2.MaxLength = 32767;
            textBoxEdit2.Multiline = false;
            textBoxEdit2.Name = "textBoxEdit2";
            textBoxEdit2.ReadOnly = false;
            textBoxEdit2.Size = new Size(261, 43);
            textBoxEdit2.TabIndex = 2;
            textBoxEdit2.TextAlignment = HorizontalAlignment.Left;
            textBoxEdit2.UseSystemPasswordChar = false;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(757, 54);
            label1.Name = "label1";
            label1.Size = new Size(41, 19);
            label1.TabIndex = 1;
            label1.Text = "نام :";
            // 
            // textBoxEdit1
            // 
            textBoxEdit1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBoxEdit1.BackColor = Color.Transparent;
            textBoxEdit1.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            textBoxEdit1.ForeColor = Color.White;
            textBoxEdit1.Image = null;
            textBoxEdit1.Location = new Point(491, 54);
            textBoxEdit1.MaxLength = 32767;
            textBoxEdit1.Multiline = false;
            textBoxEdit1.Name = "textBoxEdit1";
            textBoxEdit1.ReadOnly = false;
            textBoxEdit1.Size = new Size(261, 43);
            textBoxEdit1.TabIndex = 0;
            textBoxEdit1.TextAlignment = HorizontalAlignment.Left;
            textBoxEdit1.UseSystemPasswordChar = false;
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(10F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(865, 532);
            Controls.Add(panel1);
            Font = new Font("Tahoma", 12F, FontStyle.Bold);
            Margin = new Padding(4);
            Name = "Form2";
            Padding = new Padding(3, 64, 4, 4);
            RightToLeft = RightToLeft.Yes;
            Text = "ثبت کالا";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private ReaLTaiizor.Controls.TextBoxEdit textBoxEdit1;
        private Label label3;
        private ReaLTaiizor.Controls.TextBoxEdit textBoxEdit3;
        private Label label2;
        private ReaLTaiizor.Controls.TextBoxEdit textBoxEdit2;
        private Label label1;
        private Label label4;
        private ReaLTaiizor.Controls.TextBoxEdit textBoxEdit4;
        private Label label6;
        private ReaLTaiizor.Controls.TextBoxEdit textBoxEdit5;
        private Label label5;
        private ReaLTaiizor.Controls.ComboBoxEdit comboBoxEdit1;
        private ReaLTaiizor.Controls.ForeverButton foreverButton1;
    }
}