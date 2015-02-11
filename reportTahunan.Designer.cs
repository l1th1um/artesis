namespace Artesis
{
    partial class reportTahunan
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(reportTahunan));
            this.panel1 = new System.Windows.Forms.Panel();
            this.blnAkhir = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.reportMonthBtn = new System.Windows.Forms.Button();
            this.CBTahun = new System.Windows.Forms.ComboBox();
            this.blnAwal = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panelGrup = new System.Windows.Forms.Panel();
            this.pemakaianRB = new System.Windows.Forms.RadioButton();
            this.pembayaranRB = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.panelGrup.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panelGrup);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.blnAkhir);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.reportMonthBtn);
            this.panel1.Controls.Add(this.CBTahun);
            this.panel1.Controls.Add(this.blnAwal);
            this.panel1.Location = new System.Drawing.Point(2, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(278, 197);
            this.panel1.TabIndex = 0;
            // 
            // blnAkhir
            // 
            this.blnAkhir.FormattingEnabled = true;
            this.blnAkhir.Location = new System.Drawing.Point(87, 39);
            this.blnAkhir.Name = "blnAkhir";
            this.blnAkhir.Size = new System.Drawing.Size(161, 21);
            this.blnAkhir.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Sampai";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Dari";
            // 
            // reportMonthBtn
            // 
            this.reportMonthBtn.Location = new System.Drawing.Point(112, 157);
            this.reportMonthBtn.Name = "reportMonthBtn";
            this.reportMonthBtn.Size = new System.Drawing.Size(75, 23);
            this.reportMonthBtn.TabIndex = 2;
            this.reportMonthBtn.Text = "Submit";
            this.reportMonthBtn.UseVisualStyleBackColor = true;
            this.reportMonthBtn.Click += new System.EventHandler(this.reportMonthBtn_Click);
            // 
            // CBTahun
            // 
            this.CBTahun.FormattingEnabled = true;
            this.CBTahun.Location = new System.Drawing.Point(87, 66);
            this.CBTahun.Name = "CBTahun";
            this.CBTahun.Size = new System.Drawing.Size(80, 21);
            this.CBTahun.TabIndex = 1;
            this.CBTahun.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CBTahun_KeyPress);
            // 
            // blnAwal
            // 
            this.blnAwal.FormattingEnabled = true;
            this.blnAwal.Location = new System.Drawing.Point(87, 10);
            this.blnAwal.Name = "blnAwal";
            this.blnAwal.Size = new System.Drawing.Size(161, 21);
            this.blnAwal.TabIndex = 0;
            this.blnAwal.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cbBulan_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 99);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Grup";
            // 
            // panelGrup
            // 
            this.panelGrup.Controls.Add(this.pemakaianRB);
            this.panelGrup.Controls.Add(this.pembayaranRB);
            this.panelGrup.Location = new System.Drawing.Point(87, 99);
            this.panelGrup.Name = "panelGrup";
            this.panelGrup.Size = new System.Drawing.Size(161, 52);
            this.panelGrup.TabIndex = 7;
            // 
            // pemakaianRB
            // 
            this.pemakaianRB.AutoSize = true;
            this.pemakaianRB.Location = new System.Drawing.Point(3, 26);
            this.pemakaianRB.Name = "pemakaianRB";
            this.pemakaianRB.Size = new System.Drawing.Size(78, 17);
            this.pemakaianRB.TabIndex = 12;
            this.pemakaianRB.Text = "Pemakaian";
            this.pemakaianRB.UseVisualStyleBackColor = true;
            // 
            // pembayaranRB
            // 
            this.pembayaranRB.AutoSize = true;
            this.pembayaranRB.Checked = true;
            this.pembayaranRB.Location = new System.Drawing.Point(3, 3);
            this.pembayaranRB.Name = "pembayaranRB";
            this.pembayaranRB.Size = new System.Drawing.Size(84, 17);
            this.pembayaranRB.TabIndex = 11;
            this.pembayaranRB.TabStop = true;
            this.pembayaranRB.Text = "Pembayaran";
            this.pembayaranRB.UseVisualStyleBackColor = true;
            // 
            // reportTahunan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(287, 220);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "reportTahunan";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Laporan Tahunan";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panelGrup.ResumeLayout(false);
            this.panelGrup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox blnAwal;
        private System.Windows.Forms.ComboBox CBTahun;
        private System.Windows.Forms.Button reportMonthBtn;
        private System.Windows.Forms.ComboBox blnAkhir;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelGrup;
        private System.Windows.Forms.RadioButton pemakaianRB;
        private System.Windows.Forms.RadioButton pembayaranRB;
        private System.Windows.Forms.Label label3;
    }
}