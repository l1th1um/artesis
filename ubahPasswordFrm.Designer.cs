namespace Artesis
{
    partial class ubahPasswordFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ubahPasswordFrm));
            this.ubahBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.oldPassword = new System.Windows.Forms.TextBox();
            this.newPassword = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.newPassword2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ubahBtn
            // 
            this.ubahBtn.Location = new System.Drawing.Point(164, 116);
            this.ubahBtn.Name = "ubahBtn";
            this.ubahBtn.Size = new System.Drawing.Size(75, 23);
            this.ubahBtn.TabIndex = 0;
            this.ubahBtn.Text = "Submit";
            this.ubahBtn.UseVisualStyleBackColor = true;
            this.ubahBtn.Click += new System.EventHandler(this.ubahBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.Location = new System.Drawing.Point(259, 116);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 1;
            this.cancelBtn.Text = "Tutup";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Password Lama";
            // 
            // oldPassword
            // 
            this.oldPassword.Location = new System.Drawing.Point(164, 29);
            this.oldPassword.Name = "oldPassword";
            this.oldPassword.Size = new System.Drawing.Size(170, 20);
            this.oldPassword.TabIndex = 3;
            this.oldPassword.UseSystemPasswordChar = true;
            // 
            // newPassword
            // 
            this.newPassword.Location = new System.Drawing.Point(164, 55);
            this.newPassword.Name = "newPassword";
            this.newPassword.Size = new System.Drawing.Size(170, 20);
            this.newPassword.TabIndex = 5;
            this.newPassword.UseSystemPasswordChar = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Password Baru";
            // 
            // newPassword2
            // 
            this.newPassword2.Location = new System.Drawing.Point(164, 81);
            this.newPassword2.Name = "newPassword2";
            this.newPassword2.Size = new System.Drawing.Size(170, 20);
            this.newPassword2.TabIndex = 7;
            this.newPassword2.UseSystemPasswordChar = true;
            this.newPassword2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.newPassword2_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(129, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Konfirmasi Password Baru";
            // 
            // ubahPasswordFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(362, 148);
            this.Controls.Add(this.newPassword2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.newPassword);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.oldPassword);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.ubahBtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ubahPasswordFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ubah Password";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ubahBtn;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox oldPassword;
        private System.Windows.Forms.TextBox newPassword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox newPassword2;
        private System.Windows.Forms.Label label3;
    }
}