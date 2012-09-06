using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnTScript
{
    public class _Node
    {
        public int IdNota { get; set; }
        public string Asunto { get; set; }
        public string Nota { get; set; }

        public _Node()
        {
            IdNota = 0;
            Asunto = string.Empty;
            Nota = string.Empty;
        }

        public override string ToString()
        {
            return IdNota.ToString() + " : " + Asunto.ToString() + " : " + Nota.ToString() ;
            //return "balb alba ";
        }
    }
}
