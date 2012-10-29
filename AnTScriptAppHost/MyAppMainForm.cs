﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using AnTScriptAppLibrary;

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
            AnTScript.Engine.ExecuteCode(textFileSourceCode.Text, new MyLibrary());

            // With MyLbrary reference, without Output device
            //AnTScript.Engine.ExecuteCode(textFileSourceCode.Text, new MyLibrary(), false);

            // With code and MyLbrary reference
            //AnTScript.Engine.ExecuteCode("var a = 123; printline a; print DemoSumNum(111,222);", new MyLibrary());
        }

        private void buttonShowConsole_Click(object sender, EventArgs e)
        {
            // Simple 
            //AnTScript.Engine.ShowConsole(textFileSourceCode.Text);

            // With MyLbrary reference
            AnTScript.Engine.ShowConsole(textFileSourceCode.Text, new MyLibrary());

            // With code and MyLbrary reference
            //AnTScript.Engine.ShowConsole("var a = 123; printline a; print DemoSumNum(111,222);", new MyLibrary());
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            object a = true;


        }

    }
}
