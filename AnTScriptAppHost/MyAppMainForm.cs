using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using AnTScriptAppLibrary;
using System.Threading;

namespace AnTScriptAppHost
{
    public partial class MyAppMainForm : Form
    {
        private string sourceCode = string.Empty;

        public MyAppMainForm(string sourceFile)
        {
            InitializeComponent();
            sourceCode = sourceFile;
        }
       
        private void Form1_Load(object sender, EventArgs e)
        {
            textFileSourceCode.Text = sourceCode;

            // Reference to an object of the application is required
            Document doc = new Document();
            doc.IdDocument = 111;
        }

        private void buttonRunScript_Click(object sender, EventArgs e)
        {
            // Simple 
            //AnTScript.Engine.ExecuteCode(textFileSourceCode.Text);

            // With MyLbrary reference
            AnTScript.AnTSEngine.ExecuteCode(textFileSourceCode.Text, new MyLibrary());

            // With MyLbrary reference, without Output device
            //AnTScript.Engine.ExecuteCode(textFileSourceCode.Text, new MyLibrary(), false);

            // With code and MyLbrary reference
            //AnTScript.Engine.ExecuteCode("var a = 123; printline a; print DemoSumNum(111,222);", new MyLibrary());

            // With only code
            //            AnTScript.AnTSEngine.ExecuteCode(@"
            //                var a = 1;
            //                var b = ""hola mundo "";
            //                for a = 1 to 10
            //                    printline b + a;
            //                end for;
            //                printline ""<<fin>>"";             
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

        }

        private void buttonShowConsole_Click(object sender, EventArgs e)
        {
            // Simple 
            //AnTScript.Engine.ShowConsole(textFileSourceCode.Text);

            // With MyLbrary reference
            AnTScript.AnTSEngine.ShowConsole(textFileSourceCode.Text, new MyLibrary());

            // With code and MyLbrary reference
            //AnTScript.Engine.ShowConsole("var a = 123; printline a; print DemoSumNum(111,222);", new MyLibrary());

        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            ClassStatic2.TestPropStatic2 = "1111";


        }

    }
}
