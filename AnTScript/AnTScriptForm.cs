using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.InteropServices;
using System.Threading;

namespace AnTScript
{
    internal partial class AnTScriptForm : Form
    {

        #region Private fields
        
        private string _sourceCodeDirWork = string.Empty;

        private const int EM_SETTABSTOPS = 0x00CB; 
        [DllImport("User32.dll", CharSet = CharSet.Auto)] 
        private static extern IntPtr SendMessage(IntPtr h, int msg, int wParam, int [] lParam);

        #endregion

        #region Properties

        public AnTSEngine Engine {get; set;} 
        
        #endregion

        #region Constructor

        public AnTScriptForm(AnTSEngine engine)
        {
            InitializeComponent();
            Engine = engine;
        }

        #endregion

        #region Form events controllers

        private void AnTScriptForm_Load(object sender, EventArgs e)
        {
            // define value of the Tab indent and change the indent
            int[] stops = { 12 };
            SendMessage(this.textSourceCode.Handle, EM_SETTABSTOPS, 1, stops);
                       
            textSourceCode.Text = Engine.SourceCode;
            statusFileName.Text = Engine.SourceCodeFile;
            if(!string.IsNullOrEmpty(Engine.SourceCodeFile))
                _sourceCodeDirWork = Path.GetDirectoryName(Engine.SourceCodeFile);
            
            Engine.InOutDevice.SetEmbeddedMode();
            splitContainer1.Panel2.Controls.Add((Control)Engine.InOutDevice);
            Engine.InOutDevice.Show();
            textSourceCode.Select(0, 0);            
        }

        private void AnTScriptForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F5)
                buttonRun_Click(this, new EventArgs());
        }

        private void buttonRun_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textSourceCode.Text.Trim()))
            {
                MessageBox.Show("No code found to run", "AnTScript");
                return;
            }

            try
            {
                toolStrip1.Enabled = false;

                Engine.InOutDevice.Clear();
                Engine.SourceCode = textSourceCode.Text;
                Engine.ClearAllVars();
                Engine.Run();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
            finally
            {
                toolStrip1.Enabled = true;
            }
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            textSourceCode.Text = "";
            statusFileName.Text = "";
            Engine.SourceCode = "";
            Engine.SourceCodeFile = "";
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_sourceCodeDirWork))
                _sourceCodeDirWork = Application.StartupPath;
            openFileDialogScript.Title = "Open AnTScript file";
            openFileDialogScript.InitialDirectory = _sourceCodeDirWork;
            openFileDialogScript.Filter = "AnTScript file (*.ants)|*.ants";
            openFileDialogScript.FileName = "";
            openFileDialogScript.CheckFileExists = true;

            if (openFileDialogScript.ShowDialog() == DialogResult.OK)            
                LoadFile(openFileDialogScript.FileName);                           
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Engine.SourceCodeFile))
            {
                saveFileDialogScript.Title = "Save AnTScript file";
                saveFileDialogScript.InitialDirectory = _sourceCodeDirWork;
                saveFileDialogScript.Filter = "AnTScript file (*.ants)|*.ants";
                saveFileDialogScript.FileName = "";

                if (saveFileDialogScript.ShowDialog() == DialogResult.OK)
                {
                    if (Path.GetExtension(saveFileDialogScript.FileName) == "")
                        saveFileDialogScript.FileName += @".ants";
                    SaveFile(saveFileDialogScript.FileName);
                    Engine.SourceCodeFile = saveFileDialogScript.FileName;
                    statusFileName.Text = Engine.SourceCodeFile;
                }
            }
            else
                SaveFile(Engine.SourceCodeFile);
        }

        #endregion

        #region Private methods

        private void LoadFile(string sourceCodeFile)
        {
            if (string.IsNullOrEmpty(sourceCodeFile))
                return;

            using (TextReader input = File.OpenText(sourceCodeFile))
                textSourceCode.Text = input.ReadToEnd().ToString();

            Engine.SourceCodeFile = sourceCodeFile;
            Engine.SourceCode = textSourceCode.Text;
            statusFileName.Text = sourceCodeFile;            

            textSourceCode.Select(0, 0);            
            _sourceCodeDirWork = Path.GetDirectoryName(sourceCodeFile);
        }

        private void SaveFile(string sourceCodeFile)
        {
            try
            {
                File.WriteAllLines(sourceCodeFile, textSourceCode.Lines);                
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        #endregion

    }

}