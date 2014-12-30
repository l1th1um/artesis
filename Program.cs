using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace Artesis
{
    static class Program
    {
        public static String salt = "qilauchuld4h";
        //public static String path_db = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\artesis.db";
        public static String path_db = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Artesis\artesis.db";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        
        static void Main()
        {
            

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            loginFrm loginFrm = new loginFrm();

            if (loginFrm.ShowDialog() == DialogResult.OK)
            {
                Application.Run(new Form1());
            }
            else
            {
                Application.Exit();
            }
            
        }
    }
}
