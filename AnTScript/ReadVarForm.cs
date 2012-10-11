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
    public partial class ReadVarForm : Form
    {
        
        private List<ReadVarItem> _readVarItems;
        public List<ReadVarItem> ReadVarItems
        {
            get { return _readVarItems; }
        }
        

        public ReadVarForm(List<ReadVarItem> readVarItems)
        {
            InitializeComponent();
            
            _readVarItems = readVarItems;
            
            GenControls();
        }

        private void GenControls()
        {
            TextBox textBox;
            Label label;
            int topText = 15;
            int leftText = 175;
            int topLabel = 15;
            int leftLabel = 10;
            int widthLabel = 150;
            int widthText = 300;
            int inc = 30;

            foreach (ReadVarItem var in _readVarItems)
            {
                label = new Label();
                label.Text = var.Label.ToString() + ": ";
                label.Top = topLabel;
                label.Left = leftLabel;
                label.Width = widthLabel;
                textBox = new TextBox();
                textBox.Text = var.VarValue.ToString();
                textBox.Top = topText;
                textBox.Left = leftText;
                textBox.Width = widthText;
                textBox.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right)));
                //dynamictextbox.Text = "(Enter some text)";
                //dynamictextbox.ID = "dynamictextbox";
                //Button dynamicbutton = new Button();
                //dynamicbutton.Click += new System.EventHandler(dynamicbutton_Click);
                //dynamicbutton.Text = "Dynamic Button";
                //Panel1.Controls.Add(dynamictextbox);
                //Panel1.Controls.Add(new LiteralControl("<BR>"));
                //Panel1.Controls.Add(new LiteralControl("<BR>"));
                //Panel1.Controls.Add(dynamicbutton);
                //ViewState["controlsadded"] = true;
                panelControls.Controls.Add(label);
                panelControls.Controls.Add(textBox);
                topText += inc;
                topLabel += inc;
            }

        
        }


        //public void SetVar(string label, Variable var, object varValue)
        //{
            
        //    if (!_vars.ContainsKey(obj))
        //        _vars.Add(obj, obj.ToString());
        //    else
        //        _vars[obj] = obj.ToString();

        //    label1.Text = label;
        //    textBox1.Text = obj.ToString();
        //    if (textBox1.Tag != obj)
        //        textBox1.Tag = obj;

        //}

        //public object SetValue(Variable var, object varValue)
        //{
        //    object res;

        //    if (_vars.ContainsKey(var))
        //    {
        //        Type t = obj.GetType();
        //        if (t == typeof(int))
        //        {
        //            res = Convert.ToInt32(_vars[obj]);
        //            return res;
        //        }
        //        else if (t == typeof(float))
        //            return Convert.ToSingle(_vars[obj]);
        //        else if (t == typeof(double))
        //            return Convert.ToDouble(_vars[obj]);
        //        else if (t == typeof(decimal))
        //            return Convert.ToDecimal(_vars[obj]);
        //        else if (t == typeof(string))
        //            return Convert.ToString(_vars[obj]);
        //        else if (t == typeof(DateTime))
        //            return Convert.ToDateTime(_vars[obj]);
        //        else if (t == typeof(bool))
        //            return Convert.ToBoolean(_vars[obj]);
        //        else
        //            return null;
        //    }
        //    else
        //        return null;                    
        //}

        private void buttonAccept_Click(object sender, EventArgs e)
        {   
                     
            //Hide();
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {

            //Hide();
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

    }
}
