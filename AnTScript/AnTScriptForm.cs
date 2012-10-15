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

        public AnTScriptForm(string source)
        {
            InitializeComponent();

            sourceCode = source;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textFileSourceCode.Text = sourceCode;
            LoadFile(sourceCode);

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
            listScan.Items.Clear();
            treeAST.Nodes.Clear();

            try
            {
                //Scanner scanner = new Scanner(textSourceCode.Text);
                //foreach (Token t in scanner.TokensList)
                //{
                //    listScan.Items.Add(t.Name);
                //}
                //Parser parser = new Parser(scanner.TokensList);               
                //CodeRun codeRun = new CodeRun(parser.Result, this);
                ////ParserTree astTree = new ParserTree(parser.Result, treeAST);
                ////treeAST.ExpandAll();

                Engine antEngine = new Engine(textSourceCode.Text, this.inOutDeviceForm);
                antEngine.Run();

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
            if(string.IsNullOrEmpty(sourceCode))
                return;

            // TODO: pendiente validar que el nombre del fichero sea correcto. 
            using (TextReader input = File.OpenText(sourceCode))
            {
                textSourceCode.Text = input.ReadToEnd().ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                #region Pruebas - Código para investigación

                /////////////// Identificando objetos
                //int a = 0;
                //int b = 0;
                //string aa = "";
                //string bb = "";

                //object x = 11;
                //object y = 11;

                //a = x.GetHashCode();
                //b = y.GetHashCode();

                //if (Object.ReferenceEquals(x, y))
                //    MessageBox.Show("iguales");



                /////////// Prueba parse bool
                //bool prueba;
                //object obj;
                //prueba = bool.Parse("true");
                //obj = prueba;
                //if (obj.Equals(true))
                //    MessageBox.Show("111");


                ////////////// Pruebas con tipos de datos numéricos
                //int i = 123;
                //float f = 1.1f;
                //double x = 10.9;                
                //decimal d = 123.45M;
                //bool b = false;

                //if (b.Equals(false))
                //    MessageBox.Show("B es 0");

                //string x0 = i.GetType().ToString();
                //string x1 = f.GetType().ToString();
                //string x2 = x.GetType().ToString();                
                //string x3 = d.GetType().ToString();
                //string x4 = b.GetType().ToString();
                ////string x5 = bb.GetType().ToString();
                ///////////////////////////////////////////////////



                //////////// identificadores de objetos con espacios de nombre y/o objetos anidados
                //object objNuevoValor;
                //IdentObject identEx = new IdentObject("a.Carpeta.Archivador.NombreArchivador");

                //Dictionary<string, object> symbolTable = new Dictionary<string, object>();
                //symbolTable.Add(identEx.Obj, new _Node(123));

                //objNuevoValor = "Nuevo NOMBRE ARCHIVADOR";

                //SetValue(symbolTable[identEx.Obj], identEx, objNuevoValor);

                //object resSetValue = symbolTable["a"];

                //object resGetValue;
                //GetValue(symbolTable[identEx.Obj], identEx, out resGetValue);

                //return;
                //// ........................................................



                ////////// Navegar por jerarquía de objetos, prueba limpia 
                //object objNuevoValor;
                //IdentDetailed identEx = new IdentDetailed("a.Carpeta.Archivador.NombreArchivador");

                //Dictionary<string, object> symbolTable = new Dictionary<string, object>();
                //symbolTable.Add(identEx.Obj, new _Node(123));

                //objNuevoValor = "Nuevo NOMBRE ARCHIVADOR";

                //SetValue(symbolTable[identEx.Obj], identEx, objNuevoValor);

                //object resultado = symbolTable["a"];
                // ........................................................



                /////////// Navegar por jerarquía de objetos  - Prueba sucia //////
                ///////////////////
                //List<string> cadObj = new List<string>();
                //string miembro = "IdCarpeta";
                //object objNuevoValor;

                //Dictionary<string, object> symbolTable = new Dictionary<string, object>();
                //symbolTable.Add("a", new _Node(123));

                //objNuevoValor = 777;

                //cadObj.Add("a");
                //cadObj.Add("Carpeta");

                //Type t = symbolTable["a"].GetType();
                //PropertyInfo pi = t.GetProperty("Carpeta");

                //object obj2 = pi.GetValue(symbolTable["a"], null);

                //Type t2 = obj2.GetType();
                //PropertyInfo pi2 = t2.GetProperty("IdCarpeta");

                //pi2.SetValue(obj2, objNuevoValor, null);

                //object resultado = symbolTable["a"];

                //return;
                // ........................................................




                ////=======================================================
                //// --- Prueba para instanciar un form
                //Type t = Type.GetType("System.Windows.Forms.Form, System.Windows.Forms, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", false, true);
                //object obj = Activator.CreateInstance(t);


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
            catch (Exception ex) { MessageBox.Show(ex.Message);
            }
        }


        #region Pruebas

        private void SetValue(object varObj, IdentObject identEx, object newValue, int i = 0)
        {
            Type t;
            PropertyInfo pi;
            string literalObjChild;
            object objChild;

            t = varObj.GetType();

            if (i < identEx.ChainObjs.Count)
            {
                literalObjChild = identEx.ChainObjs[i];
                pi = t.GetProperty(literalObjChild);
                objChild = pi.GetValue(varObj, null);
                i++;
                SetValue(objChild, identEx, newValue, i);
            }
            else
            {
                pi = t.GetProperty(identEx.Member);
                pi.SetValue(varObj, newValue, null);
            }
                       
            return;
        }

        private void GetValue(object varObj, IdentObject identEx, out object newValue, int i = 0)
        {
            Type t;
            PropertyInfo pi;
            string literalObjChild;
            object objChild;

            t = varObj.GetType();

            if (i < identEx.ChainObjs.Count)
            {
                literalObjChild = identEx.ChainObjs[i];
                pi = t.GetProperty(literalObjChild);
                objChild = pi.GetValue(varObj, null);
                i++;
                GetValue(objChild, identEx, out newValue, i);
            }
            else
            {
                pi = t.GetProperty(identEx.Member);
                //pi.SetValue(varObj, newValue, null);
                newValue = pi.GetValue(varObj, null);
            }

            return;
        }

        #endregion

    }

}