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
    public partial class reportHarian : Form
    {
        Dictionary<string, string> bulanDic = new Dictionary<string, string>();
        List<int> yearList = new List<int>();
        DateTime now = DateTime.Now;

        public reportHarian()
        {
            InitializeComponent();
            this.getMonth();
            this.getYear();
            awalTgl.SelectedIndex = now.Day - 1;
            akhirTgl.SelectedIndex = now.Day - 1;
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

            awalBln.DataSource = new BindingSource(bulanDic, null);
            awalBln.DisplayMember = "Value";
            awalBln.ValueMember = "Key";

            //DateTime now = DateTime.Now;
            awalBln.SelectedIndex = now.Month - 1;

            akhirBln.DataSource = new BindingSource(bulanDic, null);
            akhirBln.DisplayMember = "Value";
            akhirBln.ValueMember = "Key";

            //DateTime now = DateTime.Now;
            akhirBln.SelectedIndex = now.Month - 1;
        }

        private void getYear()
        {
            using (SQLiteConnection conn = new SQLiteConnection(@"Data Source =" + Program.path_db))
            {
                conn.Open();

                SQLiteCommand cmd = new SQLiteCommand("SELECT DISTINCT(strftime('%Y', tanggal)) as tahun FROM meteran", conn);
                SQLiteDataReader reader = cmd.ExecuteReader();

                //DateTime now = DateTime.Now;

                if (reader.HasRows)
                {
                    int nextYear = 0;

                    while (reader.Read())
                    {
                        awalThn.Items.Add(reader.GetString(0));
                        akhirThn.Items.Add(reader.GetString(0));

                        nextYear = Int32.Parse(reader.GetValue(0).ToString()) + 1;
                    }

                    awalThn.Items.Add(nextYear.ToString());
                    akhirThn.Items.Add(nextYear.ToString());
                }
                else
                {
                    awalThn.Items.Add(now.Year.ToString());
                    akhirThn.Items.Add(now.Year.ToString());
                }

                awalThn.SelectedItem = now.Year.ToString();
                akhirThn.SelectedItem = now.Year.ToString();
                conn.Close();
            }
        }

        private void reportMonthBtn_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();

            string hariAwal = awalTgl.SelectedItem.ToString();
            string bulanAwal = bulanDic[awalBln.SelectedValue.ToString()];
            string tahunAwal = awalThn.SelectedItem.ToString();

            string hariAkhir = akhirTgl.SelectedItem.ToString();
            string bulanAkhir = bulanDic[akhirBln.SelectedValue.ToString()];
            string tahunAkhir = akhirThn.SelectedItem.ToString();

            DateTime awalDt = new DateTime(Convert.ToInt32(tahunAwal), Convert.ToInt32(awalBln.SelectedValue.ToString()), Convert.ToInt32(hariAwal));
            DateTime akhirDt = new DateTime(Convert.ToInt32(tahunAkhir), Convert.ToInt32(akhirBln.SelectedValue.ToString()), Convert.ToInt32(hariAkhir));

            if (awalDt <= akhirDt)
            {
                string periode = String.Format("{0} {1} {2} - {3} {4} {5}", hariAwal, bulanAwal, tahunAwal, hariAkhir, bulanAkhir, tahunAkhir);

                string periodeAwal = String.Format("{0}-{1}-{2}", tahunAwal, awalBln.SelectedValue.ToString(), hariAwal);
                string periodeAkhir = String.Format("{0}-{1}-{2}", tahunAkhir, akhirBln.SelectedValue.ToString(), hariAkhir);

                sfd.FileName = "Laporan Tanggal " + periode + ".xlsx";
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

                        try
                        {
                            //Start Excel and get Application object.
                            oXL = new Excel.Application();
                            oXL.Visible = true;

                            //Get a new workbook.
                            oWB = (Excel._Workbook)(oXL.Workbooks.Add(Missing.Value));
                            oSheet = (Excel._Worksheet)oWB.ActiveSheet;

                            oSheet.Cells[1, 1] = "Laporan Tanggal " + periode;
                            oSheet.Cells[1, 1].Font.Bold = true;
                            oSheet.Range[oSheet.Cells[1, 1], oSheet.Cells[1, 8]].Merge();
                            oSheet.Cells[1, 1].Style.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                            int init_row = 3;
                            oSheet.Cells[1, 1].ColumnWidth = 4;
                            oSheet.Cells[1, 2].ColumnWidth = 28;
                            oSheet.Cells[1, 3].ColumnWidth = 10;
                            oSheet.Cells[1, 4].ColumnWidth = 22;
                            oSheet.Cells[1, 5].ColumnWidth = 8;
                            oSheet.Cells[1, 6].ColumnWidth = 22;
                            oSheet.Cells[1, 7].ColumnWidth = 14;

                            //Add table headers going cell by cell.
                            oSheet.Cells[init_row, 1] = "No.";
                            oSheet.Cells[init_row, 2] = "No. Kuitansi";
                            oSheet.Cells[init_row, 3] = "No. Pelanggan";
                            oSheet.Cells[init_row, 4] = "Nama";
                            oSheet.Cells[init_row, 5] = "RT";
                            oSheet.Cells[init_row, 6] = "Tanggal Pembayaran";
                            oSheet.Cells[init_row, 7] = "Jumlah";
                            oSheet.Range[oSheet.Cells[init_row, 1], oSheet.Cells[init_row, 7]].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                            oSheet.Range[oSheet.Cells[init_row, 1], oSheet.Cells[init_row, 7]].Style.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                            oSheet.Range[oSheet.Cells[init_row, 1], oSheet.Cells[init_row, 7]].Style.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                            oSheet.Range[oSheet.Cells[init_row, 1], oSheet.Cells[init_row, 7]].WrapText = true;

                            oSheet.Cells[init_row, 1].EntireRow.Font.Bold = true;
                            oSheet.Cells[init_row, 1].RowHeight = 18;

                            init_row++;

                            int total = 0;

                            using (SQLiteConnection conn = new SQLiteConnection(@"Data Source =" + Program.path_db))
                            {
                                conn.Open();

                                String query = "SELECT no_invoice, invoice_suffix, member_id, urut_rt, nama, rt, tgl_bayar, jumlah ";
                                query += "FROM pembayaran p ";
                                query += "JOIN meteran m ON m.id = p.meteran_id ";
                                query += "JOIN members u ON u.id = m.member_id ";
                                query += "WHERE tgl_bayar BETWEEN '" + periodeAwal + "' AND '" + periodeAkhir + "' ";
                                query += "ORDER BY tgl_bayar, rt, nama";

                                //System.Diagnostics.Debug.WriteLine(query);

                                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                                {
                                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                                    {
                                        int no = 1;
                                        while (reader.Read())
                                        {
                                            oSheet.Cells[init_row, 1] = no;
                                            oSheet.Cells[init_row, 2] = string.Format("{0:000}", reader.GetValue(0)) + reader.GetValue(1).ToString();
                                            oSheet.Cells[init_row, 3] = "'" + reader.GetValue(3).ToString() + "." + reader.GetValue(5).ToString(); //No Urut
                                            oSheet.Cells[init_row, 4] = reader.GetValue(4).ToString(); //Nama
                                            oSheet.Cells[init_row, 5] = "'" + reader.GetValue(5).ToString() + " /09";
                                            oSheet.Cells[init_row, 6] = String.Format(new System.Globalization.CultureInfo("id-ID"), "{0:dd MMMM yyyy HH:mm}", reader.GetDateTime(6)); 
                                            oSheet.Cells[init_row, 7] = reader.GetValue(7);

                                            total += reader.GetInt32(7);

                                            no++;
                                            init_row++;
                                        }
                                    }
                                }
                                conn.Close();
                            }

                            oSheet.Cells[init_row, 1] = "Jumlah";
                            oSheet.Cells[init_row, 7] = total;
                            oSheet.Range[oSheet.Cells[init_row, 1], oSheet.Cells[init_row, 6]].Merge();
                            oSheet.Cells[init_row, 1].Style.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                            oSheet.Cells[init_row, 1].Font.Bold = true;
                            oSheet.Cells[init_row, 7].Font.Bold = true;

                            oSheet.get_Range("D4", "D" + init_row).Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                            oSheet.get_Range("G4", "G" + init_row).Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                            oSheet.get_Range("F4", "F" + init_row).NumberFormat = "dd/mm/yyyy hh:mm";
                            oSheet.get_Range("H4", "G" + init_row).NumberFormat = "#,###,###";
                            oSheet.get_Range("A3", "G" + init_row).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

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
                MessageBox.Show("Tanggal Sampai Harus Lebih Besar dari Tanggal Awal");
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

    }
}
