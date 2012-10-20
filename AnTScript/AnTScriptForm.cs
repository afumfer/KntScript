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

        private string SourceCode {get; set;}
        private IInOutDevice InOutDevice { get; set; }
        private Library FunctionLibrary { get; set; }
        
        #endregion

        #region Constructor

        public AnTScriptForm(string source, IInOutDevice inOutDevice, Library functionLibrary)
        {
            InitializeComponent();

            this.SourceCode = source;
            this.InOutDevice = inOutDevice;
            this.FunctionLibrary = functionLibrary;
        }

        #endregion

        #region Form events controllers

        private void AnTScriptForm_Load(object sender, EventArgs e)
        {
            // define value of the Tab indent and change the indent
            int[] stops = { 12 };            
            SendMessage(this.textSourceCode.Handle, EM_SETTABSTOPS, 1, stops);

            if (File.Exists(SourceCode))                        
                LoadFile(SourceCode);            
            else
            {
                textSourceCode.Text = SourceCode;
                SourceCode = "";
                statusFileName.Text = "";
            }

            InOutDevice = new InOutDefaultDeviceForm();
            InOutDevice.SetEmbeddedMode();
            splitContainer1.Panel2.Controls.Add((Control)InOutDevice);
            InOutDevice.Show();          
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
                MessageBox.Show("No code to run", "AnTScript");
                return;
            }

            InOutDevice.Clear();

            try
            {
                Engine antsEngine = new Engine(textSourceCode.Text, this.InOutDevice, this.FunctionLibrary);
                antsEngine.Run();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
            
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            textSourceCode.Text = "";
            statusFileName.Text = "";
            SourceCode = "";
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
            if (string.IsNullOrEmpty(SourceCode))
            {
                saveFileDialogScript.Title = "Open AnTScript file";
                saveFileDialogScript.InitialDirectory = _sourceCodeDirWork;
                saveFileDialogScript.Filter = "AnTScript file (*.ants)|*.ants";
                saveFileDialogScript.FileName = "";
                saveFileDialogScript.CheckFileExists = true;

                if (saveFileDialogScript.ShowDialog() == DialogResult.OK)
                {
                    if (Path.GetExtension(saveFileDialogScript.FileName) == "")
                        saveFileDialogScript.FileName += @".ants";
                    SaveFile(saveFileDialogScript.FileName);
                    SourceCode = saveFileDialogScript.FileName;
                    statusFileName.Text = SourceCode;
                }
            }
            else
                SaveFile(SourceCode);
        }

        #endregion

        #region Private methods

        private void LoadFile(string sourceCodeFile)
        {
            if (string.IsNullOrEmpty(sourceCodeFile))
                return;
            
            using (TextReader input = File.OpenText(sourceCodeFile))            
                textSourceCode.Text = input.ReadToEnd().ToString();
            
            textSourceCode.Select(0, 0);
            statusFileName.Text = sourceCodeFile;
            SourceCode = sourceCodeFile;
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