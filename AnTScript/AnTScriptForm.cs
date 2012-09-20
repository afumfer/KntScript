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

    public partial class AnTScriptForm : Form
    {
        private string sourceCode = string.Empty;

        public AnTScriptForm(string source)
        {
            InitializeComponent();

            sourceCode = source;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textFileSourceCode.Text = sourceCode;
            LoadFile(sourceCode);
        }

        private void buttonRun_Click(object sender, EventArgs e)
        {

            textOut.Clear();
            listScan.Items.Clear();
            treeAST.Nodes.Clear();

            try
            {
                Scanner scanner = null;

                scanner = new Scanner(textSourceCode.Text);

                foreach (Token t in scanner.TokensList)
                {
                    listScan.Items.Add(t.Name);
                }

                Parser parser = new Parser(scanner.TokensList);
                ////ParserTree astTree = new ParserTree(parser.Result, treeAST);

                CodeRun codeRun = new CodeRun(parser.Result, textOut);

                //treeAST.ExpandAll();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }

        }

        private void buttonLoadFile_Click(object sender, EventArgs e)
        {
            LoadFile(textFileSourceCode.Text);
            sourceCode = textFileSourceCode.Text;
        }

        private void LoadFile(string sourceCode)
        {
            using (TextReader input = File.OpenText(sourceCode))
            {
                textSourceCode.Text = input.ReadToEnd().ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //Form a = new System.Windows.Forms.Form();
                //a.Show();

                Type t = Type.GetType("System.Windows.Forms.Form, System.Windows.Forms, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", false, true);
                object obj = Activator.CreateInstance(t);


                ////=======================================================
                //// --- Pruebas invocación a un método por reflexión
                //Type t = Type.GetType("AnTScript._Node", false, true);
                //object obj = Activator.CreateInstance(t);
                //MethodInfo mi = t.GetMethod("PruebaMetodoB");
                //// Invoke method ('null' for no parameters)
                //object[] param = new object[1];
                //param[0] = "bla bla bla ";
                //object ret;
                //ret = mi.Invoke(obj, param);
                //if (ret != null)
                //    MessageBox.Show(ret.ToString());
                //// ---
                ////=======================================================

                return;

                #region Pruebas - Basura

                ////=======================================================
                //// --- Pruebas manipulación propiedades por reflexión
                //string CadenaObjeto = "AnTScript._Node";                
                //var obj = AppDomain.CurrentDomain.CreateInstance("AnTScript", CadenaObjeto);                
                //PropertyInfo pi = t.GetProperty("IdNota");
                //int x1 = 1;
                //pi.SetValue(obj, x1, null);
                //MessageBox.Show(string.Format("Set Value > {0} !", obj.ToString()));


                //// Get metadata for the Minivan type.                
                //Type t = Type.GetType("AnTScript._Node", false, true);
                ////Type t = typeof(_Node);                                
                //object obj = Activator.CreateInstance(t);
                //PropertyInfo pi = t.GetProperty("IdNota");
                //int x1 = 1;
                //pi.SetValue(obj, x1, null);
                //MessageBox.Show(string.Format("Set Value > {0} !", obj.ToString()));

                ////object x;
                ////x = pi.GetValue(obj, null);

                ////MessageBox.Show(string.Format("Get Value {0} !", x.ToString()));
                ////=======================================================

                #endregion

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
    }

}