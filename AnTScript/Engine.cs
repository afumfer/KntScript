using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AnTScript
{
    public class Engine
    {

        private string _sourceCodeFile;
        public string SourceCodeFile
        {
            get { return _sourceCodeFile; }
            set
            {
                _sourceCodeFile = value;
                using (TextReader input = File.OpenText(_sourceCodeFile))                
                    SourceCode = input.ReadToEnd().ToString();                
            }
        }

        public string SourceCode { get; set; }

        private IInOutDevice _inOutDevice;
        public IInOutDevice InOutDevice
        {
            get
            {
                if (_inOutDevice == null)                
                    _inOutDevice = new InOutDefaultDeviceForm();                
                return _inOutDevice;
            }

            set { _inOutDevice = value; }
        }

        private Library _functionLibrary;
        public Library FunctionLibrary 
        {
            get
            {
                if(_functionLibrary == null)
                    _functionLibrary = new Library();
                return _functionLibrary;
            }
            set {_functionLibrary = value;}
        }

        public Engine()
        { }

        public Engine(string sourceCode, IInOutDevice inOutDevice, Library functionLibrary)
        {
            SourceCode = sourceCode;
            InOutDevice = inOutDevice;
            FunctionLibrary = functionLibrary;
        }

        public void Run()
        {
            try
            {
                Scanner scanner = new Scanner(SourceCode);                
                Parser parser = new Parser(scanner.TokensList);
                CodeRun codeRun = new CodeRun(parser.Result, InOutDevice, FunctionLibrary);
            }
            catch (Exception err)
            {
                throw err; 
            }        
        }

        #region Static Methods

        public static void ShowConsole(string sourceFile)
        {
            InOutDefaultDeviceForm InOutDevice = new InOutDefaultDeviceForm();
            Library functionLibrary = new Library();
            ShowConsole(sourceFile, InOutDevice, functionLibrary);            
        }

        public static void ShowConsole(string sourceFile, Library functionLibrary)
        {
            InOutDefaultDeviceForm InOutDevice = new InOutDefaultDeviceForm();
            ShowConsole(sourceFile, InOutDevice, functionLibrary);
        }

        public static void ShowConsole(string sourceFile, IInOutDevice inOutDevice, Library functionLibrary)
        {
            AnTScriptForm f = new AnTScriptForm(sourceFile, inOutDevice, functionLibrary);
            f.Show();
        }

        public static void ExecuteCode(string sourceFile, bool visibleInOutDevice = true)
        {            
            InOutDefaultDeviceForm InOutDevice = new InOutDefaultDeviceForm();
            Library library = new Library();
            ExecuteCode(sourceFile, InOutDevice, library, visibleInOutDevice);
        }

        public static void ExecuteCode(string sourceFile, Library functionLibrary, bool visibleInOutDevice = true)
        {
            InOutDefaultDeviceForm InOutDevice = new InOutDefaultDeviceForm();            
            ExecuteCode(sourceFile, InOutDevice, functionLibrary, visibleInOutDevice);
        }

        public static void ExecuteCode(string sourceFile, IInOutDevice InOutDevice, Library library, bool visibleInOutDevice = true)
        {
            string code;            

            if (File.Exists(sourceFile))            
                using (TextReader input = File.OpenText(sourceFile))                
                    code = input.ReadToEnd().ToString();                            
            else            
                code = sourceFile;

            if (visibleInOutDevice)
                InOutDevice.Show();

            try
            {
                Scanner scanner = new Scanner(code);
                Parser parser = new Parser(scanner.TokensList);
                CodeRun codeRun = new CodeRun(parser.Result, InOutDevice, library);
            }
            catch (Exception err)
            {
                if(!visibleInOutDevice)
                    InOutDevice.Show();
                InOutDevice.Print(err.Message);
                //throw err;
            }
        }


        #endregion

    }
}
