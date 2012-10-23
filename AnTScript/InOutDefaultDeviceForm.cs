using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AnTScript
{
    internal partial class InOutDefaultDeviceForm : Form, IInOutDevice
    {
        public InOutDefaultDeviceForm()
        {
            InitializeComponent();
        }

        #region IInOutDevice members

        public void Print(string str, bool newLine = false)
        {
            textOut.AppendText(@str);
            if (newLine)
                textOut.AppendText("\r\n");
        }

        public bool ReadVars(List<ReadVarItem> readVarItmes)
        {
            AnTScript.ReadVarForm f = new AnTScript.ReadVarForm(readVarItmes);
            if (f.ShowDialog() == DialogResult.OK)
            {
                readVarItmes = f.ReadVarItems;
                return true;
            }
            else
                return false;

        }

        public void Clear()
        {
            textOut.Clear();
        }
       
        public void SetEmbeddedMode()
        {
            this.TopLevel = false;
            this.FormBorderStyle = FormBorderStyle.None;            
            this.Dock = DockStyle.Fill;
        }

        public string GetOutContent()
        {
            return textOut.Text;
        }

        #endregion

    }
}
