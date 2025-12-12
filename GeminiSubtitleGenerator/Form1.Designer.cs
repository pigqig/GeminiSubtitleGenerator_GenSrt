namespace GeminiSubtitleGenerator
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            menuFile = new ToolStripMenuItem();
            menuSaveKey = new ToolStripMenuItem();
            menuHelp = new ToolStripMenuItem();
            menuAbout = new ToolStripMenuItem();
            panelHeader = new Panel();
            lblTitle = new Label();
            lblSubtitle = new Label();
            panelTopControls = new Panel();
            grpInput = new GroupBox();
            lblApiKey = new Label();
            txtApiKey = new TextBox();
            lblModel = new Label();
            cmbModel = new ComboBox();
            lblSrtPath = new Label();
            txtSrtPath = new TextBox();
            btnBrowseSrt = new Button();
            lblTempPath = new Label();
            txtTempPath = new TextBox();
            btnBrowseTemp = new Button();
            btnAddFiles = new Button();
            btnClear = new Button();
            panelStatus = new Panel();
            lblStatus = new Label();
            panelLog = new Panel();
            rtbLog = new RichTextBox();
            lblLogHeader = new Label();
            panelBottomActions = new Panel();
            btnStart = new Button();
            btnManualMerge = new Button();
            spacer = new Panel();
            btnCancel = new Button();
            btnPause = new Button();
            progressBar1 = new ProgressBar();
            panelFill = new Panel();
            lstFiles = new ListBox();
            lblListHeader = new Label();
            menuStrip1.SuspendLayout();
            panelHeader.SuspendLayout();
            panelTopControls.SuspendLayout();
            grpInput.SuspendLayout();
            panelStatus.SuspendLayout();
            panelLog.SuspendLayout();
            panelBottomActions.SuspendLayout();
            panelFill.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { menuFile, menuHelp });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(850, 24);
            menuStrip1.TabIndex = 6;
            // 
            // menuFile
            // 
            menuFile.DropDownItems.AddRange(new ToolStripItem[] { menuSaveKey });
            menuFile.Name = "menuFile";
            menuFile.Size = new Size(43, 20);
            menuFile.Text = "設定";
            // 
            // menuSaveKey
            // 
            menuSaveKey.Name = "menuSaveKey";
            menuSaveKey.Size = new Size(146, 22);
            menuSaveKey.Text = "儲存目前設定";
            menuSaveKey.Click += menuSaveKey_Click;
            // 
            // menuHelp
            // 
            menuHelp.DropDownItems.AddRange(new ToolStripItem[] { menuAbout });
            menuHelp.Name = "menuHelp";
            menuHelp.Size = new Size(43, 20);
            menuHelp.Text = "說明";
            // 
            // menuAbout
            // 
            menuAbout.Name = "menuAbout";
            menuAbout.Size = new Size(122, 22);
            menuAbout.Text = "關於作者";
            menuAbout.Click += menuAbout_Click;
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.FromArgb(30, 30, 30);
            panelHeader.Controls.Add(lblTitle);
            panelHeader.Controls.Add(lblSubtitle);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 24);
            panelHeader.Name = "panelHeader";
            panelHeader.Padding = new Padding(20);
            panelHeader.Size = new Size(850, 80);
            panelHeader.TabIndex = 5;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(20, 15);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(235, 32);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "酷意 AI 字幕產生器";
            // 
            // lblSubtitle
            // 
            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblSubtitle.ForeColor = Color.Gray;
            lblSubtitle.Location = new Point(25, 50);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(86, 15);
            lblSubtitle.TabIndex = 1;
            lblSubtitle.Text = "Author: Joseph";
            // 
            // panelTopControls
            // 
            panelTopControls.BackColor = Color.FromArgb(45, 45, 48);
            panelTopControls.Controls.Add(grpInput);
            panelTopControls.Dock = DockStyle.Top;
            panelTopControls.Location = new Point(0, 104);
            panelTopControls.Name = "panelTopControls";
            panelTopControls.Padding = new Padding(10);
            panelTopControls.Size = new Size(850, 220);
            panelTopControls.TabIndex = 1;
            // 
            // grpInput
            // 
            grpInput.Controls.Add(lblApiKey);
            grpInput.Controls.Add(txtApiKey);
            grpInput.Controls.Add(lblModel);
            grpInput.Controls.Add(cmbModel);
            grpInput.Controls.Add(lblSrtPath);
            grpInput.Controls.Add(txtSrtPath);
            grpInput.Controls.Add(btnBrowseSrt);
            grpInput.Controls.Add(lblTempPath);
            grpInput.Controls.Add(txtTempPath);
            grpInput.Controls.Add(btnBrowseTemp);
            grpInput.Controls.Add(btnAddFiles);
            grpInput.Controls.Add(btnClear);
            grpInput.Dock = DockStyle.Fill;
            grpInput.ForeColor = Color.White;
            grpInput.Location = new Point(10, 10);
            grpInput.Name = "grpInput";
            grpInput.Size = new Size(830, 200);
            grpInput.TabIndex = 0;
            grpInput.TabStop = false;
            grpInput.Text = "參數設定";
            // 
            // lblApiKey
            // 
            lblApiKey.AutoSize = true;
            lblApiKey.Location = new Point(20, 30);
            lblApiKey.Name = "lblApiKey";
            lblApiKey.Size = new Size(50, 15);
            lblApiKey.TabIndex = 0;
            lblApiKey.Text = "API Key:";
            // 
            // txtApiKey
            // 
            txtApiKey.BackColor = Color.FromArgb(30, 30, 30);
            txtApiKey.BorderStyle = BorderStyle.FixedSingle;
            txtApiKey.ForeColor = Color.White;
            txtApiKey.Location = new Point(100, 27);
            txtApiKey.Name = "txtApiKey";
            txtApiKey.Size = new Size(350, 23);
            txtApiKey.TabIndex = 1;
            // 
            // lblModel
            // 
            lblModel.AutoSize = true;
            lblModel.Location = new Point(470, 30);
            lblModel.Name = "lblModel";
            lblModel.Size = new Size(77, 15);
            lblModel.TabIndex = 2;
            lblModel.Text = "Gemini 模型:";
            // 
            // cmbModel
            // 
            cmbModel.BackColor = Color.FromArgb(30, 30, 30);
            cmbModel.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbModel.ForeColor = Color.White;
            cmbModel.Location = new Point(560, 27);
            cmbModel.Name = "cmbModel";
            cmbModel.Size = new Size(180, 23);
            cmbModel.TabIndex = 3;
            // 
            // lblSrtPath
            // 
            lblSrtPath.AutoSize = true;
            lblSrtPath.Location = new Point(20, 65);
            lblSrtPath.Name = "lblSrtPath";
            lblSrtPath.Size = new Size(62, 15);
            lblSrtPath.TabIndex = 4;
            lblSrtPath.Text = "字幕儲存:";
            // 
            // txtSrtPath
            // 
            txtSrtPath.BackColor = Color.FromArgb(30, 30, 30);
            txtSrtPath.BorderStyle = BorderStyle.FixedSingle;
            txtSrtPath.ForeColor = Color.White;
            txtSrtPath.Location = new Point(100, 62);
            txtSrtPath.Name = "txtSrtPath";
            txtSrtPath.Size = new Size(550, 23);
            txtSrtPath.TabIndex = 5;
            // 
            // btnBrowseSrt
            // 
            btnBrowseSrt.BackColor = Color.Gray;
            btnBrowseSrt.FlatStyle = FlatStyle.Flat;
            btnBrowseSrt.Location = new Point(660, 61);
            btnBrowseSrt.Name = "btnBrowseSrt";
            btnBrowseSrt.Size = new Size(40, 25);
            btnBrowseSrt.TabIndex = 6;
            btnBrowseSrt.Text = "...";
            btnBrowseSrt.UseVisualStyleBackColor = false;
            btnBrowseSrt.Click += btnBrowseSrt_Click;
            // 
            // lblTempPath
            // 
            lblTempPath.AutoSize = true;
            lblTempPath.Location = new Point(20, 100);
            lblTempPath.Name = "lblTempPath";
            lblTempPath.Size = new Size(62, 15);
            lblTempPath.TabIndex = 7;
            lblTempPath.Text = "音效暫存:";
            // 
            // txtTempPath
            // 
            txtTempPath.BackColor = Color.FromArgb(30, 30, 30);
            txtTempPath.BorderStyle = BorderStyle.FixedSingle;
            txtTempPath.ForeColor = Color.White;
            txtTempPath.Location = new Point(100, 97);
            txtTempPath.Name = "txtTempPath";
            txtTempPath.Size = new Size(550, 23);
            txtTempPath.TabIndex = 8;
            // 
            // btnBrowseTemp
            // 
            btnBrowseTemp.BackColor = Color.Gray;
            btnBrowseTemp.FlatStyle = FlatStyle.Flat;
            btnBrowseTemp.Location = new Point(660, 96);
            btnBrowseTemp.Name = "btnBrowseTemp";
            btnBrowseTemp.Size = new Size(40, 25);
            btnBrowseTemp.TabIndex = 9;
            btnBrowseTemp.Text = "...";
            btnBrowseTemp.UseVisualStyleBackColor = false;
            btnBrowseTemp.Click += btnBrowseTemp_Click;
            // 
            // btnAddFiles
            // 
            btnAddFiles.BackColor = Color.FromArgb(0, 122, 204);
            btnAddFiles.FlatStyle = FlatStyle.Flat;
            btnAddFiles.ForeColor = Color.White;
            btnAddFiles.Location = new Point(20, 150);
            btnAddFiles.Name = "btnAddFiles";
            btnAddFiles.Size = new Size(120, 35);
            btnAddFiles.TabIndex = 10;
            btnAddFiles.Text = "+ 加入影片";
            btnAddFiles.UseVisualStyleBackColor = false;
            btnAddFiles.Click += btnAddFiles_Click;
            // 
            // btnClear
            // 
            btnClear.BackColor = Color.FromArgb(63, 63, 70);
            btnClear.FlatStyle = FlatStyle.Flat;
            btnClear.ForeColor = Color.White;
            btnClear.Location = new Point(150, 150);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(100, 35);
            btnClear.TabIndex = 11;
            btnClear.Text = "清除列表";
            btnClear.UseVisualStyleBackColor = false;
            btnClear.Click += btnClear_Click;
            // 
            // panelStatus
            // 
            panelStatus.BackColor = Color.FromArgb(0, 122, 204);
            panelStatus.Controls.Add(lblStatus);
            panelStatus.Dock = DockStyle.Bottom;
            panelStatus.Location = new Point(0, 670);
            panelStatus.Name = "panelStatus";
            panelStatus.Size = new Size(850, 30);
            panelStatus.TabIndex = 4;
            // 
            // lblStatus
            // 
            lblStatus.Dock = DockStyle.Fill;
            lblStatus.ForeColor = Color.White;
            lblStatus.Location = new Point(0, 0);
            lblStatus.Name = "lblStatus";
            lblStatus.Padding = new Padding(10, 0, 0, 0);
            lblStatus.Size = new Size(850, 30);
            lblStatus.TabIndex = 0;
            lblStatus.Text = "準備就緒";
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panelLog
            // 
            panelLog.BackColor = Color.Black;
            panelLog.Controls.Add(rtbLog);
            panelLog.Controls.Add(lblLogHeader);
            panelLog.Dock = DockStyle.Bottom;
            panelLog.Location = new Point(0, 520);
            panelLog.Name = "panelLog";
            panelLog.Padding = new Padding(5);
            panelLog.Size = new Size(850, 150);
            panelLog.TabIndex = 3;
            // 
            // rtbLog
            // 
            rtbLog.BackColor = Color.FromArgb(20, 20, 20);
            rtbLog.BorderStyle = BorderStyle.None;
            rtbLog.Dock = DockStyle.Fill;
            rtbLog.Font = new Font("Consolas", 10F, FontStyle.Regular, GraphicsUnit.Point);
            rtbLog.ForeColor = Color.FromArgb(0, 255, 0);
            rtbLog.Location = new Point(5, 28);
            rtbLog.Name = "rtbLog";
            rtbLog.ReadOnly = true;
            rtbLog.Size = new Size(840, 117);
            rtbLog.TabIndex = 0;
            rtbLog.Text = "";
            // 
            // lblLogHeader
            // 
            lblLogHeader.BackColor = Color.FromArgb(30, 30, 30);
            lblLogHeader.Dock = DockStyle.Top;
            lblLogHeader.ForeColor = Color.Gray;
            lblLogHeader.Location = new Point(5, 5);
            lblLogHeader.Name = "lblLogHeader";
            lblLogHeader.Size = new Size(840, 23);
            lblLogHeader.TabIndex = 1;
            lblLogHeader.Text = "執行紀錄 (Log) >_";
            // 
            // panelBottomActions
            // 
            panelBottomActions.BackColor = Color.FromArgb(37, 37, 38);
            panelBottomActions.Controls.Add(btnStart);
            panelBottomActions.Controls.Add(btnManualMerge);
            panelBottomActions.Controls.Add(spacer);
            panelBottomActions.Controls.Add(btnCancel);
            panelBottomActions.Controls.Add(btnPause);
            panelBottomActions.Controls.Add(progressBar1);
            panelBottomActions.Dock = DockStyle.Bottom;
            panelBottomActions.Location = new Point(0, 450);
            panelBottomActions.Name = "panelBottomActions";
            panelBottomActions.Padding = new Padding(10);
            panelBottomActions.Size = new Size(850, 70);
            panelBottomActions.TabIndex = 2;
            // 
            // btnStart
            // 
            btnStart.BackColor = Color.FromArgb(40, 167, 69);
            btnStart.Dock = DockStyle.Right;
            btnStart.FlatStyle = FlatStyle.Flat;
            btnStart.ForeColor = Color.White;
            btnStart.Location = new Point(620, 10);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(120, 40);
            btnStart.TabIndex = 0;
            btnStart.Text = "▶ 開始處理";
            btnStart.UseVisualStyleBackColor = false;
            btnStart.Click += btnStart_Click;
            // 
            // btnManualMerge
            // 
            btnManualMerge.BackColor = Color.FromArgb(108, 117, 125);
            btnManualMerge.Dock = DockStyle.Right;
            btnManualMerge.FlatStyle = FlatStyle.Flat;
            btnManualMerge.ForeColor = Color.White;
            btnManualMerge.Location = new Point(740, 10);
            btnManualMerge.Name = "btnManualMerge";
            btnManualMerge.Size = new Size(100, 40);
            btnManualMerge.TabIndex = 1;
            btnManualMerge.Text = "🛠 手動合併";
            btnManualMerge.UseVisualStyleBackColor = false;
            btnManualMerge.Click += btnManualMerge_Click;
            // 
            // spacer
            // 
            spacer.BackColor = Color.Transparent;
            spacer.Dock = DockStyle.Fill;
            spacer.Location = new Point(210, 10);
            spacer.Name = "spacer";
            spacer.Size = new Size(630, 40);
            spacer.TabIndex = 2;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.FromArgb(220, 53, 69);
            btnCancel.Dock = DockStyle.Left;
            btnCancel.Enabled = false;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(110, 10);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(100, 40);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "⏹ 取消";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnPause
            // 
            btnPause.BackColor = Color.FromArgb(255, 193, 7);
            btnPause.Dock = DockStyle.Left;
            btnPause.Enabled = false;
            btnPause.FlatStyle = FlatStyle.Flat;
            btnPause.Location = new Point(10, 10);
            btnPause.Name = "btnPause";
            btnPause.Size = new Size(100, 40);
            btnPause.TabIndex = 4;
            btnPause.Text = "⏸ 暫停";
            btnPause.UseVisualStyleBackColor = false;
            btnPause.Click += btnPause_Click;
            // 
            // progressBar1
            // 
            progressBar1.Dock = DockStyle.Bottom;
            progressBar1.Location = new Point(10, 50);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(830, 10);
            progressBar1.TabIndex = 5;
            // 
            // panelFill
            // 
            panelFill.BackColor = Color.FromArgb(45, 45, 48);
            panelFill.Controls.Add(lstFiles);
            panelFill.Controls.Add(lblListHeader);
            panelFill.Dock = DockStyle.Fill;
            panelFill.Location = new Point(0, 324);
            panelFill.Name = "panelFill";
            panelFill.Padding = new Padding(10);
            panelFill.Size = new Size(850, 126);
            panelFill.TabIndex = 0;
            // 
            // lstFiles
            // 
            lstFiles.BackColor = Color.FromArgb(30, 30, 30);
            lstFiles.BorderStyle = BorderStyle.None;
            lstFiles.Dock = DockStyle.Fill;
            lstFiles.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lstFiles.ForeColor = Color.White;
            lstFiles.ItemHeight = 17;
            lstFiles.Location = new Point(10, 35);
            lstFiles.Name = "lstFiles";
            lstFiles.Size = new Size(830, 81);
            lstFiles.TabIndex = 0;
            // 
            // lblListHeader
            // 
            lblListHeader.Dock = DockStyle.Top;
            lblListHeader.ForeColor = Color.WhiteSmoke;
            lblListHeader.Location = new Point(10, 10);
            lblListHeader.Name = "lblListHeader";
            lblListHeader.Size = new Size(830, 25);
            lblListHeader.TabIndex = 1;
            lblListHeader.Text = "待處理清單:";
            // 
            // Form1
            // 
            ClientSize = new Size(850, 700);
            Controls.Add(panelFill);
            Controls.Add(panelTopControls);
            Controls.Add(panelBottomActions);
            Controls.Add(panelLog);
            Controls.Add(panelStatus);
            Controls.Add(panelHeader);
            Controls.Add(menuStrip1);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            Text = "酷意 AI 字幕產生器 - v6.0";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            panelTopControls.ResumeLayout(false);
            grpInput.ResumeLayout(false);
            grpInput.PerformLayout();
            panelStatus.ResumeLayout(false);
            panelLog.ResumeLayout(false);
            panelBottomActions.ResumeLayout(false);
            panelFill.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuFile, menuSaveKey, menuHelp, menuAbout;
        private System.Windows.Forms.Panel panelHeader, panelTopControls, panelFill, panelBottomActions, panelLog, panelStatus;
        private System.Windows.Forms.GroupBox grpInput;
        private System.Windows.Forms.Label lblTitle, lblSubtitle, lblApiKey, lblModel, lblSrtPath, lblTempPath, lblListHeader, lblLogHeader, lblStatus;
        private System.Windows.Forms.TextBox txtApiKey, txtSrtPath, txtTempPath;
        private System.Windows.Forms.ComboBox cmbModel;
        private System.Windows.Forms.ListBox lstFiles;
        private System.Windows.Forms.Button btnAddFiles, btnClear, btnStart, btnManualMerge, btnPause, btnCancel, btnBrowseSrt, btnBrowseTemp;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.RichTextBox rtbLog;
        private Panel spacer;
    }
}