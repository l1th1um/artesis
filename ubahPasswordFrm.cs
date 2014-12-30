using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Data.SQLite;

namespace Artesis
{
    public partial class ubahPasswordFrm : Form
    {
        //SQLiteConnection conn = new SQLiteConnection(@"Data Source ="+ Program.path_db);

        public ubahPasswordFrm()
        {
            InitializeComponent();

            this.ActiveControl = oldPassword;

        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ubahBtn_Click(object sender, EventArgs e)
        {
            String old_password = oldPassword.Text;
            String new_password = newPassword.Text;
            String new_password2 = newPassword2.Text;

            MD5 md5Hash = MD5.Create();

            if (new_password.Length < 5)
            {
                MessageBox.Show("Password minimal 5 karakter");                
            }
            else if (new_password != new_password2)
            {
                MessageBox.Show("Konfirmasi Password tidak sama");                
            }
            else
            {
                String password = GetMd5Hash(md5Hash, old_password + Program.salt);

                using (SQLiteConnection conn = new SQLiteConnection(@"Data Source =" + Program.path_db))
                {
                    conn.Open();
                    string command = "SELECT * FROM users WHERE password = '" + password + "'";

                    SQLiteCommand query = new SQLiteCommand(command, conn);

                    SQLiteDataReader reader = query.ExecuteReader();

                    if (reader.Read())
                    {
                        String password2 = GetMd5Hash(md5Hash, new_password + Program.salt);

                        string command2 = "UPDATE users SET password = '" + password2 + "'";

                        SQLiteCommand query2 = new SQLiteCommand(command2, conn);

                        int rowAffected = query2.ExecuteNonQuery();

                        if (rowAffected > 0)
                        {
                            MessageBox.Show("Password Telah Diubah");
                        }
                        else
                        {
                            MessageBox.Show("Error ! Perubahan Password Tidak Dapat Dilakukan");
                        }

                        
                        this.DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        oldPassword.Text = "";
                        newPassword.Text = "";
                        newPassword2.Text = "";                        

                        oldPassword.Focus();

                        MessageBox.Show("Password Salah");
                    }
                    conn.Close();
                }
                
            }
            
        }

        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash. 
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes 
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data  
            // and format each one as a hexadecimal string. 
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string. 
            return sBuilder.ToString();
        }

        // Verify a hash against a string. 
        static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            // Hash the input. 
            string hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void newPassword2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                this.ubahBtn_Click(sender, e);
            }
        }

    }
}
