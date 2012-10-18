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

        private const int EM_SETTABSTOPS = 0x00CB; 
        [DllImport("User32.dll", CharSet = CharSet.Auto)] 
        public static extern IntPtr SendMessage(IntPtr h, int msg, int wParam, int [] lParam);

        private string sourceCode = string.Empty;
        private string sourceCodeDirWork = string.Empty;

        private InOutDefaultDeviceForm inOutDeviceForm;

        private Dictionary<string, Type> typeCache = new Dictionary<string,Type>();

        #endregion

        #region Constructor

        public AnTScriptForm(string source)
        {
            InitializeComponent();

            sourceCode = source;            
        }

        #endregion

        #region Form events controllers

        private void Form1_Load(object sender, EventArgs e)
        {
            // define value of the Tab indent and change the indent
            int[] stops = { 12 };            
            SendMessage(this.textSourceCode.Handle, EM_SETTABSTOPS, 1, stops);

            if (File.Exists(sourceCode))                        
                LoadFile(sourceCode);            
            else
            {
                textSourceCode.Text = sourceCode;
                sourceCode = "";
                statusFileName.Text = "";
            }

            inOutDeviceForm = new InOutDefaultDeviceForm();
            inOutDeviceForm.TopLevel = false;
            inOutDeviceForm.FormBorderStyle = FormBorderStyle.None;
            splitContainer1.Panel2.Controls.Add(inOutDeviceForm);
            inOutDeviceForm.Dock = DockStyle.Fill;
            inOutDeviceForm.Show();          
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

            inOutDeviceForm.Clear();

            try
            {
                Engine antsEngine = new Engine(textSourceCode.Text, this.inOutDeviceForm);
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
            sourceCode = "";
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(sourceCodeDirWork))
                sourceCodeDirWork = Application.StartupPath;
            openFileDialogScript.Title = "Open AnTScript file";
            openFileDialogScript.InitialDirectory = sourceCodeDirWork;
            openFileDialogScript.Filter = "AnTScript file (*.ants)|*.ants";
            openFileDialogScript.FileName = "";
            openFileDialogScript.CheckFileExists = true;

            if (openFileDialogScript.ShowDialog() == DialogResult.OK)            
                LoadFile(openFileDialogScript.FileName);                           
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(sourceCode))
            {
                saveFileDialogScript.Title = "Open AnTScript file";
                saveFileDialogScript.InitialDirectory = sourceCodeDirWork;
                saveFileDialogScript.Filter = "AnTScript file (*.ants)|*.ants";
                saveFileDialogScript.FileName = "";
                saveFileDialogScript.CheckFileExists = true;

                if (saveFileDialogScript.ShowDialog() == DialogResult.OK)
                {
                    if (Path.GetExtension(saveFileDialogScript.FileName) == "")
                        saveFileDialogScript.FileName += @".ants";
                    SaveFile(saveFileDialogScript.FileName);
                    sourceCode = saveFileDialogScript.FileName;
                    statusFileName.Text = sourceCode;
                }
            }
            else
                SaveFile(sourceCode);
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
            sourceCode = sourceCodeFile;
            sourceCodeDirWork = Path.GetDirectoryName(sourceCodeFile);
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