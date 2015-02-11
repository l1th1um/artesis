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
    public partial class reportPelanggan : Form
    {
        Dictionary<string, string> bulanDic = new Dictionary<string, string>();
        List<int> yearList = new List<int>();

        public reportPelanggan()
        {
            InitializeComponent();            
            this.getYear();
            this.prepareComboBoxMember();

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
            if (cbAnggota.SelectedValue.ToString() != "0")
            {
                String memberID = cbAnggota.SelectedValue.ToString();
                String nama = this.getMember(Int32.Parse(memberID));

                SaveFileDialog sfd = new SaveFileDialog();
                                
                String tahun = CBTahun.SelectedItem.ToString();

                sfd.FileName = "Laporan " + nama + "  Tahun  " + tahun + ".xls";
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

                            oSheet.Cells[1, 1] = "Laporan Tahun "  + tahun;
                            oSheet.Cells[1, 1].Font.Bold = true;
                            oSheet.Range[oSheet.Cells[1, 1], oSheet.Cells[1, 5]].Merge();
                            oSheet.Cells[1, 1].Style.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                            oSheet.Cells[3, 1] = "No Pelanggan";
                            oSheet.Cells[4, 1] = "Nama";
                            oSheet.Cells[5, 1] = "Alamat";
                            oSheet.Cells[6, 1] = "No Telepon";
                            oSheet.Range[oSheet.Cells[3, 1], oSheet.Cells[3, 2]].Merge();
                            oSheet.Range[oSheet.Cells[4, 1], oSheet.Cells[4, 2]].Merge();
                            oSheet.Range[oSheet.Cells[5, 1], oSheet.Cells[5, 2]].Merge();
                            oSheet.Range[oSheet.Cells[6, 1], oSheet.Cells[6, 2]].Merge();                            

                            using (SQLiteConnection conn = new SQLiteConnection(@"Data Source =" + Program.path_db))
                            {
                                conn.Open();

                                String query = "SELECT * FROM members WHERE id = " + memberID;                                

                                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                                {
                                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                                    {                             
                                        if (reader.Read())
                                        {
                                            String alamat = "Blok " + reader.GetValue(4) + " No. " + reader.GetValue(5);
                                            alamat += " RT " + reader.GetValue(6) + "/09";

                                            oSheet.Cells[3, 3] = "'" + string.Format("{0:00}", reader.GetValue(1)) + "." + reader.GetValue(6);
                                            oSheet.Cells[4, 3] = reader.GetValue(2);
                                            oSheet.Cells[5, 3] = alamat;
                                            oSheet.Cells[6, 3] = reader.GetValue(3);
                                        }
                                    }
                                }
                                conn.Close();
                            }
                            //bulanDic[cbAnggota.SelectedValue.ToString()];

                            int init_row = 9;
                            oSheet.Cells[1, 1].ColumnWidth = 4;
                            oSheet.Cells[1, 2].ColumnWidth = 28;
                            oSheet.Cells[1, 3].ColumnWidth = 28;
                            oSheet.Cells[1, 4].ColumnWidth = 18;
                            oSheet.Cells[1, 5].ColumnWidth = 14;
                            

                            //Add table headers going cell by cell.
                            oSheet.Cells[init_row, 1] = "No.";
                            oSheet.Cells[init_row, 2] = "Bulan";
                            oSheet.Cells[init_row, 3] = "No. Kuitansi";
                            oSheet.Cells[init_row, 4] = "Tanggal Pembayaran";
                            oSheet.Cells[init_row, 5] = "Jumlah";
                            
                            oSheet.Range[oSheet.Cells[init_row, 1], oSheet.Cells[init_row, 5]].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                            oSheet.Range[oSheet.Cells[init_row, 1], oSheet.Cells[init_row, 5]].Style.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                            oSheet.Range[oSheet.Cells[init_row, 1], oSheet.Cells[init_row, 5]].Style.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                            oSheet.Range[oSheet.Cells[init_row, 1], oSheet.Cells[init_row, 5]].WrapText = true;

                            oSheet.Cells[init_row, 1].EntireRow.Font.Bold = true;
                            oSheet.Cells[init_row, 1].RowHeight = 18;

                            init_row++;

                            int total = 0;

                            using (SQLiteConnection conn = new SQLiteConnection(@"Data Source =" + Program.path_db))
                            {
                                conn.Open();

                                String query = "SELECT strftime('%m', tanggal) as bln, no_invoice, invoice_suffix, tgl_bayar, jumlah ";
                                query += "FROM pembayaran p ";
                                query += "JOIN meteran m ON m.id = p.meteran_id ";                                
                                query += "WHERE strftime('%Y', tgl_bayar) = '" + tahun + "' ";
                                query += "AND member_id  = " + memberID + " ";
                                query += "ORDER BY bln";

                                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                                {
                                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                                    {
                                        int no = 1;
                                        while (reader.Read())
                                        {
                                            oSheet.Cells[init_row, 1] = no;
                                            oSheet.Cells[init_row, 2] = bulanDic[reader.GetValue(0).ToString()];
                                            oSheet.Cells[init_row, 3] = string.Format("{0:000}", reader.GetValue(1)) + reader.GetValue(2).ToString();
                                            oSheet.Cells[init_row, 4] = String.Format(new System.Globalization.CultureInfo("id-ID"), "{0:dd MMMM yyyy}", reader.GetDateTime(3));
                                            oSheet.Cells[init_row, 5] = reader.GetValue(4).ToString(); 

                                            total += reader.GetInt32(4);

                                            no++;
                                            init_row++;
                                        }
                                    }
                                }
                            }

                            oSheet.Cells[init_row, 1] = "Jumlah";
                            oSheet.Cells[init_row, 5] = total;
                            oSheet.Range[oSheet.Cells[init_row, 1], oSheet.Cells[init_row, 4]].Merge();
                            oSheet.Cells[init_row, 1].Style.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                            oSheet.Cells[init_row, 1].Font.Bold = true;
                            oSheet.Cells[init_row, 5].Font.Bold = true;

                            //oSheet.get_Range("E4", "E" + init_row).Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                            oSheet.get_Range("A3", "C7").Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                            oSheet.get_Range("A3", "C7").Font.Bold = true;

                            oSheet.get_Range("E10", "E" + init_row).Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                            oSheet.get_Range("D10", "D" + init_row).NumberFormat = "dd/mm/yyyy hh:mm";
                            oSheet.get_Range("E10", "E" + init_row).NumberFormat = "#,###,###";
                            oSheet.get_Range("A9", "E" + init_row).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

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
                MessageBox.Show("Pilih Anggota");
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

        private void prepareComboBoxMember()
        {
            using (SQLiteConnection conn = new SQLiteConnection(@"Data Source =" + Program.path_db))
            {
                conn.Open();

                DataTable dt = new DataTable();
                string command = "SELECT id, nama || ' (' || (substr('00'||urut_rt, -2, 2) || '.' || rt) || ')' as nama_anggota FROM members WHERE active = 1 ORDER BY nama_anggota";
                using (SQLiteDataAdapter da = new SQLiteDataAdapter(command, conn))
                {
                    dt.Columns.Add("id", typeof(string));
                    dt.Columns.Add("nama_anggota", typeof(string));

                    DataRow row = dt.NewRow();
                    row["Id"] = 0;
                    row["nama_anggota"] = "- Pilih Anggota - ";
                    dt.Rows.Add(row);

                    da.Fill(dt);

                    cbAnggota.ValueMember = "id";
                    cbAnggota.DisplayMember = "nama_anggota";
                    cbAnggota.DataSource = dt;
                }

                conn.Close();
            }
        }

        public string getMember(int id)
        {
            using (SQLiteConnection conn = new SQLiteConnection(@"Data Source =" + Program.path_db))
            {
                conn.Open();
                string command = "SELECT nama FROM members WHERE id = " + id;
                using (SQLiteCommand query = new SQLiteCommand(command, conn))
                {
                    using (SQLiteDataReader reader = query.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader.GetValue(0).ToString();
                        }
                        else
                        {
                            return "";
                        }
                    }
                }
            }
        }

    }
}
