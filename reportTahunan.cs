using System;
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
    public partial class reportTahunan : Form
    {
        Dictionary<string, string> bulanDic = new Dictionary<string, string>();
        List<int> yearList = new List<int>();

        public reportTahunan()
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

            blnAwal.DataSource = new BindingSource(bulanDic, null);
            blnAwal.DisplayMember = "Value";
            blnAwal.ValueMember = "Key";

            blnAwal.SelectedIndex = 0;

            blnAkhir.DataSource = new BindingSource(bulanDic, null);
            blnAkhir.DisplayMember = "Value";
            blnAkhir.ValueMember = "Key";

            DateTime now = DateTime.Now;
            blnAkhir.SelectedIndex = now.Month - 1;          
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
            if (blnAkhir.SelectedIndex >= blnAwal.SelectedIndex)
            {
                SaveFileDialog sfd = new SaveFileDialog();

                string tahun = CBTahun.SelectedItem.ToString();

                string title = "Pembayaran";

                if (pemakaianRB.Checked)
                {
                    title = "Pemakaian";
                }
                    

                sfd.FileName = "Laporan " + title + " " + bulanDic[blnAwal.SelectedValue.ToString()] + "-" + bulanDic[blnAkhir.SelectedValue.ToString()] + " Tahun " + tahun + ".xls";
                sfd.Filter = "Excel files |*.xls";
                sfd.RestoreDirectory = true;

                try
                {
                    if (sfd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    {
                        return;
                    }
                    else
                    {
                        int row = 1;
                        Excel.Application oXL;
                        Excel._Workbook oWB;
                        Excel._Worksheet oSheet;
                        //Excel.Range oRng;

                        try
                        {
                            //Start Excel and get Application object.
                            oXL = new Excel.Application();
                            oXL.Visible = true;

                            //Get a new workbook.
                            oWB = (Excel._Workbook)(oXL.Workbooks.Add(Missing.Value));
                            oSheet = (Excel._Worksheet)oWB.ActiveSheet;

                            for (int x = blnAwal.SelectedIndex + 1; x <= blnAkhir.SelectedIndex + 1; x++)
                            {
                                String tanggal = string.Format("{0}-{1:00}-{2}", CBTahun.SelectedItem, x, "01");

                                oSheet.Cells[row, 1] = "Laporan "+ title + " Bulan " + bulanDic[string.Format("{0:00}", x)] + " " + tahun;
                                oSheet.Cells[row, 1].Font.Bold = true;
                                oSheet.Range[oSheet.Cells[row, 1], oSheet.Cells[row, 9]].Merge();
                                oSheet.Cells[row, 1].Style.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                                row += 2;
                                oSheet.Cells[row, 1].ColumnWidth = 4;
                                oSheet.Cells[row, 2].ColumnWidth = 28;
                                oSheet.Cells[row, 3].ColumnWidth = 10;
                                oSheet.Cells[row, 4].ColumnWidth = 22;
                                oSheet.Cells[row, 5].ColumnWidth = 8;
                                oSheet.Cells[row, 6].ColumnWidth = 18;
                                oSheet.Cells[row, 7].ColumnWidth = 12;
                                oSheet.Cells[row, 8].ColumnWidth = 14;
                                oSheet.Cells[row, 9].ColumnWidth = 13;

                                //Add table headers going cell by cell.
                                oSheet.Cells[row, 1] = "No.";
                                oSheet.Cells[row, 2] = "No. Kuitansi";
                                oSheet.Cells[row, 3] = "No. Pelanggan";
                                oSheet.Cells[row, 4] = "Nama";
                                oSheet.Cells[row, 5] = "RT";
                                oSheet.Cells[row, 6] = "Tanggal Pembayaran";
                                oSheet.Cells[row, 7] = "Pemakaian";
                                oSheet.Cells[row, 8] = "Jumlah";
                                oSheet.Cells[row, 9] = "Keterangan";

                                oSheet.Range[oSheet.Cells[row, 1], oSheet.Cells[row, 9]].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                                oSheet.Range[oSheet.Cells[row, 1], oSheet.Cells[row, 9]].Style.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                oSheet.Range[oSheet.Cells[row, 1], oSheet.Cells[row, 9]].Style.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                                oSheet.Range[oSheet.Cells[row, 1], oSheet.Cells[row, 9]].WrapText = true;

                                oSheet.Cells[row, 1].EntireRow.Font.Bold = true;
                                oSheet.Cells[row, 1].RowHeight = 18;

                                row++;

                                int start_row = row;

                                using (SQLiteConnection conn = new SQLiteConnection(@"Data Source =" + Program.path_db))
                                {
                                    conn.Open();

                                    String query = "";
                                    if (pemakaianRB.Checked)
                                    {
                                        query = "SELECT no_invoice, invoice_suffix, member_id, urut_rt, nama, rt, tgl_bayar, jumlah, awal, akhir, bayar ";
                                        query += "FROM meteran m ";
                                        query += "LEFT JOIN pembayaran p ON m.id = p.meteran_id ";
                                        query += "JOIN members u ON u.id = m.member_id ";
                                        query += "WHERE m.tanggal = '" + tanggal + "' ";
                                        query += "AND awal IS NOT NULL AND akhir IS NOT NULL ";
                                        query += "ORDER BY rt, nama";
                                    }
                                    else
                                    {
                                        query = "SELECT no_invoice, invoice_suffix, member_id, urut_rt, nama, rt, tgl_bayar, jumlah, awal, akhir, bayar ";
                                        query += "FROM pembayaran p ";
                                        query += "JOIN meteran m ON m.id = p.meteran_id ";
                                        query += "JOIN members u ON u.id = m.member_id ";
                                        query += "WHERE strftime('%m', tgl_bayar) = '" + string.Format("{0:00}", x) +"' ";
                                        query += "AND strftime('%Y', tgl_bayar)  = '" + tahun + "' ";
                                        query += "ORDER BY rt, urut_rt";
                                    }
                                    

                                    System.Diagnostics.Debug.WriteLine(query);

                                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                                    {
                                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                                        {
                                            int no = 1;
                                                while (reader.Read())
                                                {
                                                    double pemakaian = reader.GetDouble(9) - reader.GetDouble(8);

                                                    string tgl_pembayaran = "";
                                                    string keterangan = "";
                                                    Int32 jumlah = 0;

                                                    if (reader.GetInt32(10) == 1)
                                                    {
                                                        keterangan = "Lunas";
                                                        tgl_pembayaran = reader.GetDateTime(6).ToString();
                                                        jumlah = reader.GetInt32(7);
                                                    }
                                                    else
                                                    {
                                                        keterangan = "Belum Dibayar";
                                                        oSheet.Range[oSheet.Cells[row, 1], oSheet.Cells[row, 9]].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Orange);
                                                    }

                                                    oSheet.Cells[row, 1] = no;
                                                    oSheet.Cells[row, 2] = string.Format("{0:000}", reader.GetValue(0)) + reader.GetValue(1).ToString();
                                                    oSheet.Cells[row, 3] = "'" + reader.GetValue(3).ToString() + "." + reader.GetValue(5).ToString(); //No Urut
                                                    oSheet.Cells[row, 4] = reader.GetValue(4).ToString(); //Nama
                                                    oSheet.Cells[row, 5] = "'" + reader.GetValue(5).ToString() + " /09";
                                                    oSheet.Cells[row, 6] = tgl_pembayaran;
                                                    oSheet.Cells[row, 7] = pemakaian.ToString(); //Pemakaian                                        
                                                    oSheet.Cells[row, 8] = jumlah;
                                                    oSheet.Cells[row, 9] = keterangan;

                                                    no++;
                                                    row++;
                                                }
                                        }
                                    }
                                    conn.Close();
                                }

                                oSheet.Cells[row, 1] = "Jumlah";
                                oSheet.Cells[row, 7] = "=sum(G" + start_row + ":G" + (row - 1) + ")";
                                oSheet.Cells[row, 8] = "=sum(H" + start_row + ":H" + (row - 1) + ")";
                                oSheet.Range[oSheet.Cells[row, 1], oSheet.Cells[row, 6]].Merge();
                                oSheet.Cells[row, 1].Style.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                oSheet.Cells[row, 1].Font.Bold = true;
                                oSheet.Cells[row, 7].Font.Bold = true;
                                oSheet.Cells[row, 8].Font.Bold = true;

                                oSheet.get_Range("D" + start_row + "", "D" + row).Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                                oSheet.get_Range("G" + start_row + "", "H" + row).Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                                oSheet.get_Range("F" + start_row + "", "F" + row).NumberFormat = "dd/mm/yyyy hh:mm";
                                //oSheet.get_Range("G4", "G" + init_row).NumberFormat = "#,###,###";
                                oSheet.get_Range("H" + start_row + "", "H" + row).NumberFormat = "#,###,###";
                                oSheet.get_Range("A" + start_row + "", "I" + row).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                                row += 4;
                            }


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
            else
            {
                MessageBox.Show("Cek Pilihan Bulan");
            }
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
