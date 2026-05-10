namespace StoreRoom.Forms
{
    partial class Form4
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
            label1 = new Label();
            textBoxEdit1 = new ReaLTaiizor.Controls.TextBoxEdit();
            textBoxEdit2 = new ReaLTaiizor.Controls.TextBoxEdit();
            label2 = new Label();
            label3 = new Label();
            textBoxEdit3 = new ReaLTaiizor.Controls.TextBoxEdit();
            label4 = new Label();
            textBoxEdit4 = new ReaLTaiizor.Controls.TextBoxEdit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(label4);
            panel1.Controls.Add(textBoxEdit4);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(textBoxEdit3);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(textBoxEdit2);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(textBoxEdit1);
            panel1.Controls.Add(foreverButton1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 64);
            panel1.Name = "panel1";
            panel1.Size = new Size(851, 286);
            panel1.TabIndex = 0;
            // 
            // foreverButton1
            // 
            foreverButton1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            foreverButton1.BackColor = Color.Transparent;
            foreverButton1.BaseColor = Color.FromArgb(35, 168, 109);
            foreverButton1.Font = new Font("Tahoma", 12F, FontStyle.Bold);
            foreverButton1.Location = new Point(546, 192);
            foreverButton1.Name = "foreverButton1";
            foreverButton1.Rounded = false;
            foreverButton1.Size = new Size(233, 70);
            foreverButton1.TabIndex = 14;
            foreverButton1.Text = "ذخیره";
            foreverButton1.TextColor = Color.FromArgb(243, 243, 243);
            foreverButton1.Click += foreverButton1_Click;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(794, 12);
            label1.Name = "label1";
            label1.Size = new Size(41, 19);
            label1.TabIndex = 16;
            label1.Text = "نام :";
            // 
            // textBoxEdit1
            // 
            textBoxEdit1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBoxEdit1.BackColor = Color.Transparent;
            textBoxEdit1.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            textBoxEdit1.ForeColor = Color.White;
            textBoxEdit1.Image = null;
            textBoxEdit1.Location = new Point(527, 12);
            textBoxEdit1.MaxLength = 32767;
            textBoxEdit1.Multiline = false;
            textBoxEdit1.Name = "textBoxEdit1";
            textBoxEdit1.ReadOnly = false;
            textBoxEdit1.Size = new Size(261, 43);
            textBoxEdit1.TabIndex = 15;
            textBoxEdit1.TextAlignment = HorizontalAlignment.Left;
            textBoxEdit1.UseSystemPasswordChar = false;
            // 
            // textBoxEdit2
            // 
            textBoxEdit2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBoxEdit2.BackColor = Color.Transparent;
            textBoxEdit2.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            textBoxEdit2.ForeColor = Color.White;
            textBoxEdit2.Image = null;
            textBoxEdit2.Location = new Point(528, 113);
            textBoxEdit2.MaxLength = 32767;
            textBoxEdit2.Multiline = false;
            textBoxEdit2.Name = "textBoxEdit2";
            textBoxEdit2.ReadOnly = false;
            textBoxEdit2.Size = new Size(261, 43);
            textBoxEdit2.TabIndex = 17;
            textBoxEdit2.TextAlignment = HorizontalAlignment.Left;
            textBoxEdit2.UseSystemPasswordChar = false;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Location = new Point(794, 113);
            label2.Name = "label2";
            label2.Size = new Size(54, 19);
            label2.TabIndex = 18;
            label2.Text = "تلفن :";
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Location = new Point(277, 28);
            label3.Name = "label3";
            label3.Size = new Size(62, 19);
            label3.TabIndex = 20;
            label3.Text = "آدرس :";
            // 
            // textBoxEdit3
            // 
            textBoxEdit3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBoxEdit3.BackColor = Color.Transparent;
            textBoxEdit3.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            textBoxEdit3.ForeColor = Color.White;
            textBoxEdit3.Image = null;
            textBoxEdit3.Location = new Point(11, 28);
            textBoxEdit3.MaxLength = 32767;
            textBoxEdit3.Multiline = false;
            textBoxEdit3.Name = "textBoxEdit3";
            textBoxEdit3.ReadOnly = false;
            textBoxEdit3.Size = new Size(261, 43);
            textBoxEdit3.TabIndex = 19;
            textBoxEdit3.TextAlignment = HorizontalAlignment.Left;
            textBoxEdit3.UseSystemPasswordChar = false;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label4.AutoSize = true;
            label4.Location = new Point(277, 113);
            label4.Name = "label4";
            label4.Size = new Size(113, 19);
            label4.TabIndex = 22;
            label4.Text = "نام فروشگاه :";
            // 
            // textBoxEdit4
            // 
            textBoxEdit4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBoxEdit4.BackColor = Color.Transparent;
            textBoxEdit4.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            textBoxEdit4.ForeColor = Color.White;
            textBoxEdit4.Image = null;
            textBoxEdit4.Location = new Point(11, 113);
            textBoxEdit4.MaxLength = 32767;
            textBoxEdit4.Multiline = false;
            textBoxEdit4.Name = "textBoxEdit4";
            textBoxEdit4.ReadOnly = false;
            textBoxEdit4.Size = new Size(261, 43);
            textBoxEdit4.TabIndex = 21;
            textBoxEdit4.TextAlignment = HorizontalAlignment.Left;
            textBoxEdit4.UseSystemPasswordChar = false;
            // 
            // Form4
            // 
            AutoScaleDimensions = new SizeF(10F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(858, 354);
            Controls.Add(panel1);
            Font = new Font("Tahoma", 12F, FontStyle.Bold);
            Margin = new Padding(4, 4, 4, 4);
            Name = "Form4";
            Padding = new Padding(3, 64, 4, 4);
            RightToLeft = RightToLeft.Yes;
            Text = "ثبت مشتری";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private ReaLTaiizor.Controls.ForeverButton foreverButton1;
        private Label label1;
        private ReaLTaiizor.Controls.TextBoxEdit textBoxEdit1;
        private Label label4;
        private ReaLTaiizor.Controls.TextBoxEdit textBoxEdit4;
        private Label label3;
        private ReaLTaiizor.Controls.TextBoxEdit textBoxEdit3;
        private Label label2;
        private ReaLTaiizor.Controls.TextBoxEdit textBoxEdit2;
    }
}