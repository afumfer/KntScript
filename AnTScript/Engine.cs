using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnTScript
{
    public class Engine
    {

        public string SourceCode { get; set; }

        public IInOutDevice InOutDevice { get; set; }

        public Engine()
        { }

        public Engine(string sourceCode, IInOutDevice inOutDevice)
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

    }
}
