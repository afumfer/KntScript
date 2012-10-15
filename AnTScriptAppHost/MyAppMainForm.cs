using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AnTScriptAppHost
{
    public partial class MyAppMainForm : Form
    {
        private string sourceCodeFile = string.Empty;

        public MyAppMainForm(string sourceFile)
        {
            InitializeComponent();
            sourceCodeFile = sourceFile;
        }
       
        private void Form1_Load(object sender, EventArgs e)
        {
            textFileSourceCode.Text = sourceCodeFile;       
        }

        private void buttonRunScript_Click(object sender, EventArgs e)
        {
            AnTScript.Engine.ExecuteCodeFile(sourceCodeFile);
            
            
        }

        private void buttonShowConsole_Click(object sender, EventArgs e)
        {
            AnTScript.Engine.ShowConsole(sourceCodeFile);                                  
        }

    }
}
