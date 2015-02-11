using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Drawing.Printing;
using System.Drawing.Drawing2D;
using System.Text.RegularExpressions;
using System.Globalization;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;


namespace Artesis
{
    public partial class Form1 : Form
    {
        //SQLiteConnection conn = new SQLiteConnection(@"Data Source =" + Program.path_db);
        SQLiteDataAdapter sda;
        DataSet ds;
        Dictionary<string, string> bulanDic = new Dictionary<string, string>();
        List<int> yearList = new List<int>();
        Dictionary<int, int> biayaBeban = new Dictionary<int, int>();
        String tanggalTagihan;
        String memberTagihan;

        //Detail Meteran
        Dictionary<double, double> usage = new Dictionary<double, double>();

        //Untuk di Print
        String blnBayar;
        String thnBayar;
        String namaBayar;
        String memberidBayar;
        String noUrutRTBayar;
        String alamatBayar;
        String invoiceBayar;
        int dendaSet;

        double awalBayar;
        double akhirBayar;
        double pemakaianBayar;
        int bebanTetap;
        int dendaBayar;
        double bebanBayar;
        double totalBayar;

        String jsonTarif;


        public Form1()
        {
            InitializeComponent();
            //conn.Open();

            this.closePanel();

            panelTagihan.Visible = true;

            this.prepareComboBoxMember();

            this.loadMember();

            this.getMonth();

            this.getYear();

            this.prepareTarif();

            /*
            biayaBeban.Add(1, 2000);
            biayaBeban.Add(11, 2250);
            biayaBeban.Add(16, 2500);
            biayaBeban.Add(21, 2750);
            biayaBeban.Add(25, 3000);
            */
            System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
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

        private void loadMember(string order_by = "id", string order = "ASC")
        {
            //Start Grid Member
            using (SQLiteConnection conn = new SQLiteConnection(@"Data Source =" + Program.path_db))
            {
                conn.Open();
                using (sda = new SQLiteDataAdapter("SELECT id, (substr('00'||urut_rt, -2, 2) || '.' || rt) as no_urut, nama, (blok || ' No. ' || no_rumah) as blok_rumah, rt, telp  FROM members WHERE active = 1 ORDER BY rt, urut_rt", conn))
                {
                    ds = new System.Data.DataSet();

                    sda.Fill(ds, "Members_List");

                    dataGridViewMember.DataSource = ds.Tables[0];

                    //dataGridViewMember.Sort(dataGridViewMember.Columns[0], ListSortDirection.Ascending);


                    dataGridViewMember.Columns[0].HeaderText = "ID Anggota";
                    dataGridViewMember.Columns[1].HeaderText = "No Urut";
                    dataGridViewMember.Columns[2].HeaderText = "Nama";
                    dataGridViewMember.Columns[3].HeaderText = "Blok";
                    dataGridViewMember.Columns[4].HeaderText = "RT/RW";
                    dataGridViewMember.Columns[5].HeaderText = "Telepon";

                    dataGridViewMember.Columns[0].Visible = false;

                    //dataGridViewMember.Columns[0].Width = 10;
                    dataGridViewMember.Columns[1].Width = 100;
                    dataGridViewMember.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dataGridViewMember.Columns[3].Width = 100;
                    dataGridViewMember.Columns[4].Width = 100;
                    dataGridViewMember.Columns[5].Width = 100;

                    dataGridViewMember.ReadOnly = true;
                }
                conn.Close();
            }
            //End Grid Member
        }

        private void cbAnggota_SelectedIndexChanged(object sender, EventArgs e)
        {
            String no_anggota = cbAnggota.SelectedValue.ToString();

            if (no_anggota != "0")
            {
                using (SQLiteConnection conn = new SQLiteConnection(@"Data Source =" + Program.path_db))
                {
                    conn.Open();
                    string command = "SELECT * FROM members WHERE active = 1 AND id = " + no_anggota;

                    using (SQLiteCommand cmd = new SQLiteCommand(command, conn))
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                String rt = reader["rt"].ToString();
                                lblAlamat.Text = "Blok " + reader["blok"].ToString() + " No " + reader["no_rumah"] + " RT " + rt + "/09";
                                lblAlamat2.Visible = true;

                                noUrutRTBayar = string.Format("{0:00}.{1}", reader["urut_rt"], rt);
                            }
                        }
                    }
                    conn.Close();
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void dataPelangganToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.loadMember();
            this.closePanel();

            panelMember.Visible = true;
        }

        private void btnAddMember_Click(object sender, EventArgs e)
        {
            DialogResult dr = new DialogResult();

            FrmAnggota addMemberDialog = new FrmAnggota();
            addMemberDialog.isUpdate.Text = "0";

            dr = addMemberDialog.ShowDialog();

            if (dr == DialogResult.OK)
            {
                this.loadMember();
            }
        }

        private void dataGridViewMember_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string member_id = dataGridViewMember.CurrentRow.Cells["id"].Value.ToString();

            using (SQLiteConnection conn = new SQLiteConnection(@"Data Source =" + Program.path_db))
            {
                conn.Open();
                string command = "SELECT *  FROM members WHERE id = " + member_id;
                using (SQLiteCommand cmd = new SQLiteCommand(command, conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            FrmAnggota updateMemberDialog = new FrmAnggota();

                            updateMemberDialog.txtName.Text = reader["nama"].ToString();
                            updateMemberDialog.txtBlock.Text = reader["blok"].ToString();
                            updateMemberDialog.txtNumber.Text = reader["no_rumah"].ToString();
                            updateMemberDialog.cbRT.SelectedItem = reader["rt"].ToString();
                            updateMemberDialog.txtPhone.Text = reader["telp"].ToString();
                            updateMemberDialog.isUpdate.Text = "1";
                            updateMemberDialog.memberID.Text = reader["id"].ToString();

                            reader.Dispose();

                            DialogResult dr = new DialogResult();

                            dr = updateMemberDialog.ShowDialog();

                            if (dr == DialogResult.OK)
                            {
                                this.loadMember();
                            }
                        }
                    }
                }
                conn.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridViewMember.SelectedRows)
            {
                //dataGridViewMember.dataGridViewMember.Rows.RemoveAt(row.Index);
                string rowId = row.Cells[0].Value.ToString();

                this.deleteMember(rowId);
            }

