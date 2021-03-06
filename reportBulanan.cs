﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace Artesis
{
    public partial class reportBulanan : Form
    {
        Dictionary<string, string> bulanDic = new Dictionary<string, string>();
        List<int> yearList = new List<int>();

        public reportBulanan()
        {
            InitializeComponent();
            this.getMonth();
            this.getYear();

        }

        private void getMonth()
        {
            bulanDic.Add("01", "Januari");
            bulanDic.Add("02", "Februari");
            bulanDic.Add("03", "Maret");
            bulanDic.Add("04", "April");
            bulanDic.Add("05", "Mei");
            bulanDic.Add("06", "Juni");
            bulanDic.Add("07", "Juli");
            bulanDic.Add("08", "Agustus");
            bulanDic.Add("09", "September");
            bulanDic.Add("10", "Oktober");
            bulanDic.Add("11", "November");
            bulanDic.Add("12", "Desember");

            cbBulan.DataSource = new BindingSource(bulanDic, null);
            cbBulan.DisplayMember = "Value";
            cbBulan.ValueMember = "Key";

            DateTime now = DateTime.Now;
            cbBulan.SelectedIndex = now.Month - 1;          
        }

        private void getYear()
        {
            using (SQLiteConnection conn = new SQLiteConnection(@"Data Source =" + Program.path_db))
            {
                conn.Open();

                SQLiteCommand cmd = new SQLiteCommand("SELECT DISTINCT(strftime('%Y', tanggal)) as tahun FROM meteran", conn);
                SQLiteDataReader reader = cmd.ExecuteReader();

                DateTime now = DateTime.Now;

                if (reader.HasRows)
                {
                    int nextYear = 0;

                    while (reader.Read())
                    {
                        CBTahun.Items.Add(reader.GetString(0));
                        nextYear = Int32.Parse(reader.GetValue(0).ToString()) + 1;
                    }

                    CBTahun.Items.Add(nextYear.ToString());
                }
                else
                {
                    CBTahun.Items.Add(now.Year.ToString());                    
                }

                CBTahun.SelectedItem = now.Year.ToString();

                conn.Close();
            }
        }

        private void reportMonthBtn_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();

            String bulan = bulanDic[cbBulan.SelectedValue.ToString()];
            String tahun = CBTahun.SelectedItem.ToString();

            sfd.FileName = "Laporan Bulan " + bulan + " " + tahun + ".xlsx";
            sfd.Filter = "Excel files |*.xlsx";
            sfd.RestoreDirectory = true;

            try
            {
                if (sfd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }
                else
                {
                    Excel.Application oXL;
                    Excel._Workbook oWB;
                    Excel._Worksheet oSheet;
                    //Excel.Range oRng;

                    Form1 frmUtama = new Form1();

                    try
                    {
                        //Start Excel and get Application object.
                        oXL = new Excel.Application();
                        oXL.Visible = true;

                        //Get a new workbook.
                        oWB = (Excel._Workbook)(oXL.Workbooks.Add(Missing.Value));
                        oSheet = (Excel._Worksheet)oWB.ActiveSheet;

                        oSheet.Cells[1, 1] = "Laporan Bulan " + bulan + " " + tahun;
                        oSheet.Cells[1, 1].Font.Bold = true;
                        oSheet.Range[oSheet.Cells[1, 1], oSheet.Cells[1, 9]].Merge();
                        oSheet.Cells[1, 1].Style.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                        
                        int init_row = 3;
                        oSheet.Cells[1, 1].ColumnWidth = 4;
                        oSheet.Cells[1, 2].ColumnWidth = 28;
                        oSheet.Cells[1, 3].ColumnWidth = 10;
                        oSheet.Cells[1, 4].ColumnWidth = 22;
                        oSheet.Cells[1, 5].ColumnWidth = 8;
                        oSheet.Cells[1, 6].ColumnWidth = 22;
                        oSheet.Cells[1, 7].ColumnWidth = 12;
                        oSheet.Cells[1, 8].ColumnWidth = 14;
                        oSheet.Cells[1, 9].ColumnWidth = 13;

                        //Add table headers going cell by cell.
                        oSheet.Cells[init_row, 1] = "No.";
                        oSheet.Cells[init_row, 2] = "No. Kuitansi";
                        oSheet.Cells[init_row, 3] = "No. Pelanggan";
                        oSheet.Cells[init_row, 4] = "Nama";
                        oSheet.Cells[init_row, 5] = "RT";
                        oSheet.Cells[init_row, 6] = "Tanggal Pembayaran";
                        oSheet.Cells[init_row, 7] = "Pemakaian";                        
                        oSheet.Cells[init_row, 8] = "Lunas";
                        oSheet.Cells[init_row, 9] = "Belum Lunas";

                        oSheet.Range[oSheet.Cells[init_row, 1], oSheet.Cells[init_row, 9]].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                        oSheet.Range[oSheet.Cells[init_row, 1], oSheet.Cells[init_row, 9]].Style.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                        oSheet.Range[oSheet.Cells[init_row, 1], oSheet.Cells[init_row, 9]].Style.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                        oSheet.Range[oSheet.Cells[init_row, 1], oSheet.Cells[init_row, 9]].WrapText = true;

                        oSheet.Cells[init_row, 1].EntireRow.Font.Bold = true;
                        oSheet.Cells[init_row, 1].RowHeight = 18;

                        init_row++;

                        using (SQLiteConnection conn = new SQLiteConnection(@"Data Source =" + Program.path_db))
                        {
                            conn.Open();

                            string periode = String.Format("{0}-{1}-{2}", tahun, cbBulan.SelectedValue.ToString(), "01");

                            String query = "SELECT no_invoice, invoice_suffix, member_id, urut_rt, nama, rt, tgl_bayar, jumlah, awal, akhir, bayar ";
                            query += "FROM meteran m ";
                            query += "LEFT JOIN pembayaran p ON m.id = p.meteran_id ";
                            query += "JOIN members u ON u.id = m.member_id ";
                            query += "WHERE m.tanggal = '" + periode + "' ";
                            query += "AND awal IS NOT NULL AND akhir IS NOT NULL ";
                            query += "ORDER BY rt, bayar DESC, urut_rt,  nama";
                            
                            //System.Diagnostics.Debug.WriteLine(query);

                            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                            {
                                using (SQLiteDataReader reader = cmd.ExecuteReader())
                                {
                                    int no = 1;
                                    while (reader.Read())
                                    {
                                        double pemakaian = reader.GetDouble(9) - reader.GetDouble(8);

                                        string tgl_pembayaran = "";
                                        //string keterangan = "";
                                        Int32 lunas = 0;
                                        Int32 blm_lunas = 0;

                                        if (reader.GetInt32(10) == 1)
                                        {
                                            tgl_pembayaran = String.Format(new System.Globalization.CultureInfo("id-ID"), "{0:dd MMMM yyyy HH:mm}", reader.GetDateTime(6));
                                            lunas = reader.GetInt32(7);
                                        }
                                        else
                                        {
                                            blm_lunas = Convert.ToInt32(frmUtama.biayaPemakaian(Convert.ToDouble(pemakaian)));
                                            oSheet.Range[oSheet.Cells[init_row, 1], oSheet.Cells[init_row, 9]].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Orange);
                                        }

                                        oSheet.Cells[init_row, 1] = no;
                                        oSheet.Cells[init_row, 2] = string.Format("{0:000}", reader.GetValue(0)) + reader.GetValue(1).ToString();
                                        oSheet.Cells[init_row, 3] = "'" + string.Format("{0:00}", reader.GetValue(3)) + "." + reader.GetValue(5).ToString(); //No Urut
                                        oSheet.Cells[init_row, 4] = reader.GetValue(4).ToString() ; //Nama
                                        oSheet.Cells[init_row, 5] = "'" + reader.GetValue(5).ToString() + "/09";
                                        oSheet.Cells[init_row, 6] = tgl_pembayaran;
                                        oSheet.Cells[init_row, 7] = pemakaian.ToString(); //Pemakaian                                        
                                        oSheet.Cells[init_row, 8] = lunas;
                                        oSheet.Cells[init_row, 9] = blm_lunas;

                                        no++;
                                        init_row++;
                                    }
                                }
                            }
                            conn.Close();
                        }

                        oSheet.Cells[init_row, 1] = "Jumlah";
                        oSheet.Cells[init_row, 7] = "=sum(G4:G" + (init_row - 1) + ")";
                        oSheet.Cells[init_row, 8] = "=sum(H4:H" + (init_row - 1) + ")";
                        oSheet.Cells[init_row, 9] = "=sum(I4:I" + (init_row - 1) + ")";

                        oSheet.Range[oSheet.Cells[init_row, 1], oSheet.Cells[init_row, 6]].Merge();
                        oSheet.Cells[init_row, 1].Style.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                        oSheet.Cells[init_row, 1].Font.Bold = true;
                        oSheet.Cells[init_row, 7].Font.Bold = true;
                        oSheet.Cells[init_row, 8].Font.Bold = true;
                        oSheet.Cells[init_row, 9].Font.Bold = true;
                         
                        oSheet.get_Range("D4", "D" + init_row).Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                        oSheet.get_Range("G4", "I" + init_row).Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                        oSheet.get_Range("F4", "F" + init_row).NumberFormat = "dd/mm/yyyy hh:mm";
                        //oSheet.get_Range("G4", "G" + init_row).NumberFormat = "#,###,###";
                        oSheet.get_Range("H4", "I" + init_row).NumberFormat = "#,###,###";
                        oSheet.get_Range("A3", "I" + init_row).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                        
                        //Make sure Excel is visible and give the user control
                        //of Microsoft Excel's lifetime.
                        oXL.Visible = true;
                        oXL.UserControl = true;

                        String filename = sfd.FileName;
                        oWB.SaveAs(filename);
                        //oWB.Close();
                        
                        MessageBox.Show("Laporan Telah Disimpan");
                    }
                    catch (Exception theException)
                    {
                        String errorMessage;
                        errorMessage = "Error: ";
                        errorMessage = String.Concat(errorMessage, theException.Message);
                        errorMessage = String.Concat(errorMessage, " Line: ");
                        errorMessage = String.Concat(errorMessage, theException.Source);

                        MessageBox.Show(errorMessage, "Error");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            }
            this.Close();
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        private void cbBulan_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                this.reportMonthBtn_Click(sender, e);
            }
        }
        
        private void CBTahun_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                this.reportMonthBtn_Click(sender, e);
            }
        }

    }
}
