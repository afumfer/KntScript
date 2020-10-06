﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using AnTScript;

namespace AnTScriptAppHost
{
    internal partial class InOutDefaultDeviceForm : Form, IInOutDevice
    {
        private bool FlagClose = false;

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
            ReadVarForm f = new ReadVarForm(readVarItmes);
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

        public void LockForm(bool lockFrm)
        {            
            FlagClose = lockFrm;
        }

        #endregion

        private void InOutDefaultDeviceForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (FlagClose)
            {
                MessageBox.Show("No se pude cerrar esta ventana hasta finalizar la ejecución del script.", "ANotas");
                e.Cancel = true;
            }
        }

        private void InOutDefaultDeviceForm_Load(object sender, EventArgs e)
        {

        }
    }
}