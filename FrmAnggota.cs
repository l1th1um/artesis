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
    public partial class FrmAnggota : Form
    {
        //SQLiteConnection conn = new SQLiteConnection(@"Data Source =" + Program.path_db);
        //SQLiteDataAdapter sda;

        public FrmAnggota()
        {
            InitializeComponent();
            cbRT.SelectedIndex = 0;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSaveMember_Click(object sender, EventArgs e)
        {
            if (isUpdate.Text == "0")
            {
                this.saveMember();
                MessageBox.Show("Data Anggota Telah ditambahkan");
            }
            else
            {
                this.updateMember();
                MessageBox.Show("Data Anggota Telah diperbaharui");
            }
            this.Close();
        }

        private void saveMember()
        {
            int urut_rt = this.maxUrutRT(cbRT.SelectedItem.ToString()) + 1;

            using (SQLiteConnection conn = new SQLiteConnection(@"Data Source =" + Program.path_db))
            {
                conn.Open();
                
                String insertCmd = "INSERT INTO members(urut_rt, nama, telp, blok, no_rumah, rt, active, created_at) ";
                insertCmd += "VALUES ("+ urut_rt +", '" + txtName.Text + "', '" + txtPhone.Text + "', '" + txtBlock.Text + "',";
                insertCmd += "'" + txtNumber.Text + "', '" + cbRT.SelectedItem.ToString() + "', 1, datetime('now'))";

                using (SQLiteCommand cmd = new SQLiteCommand(insertCmd, conn))
                {
                    int rowAffected = cmd.ExecuteNonQuery();
                }

                conn.Close();
            }          
        }
        
        private void updateMember()
        {
            using (SQLiteConnection conn = new SQLiteConnection(@"Data Source =" + Program.path_db))
            {
                conn.Open();
                String command = "UPDATE members SET nama = '" + txtName.Text +"', ";
                command += "telp = '" + txtPhone.Text +"', blok = '" + txtBlock.Text + "',";
                command += "no_rumah = '"+ txtNumber.Text +"', rt = '"+ cbRT.SelectedItem.ToString() +"', ";
                command += "updated_at = datetime('now') WHERE id = " + memberID.Text;
                using (SQLiteCommand cmd = new SQLiteCommand(command, conn))
                {
                    cmd.ExecuteNonQuery();
                }

                conn.Close();
            }
        }


        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                this.DialogResult = DialogResult.OK;
                this.btnSaveMember_Click(sender, e);
            }
        }

        public int maxUrutRT(String rt)
        {
            using (SQLiteConnection conn = new SQLiteConnection(@"Data Source =" + Program.path_db))
            {
                conn.Open();
                string command = "SELECT MAX(urut_rt) as maks_urut FROM members WHERE rt = '" + rt + "'";

                Console.WriteLine(command);

                using (SQLiteCommand cmd = new SQLiteCommand(command, conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (reader["maks_urut"] != DBNull.Value)
                            {
                                return reader.GetInt32(0);
                            }
                            else
                            {
                                return 0;
                            }                            
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
                conn.Close();
            }            
        }


       
    }
}
