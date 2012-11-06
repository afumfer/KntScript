using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace AnTScript
{
    public class AnTSEngine
    {
        #region Properties

        public string SourceCodeFile {get; set;}
        
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

        private Dictionary<string, object> _symbolTable;
        public Dictionary<string, object> SymbolTable
        {
            get { return _symbolTable; }        
        }

        #endregion

        #region Constructor

        public AnTSEngine(string sourceCodeFile, string sourceCode, IInOutDevice inOutDevice, Library functionLibrary)
        {
            SourceCodeFile = sourceCodeFile;
            SourceCode = sourceCode;
            InOutDevice = inOutDevice;
            FunctionLibrary = functionLibrary;
            _symbolTable = new Dictionary<string, object>();
        }

        public AnTSEngine(string sourceCode)
            : this("", sourceCode, new InOutDefaultDeviceForm(), new Library())
        {

        }

        #endregion

        #region Public methods

        public void Run()
        {
            try
            {
                Scanner scanner = new Scanner(SourceCode);                
                Parser parser = new Parser(scanner.TokensList);
                CodeRun codeRun = new CodeRun(parser.Result, InOutDevice, FunctionLibrary, _symbolTable);
            }
            catch (Exception err)
            {
                throw err; 
            }        
        }

        public void AddVar(string ident, object value)
        {            
            if (string.IsNullOrEmpty(ident))
                throw new System.Exception(" variable is required ");

            if (!_symbolTable.ContainsKey(ident))
                _symbolTable.Add(ident, value);
            else
                throw new System.Exception(" variable '" + ident + "' already declared");            
        }

        public object GetVar(string ident)
        {
            if (_symbolTable.ContainsKey(ident))                
                return _symbolTable[ident];
            else
                //return null;
                throw new System.Exception(" undeclared variable '" + ident);
        }

        #endregion

        #region Static Methods

        public static void ShowConsole(string source)
        {
            InOutDefaultDeviceForm InOutDevice = new InOutDefaultDeviceForm();
            Library functionLibrary = new Library();
            ShowConsole(source, InOutDevice, functionLibrary);            
        }

        public static void ShowConsole(string source, Library functionLibrary)
        {
            InOutDefaultDeviceForm InOutDevice = new InOutDefaultDeviceForm();
            ShowConsole(source, InOutDevice, functionLibrary);
        }

        public static void ShowConsole(string source, IInOutDevice inOutDevice, Library functionLibrary)
        {
            string sourceCodeFile;
            string sourceCode;

            if (File.Exists(source))
            {
                sourceCodeFile = source;
                using (TextReader input = File.OpenText(source))
                    sourceCode = input.ReadToEnd().ToString();
            }
            else
            {
                sourceCode = source;
                sourceCodeFile = "";
            }

            AnTSEngine engine = new AnTSEngine(sourceCodeFile, sourceCode, inOutDevice, functionLibrary);

            AnTScriptForm f = new AnTScriptForm(engine);
            f.Show();
        }

        public static void ExecuteCode(string source, bool visibleInOutDevice = true)
        {            
            InOutDefaultDeviceForm InOutDevice = new InOutDefaultDeviceForm();
            Library library = new Library();
            ExecuteCode(source, InOutDevice, library, visibleInOutDevice);
        }

        public static void ExecuteCode(string source, Library functionLibrary, bool visibleInOutDevice = true)
        {
            InOutDefaultDeviceForm InOutDevice = new InOutDefaultDeviceForm();            
            ExecuteCode(source, InOutDevice, functionLibrary, visibleInOutDevice);
        }

        public static void ExecuteCode(string source, IInOutDevice InOutDevice, Library library, bool visibleInOutDevice = true)
        {
            string code;

            if (File.Exists(source))
            {
                using (TextReader input = File.OpenText(source))
                    code = input.ReadToEnd().ToString();
            }
            else
            {
                code = source;
                source = "";
            }

            if (visibleInOutDevice)
                InOutDevice.Show();

            try
            {
                AnTSEngine engine = new AnTSEngine(source, code, InOutDevice, library);
                engine.Run();
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
