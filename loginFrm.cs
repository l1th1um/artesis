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
    public partial class loginFrm : Form
    {
        //SQLiteConnection conn = new SQLiteConnection(@"Data Source =" + Program.path_db);

        public loginFrm()
        {
            InitializeComponent();

            //MessageBox.Show(Program.path_db);
            this.ActiveControl = usernameTxt;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {

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

        private void loginBtn_Click(object sender, EventArgs e)
        {
            //conn.Open();
            String username = usernameTxt.Text;
            String password = passwordTxt.Text;

            MD5 md5Hash = MD5.Create();
            
            password = GetMd5Hash(md5Hash, password + Program.salt);

            using (SQLiteConnection conn = new SQLiteConnection(@"Data Source =" + Program.path_db))
            {
                conn.Open();
                string command = "SELECT * FROM users WHERE username = '" + username + "' AND password = '" + password + "'";

                using (SQLiteCommand cmd = new SQLiteCommand(command, conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            this.DialogResult = DialogResult.OK;
                        }
                        else
                        {
                            usernameTxt.Text = "";
                            passwordTxt.Text = "";
                            
                            usernameTxt.Focus();

                            MessageBox.Show("Username atau Password Salah");
                        }
                    }
                }
                conn.Close();
            }
            

            
        }

        private void passwordTxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                this.loginBtn_Click(sender, e);
            }
        }
    }
}
