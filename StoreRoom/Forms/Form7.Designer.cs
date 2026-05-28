namespace StoreRoom.Forms
{
    partial class Form7
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
            components = new System.ComponentModel.Container();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            flowLayoutPanel1 = new FlowLayoutPanel();
            foxButton1 = new ReaLTaiizor.Controls.FoxButton();
            dungeonNumeric1 = new ReaLTaiizor.Controls.DungeonNumeric();
            textBoxEdit1 = new ReaLTaiizor.Controls.TextBoxEdit();
            panel1 = new Panel();
            poisonDataGridView1 = new ReaLTaiizor.Controls.PoisonDataGridView();
            poisonContextMenuStrip1 = new ReaLTaiizor.Controls.PoisonContextMenuStrip(components);
            toolStripMenuItem1 = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripMenuItem();
            toolStripMenuItem3 = new ToolStripMenuItem();
            toolStripMenuItem4 = new ToolStripMenuItem();
            flowLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)poisonDataGridView1).BeginInit();
            poisonContextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(foxButton1);
            flowLayoutPanel1.Controls.Add(dungeonNumeric1);
            flowLayoutPanel1.Dock = DockStyle.Bottom;
            flowLayoutPanel1.Location = new Point(3, 602);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(1023, 58);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // foxButton1
            // 
            foxButton1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            foxButton1.BackColor = Color.Transparent;
            foxButton1.BaseColor = Color.FromArgb(249, 249, 249);
            foxButton1.BorderColor = Color.FromArgb(193, 193, 193);
            foxButton1.DisabledBaseColor = Color.FromArgb(249, 249, 249);
            foxButton1.DisabledBorderColor = Color.FromArgb(209, 209, 209);
            foxButton1.DisabledTextColor = Color.FromArgb(166, 178, 190);
            foxButton1.DownColor = Color.FromArgb(232, 232, 232);
            foxButton1.EnabledCalc = true;
            foxButton1.Font = new Font("Tahoma", 12F, FontStyle.Bold);
            foxButton1.ForeColor = Color.Black;
            foxButton1.Location = new Point(897, 3);
            foxButton1.Name = "foxButton1";
            foxButton1.OverColor = Color.FromArgb(242, 242, 242);
            foxButton1.Size = new Size(123, 52);
            foxButton1.TabIndex = 0;
            foxButton1.Text = "نمایش";
            foxButton1.Click += foxButton1_Click;
            // 
            // dungeonNumeric1
            // 
            dungeonNumeric1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            dungeonNumeric1.BackColor = Color.Transparent;
            dungeonNumeric1.BackColorA = Color.FromArgb(246, 246, 246);
            dungeonNumeric1.BackColorB = Color.FromArgb(254, 254, 254);
            dungeonNumeric1.BorderColor = Color.FromArgb(180, 180, 180);
            dungeonNumeric1.ButtonForeColorA = Color.FromArgb(75, 75, 75);
            dungeonNumeric1.ButtonForeColorB = Color.FromArgb(75, 75, 75);
            dungeonNumeric1.Font = new Font("Tahoma", 12F, FontStyle.Bold);
            dungeonNumeric1.ForeColor = Color.Black;
            dungeonNumeric1.Location = new Point(733, 3);
            dungeonNumeric1.Maximum = 100L;
            dungeonNumeric1.Minimum = 0L;
            dungeonNumeric1.MinimumSize = new Size(93, 28);
            dungeonNumeric1.Name = "dungeonNumeric1";
            dungeonNumeric1.Size = new Size(158, 28);
            dungeonNumeric1.TabIndex = 6;
            dungeonNumeric1.Text = "dungeonNumeric1";
            dungeonNumeric1.TextAlignment = ReaLTaiizor.Controls.DungeonNumeric._TextAlignment.Near;
            dungeonNumeric1.Value = 20L;
            // 
            // textBoxEdit1
            // 
            textBoxEdit1.BackColor = Color.Transparent;
            textBoxEdit1.Dock = DockStyle.Top;
            textBoxEdit1.Font = new Font("Tahoma", 11F);
            textBoxEdit1.ForeColor = Color.White;
            textBoxEdit1.Image = null;
            textBoxEdit1.Location = new Point(3, 64);
            textBoxEdit1.MaxLength = 32767;
            textBoxEdit1.Multiline = false;
            textBoxEdit1.Name = "textBoxEdit1";
            textBoxEdit1.ReadOnly = false;
            textBoxEdit1.RightToLeft = RightToLeft.Yes;
            textBoxEdit1.Size = new Size(1023, 41);
            textBoxEdit1.TabIndex = 2;
            textBoxEdit1.TextAlignment = HorizontalAlignment.Left;
            textBoxEdit1.UseSystemPasswordChar = false;
            textBoxEdit1.TextChanged += textBoxEdit1_TextChanged_1;
            textBoxEdit1.Enter += textBoxEdit1_Enter;
            // 
            // panel1
            // 
            panel1.Controls.Add(poisonDataGridView1);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(3, 287);
            panel1.Name = "panel1";
            panel1.Size = new Size(1023, 315);
            panel1.TabIndex = 4;
            // 
            // poisonDataGridView1
            // 
            poisonDataGridView1.AllowUserToResizeRows = false;
            poisonDataGridView1.BackgroundColor = Color.FromArgb(255, 255, 255);
            poisonDataGridView1.BorderStyle = BorderStyle.None;
            poisonDataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.None;
            poisonDataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(0, 174, 219);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = Color.FromArgb(255, 255, 255);
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(0, 198, 247);
            dataGridViewCellStyle1.SelectionForeColor = Color.FromArgb(17, 17, 17);
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            poisonDataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            poisonDataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            poisonDataGridView1.ContextMenuStrip = poisonContextMenuStrip1;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(255, 255, 255);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Pixel);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(136, 136, 136);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(0, 198, 247);
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(17, 17, 17);
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            poisonDataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            poisonDataGridView1.Dock = DockStyle.Fill;
            poisonDataGridView1.EnableHeadersVisualStyles = false;
            poisonDataGridView1.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Pixel);
            poisonDataGridView1.GridColor = Color.FromArgb(255, 255, 255);
            poisonDataGridView1.Location = new Point(0, 0);
            poisonDataGridView1.Name = "poisonDataGridView1";
            poisonDataGridView1.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(0, 174, 219);
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Pixel);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(255, 255, 255);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(0, 198, 247);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(17, 17, 17);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            poisonDataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            poisonDataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            poisonDataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            poisonDataGridView1.Size = new Size(1023, 315);
            poisonDataGridView1.TabIndex = 0;
            poisonDataGridView1.CellClick += poisonDataGridView1_CellClick;
            // 
            // poisonContextMenuStrip1
            // 
            poisonContextMenuStrip1.Font = new Font("Tahoma", 12F, FontStyle.Bold);
            poisonContextMenuStrip1.Items.AddRange(new ToolStripItem[] { toolStripMenuItem1, toolStripMenuItem2, toolStripMenuItem3, toolStripMenuItem4 });
            poisonContextMenuStrip1.Name = "poisonContextMenuStrip1";
            poisonContextMenuStrip1.RightToLeft = RightToLeft.Yes;
            poisonContextMenuStrip1.Size = new Size(181, 122);
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(180, 24);
            toolStripMenuItem1.Text = "ویرایش";
            toolStripMenuItem1.Click += toolStripMenuItem1_Click;
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Size(180, 24);
            toolStripMenuItem2.Text = "حذف";
            // 
            // toolStripMenuItem3
            // 
            toolStripMenuItem3.Name = "toolStripMenuItem3";
            toolStripMenuItem3.Size = new Size(180, 24);
            toolStripMenuItem3.Text = "ثبت ورود کالا";
            toolStripMenuItem3.Click += toolStripMenuItem3_Click;
            // 
            // toolStripMenuItem4
            // 
            toolStripMenuItem4.Name = "toolStripMenuItem4";
            toolStripMenuItem4.Size = new Size(180, 24);
            toolStripMenuItem4.Text = "ثبت خروج کالا";
            // 
            // Form7
            // 
            AutoScaleDimensions = new SizeF(10F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1030, 664);
            Controls.Add(panel1);
            Controls.Add(textBoxEdit1);
            Controls.Add(flowLayoutPanel1);
            Font = new Font("Tahoma", 12F, FontStyle.Bold);
            Margin = new Padding(4);
            Name = "Form7";
            Padding = new Padding(3, 64, 4, 4);
            RightToLeft = RightToLeft.Yes;
            Text = "لیست کالاها";
            Load += Form7_Load;
            flowLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)poisonDataGridView1).EndInit();
            poisonContextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private FlowLayoutPanel flowLayoutPanel1;
        private ReaLTaiizor.Controls.TextBoxEdit textBoxEdit1;
        private ReaLTaiizor.Controls.DungeonNumeric dungeonNumeric1;
        private ReaLTaiizor.Controls.FoxButton foxButton1;
        private Panel panel1;
        private ReaLTaiizor.Controls.PoisonDataGridView poisonDataGridView1;
        private ReaLTaiizor.Controls.PoisonContextMenuStrip poisonContextMenuStrip1;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem toolStripMenuItem2;
        private ToolStripMenuItem toolStripMenuItem3;
        private ToolStripMenuItem toolStripMenuItem4;
    }
}