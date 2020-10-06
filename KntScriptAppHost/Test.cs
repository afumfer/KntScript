using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using KntScript;

namespace KntScriptAppHost
{
    public partial class Test : Form
    {
        public Test()
        {
            InitializeComponent();
        }

        private void buttonRunScript_Click(object sender, EventArgs e)
        {
            var kntScript = new KntSEngine(new InOutDeviceForm());

            kntScript.Run(@"
                            var a = 1;
                            var b = ""hola mundo "";
                            for a = 1 to 10
                                printline b + a;
                            end for;
                            printline ""<<fin>>"";
                            readvar {""Texto"": b };
                            printline b;
                            'CloseOutWindow();             
                        ");


            #region TODO: reescribir pruebas ....

            // With MyLbrary reference
            //AnTScript.AnTSEngine.ExecuteCode(textFileSourceCode.Text, new MyLibrary());

            // With MyLbrary reference, without Output device
            //AnTScript.Engine.ExecuteCode(textFileSourceCode.Text, new MyLibrary(), false);

            // With code and MyLbrary reference
            //AnTScript.Engine.ExecuteCode("var a = 123; printline a; print DemoSumNum(111,222);", new MyLibrary());

            // With only code
            //AnTScript.AnTSEngine.ExecuteCode(@"
            //                var a = 1;
            //                var b = ""hola mundo "";
            //                for a = 1 to 10
            //                    printline b + a;
            //                end for;
            //                printline ""<<fin>>"";
            //                readvar {""Texto"": b };
            //                CloseOutWindow();             
            //            ");


            //            // Another way to do it, with added variable
            //            var a = new Document();
            //            a.Description = "Bla bla";

            //            var engine = new AnTScript.AnTSEngine(@"
            //                var i = 0;                
            //                var b = ""hola mundo "";
            //                ' Esta variable viene de la aplicación anfitriona
            //                _a.Description = ""Otro valor para a.Description"";
            //                for i = 1 to 10
            //                    printline b + _a.Description;
            //                end for;
            //                printline ""<<fin>>""; 
            //                ");

            //            engine.AddVar("_a", a);

            //            engine.Run();

            //            var b = (Document)engine.GetVar("_a"); 
            //            MessageBox.Show(a.Description + " -- " + b.Description);

            #endregion 
        }

        private void buttonRunScriptFile_Click(object sender, EventArgs e)
        {
            var kntScript = new KntSEngine(new InOutDeviceForm());

            kntScript.RunFile(@"D:\Dev\KntScript\kntscript_dev\Samples\loop2.knts");
        }

        private void buttonShowConsole_Click(object sender, EventArgs e)
        {
            var kntEngine = new KntSEngine(new InOutDeviceForm());

            KntScriptConsoleForm f = new KntScriptConsoleForm(kntEngine);
            f.Show();
        }

    }
}
