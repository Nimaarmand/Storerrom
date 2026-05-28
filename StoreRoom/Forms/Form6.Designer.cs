namespace StoreRoom.Forms
{
    partial class Form6
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
            panel1.Controls.Add(label2);
            panel1.Controls.Add(textBoxEdit2);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(textBoxEdit1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 64);
            panel1.Name = "panel1";
            panel1.Size = new Size(826, 307);
            panel1.TabIndex = 0;
            // 
            // foreverButton1
            // 
            foreverButton1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            foreverButton1.BackColor = Color.Transparent;
            foreverButton1.BaseColor = Color.FromArgb(35, 168, 109);
            foreverButton1.Font = new Font("Tahoma", 12F, FontStyle.Bold);
            foreverButton1.Location = new Point(501, 146);
            foreverButton1.Name = "foreverButton1";
            foreverButton1.Rounded = false;
            foreverButton1.Size = new Size(233, 70);
            foreverButton1.TabIndex = 13;
            foreverButton1.Text = "ذخیره";
            foreverButton1.TextColor = Color.FromArgb(243, 243, 243);
            foreverButton1.Click += foreverButton1_Click;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Location = new Point(302, 31);
            label2.Name = "label2";
            label2.Size = new Size(87, 19);
            label2.TabIndex = 5;
            label2.Text = "توضیحات :";
            // 
            // textBoxEdit2
            // 
            textBoxEdit2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBoxEdit2.BackColor = Color.Transparent;
            textBoxEdit2.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            textBoxEdit2.ForeColor = Color.White;
            textBoxEdit2.Image = null;
            textBoxEdit2.Location = new Point(36, 31);
            textBoxEdit2.MaxLength = 32767;
            textBoxEdit2.Multiline = false;
            textBoxEdit2.Name = "textBoxEdit2";
            textBoxEdit2.ReadOnly = false;
            textBoxEdit2.Size = new Size(261, 43);
            textBoxEdit2.TabIndex = 4;
            textBoxEdit2.TextAlignment = HorizontalAlignment.Left;
            textBoxEdit2.UseSystemPasswordChar = false;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(740, 31);
            label1.Name = "label1";
            label1.Size = new Size(41, 19);
            label1.TabIndex = 3;
            label1.Text = "نام :";
            // 
            // textBoxEdit1
            // 
            textBoxEdit1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBoxEdit1.BackColor = Color.Transparent;
            textBoxEdit1.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            textBoxEdit1.ForeColor = Color.White;
            textBoxEdit1.Image = null;
            textBoxEdit1.Location = new Point(473, 31);
            textBoxEdit1.MaxLength = 32767;
            textBoxEdit1.Multiline = false;
            textBoxEdit1.Name = "textBoxEdit1";
            textBoxEdit1.ReadOnly = false;
            textBoxEdit1.Size = new Size(261, 43);
            textBoxEdit1.TabIndex = 2;
            textBoxEdit1.TextAlignment = HorizontalAlignment.Left;
            textBoxEdit1.UseSystemPasswordChar = false;
            // 
            // Form6
            // 
            AutoScaleDimensions = new SizeF(10F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(833, 375);
            Controls.Add(panel1);
            Font = new Font("Tahoma", 12F, FontStyle.Bold);
            Margin = new Padding(4);
            Name = "Form6";
            Padding = new Padding(3, 64, 4, 4);
            RightToLeft = RightToLeft.Yes;
            Text = "ثبت دسته بندی ";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label label2;
        private Label label1;
        public ReaLTaiizor.Controls.TextBoxEdit textBoxEdit2;
        public ReaLTaiizor.Controls.TextBoxEdit textBoxEdit1;
        public ReaLTaiizor.Controls.ForeverButton foreverButton1;
    }
}