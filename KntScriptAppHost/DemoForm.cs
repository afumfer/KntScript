﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using KntScript;

namespace KntScriptAppHost
{
    public partial class DemoForm : Form
    {
        #region Private fields

        private string _pathSampleScripts = @"..\..\..\..\Samples\";
        private string _selectedFile;

        #endregion 

        public DemoForm()
        {
            InitializeComponent();
        }

        #region Form events handlers

        private void DemoForm_Load(object sender, EventArgs e)
        {
            LoadListScripts(_pathSampleScripts);
        }

        private void buttonRunScript_Click(object sender, EventArgs e)
        {
            var kntScript = new KntSEngine(new InOutDeviceForm());

            kntScript.Run(@"
                            var i = 1;
                            var str = ""Hello world "";
                            for i = 1 to 10
                                printline str + i;
                            end for;                            
                            printline """";
                            str = ""(type text here ...) "";
                            readvar {""Example input str var:"": str };
                            printline str;
                            printline ""<< end >>"";                            
                        ");            
        }

        private void buttonInteract_Click(object sender, EventArgs e)
        {            
            //
            // Demo, inject variables and personalized api library 
            //

            var kntScript = new KntSEngine(new InOutDeviceForm(), new MyLibrary());
            
            var a = new DocumentDummy();
            a.Description = "My object, to inject in script.";

            // inject variable
            kntScript.AddVar("_a", a);

            var code = @"printline ""Demo external variables / MyLibrary injected"";

                        ' This variable (_a) comes from the host application
                        printline _a.Description;
                        printline _a.IdDocument;
                        printline _a.CreationDateTime;
                        printline _a.Folder.Name;
                        _a.DocumentTestMethodA("" param A "");
                        var b = _a.DocumentTestMethodB("" == param C =="");
                        printline b;

                        ' Test MyLibrary (injected library)
                        printline """";
                        printline ""Test MyLibrary"";
                        var colec = ColecDocDemo();
                        foreach x in colec
                            printline x.Description;
                        end foreach;

                        printline """";
                        _a.Description = ""KntScript - changed description property !!"";                                                
                        printline _a.Description;
                        printline """";

                        printline ""<< end >>""; 
                        ";

            kntScript.Run(code);

            var b = (DocumentDummy)kntScript.GetVar("_a");  // -> a 
            MessageBox.Show(a.Description + " <==> " + b.Description);
        }

        private void buttonRunBackground_Click(object sender, EventArgs e)
        {
            var kntScript = new KntSEngine(new InOutDeviceForm());

            var code = @"
                        var i = 1;
                        var str = ""Hello world "";
                        for i = 1 to 3000
                            printline str + i;
                        end for;                            
                        printline ""<< end >>"";
                    ";

            // --- Synchronous version
            // kntScript.Run(code);

            // --- Asynchronous version
            var t = new Thread(() => kntScript.Run(code));
            t.IsBackground = false;
            t.Start();            
        }

        private void buttonShowConsole_Click(object sender, EventArgs e)
        {
            var kntEngine = new KntSEngine(new InOutDeviceForm(), new MyLibrary());

            KntScriptConsoleForm f = new KntScriptConsoleForm(kntEngine);
            f.Show();
        }

        private void buttonShowSample_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedFile))
            {
                MessageBox.Show("File no seleted.");
                return;
            }

            var kntEngine = new KntSEngine(new InOutDeviceForm(), new MyLibrary());

            KntScriptConsoleForm f = new KntScriptConsoleForm(kntEngine, _pathSampleScripts + _selectedFile);
            f.Show();
        }

        private void buttonRunSample_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedFile))
            {
                MessageBox.Show("File no seleted.");
                return;                   
            }

            var kntScript = new KntSEngine(new InOutDeviceForm(), new MyLibrary());

            kntScript.RunFile(_pathSampleScripts + _selectedFile);
        }

        private void listSamples_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedFile = listSamples.SelectedItem.ToString();
        }

        #endregion 

        #region Private methods

        private void LoadListScripts(string pathSampleScripts)
        {
            if (Directory.Exists(pathSampleScripts))
            {
                DirectoryInfo directory = new DirectoryInfo(pathSampleScripts);
                FileInfo[] files = directory.GetFiles("*.knts");
                
                foreach (var file in files)
                    listSamples.Items.Add(file.Name);
            }
            else            
                MessageBox.Show("{0} is not a valid directory.", _pathSampleScripts);            
        }

        #endregion
    }
}
