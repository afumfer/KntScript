using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using AnTScript;

namespace AnTScriptAppHost
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

            string strFich = "";

            if (args.Length > 0)
                strFich = Application.StartupPath + @"\" + args[0];


            if (string.IsNullOrEmpty(strFich))
                strFich = Application.StartupPath + @"\test.ants";

            Application.Run(new MyAppMainForm(strFich));
        }
    }
}
