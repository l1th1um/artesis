namespace Artesis
{
    partial class reportHarian
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(reportHarian));
            this.panel1 = new System.Windows.Forms.Panel();
            this.awalTgl = new System.Windows.Forms.ComboBox();
            this.reportMonthBtn = new System.Windows.Forms.Button();
            this.awalThn = new System.Windows.Forms.ComboBox();
            this.awalBln = new System.Windows.Forms.ComboBox();
            this.akhirTgl = new System.Windows.Forms.ComboBox();
            this.akhirThn = new System.Windows.Forms.ComboBox();
            this.akhirBln = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.akhirTgl);
            this.panel1.Controls.Add(this.akhirThn);
            this.panel1.Controls.Add(this.akhirBln);
            this.panel1.Controls.Add(this.awalTgl);
            this.panel1.Controls.Add(this.reportMonthBtn);
            this.panel1.Controls.Add(this.awalThn);
            this.panel1.Controls.Add(this.awalBln);
            this.panel1.Location = new System.Drawing.Point(2, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(469, 116);
            this.panel1.TabIndex = 0;
            // 
            // awalTgl
            // 
            this.awalTgl.FormattingEnabled = true;
            this.awalTgl.Items.AddRange(new object[] {
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31"});
            this.awalTgl.Location = new System.Drawing.Point(85, 32);
            this.awalTgl.Name = "awalTgl";
            this.awalTgl.Size = new System.Drawing.Size(40, 21);
            this.awalTgl.TabIndex = 3;
            // 
            // reportMonthBtn
            // 
            this.reportMonthBtn.Location = new System.Drawing.Point(382, 71);
            this.reportMonthBtn.Name = "reportMonthBtn";
            this.reportMonthBtn.Size = new System.Drawing.Size(75, 23);
            this.reportMonthBtn.TabIndex = 2;
            this.reportMonthBtn.Text = "Submit";
            this.reportMonthBtn.UseVisualStyleBackColor = true;
            this.reportMonthBtn.Click += new System.EventHandler(this.reportMonthBtn_Click);
            // 
            // awalThn
            // 
            this.awalThn.FormattingEnabled = true;
            this.awalThn.Location = new System.Drawing.Point(284, 32);
            this.awalThn.Name = "awalThn";
            this.awalThn.Size = new System.Drawing.Size(80, 21);
            this.awalThn.TabIndex = 1;
            // 
            // awalBln
            // 
            this.awalBln.FormattingEnabled = true;
            this.awalBln.Location = new System.Drawing.Point(131, 32);
            this.awalBln.Name = "awalBln";
            this.awalBln.Size = new System.Drawing.Size(147, 21);
            this.awalBln.TabIndex = 0;
            // 
            // akhirTgl
            // 
            this.akhirTgl.FormattingEnabled = true;
            this.akhirTgl.Items.AddRange(new object[] {
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31"});
            this.akhirTgl.Location = new System.Drawing.Point(85, 73);
            this.akhirTgl.Name = "akhirTgl";
            this.akhirTgl.Size = new System.Drawing.Size(40, 21);
            this.akhirTgl.TabIndex = 6;
            // 
            // akhirThn
            // 
            this.akhirThn.FormattingEnabled = true;
            this.akhirThn.Location = new System.Drawing.Point(284, 73);
            this.akhirThn.Name = "akhirThn";
            this.akhirThn.Size = new System.Drawing.Size(80, 21);
            this.akhirThn.TabIndex = 5;
            // 
            // akhirBln
            // 
            this.akhirBln.FormattingEnabled = true;
            this.akhirBln.Location = new System.Drawing.Point(131, 73);
            this.akhirBln.Name = "akhirBln";
            this.akhirBln.Size = new System.Drawing.Size(147, 21);
            this.akhirBln.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Dari";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Sampai";
            // 
            // reportHarian
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(483, 141);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "reportHarian";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Laporan Harian";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox awalBln;
        private System.Windows.Forms.ComboBox awalThn;
        private System.Windows.Forms.Button reportMonthBtn;
        private System.Windows.Forms.ComboBox awalTgl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox akhirTgl;
        private System.Windows.Forms.ComboBox akhirThn;
        private System.Windows.Forms.ComboBox akhirBln;
    }
}