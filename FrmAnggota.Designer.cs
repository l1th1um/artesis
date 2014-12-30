namespace Artesis
{
    partial class FrmAnggota
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAnggota));
            this.label1 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBlock = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtNumber = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.isUpdate = new System.Windows.Forms.Label();
            this.memberID = new System.Windows.Forms.Label();
            this.cbRT = new System.Windows.Forms.ComboBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSaveMember = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Nama";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(84, 16);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(188, 20);
            this.txtName.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(22, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "RT";
            // 
            // txtBlock
            // 
            this.txtBlock.Location = new System.Drawing.Point(84, 42);
            this.txtBlock.Name = "txtBlock";
            this.txtBlock.Size = new System.Drawing.Size(38, 20);
            this.txtBlock.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Blok";
            // 
            // txtNumber
            // 
            this.txtNumber.Location = new System.Drawing.Point(84, 68);
            this.txtNumber.Name = "txtNumber";
            this.txtNumber.Size = new System.Drawing.Size(38, 20);
            this.txtNumber.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(21, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "No";
            // 
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(84, 122);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(177, 20);
            this.txtPhone.TabIndex = 4;
            this.txtPhone.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPhone_KeyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 125);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Telepon";
            // 
            // isUpdate
            // 
            this.isUpdate.AutoSize = true;
            this.isUpdate.Location = new System.Drawing.Point(23, 152);
            this.isUpdate.Name = "isUpdate";
            this.isUpdate.Size = new System.Drawing.Size(13, 13);
            this.isUpdate.TabIndex = 12;
            this.isUpdate.Text = "0";
            this.isUpdate.Visible = false;
            // 
            // memberID
            // 
            this.memberID.AutoSize = true;
            this.memberID.Location = new System.Drawing.Point(23, 165);
            this.memberID.Name = "memberID";
            this.memberID.Size = new System.Drawing.Size(13, 13);
            this.memberID.TabIndex = 13;
            this.memberID.Text = "0";
            this.memberID.Visible = false;
            // 
            // cbRT
            // 
            this.cbRT.FormattingEnabled = true;
            this.cbRT.Items.AddRange(new object[] {
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07"});
            this.cbRT.Location = new System.Drawing.Point(84, 96);
            this.cbRT.Name = "cbRT";
            this.cbRT.Size = new System.Drawing.Size(57, 21);
            this.cbRT.TabIndex = 3;
            // 
            // btnClose
            // 
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(191, 152);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(81, 35);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Tutup";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSaveMember
            // 
            this.btnSaveMember.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSaveMember.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveMember.Image")));
            this.btnSaveMember.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSaveMember.Location = new System.Drawing.Point(98, 152);
            this.btnSaveMember.Name = "btnSaveMember";
            this.btnSaveMember.Size = new System.Drawing.Size(87, 35);
            this.btnSaveMember.TabIndex = 5;
            this.btnSaveMember.Text = "Simpan";
            this.btnSaveMember.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSaveMember.UseVisualStyleBackColor = true;
            this.btnSaveMember.Click += new System.EventHandler(this.btnSaveMember_Click);
            // 
            // FrmAnggota
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 197);
            this.Controls.Add(this.cbRT);
            this.Controls.Add(this.memberID);
            this.Controls.Add(this.isUpdate);
            this.Controls.Add(this.txtPhone);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.txtBlock);
            this.Controls.Add(this.txtNumber);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSaveMember);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmAnggota";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Tambah Anggota";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSaveMember;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        internal System.Windows.Forms.TextBox txtName;
        internal System.Windows.Forms.TextBox txtBlock;
        internal System.Windows.Forms.TextBox txtNumber;
        internal System.Windows.Forms.TextBox txtPhone;
        internal System.Windows.Forms.Label isUpdate;
        internal System.Windows.Forms.Label memberID;
        internal System.Windows.Forms.ComboBox cbRT;
    }
}