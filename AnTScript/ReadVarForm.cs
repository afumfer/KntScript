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
    internal partial class ReadVarForm : Form
    {

        TextBox textFirst;

        private List<ReadVarItem> _readVarItems;
        internal List<ReadVarItem> ReadVarItems
        {
            get { return _readVarItems; }
        }
        

        internal ReadVarForm(List<ReadVarItem> readVarItems)
        {
            InitializeComponent();
            
            _readVarItems = readVarItems;
            
            GenControls();
        }

        private void ReadVarForm_Load(object sender, EventArgs e)
        {

        }

        private void GenControls()
        {
            TextBox textBox;
            Label label;
            int topText = 15;
            int leftText = 275;
            int topLabel = 15;
            int leftLabel = 10;
            int widthLabel = 250;
            int widthText = 300;
            int inc = 30;
            int i = 0;


            foreach (ReadVarItem var in _readVarItems)
            {
                label = new Label();
                label.Font = new Font(new FontFamily("Courier New"), 10);                
                label.Text = var.Label.ToString();
                label.Top = topLabel;
                label.Left = leftLabel;
                label.Width = widthLabel;
                
                textBox = new TextBox();
                textBox.Tag = var.VarIdent;
                textBox.Text = var.VarValue.ToString();
                textBox.Top = topText;
                textBox.Left = leftText;
                textBox.Width = widthText;
                textBox.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right)));
                
                panelControls.Controls.Add(label);
                panelControls.Controls.Add(textBox);

                if (i == 0)
                    textFirst = textBox;

                topText += inc;
                topLabel += inc;
                i++;
            }        

        }

        private void CaptureVars()
        {
            foreach(Control c in panelControls.Controls)
            {
                foreach(ReadVarItem var in _readVarItems)
                {
                    if(var.VarIdent == c.Tag)
                    {                        
                        var.VarNewValueText = c.Text;
                        break;
                    }
                }
            }
        }

        private void buttonAccept_Click(object sender, EventArgs e)
        {
            CaptureVars();                    
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

    }
}
