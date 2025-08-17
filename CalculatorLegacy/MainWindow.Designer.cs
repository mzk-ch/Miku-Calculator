namespace CalculatorLegacy
{
    partial class MainWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            pnlText = new Panel();
            lblText = new Label();
            backupButtonPanel1 = new BackupButtonPanel();
            keyBoardPanel1 = new KeyBoardPanel();
            pnlText.SuspendLayout();
            SuspendLayout();
            // 
            // pnlText
            // 
            pnlText.Controls.Add(lblText);
            pnlText.Dock = DockStyle.Top;
            pnlText.Location = new Point(0, 0);
            pnlText.Name = "pnlText";
            pnlText.Size = new Size(729, 100);
            pnlText.TabIndex = 0;
            // 
            // lblText
            // 
            lblText.BackColor = Color.Black;
            lblText.Dock = DockStyle.Fill;
            lblText.Font = new Font("Microsoft YaHei UI", 48F, FontStyle.Regular, GraphicsUnit.Point, 134);
            lblText.Location = new Point(0, 0);
            lblText.Name = "lblText";
            lblText.Size = new Size(729, 100);
            lblText.TabIndex = 0;
            lblText.Text = "1,000,000,000,000,000";
            lblText.TextAlign = ContentAlignment.TopRight;
            // 
            // backupButtonPanel1
            // 
            backupButtonPanel1.BackColor = Color.FromArgb(153, 153, 153);
            backupButtonPanel1.Dock = DockStyle.Top;
            backupButtonPanel1.ForeColor = Color.FromArgb(61, 193, 192);
            backupButtonPanel1.Location = new Point(0, 100);
            backupButtonPanel1.Name = "backupButtonPanel1";
            backupButtonPanel1.Size = new Size(729, 90);
            backupButtonPanel1.TabIndex = 1;
            // 
            // keyBoardPanel1
            // 
            keyBoardPanel1.BackColor = Color.FromArgb(153, 153, 153);
            keyBoardPanel1.Dock = DockStyle.Fill;
            keyBoardPanel1.Font = new Font("Microsoft YaHei UI", 26.25F, FontStyle.Regular, GraphicsUnit.Point, 134);
            keyBoardPanel1.ForeColor = Color.FromArgb(255, 8, 141);
            keyBoardPanel1.Location = new Point(0, 190);
            keyBoardPanel1.Name = "keyBoardPanel1";
            keyBoardPanel1.Size = new Size(729, 334);
            keyBoardPanel1.TabIndex = 2;
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(729, 524);
            Controls.Add(keyBoardPanel1);
            Controls.Add(backupButtonPanel1);
            Controls.Add(pnlText);
            DoubleBuffered = true;
            Icon = (Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            MaximizeBox = false;
            Name = "MainWindow";
            Text = "初音電卓";
            KeyDown += MainWindow_KeyDown;
            pnlText.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlText;
        private Label lblText;
        private CalculateButton whiteFontButton1;
        private BackupButtonPanel backupButtonPanel1;
        private KeyBoardPanel keyBoardPanel1;
    }
}