            MessageBox.Show("Data Anggota Telah Dihapus");
            this.loadMember();
        }

        private void deleteMember(string Id)
        {
            using (SQLiteConnection conn = new SQLiteConnection(@"Data Source =" + Program.path_db))
            {
                conn.Open();

                using (SQLiteCommand cmd = new SQLiteCommand("UPDATE members SET active = 0 WHERE id = " + Id, conn))
                {
                    cmd.ExecuteNonQuery();
                }

                conn.Close();
            }
            //cmd.Dispose();

        }

        private void dataMeteranToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.closePanel();
            this.clearDatagridMeteran();
            lblMeteran.Visible = false;
            panelMeteranBulanan.Visible = true;
        }

        private void clearDatagridMeteran()
        {
            dgvMeteran.DataSource = null;
            dgvMeteran.Rows.Clear();
            dgvMeteran.Columns.Clear();
        }

        private void prepareMeteran()
        {
            this.clearDatagridMeteran();

            String tanggal = CBTahun.SelectedItem + "-" + cbBulan.SelectedValue + "-01";
            tglTxt.Text = tanggal;

            String periode = bulanDic[cbBulan.SelectedValue.ToString()] + " " + CBTahun.SelectedItem;
            lblMeteran.Text = "Data Meteran Bulan " + periode;
            lblMeteran.Visible = true;

            DateTime dt = new DateTime(Convert.ToInt32(CBTahun.SelectedItem), Convert.ToInt32(cbBulan.SelectedValue), 01);
            DateTime dt2 = dt.AddMonths(-1);

            String blnSebelum = String.Format("{0:yyyy-MM-dd}", dt2);

            /*
            String query = "SELECT m.id, m.nama, t.awal, t.akhir,bayar ";
                   query += " FROM members m";
                   query += " LEFT JOIN meteran t ON m.id = t.member_id AND t.tanggal = '"+ tanggal + "'";
                   query += " WHERE m.active = 1";*/

            String query = "SELECT m.id, (substr('00'||urut_rt, -2, 2) || '.' || rt) as no_urut, m.nama, (rt || '/09') as rtrw, ";
            query += " ('Blok ' || blok || ' No. ' || no_rumah) as alamat, ";
            query += " ifnull(t.awal, (SELECT ifnull(akhir, 0) FROM members m2 ";
            query += " LEFT JOIN meteran t2 ON m2.id = t2.member_id AND t2.tanggal = '" + blnSebelum + "'";
            query += " where m2.id = m.id)) as awal,   t.akhir, bayar ";
            query += " FROM members m ";
            query += " LEFT JOIN meteran t ON m.id = t.member_id AND t.tanggal = '" + tanggal + "'";
            query += " WHERE m.active = 1 ORDER BY rt, urut_rt";

            //System.Diagnostics.Debug.WriteLine(query);

            using (SQLiteConnection conn = new SQLiteConnection(@"Data Source =" + Program.path_db))
            {
                conn.Open();
                sda = new SQLiteDataAdapter(query, conn);
                ds = new System.Data.DataSet();

                sda.Fill(ds, "Meteran_List");

                dgvMeteran.DataSource = ds.Tables[0];
                
                dgvMeteran.Columns[0].HeaderText = "id";
                dgvMeteran.Columns[1].HeaderText = "No Pelanggan";
                dgvMeteran.Columns[2].HeaderText = "Nama";
                dgvMeteran.Columns[3].HeaderText = "RT";
                dgvMeteran.Columns[4].HeaderText = "Alamat";
                dgvMeteran.Columns[5].HeaderText = "Awal";
                dgvMeteran.Columns[6].HeaderText = "Akhir";
                dgvMeteran.Columns[7].HeaderText = "Bayar";

                dgvMeteran.Columns[0].Visible = false;
                dgvMeteran.Columns[7].Visible = false;
                
                dgvMeteran.Columns[2].Width = 100;
                dgvMeteran.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvMeteran.Columns[3].Width = 100;
                dgvMeteran.Columns[4].Width = 200;
                dgvMeteran.Columns[5].Width = 100;
                dgvMeteran.Columns[6].Width = 100;

                dgvMeteran.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvMeteran.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvMeteran.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                dgvMeteran.ReadOnly = false;
                dgvMeteran.Columns[1].ReadOnly = true;
                dgvMeteran.Columns[2].ReadOnly = true;

                DataGridViewImageColumn imageCol = new DataGridViewImageColumn();
                imageCol.HeaderText = "Bayar";
                imageCol.Name = "image";
                imageCol.Image = Properties.Resources.no_image;
                dgvMeteran.Columns.Add(imageCol);

                //Image Column Print
                DataGridViewImageColumn imagePrint = new DataGridViewImageColumn();
                imagePrint.HeaderText = "";
                imagePrint.Name = "imagePrint";
                imagePrint.Image = Properties.Resources.no_image;
                dgvMeteran.Columns.Add(imagePrint);
                
                //View Column Print
                DataGridViewImageColumn imageView = new DataGridViewImageColumn();
                imageView.HeaderText = "";
                imageView.Name = "imageView";
                imageView.Image = Properties.Resources.no_image;
                dgvMeteran.Columns.Add(imageView);
                

                dgvMeteran.Columns[9].DefaultCellStyle.SelectionBackColor = dgvMeteran.DefaultCellStyle.BackColor;
                dgvMeteran.Columns[9].DefaultCellStyle.SelectionForeColor = dgvMeteran.DefaultCellStyle.ForeColor;

                for (int x = 0; x <= dgvMeteran.Rows.Count - 1; x++)
                {
                    if (dgvMeteran.Rows[x].Cells[7].Value.ToString() == "1")
                    {
                        Image bayar = Properties.Resources.check;
                        dgvMeteran.Rows[x].Cells["image"].Value = bayar;

                        Image printerIcon = Properties.Resources.printer;
                        dgvMeteran.Rows[x].Cells["imagePrint"].Value = printerIcon;

                        Image viewIcon = Properties.Resources.view;
                        dgvMeteran.Rows[x].Cells["imageView"].Value = viewIcon;
                    }
                }

                foreach (DataGridViewColumn col in dgvMeteran.Columns)
                {
                    col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    col.HeaderCell.Style.Font = new Font("Arial", 12F, FontStyle.Bold, GraphicsUnit.Pixel);
                }

                conn.Close();
            }

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
                        cbTahunTghn.Items.Add(reader.GetString(0));
                        nextYear = Int32.Parse(reader.GetValue(0).ToString()) + 1;
                    }
                    CBTahun.Items.Add(nextYear.ToString());
                    cbTahunTghn.Items.Add(nextYear.ToString());
                }
                else
                {
                    CBTahun.Items.Add(now.Year.ToString());
                    cbTahunTghn.Items.Add(now.Year.ToString());
                }

                //CBTa.Items[cbTahunTghn.SelectedIndex] = now.Year.ToString();
                //cbTahunTghn.Items[cbTahunTghn.SelectedIndex] = now.Year.ToString();
                CBTahun.SelectedItem = now.Year.ToString();
                cbTahunTghn.SelectedItem = now.Year.ToString();

                reader.Dispose();
                conn.Close();
            }
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

            cbBulanTghn.DataSource = new BindingSource(bulanDic, null);
            cbBulanTghn.DisplayMember = "Value";
            cbBulanTghn.ValueMember = "Key";
            cbBulanTghn.SelectedIndex = now.Month - 1;
        }


        private void closePanel()
        {
            panelMember.Visible = false;
            panelMeteranBulanan.Visible = false;
            panelTagihan.Visible = false;
        }

        private void btnCekMeteran_Click(object sender, EventArgs e)
        {
            this.prepareMeteran();
        }

        private void dgvMeteran_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(CheckKey);
            if (dgvMeteran.CurrentCell.ColumnIndex == 2 || dgvMeteran.CurrentCell.ColumnIndex == 3)
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(CheckKey);
                }
            }
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

        private void dgvMeteran_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            using (SQLiteConnection conn = new SQLiteConnection(@"Data Source =" + Program.path_db))
            {
                conn.Open();

                string member_id = dgvMeteran.CurrentRow.Cells["id"].Value.ToString();
                int columnIdx = e.ColumnIndex;
                double meteranVal = (double)dgvMeteran.CurrentRow.Cells[columnIdx].Value;

                String field = "awal";

                if (columnIdx == 3)
                {
                    if (dgvMeteran.CurrentRow.Cells[2].Value != DBNull.Value)
                    {
                        if (float.Parse(dgvMeteran.CurrentRow.Cells[3].Value.ToString()) <= float.Parse(dgvMeteran.CurrentRow.Cells[2].Value.ToString()))
                        {
                            MessageBox.Show("Meteran akhir harus lebih besar daripada meteran awal");
                            return;
                        }
                    }

                    field = "akhir";
                }

                SQLiteCommand cmd = new SQLiteCommand("SELECT *  FROM meteran WHERE member_id = " + member_id + " AND tanggal = '" + tglTxt.Text + "'", conn);

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        SQLiteCommand cmd2 = new SQLiteCommand("UPDATE meteran SET  " + field + " = '" + meteranVal + "', updated_at = datetime('now') WHERE  member_id = " + member_id + " AND bayar = 0 AND tanggal = '" + tglTxt.Text + "'", conn);
                        cmd2.ExecuteNonQuery();
                    }
                    else
                    {
                        SQLiteCommand cmd2 = new SQLiteCommand("INSERT INTO meteran ('" + field + "', member_id, tanggal, bayar, updated_at)  VALUES(" + meteranVal + ",  " + member_id + ",'" + tglTxt.Text + "', 0, datetime('now') )", conn);
                        cmd2.ExecuteNonQuery();

                        if (columnIdx == 3)
                        {
                            SQLiteCommand cmd3 = new SQLiteCommand("UPDATE meteran SET  awal = '" + float.Parse(dgvMeteran.CurrentRow.Cells[2].Value.ToString()) + "', updated_at = datetime('now') WHERE  member_id = " + member_id + " AND bayar = 0 AND tanggal = '" + tglTxt.Text + "'", conn);
                            cmd3.ExecuteNonQuery();
                        }
                    }
                }
                conn.Close();
            }
        }

        private void pembayaranTagihanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.closePanel();
            this.prepareComboBoxMember();

            gbTagihan.Visible = false;
            lblAlamat.Text = "";
            lblAlamat2.Visible = false;
            cbAnggota.SelectedIndex = 0;
            panelTagihan.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            gbTagihan.Visible = false;
            this.resetTagihan();

            if (cbAnggota.SelectedValue.ToString() != "0")
            {
                memberTagihan = cbAnggota.SelectedValue.ToString();
                namaBayar = cbAnggota.SelectedText.ToString();

                String bulan = cbBulanTghn.SelectedValue.ToString();
                String tahun = cbTahunTghn.SelectedItem.ToString();
                tanggalTagihan = tahun + "-" + bulan + "-01";
                
                using (SQLiteConnection conn = new SQLiteConnection(@"Data Source =" + Program.path_db))
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand("SELECT *  FROM meteran WHERE member_id = " + memberTagihan + " AND tanggal = '" + tanggalTagihan + "'", conn);

                    SQLiteDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        if (reader.GetValue(5).ToString() == "1")
                        {
                            MessageBox.Show("Tagihan Sudah Dibayar");
                        }
                        else
                        {
                            if (reader.GetValue(3) == DBNull.Value || reader.GetValue(4) == DBNull.Value)
                            {
                                MessageBox.Show("Data Meteran belum diinput");
                            }
                            else
                            {

                                double pemakaian = reader.GetDouble(4) - reader.GetDouble(3);

                                txtPemakaian.Text = pemakaian.ToString("N");


                                int denda = this.denda(bulan, tahun);

                                dendaTxt.Text = denda.ToString("N0");


                                bebanTxt.Text = bebanTetap.ToString("N0");


                                double biayaPemakaian = this.biayaPemakaian(pemakaian);

                                biayaTxt.Text = biayaPemakaian.ToString("N0");


                                double total = biayaPemakaian + bebanTetap + denda;

                                lblTotal.Text = total.ToString("N0");

                                gbTagihan.Visible = true;

                                //Prepare for Print
                                blnBayar = bulanDic[bulan].ToUpper();
                                thnBayar = tahun;
                                memberidBayar = cbAnggota.SelectedValue.ToString();

                                //noUrutRTBayar = 

                                namaBayar = this.getMember(Int32.Parse(memberidBayar));

                                alamatBayar = lblAlamat.Text;
                                awalBayar = reader.GetDouble(3);
                                akhirBayar = reader.GetDouble(4);
                                pemakaianBayar = pemakaian;
                                bebanBayar = biayaPemakaian;
                                dendaBayar = denda;
                                totalBayar = total;

                                this.setTarifDetail();
                            }
                        }
                        reader.Close();
                        reader.Dispose();
                        conn.Close();
                    }
                    else
                    {
                        MessageBox.Show("Data Meteran Belum Diinput");
                    }


                }

            }
        }

        public string getMember(int id)
        {
            using (SQLiteConnection conn = new SQLiteConnection(@"Data Source =" + Program.path_db))
            {
                conn.Open();
                string command = "SELECT nama FROM members WHERE id = " + id;
                SQLiteCommand query = new SQLiteCommand(command, conn);

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

        public int denda(string bulan, string tahun)
        {
            DateTime now = DateTime.Now;
            DateTime jatuhTempo = new DateTime(Int32.Parse(tahun), Int32.Parse(bulan), 20);

            int denda = 0;

            //MessageBox.Show(jatuhTempo.ToString() + " " + now.ToString());

            if (now > jatuhTempo)
            {
                denda = dendaSet;
            }

            return denda;
        }

        public double biayaPemakaian(double pemakaian)
        {
            double sisa;

            double biaya = 0;

            usage[1] = 0;
            usage[11] = 0;
            usage[16] = 0;
            usage[21] = 0;
            usage[25] = 0;

            if (pemakaian > 25)
            {
                sisa = pemakaian - 25;
                pemakaian = 25;

                usage[25] = sisa;

                biaya += sisa * biayaBeban[25];
            }

            if (pemakaian >= 21 && pemakaian <= 25)
            {
                sisa = pemakaian - 20;
                pemakaian = 20;

                usage[21] = sisa;

                biaya += sisa * biayaBeban[21];
            }

            if (pemakaian >= 16 && pemakaian <= 20)
            {
                sisa = pemakaian - 15;
                pemakaian = 15;

                usage[16] = sisa;

                biaya += sisa * biayaBeban[16];
            }

            if (pemakaian >= 11 && pemakaian <= 15)
            {
                sisa = pemakaian - 10;
                pemakaian = 10;

                usage[11] = sisa;

                biaya += sisa * biayaBeban[11];
            }

            if (pemakaian <= 10)
            {
                usage[1] = pemakaian;
                biaya += pemakaian * biayaBeban[1];
            }


            return biaya;
        }

        private void resetTagihan()
        {
            txtPemakaian.Text = "";
            biayaTxt.Text = "";
            dendaTxt.Text = "";
            bebanTxt.Text = "";
        }

        private void byrBtn_Click(object sender, EventArgs e)
        {

            using (SQLiteConnection conn = new SQLiteConnection(@"Data Source =" + Program.path_db))
            {
                conn.Open();

                SQLiteCommand cmd = new SQLiteCommand("BEGIN", conn);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                /*
                using (SQLiteCommand cmd = new SQLiteCommand("UPDATE meteran SET  bayar = 1, bayar_at = datetime('now') WHERE  member_id = " + memberTagihan + " AND bayar = 0 AND tanggal = '" + tanggalTagihan + "'", conn))
                {*/

                cmd = new SQLiteCommand("UPDATE meteran SET  bayar = 1, bayar_at = datetime('now') WHERE  member_id = " + memberTagihan + " AND bayar = 0 AND tanggal = '" + tanggalTagihan + "'", conn);

                //cmd.Connection.Open();                    
                int rowsAffected = cmd.ExecuteNonQuery();  //Deadlock in Here
                cmd.Dispose();

                if (rowsAffected > 0)
                {
                    //Check Max Invoice

                    DateTime now = DateTime.Now;
                    String suffix = "/PAA-LMA/09/" + ToRoman(now.Month) + "/" + now.Year.ToString();

                    string command2 = "SELECT max(no_invoice) as max_invoice FROM pembayaran WHERE invoice_suffix = '" + suffix + "'";

                    int max_invoice = 1;

                    using (SQLiteCommand cmd2 = new SQLiteCommand(command2, conn))
                    {
                        using (SQLiteDataReader reader2 = cmd2.ExecuteReader())
                        {

                            if (reader2.Read())
                            {
                                if (reader2["max_invoice"] != DBNull.Value)
                                {
                                    max_invoice = reader2.GetInt32(0) + 1;
                                }
                            }
                        }

                    }
                    invoiceBayar = string.Format("{0:000}", max_invoice) + suffix;
                    //End Check Max Invoice

                    //Check ID
                    int meteran_id = 0;

                    string command3 = "SELECT id FROM meteran WHERE member_id = " + memberTagihan + " AND tanggal = '" + tanggalTagihan + "'";

                    using (SQLiteCommand cmd3 = new SQLiteCommand(command3, conn))
                    {
                        using (SQLiteDataReader reader3 = cmd3.ExecuteReader())
                        {
                            if (reader3.Read())
                            {
                                meteran_id = reader3.GetInt32(0);
                            }
                        }

                    }
                    //End Check Max Invoice


                    String savePayment = "INSERT INTO pembayaran(meteran_id, no_invoice, invoice_suffix, jumlah, tgl_bayar, beban, denda, biaya_pemakaian, tarif) VALUES  ";
                    savePayment += "(" + meteran_id + ", " + max_invoice + ", '" + suffix + "', " + totalBayar + " , datetime('now'), " + bebanTetap  + "," + dendaBayar + ", " + bebanBayar +",'" + jsonTarif + "')";

                    System.Diagnostics.Debug.WriteLine(savePayment);

                    using (SQLiteCommand cmd4 = new SQLiteCommand(savePayment, conn))
                    {
                        cmd4.ExecuteNonQuery();
                    }

                    cmd = new SQLiteCommand("END", conn);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    //Prepare for Kuitansi
                    memberidBayar = memberTagihan;
                    
                    PrintDialog printDialog = new PrintDialog();

                    PrintDocument printDoc = new PrintDocument();

                    printDialog.Document = printDoc;

                    printDoc.PrintPage += new PrintPageEventHandler(printDoc_PrintPage);

                    DialogResult result = printDialog.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        printDoc.Print();

                        this.pembayaranTagihanToolStripMenuItem_Click(sender, e);

                        MessageBox.Show("Data Pembayaran Telah Disimpan");

                    }
                }
                else
                {
                    MessageBox.Show("Error ! Data Pembayaran tidak disimpan");
                }
                //}
                conn.Close();
                conn.Dispose();
            }

            this.pembayaranTagihanToolStripMenuItem_Click(sender, e);
        }

        void printDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            this.printKuitansi(e);
        }

        private void printKuitansi(PrintPageEventArgs e)
        {
            using (SQLiteConnection conn = new SQLiteConnection(@"Data Source =" + Program.path_db))
            {
                conn.Open();

                String command = "SELECT * FROM members u ";
                command += "LEFT JOIN meteran m ON u.id = m.member_id AND tanggal = '" + tanggalTagihan + "' ";
                command += "LEFT JOIN pembayaran p ON p.meteran_id = m.id ";
                command += "WHERE u.id = '" + memberidBayar + "'";

                using (SQLiteCommand cmd = new SQLiteCommand(command, conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Graphics graphic = e.Graphics;

                            Font font = new Font("Arial", 12, GraphicsUnit.Pixel);
                            Font fontUnderline = new Font("Arial", 16, FontStyle.Underline | FontStyle.Bold, GraphicsUnit.Pixel);
                            Font fontHeader1 = new Font("Arial", 16, FontStyle.Bold, GraphicsUnit.Pixel);
                            Font fontHeader2 = new Font("Arial", 14, FontStyle.Bold, GraphicsUnit.Pixel);
                            Font footerBold = new Font("Arial", 8, FontStyle.Bold, GraphicsUnit.Pixel);
                            Font footer = new Font("Arial", 8, GraphicsUnit.Pixel);
                            Font footer_small = new Font("Arial", 6, GraphicsUnit.Pixel);

                            float fontHeight = font.GetHeight();

                            int startX = 40;
                            int startY = 20;
                            int offset = 20;
                            int tab1 = 110;
                            int tab2 = 150;
                            int tab3 = 480;
                            int tab4 = 630;
                            int tab5 = 650;
                            int recWidth = 90;
                            int recHeight = 20;
                            int spacing = 20;

                            StringFormat stringFormat = new StringFormat();
                            stringFormat.Alignment = StringAlignment.Far;
                            stringFormat.LineAlignment = StringAlignment.Near;
                            //Rectangle rect1 = new Rectangle(10, 10, 130, 140);


                            String periode = String.Format(new System.Globalization.CultureInfo("id-ID"), "{0:MMMM yyyy}", reader.GetDateTime(12)).ToUpper();
                            double pemakaian = reader.GetDouble(14) - reader.GetDouble(13);
                            String alamat = "Blok " + reader["blok"].ToString() + " No. " + reader["no_rumah"].ToString() + " RT " + reader["rt"].ToString() + "/09";
                            String tglPembayaran = String.Format(new System.Globalization.CultureInfo("id-ID"), "{0:dd MMMM yyyy}", reader.GetDateTime(24));
                            Dictionary<string, string> tarif = reader["tarif"].ToString().FromJsonToDictionary();


                            graphic.DrawString("PENGELOLA AIR ARTESIS PERUM LAKSANA MEKAR ASRI", fontHeader1, new SolidBrush(Color.Black), startX + 180, startY);

                            graphic.DrawString("RW 09 Desa Laksana Mekar Kec. Padalarang Kab. Bandung Barat", fontHeader1, new SolidBrush(Color.Black), startX + 160, startY + offset);
                            offset += 70;

                            graphic.DrawString("KUITANSI PEMBAYARAN AIR BULAN " + periode, fontHeader2, new SolidBrush(Color.Black), startX + 220, startY + offset);

                            offset += 100;

                            //Logo

                            //String logoPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Artesis\logo.gif";
                            var logo = new Bitmap(Artesis.Properties.Resources.logo);
                            Point logoPoint = new Point(50, 20);
                            graphic.DrawImage(logo, logoPoint);

                            //End Logo

                            graphic.DrawString("No. Kuitansi", font, new SolidBrush(Color.Black), startX, startY + offset);
                            graphic.DrawString(":", font, new SolidBrush(Color.Black), startX + tab1, startY + offset);
                            graphic.DrawString(string.Format("{0:000}", reader["no_invoice"]) + reader["invoice_suffix"].ToString(), font, new SolidBrush(Color.Black), startX + tab2, startY + offset);

                            graphic.DrawString("Meteran Awal", font, new SolidBrush(Color.Black), startX + tab3, startY + offset);
                            graphic.DrawString(":", font, new SolidBrush(Color.Black), startX + tab4, startY + offset);
                            graphic.DrawString(string.Format("{0:N0}", reader["awal"]) + " m3", font, new SolidBrush(Color.Black), new Rectangle(startX + tab5, startY + offset, recWidth, recHeight), stringFormat);


                            offset += spacing;

                            graphic.DrawString("No. Pelanggan", font, new SolidBrush(Color.Black), startX, startY + offset);
                            graphic.DrawString(":", font, new SolidBrush(Color.Black), startX + tab1, startY + offset);
                            graphic.DrawString(string.Format("{0:00}", reader["urut_rt"]) + "." + reader["rt"].ToString(), font, new SolidBrush(Color.Black), startX + tab2, startY + offset);

                            graphic.DrawString("Meteran Akhir", font, new SolidBrush(Color.Black), startX + tab3, startY + offset);
                            graphic.DrawString(":", font, new SolidBrush(Color.Black), startX + tab4, startY + offset);
                            graphic.DrawString(string.Format("{0:N0}", reader["akhir"]) + " m3", font, new SolidBrush(Color.Black), new Rectangle(startX + tab5, startY + offset, recWidth, recHeight), stringFormat);

                            offset += spacing;

                            graphic.DrawString("Nama", font, new SolidBrush(Color.Black), startX, startY + offset);
                            graphic.DrawString(":", font, new SolidBrush(Color.Black), startX + tab1, startY + offset);
                            graphic.DrawString(reader["nama"].ToString(), font, new SolidBrush(Color.Black), startX + tab2, startY + offset);

                            graphic.DrawString("Jumlah Pemakaian", font, new SolidBrush(Color.Black), startX + tab3, startY + offset);
                            graphic.DrawString(":", font, new SolidBrush(Color.Black), startX + tab4, startY + offset);
                            graphic.DrawString(string.Format("{0:N0}", pemakaian) + " m3", font, new SolidBrush(Color.Black), new Rectangle(startX + tab5, startY + offset, recWidth, recHeight), stringFormat);

                            offset += spacing;

                            graphic.DrawString("Alamat", font, new SolidBrush(Color.Black), startX, startY + offset);
                            graphic.DrawString(":", font, new SolidBrush(Color.Black), startX + tab1, startY + offset);
                            graphic.DrawString(alamat, font, new SolidBrush(Color.Black), startX + tab2, startY + offset);
                            
                            graphic.DrawString("Beban Tetap", font, new SolidBrush(Color.Black), startX + tab3, startY + offset);
                            graphic.DrawString(":", font, new SolidBrush(Color.Black), startX + tab4, startY + offset);
                            graphic.DrawString(string.Format("{0:N0}", reader["beban"]), font, new SolidBrush(Color.Black), new Rectangle(startX + tab5, startY + offset, recWidth, recHeight), stringFormat);

                            offset += spacing;

                            graphic.DrawString("Perumahan Laksana Mekar Asri Padalarang", font, new SolidBrush(Color.Black), startX + tab2, startY + offset);

                            graphic.DrawString("Biaya Pemakaian", font, new SolidBrush(Color.Black), startX + tab3, startY + offset);
                            graphic.DrawString(":", font, new SolidBrush(Color.Black), startX + tab4, startY + offset);
                            graphic.DrawString(string.Format("{0:N0}", reader["biaya_pemakaian"]), font, new SolidBrush(Color.Black), new Rectangle(startX + tab5, startY + offset, recWidth, recHeight), stringFormat);

                            offset += spacing;

                            graphic.DrawString("Tanggal Pembayaran", font, new SolidBrush(Color.Black), startX, startY + offset);
                            graphic.DrawString(":", font, new SolidBrush(Color.Black), startX + tab1, startY + offset);
                            graphic.DrawString(tglPembayaran, font, new SolidBrush(Color.Black), startX + tab2, startY + offset);

                            graphic.DrawString("Denda", font, new SolidBrush(Color.Black), startX + tab3, startY + offset);
                            graphic.DrawString(":", font, new SolidBrush(Color.Black), startX + tab4, startY + offset);
                            graphic.DrawString(string.Format("{0:N0}", reader["denda"]), font, new SolidBrush(Color.Black), new Rectangle(startX + tab5, startY + offset, recWidth, recHeight), stringFormat);

                            offset += spacing;
                            
                            graphic.DrawString("TOTAL", font, new SolidBrush(Color.Black), startX + tab3, startY + offset);
                            graphic.DrawString(":", font, new SolidBrush(Color.Black), startX + tab4, startY + offset);
                            graphic.DrawString(string.Format("{0:N0}", reader["jumlah"]), fontUnderline, new SolidBrush(Color.Black), new Rectangle(startX + tab5, startY + offset, recWidth, recHeight), stringFormat);

                            offset += 40;

                            //Footer
                            int tabFooter1 = 80;
                            int tabFooter2 = 110;
                            int tabFooter3 = 230;
                            int tabFooter4 = 310;
                            int tabFooter5 = 340;

                            graphic.DrawString("Daftar Tarif : ", footerBold, new SolidBrush(Color.Black), startX, startY + offset);
                            offset += spacing;

                            graphic.DrawString("Beban Tetap", footer, new SolidBrush(Color.Black), startX, startY + offset);
                            graphic.DrawString(":", footer, new SolidBrush(Color.Black), startX + tabFooter1, startY + offset);
                            graphic.DrawString(tarif["beban"].Replace('.', ','), footer, new SolidBrush(Color.Black), startX + tabFooter2, startY + offset);

                            graphic.DrawString("16-20 m3", footer, new SolidBrush(Color.Black), startX + tabFooter3, startY + offset);
                            graphic.DrawString(":", footer, new SolidBrush(Color.Black), startX + tabFooter4, startY + offset);
                            graphic.DrawString(tarif["tarif3"].Replace('.', ','), footer, new SolidBrush(Color.Black), startX + tabFooter5, startY + offset);

                            offset += spacing;

                            graphic.DrawString("0 - 10 m3", footer, new SolidBrush(Color.Black), startX, startY + offset);
                            graphic.DrawString(":", footer, new SolidBrush(Color.Black), startX + tabFooter1, startY + offset);
                            graphic.DrawString(tarif["tarif1"].Replace('.', ','), footer, new SolidBrush(Color.Black), startX + tabFooter2, startY + offset);

                            graphic.DrawString("21-25 m3", footer, new SolidBrush(Color.Black), startX + tabFooter3, startY + offset);
                            graphic.DrawString(":", footer, new SolidBrush(Color.Black), startX + tabFooter4, startY + offset);
                            graphic.DrawString(tarif["tarif4"].Replace('.', ','), footer, new SolidBrush(Color.Black), startX + tabFooter5, startY + offset);

                            offset += spacing;

                            graphic.DrawString("11 - 15 m3", footer, new SolidBrush(Color.Black), startX, startY + offset);
                            graphic.DrawString(":", footer, new SolidBrush(Color.Black), startX + tabFooter1, startY + offset);
                            graphic.DrawString(tarif["tarif2"].Replace('.', ','), footer, new SolidBrush(Color.Black), startX + tabFooter2, startY + offset);

                            graphic.DrawString("> 25 m3", footer, new SolidBrush(Color.Black), startX + tabFooter3, startY + offset);
                            graphic.DrawString(":", footer, new SolidBrush(Color.Black), startX + tabFooter4, startY + offset);
                            graphic.DrawString(tarif["tarif5"].Replace('.',','), footer, new SolidBrush(Color.Black), startX + tabFooter5, startY + offset);

                            offset += 40;

                            graphic.DrawString("* Pembayaran air artesis dimulai setelah tanggal 5", footer_small, new SolidBrush(Color.Black), startX, startY + offset);

                            offset += 10;
                            graphic.DrawString("* Pembayaran air artesis setelah tanggal 20 dikenakan denda keterlambatan", footer_small, new SolidBrush(Color.Black), startX, startY + offset);
                        }
                    }
                }
                conn.Close();
            }            
        }
        private void tentangToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //DialogResult dr = new DialogResult();

            About about = new About();

            about.ShowDialog();

        }

        private void ubahPasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = new DialogResult();

            ubahPasswordFrm ubahPasswordDialog = new ubahPasswordFrm();

            //FrmAnggota addMemberDialog = new FrmAnggota();
            //addMemberDialog.isUpdate.Text = "0";

            dr = ubahPasswordDialog.ShowDialog();

            if (dr == DialogResult.OK)
            {
                this.loadMember();
            }
        }

        private void pengaturanTarifToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = new DialogResult();

            tarifFrm tarifDialog = new tarifFrm();

            dr = tarifDialog.ShowDialog();

            if (dr == DialogResult.OK)
            {
                this.prepareTarif();
            }
        }

        private void prepareTarif()
        {
            using (SQLiteConnection conn = new SQLiteConnection(@"Data Source =" + Program.path_db))
            {
                conn.Open();
                String command = "SELECT * FROM tarif";
                using (SQLiteCommand cmd = new SQLiteCommand(command, conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            bebanTetap = reader.GetInt32(0);
                            dendaSet = reader.GetInt32(1);

                            biayaBeban[1] = reader.GetInt32(2);
                            biayaBeban[11] = reader.GetInt32(3);
                            biayaBeban[16] = reader.GetInt32(4);
                            biayaBeban[21] = reader.GetInt32(5);
                            biayaBeban[25] = reader.GetInt32(6);

                            Dictionary<string, string> tarifDict = new Dictionary<string, string>();

                            tarifDict.Add("beban", string.Format("{0:N0}", reader["beban_tetap"]));
                            tarifDict.Add("denda", string.Format("{0:N0}", reader["denda"]));
                            tarifDict.Add("tarif1", string.Format("{0:N0}", reader["tarif1"]));
                            tarifDict.Add("tarif2", string.Format("{0:N0}", reader["tarif2"]));
                            tarifDict.Add("tarif3", string.Format("{0:N0}", reader["tarif3"]));
                            tarifDict.Add("tarif4", string.Format("{0:N0}", reader["tarif4"]));
                            tarifDict.Add("tarif5", string.Format("{0:N0}", reader["tarif5"]));

                            jsonTarif = tarifDict.FromDictionaryToJson();
                        }
                    }
                }
                conn.Close();
            }
        }

        private void setTarifDetail()
        {
            tarif1Lbl.Text = usage[1].ToString("N") + " x " + biayaBeban[1].ToString("N0");
            tarif11Lbl.Text = usage[11].ToString("N") + " x " + biayaBeban[11].ToString("N0");
            tarif16Lbl.Text = usage[16].ToString("N") + " x " + biayaBeban[16].ToString("N0");
            tarif21Lbl.Text = usage[21].ToString("N") + " x " + biayaBeban[21].ToString("N0");
            tarif25Lbl.Text = usage[25].ToString("N") + " x " + biayaBeban[25].ToString("N0");

            sum1Lbl.Text = (usage[1] * biayaBeban[1]).ToString("N0");
            sum11Lbl.Text = (usage[11] * biayaBeban[11]).ToString("N0");
            sum16Lbl.Text = (usage[16] * biayaBeban[16]).ToString("N0");
            sum21Lbl.Text = (usage[21] * biayaBeban[21]).ToString("N0");
            sum25Lbl.Text = (usage[25] * biayaBeban[25]).ToString("N0");

            totalSumLbl.Text = bebanBayar.ToString("N0");
        }

        public int maxInvoiceNumber(String suffix)
        {
            using (SQLiteConnection conn = new SQLiteConnection(@"Data Source =" + Program.path_db))
            {
                conn.Open();
                string command = "SELECT MAX(no_invoice) as maks_no FROM pembayaran WHERE invoice_suffix = " + suffix;

                using (SQLiteCommand cmd = new SQLiteCommand(command, conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader.GetInt32(0);
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
            }
        }

        public static string ToRoman(int number)
        {
            if ((number < 0) || (number > 3999)) throw new ArgumentOutOfRangeException("insert value betwheen 1 and 3999");
            if (number < 1) return string.Empty;
            if (number >= 1000) return "M" + ToRoman(number - 1000);
            if (number >= 900) return "CM" + ToRoman(number - 900); //EDIT: i've typed 400 instead 900
            if (number >= 500) return "D" + ToRoman(number - 500);
            if (number >= 400) return "CD" + ToRoman(number - 400);
            if (number >= 100) return "C" + ToRoman(number - 100);
            if (number >= 90) return "XC" + ToRoman(number - 90);
            if (number >= 50) return "L" + ToRoman(number - 50);
            if (number >= 40) return "XL" + ToRoman(number - 40);
            if (number >= 10) return "X" + ToRoman(number - 10);
            if (number >= 9) return "IX" + ToRoman(number - 9);
            if (number >= 5) return "V" + ToRoman(number - 5);
            if (number >= 4) return "IV" + ToRoman(number - 4);
            if (number >= 1) return "I" + ToRoman(number - 1);
            throw new ArgumentOutOfRangeException("something bad happened");
        }

        private void harianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = new DialogResult();

            reportHarian reportHr = new reportHarian();

            dr = reportHr.ShowDialog();

            if (dr == DialogResult.OK)
            {
                //this.prepareTarif();
            }
        }

        private void bulananToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = new DialogResult();

            reportBulanan reportBln = new reportBulanan();

            dr = reportBln.ShowDialog();

            if (dr == DialogResult.OK)
            {
                //this.prepareTarif();
            }
        }

        private void pelangganToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = new DialogResult();

            reportPelanggan reportPelanggan = new reportPelanggan();

            dr = reportPelanggan.ShowDialog();

            if (dr == DialogResult.OK)
            {
                //this.prepareTarif();
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "Excel Files (.xls, .xlsx)|*.xls;*.xlsx";
            openFileDialog1.FilterIndex = 1;


            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                String filename = openFileDialog1.FileName;

                Excel.Application oXL = new Excel.Application();
                object misValue = System.Reflection.Missing.Value;


                oXL.DisplayAlerts = false;

                Excel.Workbook oWB = oXL.Workbooks.Open(filename);
                Excel.Worksheet oSheet = oWB.Worksheets.get_Item(1);

                Excel.Range last = oSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);

                int lastRow = last.Row;

                //int lastColumn = last.Column;



                using (SQLiteConnection conn = new SQLiteConnection(@"Data Source =" + Program.path_db))
                {
                    conn.Open();

                    for (int i = 2; i <= lastRow; i++)
                    {
                        System.Array data = (System.Array)oSheet.get_Range("A" + i.ToString(), "E" + i.ToString()).Cells.Value;

                        if (data.GetValue(1, 1) != null && data.GetValue(1, 2) != null && data.GetValue(1, 3) != null && data.GetValue(1, 4) != null && data.GetValue(1, 5) != null)
                        {
                            FrmAnggota frmAnggota = new FrmAnggota();
                            int urut_rt = frmAnggota.maxUrutRT(data.GetValue(1, 5).ToString()) + 1;

                            String insertCmd = "INSERT INTO members(urut_rt, nama, telp, blok, no_rumah, rt, active, created_at) ";
                            insertCmd += "VALUES (" + urut_rt + ", '" + data.GetValue(1, 1).ToString() + "', '" + data.GetValue(1, 2).ToString() + "', '" + data.GetValue(1, 3).ToString() + "',";
                            insertCmd += "'" + data.GetValue(1, 4).ToString() + "', '" + data.GetValue(1, 5).ToString() + "', 1, datetime('now'))";

                            Console.WriteLine(insertCmd);

                            using (SQLiteCommand cmd = new SQLiteCommand(insertCmd, conn))
                            {
                                cmd.ExecuteNonQuery();
                            }
                        }
                        
                    }

                    conn.Close();
                }

                MessageBox.Show("Data Anggota telah Ditambahkan");

                oWB.Close();
                    
                oXL.Quit();
            }

            this.loadMember();

        }

        private void dgvMeteran_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //Disini
            string member_id = dgvMeteran.CurrentRow.Cells["id"].Value.ToString();
            string tanggal = CBTahun.SelectedItem + "-" + cbBulan.SelectedValue + "-01";

            if (e.ColumnIndex == 9)
            {
                //Print Kuitansi Individual

                tanggalTagihan = tanggal;
                memberidBayar = member_id;


                PrintDialog printDialog = new PrintDialog();

                PrintDocument printDoc = new PrintDocument();

                printDialog.Document = printDoc;

                printDoc.PrintPage += new PrintPageEventHandler(printDoc_PrintPage);

                DialogResult result = printDialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    printDoc.Print();
                }
            }
            else if (e.ColumnIndex == 10)
            {
                //MessageBox.Show("View Kakak - " + member_id + " - " + bulan + " - " + tahun);
                viewBayarFrm viewFrm = new viewBayarFrm();

                String command = "SELECT * FROM members u ";
                command += "LEFT JOIN meteran m ON u.id = m.member_id AND tanggal = '" + tanggal + "' ";
                command += "LEFT JOIN pembayaran p ON p.meteran_id = m.id ";
                command += "WHERE u.id = '" + member_id + "'";


                using (SQLiteConnection conn = new SQLiteConnection(@"Data Source =" + Program.path_db))
                {
                    conn.Open();
                    
                    SQLiteCommand query = new SQLiteCommand(command, conn);

                    using (SQLiteDataReader reader = query.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            DateTime tgl_bayar = reader.GetDateTime(24);
                            
                            viewFrm.kuitansiTxt.Text = string.Format("{0:000}", reader["no_invoice"]) + reader["invoice_suffix"].ToString();
                            viewFrm.namaTxt.Text = reader["nama"].ToString();
                            viewFrm.tanggalTxt.Text = String.Format(new System.Globalization.CultureInfo("id-ID"), "{0:dd MMMM yyyy}", tgl_bayar);
                            viewFrm.urutTxt.Text = string.Format("{0:00}", reader["urut_rt"]) + "/" + reader["rt"].ToString();
                            viewFrm.alamatTxt.Text = "Blok " + reader["blok"].ToString() + " No. " + reader["no_rumah"].ToString();

                            viewFrm.awalTxt.Text = string.Format("{0:N}", reader["awal"]);
                            viewFrm.akhirTxt.Text = string.Format("{0:N}", reader["akhir"]);

                            double pemakaian = reader.GetDouble(14) - reader.GetDouble(13);

                            viewFrm.pemakaianTxt.Text = string.Format("{0:N}", pemakaian);

                            viewFrm.bebanTxt.Text = string.Format("{0:N}", reader.GetDouble(25) );
                            viewFrm.dendaTxt.Text = string.Format("{0:N}", reader.GetDouble(26));
                            viewFrm.subtotalTxt.Text = string.Format("{0:N}", reader.GetDouble(27));

                            viewFrm.totalTxt.Text = string.Format("{0:N}", reader.GetDouble(23));

                            double biayaPemakaian = this.biayaPemakaian(pemakaian);

                            viewFrm.totalSumLbl.Text = string.Format("{0:N}", reader.GetDouble(27));

                            viewFrm.tarif1Lbl.Text = usage[1].ToString("N") + " x " + biayaBeban[1].ToString("N0");
                            viewFrm.tarif11Lbl.Text = usage[11].ToString("N") + " x " + biayaBeban[11].ToString("N0");
                            viewFrm.tarif16Lbl.Text = usage[16].ToString("N") + " x " + biayaBeban[16].ToString("N0");
                            viewFrm.tarif21Lbl.Text = usage[21].ToString("N") + " x " + biayaBeban[21].ToString("N0");
                            viewFrm.tarif25Lbl.Text = usage[25].ToString("N") + " x " + biayaBeban[25].ToString("N0");

                            viewFrm.sum1Lbl.Text = (usage[1] * biayaBeban[1]).ToString("N0");
                            viewFrm.sum11Lbl.Text = (usage[11] * biayaBeban[11]).ToString("N0");
                            viewFrm.sum16Lbl.Text = (usage[16] * biayaBeban[16]).ToString("N0");
                            viewFrm.sum21Lbl.Text = (usage[21] * biayaBeban[21]).ToString("N0");
                            viewFrm.sum25Lbl.Text = (usage[25] * biayaBeban[25]).ToString("N0");                            
                        }
                    }
                }

                viewFrm.ShowDialog();                
            }
        }

        private void exportBtn_Click(object sender, EventArgs e)
        {
            //Buat Export Data Pelanggan
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.FileName = "Data Pelanggan.xls";
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

                        oSheet.Cells[1, 1] = "Data Pelanggan";
                        oSheet.Cells[1, 1].Font.Bold = true;
                        oSheet.Range[oSheet.Cells[1, 1], oSheet.Cells[1, 6]].Merge();
                        oSheet.Cells[1, 1].Style.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                        int init_row = 3;
                        oSheet.Cells[1, 1].ColumnWidth = 4;
                        oSheet.Cells[1, 2].ColumnWidth = 10;
                        oSheet.Cells[1, 3].ColumnWidth = 24;
                        oSheet.Cells[1, 4].ColumnWidth = 30;
                        oSheet.Cells[1, 5].ColumnWidth = 10;
                        oSheet.Cells[1, 6].ColumnWidth = 15;
                        
                        //Add table headers going cell by cell.
                        oSheet.Cells[init_row, 1] = "No.";
                        oSheet.Cells[init_row, 2] = "No. Pelanggan";
                        oSheet.Cells[init_row, 3] = "Nama";
                        oSheet.Cells[init_row, 4] = "Alamat";
                        oSheet.Cells[init_row, 5] = "RT";
                        oSheet.Cells[init_row, 6] = "Telepon";                        
                        oSheet.Range[oSheet.Cells[init_row, 1], oSheet.Cells[init_row, 6]].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                        oSheet.Range[oSheet.Cells[init_row, 1], oSheet.Cells[init_row, 6]].Style.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                        oSheet.Range[oSheet.Cells[init_row, 1], oSheet.Cells[init_row, 6]].Style.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                        oSheet.Range[oSheet.Cells[init_row, 1], oSheet.Cells[init_row, 6]].WrapText = true;

                        oSheet.Cells[init_row, 1].EntireRow.Font.Bold = true;
                        oSheet.Cells[init_row, 1].RowHeight = 18;

                        init_row++;

                        using (SQLiteConnection conn = new SQLiteConnection(@"Data Source =" + Program.path_db))
                        {
                            conn.Open();

                            String query = "SELECT * FROM members WHERE active = 1 ORDER BY rt, urut_rt";                            

                            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                            {
                                using (SQLiteDataReader reader = cmd.ExecuteReader())
                                {
                                    int no = 1;
                                    while (reader.Read())
                                    {
                                        oSheet.Cells[init_row, 1] = no;
                                        oSheet.Cells[init_row, 2] = "'" + string.Format("{0:00}", reader.GetValue(1)) + "." + reader["rt"].ToString();
                                        oSheet.Cells[init_row, 3] = reader["nama"].ToString(); //Nama
                                        oSheet.Cells[init_row, 4] = "Blok " + reader["blok"].ToString() + " No. " + reader["no_rumah"].ToString(); //Nama
                                        oSheet.Cells[init_row, 5] = "'" + reader.GetValue(6).ToString() + "/09";
                                        oSheet.Cells[init_row, 6] = reader["telp"].ToString(); //Nama

                                        no++;
                                        init_row++;
                                    }
                                }
                            }
                            conn.Close();
                        }

                        init_row--;

                        oSheet.get_Range("C4", "F" + init_row).Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                        oSheet.get_Range("E4", "E" + init_row).Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                        oSheet.get_Range("A3", "F" + init_row).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                        //Make sure Excel is visible and give the user control
                        //of Microsoft Excel's lifetime.
                        oXL.Visible = true;
                        oXL.UserControl = true;

                        String filename = sfd.FileName;
                        oWB.SaveAs(filename);
                        //oWB.Close();

                        MessageBox.Show("Data Pelanggan Telah Disimpan");
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
        }

        private void tahunanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Laporan Tahunan
            DialogResult dr = new DialogResult();

            reportTahunan reportTahunan = new reportTahunan();

            dr = reportTahunan.ShowDialog();

            if (dr == DialogResult.OK)
            {
                //this.prepareTarif();
            }
        }
    
    }

    public static class Extensions
    {
        public static string FromDictionaryToJson(this Dictionary<string, string> dictionary)
        {
            var kvs = dictionary.Select(kvp => string.Format("\"{0}\":\"{1}\"", kvp.Key, string.Join(",", kvp.Value)));
            return string.Concat("{", string.Join(",", kvs), "}");
        }

        public static Dictionary<string, string> FromJsonToDictionary(this string json)
        {
            string[] keyValueArray = json.Replace("{", string.Empty).Replace("}", string.Empty).Replace("\"", string.Empty).Split(',');
            return keyValueArray.ToDictionary(item => item.Split(':')[0], item => item.Split(':')[1]);
        }
    }
}
