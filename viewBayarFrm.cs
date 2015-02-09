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
    public partial class viewBayarFrm : Form
    {
        //SQLiteConnection conn = new SQLiteConnection(@"Data Source ="+ Program.path_db);

        public viewBayarFrm()
        {
            InitializeComponent();        

        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

                                                                         
    }
}
