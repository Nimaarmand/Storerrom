namespace StoreRoom.Forms
{
    partial class Form14
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
            label3 = new Label();
            textBoxEdit2 = new ReaLTaiizor.Controls.TextBoxEdit();
            label1 = new Label();
            textBoxEdit1 = new ReaLTaiizor.Controls.TextBoxEdit();
            foreverButton1 = new ReaLTaiizor.Controls.ForeverButton();
            SuspendLayout();
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Location = new Point(419, 78);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(96, 19);
            label3.TabIndex = 22;
            label3.Text = "نام کاربری :";
            // 
            // textBoxEdit2
            // 
            textBoxEdit2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBoxEdit2.BackColor = Color.Transparent;
            textBoxEdit2.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            textBoxEdit2.ForeColor = Color.White;
            textBoxEdit2.Image = null;
            textBoxEdit2.Location = new Point(38, 172);
            textBoxEdit2.Margin = new Padding(4, 4, 4, 4);
            textBoxEdit2.MaxLength = 32767;
            textBoxEdit2.Multiline = false;
            textBoxEdit2.Name = "textBoxEdit2";
            textBoxEdit2.ReadOnly = false;
            textBoxEdit2.RightToLeft = RightToLeft.Yes;
            textBoxEdit2.Size = new Size(373, 43);
            textBoxEdit2.TabIndex = 21;
            textBoxEdit2.TextAlignment = HorizontalAlignment.Left;
            textBoxEdit2.UseSystemPasswordChar = false;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(419, 172);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(83, 19);
            label1.TabIndex = 24;
            label1.Text = "رمز عبور :";
            // 
            // textBoxEdit1
            // 
            textBoxEdit1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBoxEdit1.BackColor = Color.Transparent;
            textBoxEdit1.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            textBoxEdit1.ForeColor = Color.White;
            textBoxEdit1.Image = null;
            textBoxEdit1.Location = new Point(38, 78);
            textBoxEdit1.Margin = new Padding(4, 4, 4, 4);
            textBoxEdit1.MaxLength = 32767;
            textBoxEdit1.Multiline = false;
            textBoxEdit1.Name = "textBoxEdit1";
            textBoxEdit1.ReadOnly = false;
            textBoxEdit1.Size = new Size(373, 43);
            textBoxEdit1.TabIndex = 23;
            textBoxEdit1.TextAlignment = HorizontalAlignment.Left;
            textBoxEdit1.UseSystemPasswordChar = false;
            // 
            // foreverButton1
            // 
            foreverButton1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            foreverButton1.BackColor = Color.Transparent;
            foreverButton1.BaseColor = Color.FromArgb(35, 168, 109);
            foreverButton1.Font = new Font("Tahoma", 12F, FontStyle.Bold);
            foreverButton1.Location = new Point(102, 258);
            foreverButton1.Margin = new Padding(4, 4, 4, 4);
            foreverButton1.Name = "foreverButton1";
            foreverButton1.Rounded = false;
            foreverButton1.Size = new Size(208, 71);
            foreverButton1.TabIndex = 25;
            foreverButton1.Text = "ورود";
            foreverButton1.TextColor = Color.FromArgb(243, 243, 243);
            foreverButton1.Click += foreverButton1_Click;
            // 
            // Form14
            // 
            AutoScaleDimensions = new SizeF(10F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(554, 346);
            Controls.Add(foreverButton1);
            Controls.Add(label1);
            Controls.Add(textBoxEdit1);
            Controls.Add(label3);
            Controls.Add(textBoxEdit2);
            Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Margin = new Padding(4, 4, 4, 4);
            Name = "Form14";
            Padding = new Padding(3, 64, 4, 4);
            RightToLeft = RightToLeft.Yes;
            Text = "ورود";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label3;
        private ReaLTaiizor.Controls.TextBoxEdit textBoxEdit2;
        private Label label1;
        private ReaLTaiizor.Controls.TextBoxEdit textBoxEdit1;
        private ReaLTaiizor.Controls.ForeverButton foreverButton1;
    }
}