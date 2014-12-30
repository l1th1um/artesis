using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;

namespace Artesis
{
    public partial class tarifFrm : Form
    {
        //using (SQLiteConnection conn = new SQLiteConnection(@"Data Source =" + Program.path_db)) conn = new using (SQLiteConnection conn = new SQLiteConnection(@"Data Source =" + Program.path_db))(@"Data Source =" + Program.path_db);

        public tarifFrm()
        {
            InitializeComponent();
            //this.ActiveControl =

            using (SQLiteConnection conn = new SQLiteConnection(@"Data Source =" + Program.path_db))
            {
                conn.Open();
                string command = "SELECT * FROM tarif";
                using (SQLiteCommand cmd = new SQLiteCommand(command, conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            bebanTetapTxt.Text = reader.GetValue(0).ToString();
                            dendaTxt.Text = reader.GetValue(1).ToString();
                            tarif1Txt.Text = reader.GetValue(2).ToString();
                            tarif2Txt.Text = reader.GetValue(3).ToString();
                            tarif3Txt.Text = reader.GetValue(4).ToString();
                            tarif4Txt.Text = reader.GetValue(5).ToString();
                            tarif5Txt.Text = reader.GetValue(6).ToString();
                        }
                    }
                }
                conn.Close();
            }
            /*
            string command = "SELECT * FROM tarif";

            SQLiteCommand query = new SQLiteCommand(command, conn);

            SQLiteDataReader reader = query.ExecuteReader();

            if (reader.Read())
            {
                bebanTetapTxt.Text = reader.GetValue(0).ToString();
                dendaTxt.Text = reader.GetValue(1).ToString();
                tarif1Txt.Text = reader.GetValue(2).ToString();
                tarif2Txt.Text = reader.GetValue(3).ToString();
                tarif3Txt.Text = reader.GetValue(4).ToString();
                tarif4Txt.Text = reader.GetValue(5).ToString();
                tarif5Txt.Text = reader.GetValue(6).ToString();
            }
            conn.Close();*/
        }

        private void exitBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CheckKey(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar)
                && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void bebanTetapTxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.CheckKey(sender, e);
        }

        private void tarif1Txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.CheckKey(sender, e);
        }

        private void tarif2Txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.CheckKey(sender, e);
        }

        private void tarif3Txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.CheckKey(sender, e);
        }

        private void tarif4Txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.CheckKey(sender, e);
        }

        private void tarif5Txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                this.button1_Click(sender, e);
            }
            else
            {
                this.CheckKey(sender, e);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
        	//conn.Open();

            using (SQLiteConnection conn = new SQLiteConnection(@"Data Source =" + Program.path_db))
            {
                conn.Open();
                string command = "UPDATE tarif SET beban_tetap = " + bebanTetapTxt.Text + ", ";
                command += "denda = " + dendaTxt.Text + ", ";
                command += "tarif1 = " + tarif1Txt.Text + ", tarif2 = " + tarif2Txt.Text + ", ";
                command += "tarif3 = " + tarif3Txt.Text + ", tarif4 = " + tarif4Txt.Text + ", ";
                command += "tarif5 = " + tarif5Txt.Text;

                using (SQLiteCommand cmd = new SQLiteCommand(command, conn))
                {
                    int rowAffected = cmd.ExecuteNonQuery();

                    if (rowAffected > 0)
                    {
                        MessageBox.Show("Pengaturan Tarif Telah Diubah");
                    }
                    else
                    {
                        MessageBox.Show("Error ! Pengaturan Tarif Tidak Dapat Dilakukan");
                    }
                }

                conn.Close();
            }
        	/*
            string command = "UPDATE tarif SET beban_tetap = " + bebanTetapTxt.Text + ",";
        	command += "denda = " + dendaTxt.Text + ",";
            command += "tarif1 = " + tarif1Txt.Text + ", tarif2 = " + tarif2Txt.Text + ",";
        	command += "tarif3 = " + tarif3Txt.Text + ", tarif4 = " + tarif4Txt.Text + ",";
        	command += "tarif5 = " + tarif5Txt.Text;
            
            Console.Out.WriteLine(command);

            SQLiteCommand query = new SQLiteCommand(command, conn);

            int rowAffected = query.ExecuteNonQuery();
            
            conn.Close();

            if (rowAffected > 0)
            {
                MessageBox.Show("Pengaturan Tarif Telah Diubah");
            }
            else
            {
                MessageBox.Show("Error ! Pengaturan Tarif Tidak Dapat Dilakukan");
            }*/

            this.DialogResult = DialogResult.OK;            
        }

        private void dendaTxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.CheckKey(sender, e);
        }
    }
}
