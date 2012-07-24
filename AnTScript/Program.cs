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

            if (args.Length != 1)
            {
                Console.WriteLine("Usage: gfn.exe program.gfn");
                return;
            }

            Application.Run(new AnTScriptForm(args[0]));
       
        }
    }
}
