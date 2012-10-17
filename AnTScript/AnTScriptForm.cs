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
        private const int EM_SETTABSTOPS = 0x00CB; 
        [DllImport("User32.dll", CharSet = CharSet.Auto)] 
        public static extern IntPtr SendMessage(IntPtr h, int msg, int wParam, int [] lParam);

        private string sourceCode = string.Empty;

        private InOutDefaultDeviceForm inOutDeviceForm;

        Dictionary<string, Type> typeCache = new Dictionary<string,Type>(); 


        public AnTScriptForm(string source)
        {
            InitializeComponent();

            sourceCode = source;            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // define value of the Tab indent  
            int[] stops = { 12 };
            // change the indent  
            SendMessage(this.textSourceCode.Handle, EM_SETTABSTOPS, 1, stops); 

            if (File.Exists(sourceCode))
            {
                LoadFile(sourceCode);
                statusLabel1.Text = sourceCode;
            }
            else
                textSourceCode.Text = sourceCode;

            inOutDeviceForm = new InOutDefaultDeviceForm();
            inOutDeviceForm.TopLevel = false;
            inOutDeviceForm.FormBorderStyle = FormBorderStyle.None;
            splitContainer1.Panel2.Controls.Add(inOutDeviceForm);
            inOutDeviceForm.Dock = DockStyle.Fill;
            inOutDeviceForm.Show();          
        }

        private void buttonRun_Click(object sender, EventArgs e)
        {
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

        private void LoadFile(string sourceCode)
        {
            if (string.IsNullOrEmpty(sourceCode))
                return;

            // TODO: pendiente validar que el nombre del fichero sea correcto. 
            using (TextReader input = File.OpenText(sourceCode))
            {
                textSourceCode.Text = input.ReadToEnd().ToString();
                textSourceCode.Select(0, 0);
            }
        }

        private void AnTScriptForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F5)
            {
                buttonRun_Click(this, new EventArgs());
            }

        }

    }

}