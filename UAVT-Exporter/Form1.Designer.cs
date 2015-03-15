namespace UAVT_Exporter {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.label1 = new System.Windows.Forms.Label();
            this.cmbDistricts = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.sfDialog = new System.Windows.Forms.SaveFileDialog();
            this.ofDialog = new System.Windows.Forms.OpenFileDialog();
            this.txtSourcePath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDestinationPath = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOpenFDSource = new System.Windows.Forms.Button();
            this.btnOpenFDDest = new System.Windows.Forms.Button();
            this.bckWorker = new System.ComponentModel.BackgroundWorker();
            this.pbStatus = new System.Windows.Forms.ProgressBar();
            this.label4 = new System.Windows.Forms.Label();
            this.lblCount = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.chDate = new System.Windows.Forms.CheckBox();
            this.rdStat = new System.Windows.Forms.RadioButton();
            this.lblMessage = new System.Windows.Forms.Label();
            this.cmbCities = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "İlçe";
            // 
            // cmbDistricts
            // 
            this.cmbDistricts.FormattingEnabled = true;
            this.cmbDistricts.Location = new System.Drawing.Point(126, 36);
            this.cmbDistricts.Name = "cmbDistricts";
            this.cmbDistricts.Size = new System.Drawing.Size(354, 21);
            this.cmbDistricts.TabIndex = 1;
            this.cmbDistricts.SelectedIndexChanged += new System.EventHandler(this.cmbDistricts_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Orjinal .dat Dosya Yolu";
            // 
            // ofDialog
            // 
            this.ofDialog.FileName = "openFileDialog1";
            // 
            // txtSourcePath
            // 
            this.txtSourcePath.Location = new System.Drawing.Point(126, 68);
            this.txtSourcePath.Name = "txtSourcePath";
            this.txtSourcePath.Size = new System.Drawing.Size(354, 20);
            this.txtSourcePath.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 238);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Oluşturulan .dat Yolu";
            this.label3.Visible = false;
            // 
            // txtDestinationPath
            // 
            this.txtDestinationPath.Location = new System.Drawing.Point(126, 234);
            this.txtDestinationPath.Name = "txtDestinationPath";
            this.txtDestinationPath.Size = new System.Drawing.Size(354, 20);
            this.txtDestinationPath.TabIndex = 5;
            this.txtDestinationPath.Visible = false;
            // 
            // btnStart
            // 
            this.btnStart.Enabled = false;
            this.btnStart.Location = new System.Drawing.Point(199, 155);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 6;
            this.btnStart.Text = "Başlat";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(299, 154);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "İptal";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOpenFDSource
            // 
            this.btnOpenFDSource.Location = new System.Drawing.Point(491, 67);
            this.btnOpenFDSource.Name = "btnOpenFDSource";
            this.btnOpenFDSource.Size = new System.Drawing.Size(34, 21);
            this.btnOpenFDSource.TabIndex = 8;
            this.btnOpenFDSource.Text = "Seçiniz...";
            this.btnOpenFDSource.UseVisualStyleBackColor = true;
            this.btnOpenFDSource.Click += new System.EventHandler(this.btnOpenFDSource_Click);
            // 
            // btnOpenFDDest
            // 
            this.btnOpenFDDest.Location = new System.Drawing.Point(491, 233);
            this.btnOpenFDDest.Name = "btnOpenFDDest";
            this.btnOpenFDDest.Size = new System.Drawing.Size(34, 21);
            this.btnOpenFDDest.TabIndex = 9;
            this.btnOpenFDDest.Text = "Seçiniz...";
            this.btnOpenFDDest.UseVisualStyleBackColor = true;
            this.btnOpenFDDest.Visible = false;
            this.btnOpenFDDest.Click += new System.EventHandler(this.btnOpenFDDest_Click);
            // 
            // pbStatus
            // 
            this.pbStatus.Location = new System.Drawing.Point(9, 125);
            this.pbStatus.Maximum = 500000;
            this.pbStatus.Name = "pbStatus";
            this.pbStatus.Size = new System.Drawing.Size(522, 23);
            this.pbStatus.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pbStatus.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 160);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Aktarılan Veri:";
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Location = new System.Drawing.Point(84, 160);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(13, 13);
            this.lblCount.TabIndex = 12;
            this.lblCount.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 100);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Tarih Kriteri";
            // 
            // chDate
            // 
            this.chDate.AutoSize = true;
            this.chDate.Location = new System.Drawing.Point(126, 100);
            this.chDate.Name = "chDate";
            this.chDate.Size = new System.Drawing.Size(118, 17);
            this.chDate.TabIndex = 14;
            this.chDate.Text = "10-12-2014 Sonrası";
            this.chDate.UseVisualStyleBackColor = true;
            this.chDate.CheckedChanged += new System.EventHandler(this.chDate_CheckedChanged);
            // 
            // rdStat
            // 
            this.rdStat.AutoSize = true;
            this.rdStat.Enabled = false;
            this.rdStat.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.rdStat.Location = new System.Drawing.Point(446, 185);
            this.rdStat.Name = "rdStat";
            this.rdStat.Size = new System.Drawing.Size(85, 17);
            this.rdStat.TabIndex = 15;
            this.rdStat.Text = "Bağlantı Yok";
            this.rdStat.UseVisualStyleBackColor = true;
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(13, 185);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(0, 13);
            this.lblMessage.TabIndex = 16;
            // 
            // cmbCities
            // 
            this.cmbCities.FormattingEnabled = true;
            this.cmbCities.Location = new System.Drawing.Point(126, 9);
            this.cmbCities.Name = "cmbCities";
            this.cmbCities.Size = new System.Drawing.Size(354, 21);
            this.cmbCities.TabIndex = 18;
            this.cmbCities.SelectedIndexChanged += new System.EventHandler(this.cmbCities_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 12);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(12, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "İl";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(543, 230);
            this.Controls.Add(this.cmbCities);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.rdStat);
            this.Controls.Add(this.chDate);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.pbStatus);
            this.Controls.Add(this.btnOpenFDDest);
            this.Controls.Add(this.btnOpenFDSource);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.txtDestinationPath);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtSourcePath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbDistricts);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tablet DB Oluşturma";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbDistricts;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.SaveFileDialog sfDialog;
        private System.Windows.Forms.OpenFileDialog ofDialog;
        private System.Windows.Forms.TextBox txtSourcePath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDestinationPath;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOpenFDSource;
        private System.Windows.Forms.Button btnOpenFDDest;
        private System.ComponentModel.BackgroundWorker bckWorker;
        private System.Windows.Forms.ProgressBar pbStatus;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chDate;
        private System.Windows.Forms.RadioButton rdStat;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.ComboBox cmbCities;
        private System.Windows.Forms.Label label6;
    }
}

