using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

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

                foreach (object o in scanner.TokensList)
                {
                    listScan.Items.Add(o.ToString());
                }

                Parser parser = new Parser(scanner.TokensList);
                ParserTree astTree = new ParserTree(parser.Result, treeAST);

                CodeRun codeRun = new CodeRun(parser.Result, textOut);

                treeAST.ExpandAll();
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
    }

}