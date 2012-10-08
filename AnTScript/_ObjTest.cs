using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace AnTScript
{
    public class _Node
    {
        public float IdNota { get; set; }
        public string Asunto { get; set; }
        public string Nota { get; set; }
        public DateTime FechaCreacion { get; set; }
        public _Folder Carpeta { get; set; }

        public _Node(bool b)
        {
            IdNota = 1;
            Asunto = string.Empty;            
            FechaCreacion = DateTime.Now;
            Carpeta = new _Folder();
            Carpeta.Descripcion = "Constructor por defecto - bool";
            Nota = "Constructor por defecto bool de la nota " + IdNota.ToString();
        }

        public _Node(int id)
        {
            IdNota = id;
            Asunto = string.Empty;            
            FechaCreacion = DateTime.Now;
            Carpeta = new _Folder();
            Carpeta.Descripcion = "Constructor sobrecarca id";
            Nota = "Versión sobrecargada con int " + IdNota.ToString();
        }

        public _Node(_Folder C)
        {
            IdNota = 1;
            Asunto = string.Empty;
            Nota = string.Empty;
            FechaCreacion = DateTime.Now;
            Carpeta = C;
            Nota = "Versión sobrecargada _Folder";
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

        public string PruebaMetodoC(string parametro)
        {
            return ">> " + parametro;
        }


    }

    public class _Folder
    {
        public int IdCarpeta { get; set; }
        public string Descripcion { get; set; }
        public string Observaciones { get; set; }
        public _Archiver Archivador { get; set; }
        
        public _Folder()
        {
            IdCarpeta = 1;
            Descripcion = "XXXXXX";
            Observaciones = string.Empty;
            Archivador = new _Archiver();
            Archivador.NombreArchivador = "YYYYYYYYYY";
        }

        public override string ToString()
        {
            return IdCarpeta.ToString() + " : " + Descripcion.ToString() + " : " + Observaciones.ToString();            
        }
    }

    public class _Archiver
    {
        public int IdArchivador { get; set; }
        public string NombreArchivador { get; set; }

        public string DemoMetodo(string param)
        {
            return "XXX --- YYY --- ZZZ: " + param;
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

        public List<_Node> ColecNode()
        {
            List<_Node> lisRes = new List<_Node>();

            lisRes.Add(new _Node(1));
            lisRes.Add(new _Node(2));
            lisRes.Add(new _Node(3));
            lisRes.Add(new _Node(4));
            lisRes.Add(new _Node(5));

            return lisRes;            
        }

    }

}

namespace AnTScript.Kernel
{
    public class NodeX
    {
        public  double IdNode { get; set; }
        public string Topic { get; set; }

        public NodeX()
        { }

        public NodeX(int id, string topic)
        {
            IdNode = id;
            Topic = topic;
        }
    }
    
}