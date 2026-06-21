namespace StoreRoom.Forms
{
    partial class Form25
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form25));
            panel1 = new Panel();
            label1 = new Label();
            cyberProgressBar1 = new ReaLTaiizor.Controls.CyberProgressBar();
            pictureBox1 = new PictureBox();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.AutoSize = true;
            panel1.Controls.Add(label1);
            panel1.Controls.Add(cyberProgressBar1);
            panel1.Dock = DockStyle.Bottom;
            panel1.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            panel1.Location = new Point(3, 714);
            panel1.Name = "panel1";
            panel1.Size = new Size(1154, 40);
            panel1.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(399, 3);
            label1.Name = "label1";
            label1.Size = new Size(59, 19);
            label1.TabIndex = 4;
            label1.Text = "label1";
            // 
            // cyberProgressBar1
            // 
            cyberProgressBar1.Alpha = 50;
            cyberProgressBar1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cyberProgressBar1.BackColor = Color.Transparent;
            cyberProgressBar1.Background = true;
            cyberProgressBar1.Background_WidthPen = 3F;
            cyberProgressBar1.BackgroundPen = true;
            cyberProgressBar1.ColorBackground = Color.FromArgb(37, 52, 68);
            cyberProgressBar1.ColorBackground_1 = Color.FromArgb(37, 52, 68);
            cyberProgressBar1.ColorBackground_2 = Color.FromArgb(41, 63, 86);
            cyberProgressBar1.ColorBackground_Pen = Color.FromArgb(29, 200, 238);
            cyberProgressBar1.ColorBackground_Value_1 = Color.FromArgb(28, 200, 238);
            cyberProgressBar1.ColorBackground_Value_2 = Color.FromArgb(100, 208, 232);
            cyberProgressBar1.ColorLighting = Color.FromArgb(29, 200, 238);
            cyberProgressBar1.ColorPen_1 = Color.FromArgb(37, 52, 68);
            cyberProgressBar1.ColorPen_2 = Color.FromArgb(41, 63, 86);
            cyberProgressBar1.ColorProgressBar = Color.FromArgb(29, 200, 238);
            cyberProgressBar1.ColorValue_Transparency = 200;
            cyberProgressBar1.CyberProgressBarStyle = ReaLTaiizor.Enum.Cyber.StateStyle.Custom;
            cyberProgressBar1.Font = new Font("Arial", 11F);
            cyberProgressBar1.ForeColor = Color.FromArgb(245, 245, 245);
            cyberProgressBar1.Lighting = false;
            cyberProgressBar1.LinearGradient_Background = false;
            cyberProgressBar1.LinearGradient_Value = false;
            cyberProgressBar1.LinearGradientPen = false;
            cyberProgressBar1.Location = new Point(464, 3);
            cyberProgressBar1.Maximum = 100;
            cyberProgressBar1.Minimum = 0;
            cyberProgressBar1.Name = "cyberProgressBar1";
            cyberProgressBar1.PenWidth = 10;
            cyberProgressBar1.ProgressText = true;
            cyberProgressBar1.RGB = false;
            cyberProgressBar1.Rounding = true;
            cyberProgressBar1.RoundingInt = 70;
            cyberProgressBar1.Size = new Size(687, 34);
            cyberProgressBar1.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            cyberProgressBar1.StartDrawingValue = 0;
            cyberProgressBar1.TabIndex = 3;
            cyberProgressBar1.Tag = "Cyber";
            cyberProgressBar1.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            cyberProgressBar1.Timer_RGB = 300;
            cyberProgressBar1.Value = 0;
            // 
            // pictureBox1
            // 
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(3, 64);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(1154, 650);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // Form25
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1160, 757);
            Controls.Add(pictureBox1);
            Controls.Add(panel1);
            Name = "Form25";
            RightToLeft = RightToLeft.Yes;
            Text = "Form25";
            WindowState = FormWindowState.Maximized;
            Load += Form25_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private PictureBox pictureBox1;
        private ReaLTaiizor.Controls.CyberProgressBar cyberProgressBar1;
        private Label label1;
    }
}