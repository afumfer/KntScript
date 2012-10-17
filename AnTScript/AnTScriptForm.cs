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

namespace AnTScript
{

    internal partial class AnTScriptForm : Form
    {
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
            if(string.IsNullOrEmpty(sourceCode))
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