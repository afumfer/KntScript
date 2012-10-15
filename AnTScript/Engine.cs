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
            get
            {
                return _sourceCodeFile;
            }
            set
            {
                _sourceCodeFile = value;
                using (TextReader input = File.OpenText(_sourceCodeFile))
                {
                    SourceCode = input.ReadToEnd().ToString();
                }
            }
        }

        public string SourceCode { get; set; }

        private IInOutDevice _inOutDevice;
        public IInOutDevice InOutDevice
        {
            get
            {
                if (_inOutDevice == null)
                {
                    _inOutDevice = new InOutDefaultDeviceForm();

                }
                return _inOutDevice;
            }

            set { _inOutDevice = value; }
        }

        public Engine()
        { }

        public Engine(string sourceCode, IInOutDevice inOutDevice = null)
        {
            SourceCode = sourceCode;
            InOutDevice = inOutDevice;
        }

        public void Run()
        {
            try
            {
                Scanner scanner = new Scanner(SourceCode);                
                Parser parser = new Parser(scanner.TokensList);
                CodeRun codeRun = new CodeRun(parser.Result, InOutDevice);
            }
            catch (Exception err)
            {
                throw err; 
            }        
        }

        #region static Method

        public static void ShowConsole(string sourceFile)
        {
            AnTScriptForm f = new AnTScriptForm(sourceFile);
            f.Show();
        }

        public static void ExecuteCodeFile(string sourceFile)
        {
            string code;
            InOutDefaultDeviceForm InOutDevice = new InOutDefaultDeviceForm();
            
            using (TextReader input = File.OpenText(sourceFile))
            {
                code = input.ReadToEnd().ToString();
            }

            InOutDevice.Show();

            try
            {
                Scanner scanner = new Scanner(code);
                Parser parser = new Parser(scanner.TokensList);
                CodeRun codeRun = new CodeRun(parser.Result, InOutDevice);
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        #endregion

    }
}
