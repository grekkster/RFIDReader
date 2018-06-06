namespace RFIDReader
{
    partial class RFIDReaderForm
    {
        /// <summary>
        /// Vyžaduje se proměnná návrháře.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Uvolněte všechny používané prostředky.
        /// </summary>
        /// <param name="disposing">hodnota true, když by se měl spravovaný prostředek odstranit; jinak false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kód generovaný Návrhářem Windows Form

        /// <summary>
        /// Metoda vyžadovaná pro podporu Návrháře - neupravovat
        /// obsah této metody v editoru kódu.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.tbRaceDayId = new System.Windows.Forms.TextBox();
            this.lblRaceDayId = new System.Windows.Forms.Label();
            this.tbDeviceId = new System.Windows.Forms.TextBox();
            this.lblDeviceId = new System.Windows.Forms.Label();
            this.tbSuffix = new System.Windows.Forms.TextBox();
            this.lblSuffix = new System.Windows.Forms.Label();
            this.tbPrefix = new System.Windows.Forms.TextBox();
            this.lblPrefix = new System.Windows.Forms.Label();
            this.cbSerialPort = new System.Windows.Forms.ComboBox();
            this.btnOpen = new System.Windows.Forms.Button();
            this.rtbInfo = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.tbRaceDayId);
            this.splitContainer.Panel1.Controls.Add(this.lblRaceDayId);
            this.splitContainer.Panel1.Controls.Add(this.tbDeviceId);
            this.splitContainer.Panel1.Controls.Add(this.lblDeviceId);
            this.splitContainer.Panel1.Controls.Add(this.tbSuffix);
            this.splitContainer.Panel1.Controls.Add(this.lblSuffix);
            this.splitContainer.Panel1.Controls.Add(this.tbPrefix);
            this.splitContainer.Panel1.Controls.Add(this.lblPrefix);
            this.splitContainer.Panel1.Controls.Add(this.cbSerialPort);
            this.splitContainer.Panel1.Controls.Add(this.btnOpen);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.rtbInfo);
            this.splitContainer.Size = new System.Drawing.Size(784, 561);
            this.splitContainer.SplitterDistance = 226;
            this.splitContainer.TabIndex = 0;
            // 
            // tbRaceDayId
            // 
            this.tbRaceDayId.Location = new System.Drawing.Point(81, 90);
            this.tbRaceDayId.Name = "tbRaceDayId";
            this.tbRaceDayId.Size = new System.Drawing.Size(133, 20);
            this.tbRaceDayId.TabIndex = 9;
            // 
            // lblRaceDayId
            // 
            this.lblRaceDayId.AutoSize = true;
            this.lblRaceDayId.Location = new System.Drawing.Point(12, 97);
            this.lblRaceDayId.Name = "lblRaceDayId";
            this.lblRaceDayId.Size = new System.Drawing.Size(66, 13);
            this.lblRaceDayId.TabIndex = 8;
            this.lblRaceDayId.Text = "RaceDay ID";
            // 
            // tbDeviceId
            // 
            this.tbDeviceId.Location = new System.Drawing.Point(81, 64);
            this.tbDeviceId.Name = "tbDeviceId";
            this.tbDeviceId.Size = new System.Drawing.Size(133, 20);
            this.tbDeviceId.TabIndex = 7;
            // 
            // lblDeviceId
            // 
            this.lblDeviceId.AutoSize = true;
            this.lblDeviceId.Location = new System.Drawing.Point(12, 71);
            this.lblDeviceId.Name = "lblDeviceId";
            this.lblDeviceId.Size = new System.Drawing.Size(55, 13);
            this.lblDeviceId.TabIndex = 6;
            this.lblDeviceId.Text = "Device ID";
            // 
            // tbSuffix
            // 
            this.tbSuffix.Location = new System.Drawing.Point(81, 38);
            this.tbSuffix.Name = "tbSuffix";
            this.tbSuffix.Size = new System.Drawing.Size(133, 20);
            this.tbSuffix.TabIndex = 5;
            this.tbSuffix.TextChanged += new System.EventHandler(this.tbSuffix_TextChanged);
            // 
            // lblSuffix
            // 
            this.lblSuffix.AutoSize = true;
            this.lblSuffix.Location = new System.Drawing.Point(12, 45);
            this.lblSuffix.Name = "lblSuffix";
            this.lblSuffix.Size = new System.Drawing.Size(33, 13);
            this.lblSuffix.TabIndex = 4;
            this.lblSuffix.Text = "Suffix";
            // 
            // tbPrefix
            // 
            this.tbPrefix.Location = new System.Drawing.Point(81, 12);
            this.tbPrefix.Name = "tbPrefix";
            this.tbPrefix.Size = new System.Drawing.Size(133, 20);
            this.tbPrefix.TabIndex = 3;
            this.tbPrefix.TextChanged += new System.EventHandler(this.tbPrefix_TextChanged);
            // 
            // lblPrefix
            // 
            this.lblPrefix.AutoSize = true;
            this.lblPrefix.Location = new System.Drawing.Point(12, 19);
            this.lblPrefix.Name = "lblPrefix";
            this.lblPrefix.Size = new System.Drawing.Size(33, 13);
            this.lblPrefix.TabIndex = 2;
            this.lblPrefix.Text = "Prefix";
            // 
            // cbSerialPort
            // 
            this.cbSerialPort.FormattingEnabled = true;
            this.cbSerialPort.Location = new System.Drawing.Point(81, 116);
            this.cbSerialPort.Name = "cbSerialPort";
            this.cbSerialPort.Size = new System.Drawing.Size(133, 21);
            this.cbSerialPort.TabIndex = 1;
            this.cbSerialPort.Click += new System.EventHandler(this.cbSerialPort_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(12, 116);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(63, 23);
            this.btnOpen.TabIndex = 0;
            this.btnOpen.Text = "Start";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // rtbInfo
            // 
            this.rtbInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbInfo.Location = new System.Drawing.Point(0, 0);
            this.rtbInfo.Name = "rtbInfo";
            this.rtbInfo.ReadOnly = true;
            this.rtbInfo.Size = new System.Drawing.Size(554, 561);
            this.rtbInfo.TabIndex = 0;
            this.rtbInfo.Text = "";
            this.rtbInfo.TextChanged += new System.EventHandler(this.rtbInfo_TextChanged);
            // 
            // RFIDReaderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.splitContainer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "RFIDReaderForm";
            this.Text = "RFID Reader";
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel1.PerformLayout();
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.ComboBox cbSerialPort;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.RichTextBox rtbInfo;
        private System.Windows.Forms.TextBox tbRaceDayId;
        private System.Windows.Forms.Label lblRaceDayId;
        private System.Windows.Forms.TextBox tbDeviceId;
        private System.Windows.Forms.Label lblDeviceId;
        private System.Windows.Forms.TextBox tbSuffix;
        private System.Windows.Forms.Label lblSuffix;
        private System.Windows.Forms.TextBox tbPrefix;
        private System.Windows.Forms.Label lblPrefix;
    }
}

