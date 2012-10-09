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
    public partial class FormInputVar : Form
    {

        private Dictionary<object, object> vars;

        public FormInputVar()
        {
            InitializeComponent();
            vars = new Dictionary<object, object>();
        }

        public void ShowInputForm()
        {
            this.ShowDialog();
        }


        public void SetVar(string label, object obj)
        {
            
            if (!vars.ContainsKey(obj))
                vars.Add(obj, obj.ToString());
            else
                vars[obj] = obj.ToString();

            label1.Text = label;
            textBox1.Text = obj.ToString();
            if (textBox1.Tag != obj)
                textBox1.Tag = obj;

        }

        public object GetValue(object obj)
        {
            object res;

            if (vars.ContainsKey(obj))
            {
                Type t = obj.GetType();
                if (t == typeof(int))
                {
                    res = Convert.ToInt32(vars[obj]);
                    return res;
                }
                else if (t == typeof(float))
                    return Convert.ToSingle(vars[obj]);
                else if (t == typeof(double))
                    return Convert.ToDouble(vars[obj]);
                else if (t == typeof(decimal))
                    return Convert.ToDecimal(vars[obj]);
                else if (t == typeof(string))
                    return Convert.ToString(vars[obj]);
                else if (t == typeof(DateTime))
                    return Convert.ToDateTime(vars[obj]);
                else if (t == typeof(bool))
                    return Convert.ToBoolean(vars[obj]);
                else
                    return null;
            }
            else
                return null;                    
        }

        private void buttonAccept_Click(object sender, EventArgs e)
        {
            vars[textBox1.Tag] = textBox1.Text;
            
            Hide();
            //DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Hide();
            //DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

    }
}
