using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

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
                // Get metadata for the Minivan type.                
                //Type t = Type.GetType("AnTScript._Node", false, true);
                Type t = typeof(_Node);                

                // Create the Minivan on the fly.
                object obj = Activator.CreateInstance(t);                

                //((_Node)obj).Asunto = "bla bla ";
                
                //MethodInfo mi = miniVan.GetMethod("TurboBoost");
                //// Invoke method ('null' for no parameters).
                //mi.Invoke(obj, null);

                PropertyInfo pi = t.GetProperty("Asunto");
               
                pi.SetValue(obj, "XXX Blabla XXXX", null);

                MessageBox.Show(string.Format("Set Value > {0} !", obj.ToString()));

                object x;

                x = pi.GetValue(obj, null);

                MessageBox.Show(string.Format("Get Value {0} !", x.ToString()));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

}