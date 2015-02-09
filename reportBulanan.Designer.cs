namespace Artesis
{
    partial class reportBulanan
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(reportBulanan));
            this.panel1 = new System.Windows.Forms.Panel();
            this.reportMonthBtn = new System.Windows.Forms.Button();
            this.CBTahun = new System.Windows.Forms.ComboBox();
            this.cbBulan = new System.Windows.Forms.ComboBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.reportMonthBtn);
            this.panel1.Controls.Add(this.CBTahun);
            this.panel1.Controls.Add(this.cbBulan);
            this.panel1.Location = new System.Drawing.Point(2, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(344, 97);
            this.panel1.TabIndex = 0;
            // 
            // reportMonthBtn
            // 
            this.reportMonthBtn.Location = new System.Drawing.Point(249, 32);
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
            this.CBTahun.Location = new System.Drawing.Point(163, 34);
            this.CBTahun.Name = "CBTahun";
            this.CBTahun.Size = new System.Drawing.Size(80, 21);
            this.CBTahun.TabIndex = 1;
            this.CBTahun.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CBTahun_KeyPress);
            // 
            // cbBulan
            // 
            this.cbBulan.FormattingEnabled = true;
            this.cbBulan.Location = new System.Drawing.Point(10, 34);
            this.cbBulan.Name = "cbBulan";
            this.cbBulan.Size = new System.Drawing.Size(147, 21);
            this.cbBulan.TabIndex = 0;
            this.cbBulan.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cbBulan_KeyPress);
            // 
            // reportBulanan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(351, 119);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "reportBulanan";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Laporan Bulanan";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cbBulan;
        private System.Windows.Forms.ComboBox CBTahun;
        private System.Windows.Forms.Button reportMonthBtn;
    }
}