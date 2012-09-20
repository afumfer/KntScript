using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace AnTScript
{
    public class _Node
    {
        public decimal IdNota { get; set; }
        public string Asunto { get; set; }
        public string Nota { get; set; }
        public DateTime FechaCreacion { get; set; }

        public _Node()
        {
            IdNota = 1;
            Asunto = string.Empty;
            Nota = string.Empty;
            FechaCreacion = DateTime.Now;
        }

        public override string ToString()
        {
            return IdNota.ToString() + " : " + Asunto.ToString() + " : " + Nota.ToString() ;            
        }

        public float PruebaMetodoA(string parametro)
        {
            MessageBox.Show("Cadena de parámetro = " + parametro);
            return 9;
        }

        public void PruebaMetodoB(object parametro)
        {
            Form a = new Form();
            a.Show();
        }


    }

    public class _Folder
    {
        public int IdCarpeta { get; set; }
        public string Descripcion { get; set; }
        public string Nota { get; set; }
        
        public _Folder()
        {
            IdCarpeta = 1;
            Descripcion = string.Empty;
            Nota = string.Empty;            
        }

        public override string ToString()
        {
            return IdCarpeta.ToString() + " : " + Descripcion.ToString() + " : " + Nota.ToString();            
        }
    }

    public class Library
    {
        public float DemoSumNum(float x, float y)
        {
            return x + y;
        }

        public void Dummy(string a)
        {
            return;
        }

    }

}
