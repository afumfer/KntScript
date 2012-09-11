using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AnTScript
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);                      

            // Debug
            //if (args.Length != 1)
            //{
            //    Console.WriteLine("Usage: gfn.exe program.gfn");
            //    return;
            //}

            string strFich = args[0];
           
            if (string.IsNullOrEmpty(strFich))
                strFich = Application.StartupPath + @"\test.ats";

            Application.Run(new AnTScriptForm(strFich));
       
        }
    }
}
