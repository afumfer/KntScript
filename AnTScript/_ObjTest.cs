using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;

namespace AnTScript
{

    //public class Document
    //{
    //    public float IdDocument { get; set; }
    //    public string Topic { get; set; }
    //    public string Description { get; set; }
    //    public DateTime CreationDateTiem { get; set; }
    //    public Folder Folder { get; set; }

    //    public Document(bool b)
    //    {
    //        IdDocument = 1;
    //        Topic = string.Empty;
    //        CreationDateTiem = DateTime.Now;
    //        Folder = new Folder();
    //        Folder.Name = "Constructor por defecto - bool";
    //        Description = "Constructor por defecto bool de la nota " + IdDocument.ToString();
    //    }

    //    public Document(int id)
    //    {
    //        IdDocument = id;
    //        Topic = string.Empty;
    //        CreationDateTiem = DateTime.Now;
    //        Folder = new Folder();
    //        Folder.Name = "Constructor sobrecarca id";
    //        Description = "Versión sobrecargada con int " + IdDocument.ToString();
    //    }

    //    public Document(Folder C)
    //    {
    //        IdDocument = 1;
    //        Topic = string.Empty;
    //        Description = string.Empty;
    //        CreationDateTiem = DateTime.Now;
    //        Folder = C;
    //        Description = "Versión sobrecargada _Folder";
    //    }

    //    public override string ToString()
    //    {
    //        return IdDocument.ToString() + " : " + Topic.ToString() + " : " + Description.ToString();
    //    }

    //    public float PruebaMetodoA(string parametro)
    //    {
    //        MessageBox.Show("Cadena de parámetro = " + parametro);
    //        return 9;
    //    }

    //    public void PruebaMetodoB(object parametro)
    //    {
    //        Form a = new Form();
    //        a.Show();
    //    }

    //    public string PruebaMetodoC(string parametro)
    //    {
    //        return ">> " + parametro;
    //    }
    //}

    //public class Folder
    //{
    //    public int IdFolder { get; set; }
    //    public string Name { get; set; }
    //    public string Comments { get; set; }
    //    public Archiver Archiver { get; set; }

    //    public Folder()
    //    {
    //        IdFolder = 1;
    //        Name = "XXXXXX";
    //        Comments = string.Empty;
    //        Archiver = new Archiver();
    //        Archiver.Name = "YYYYYYYYYY";
    //    }

    //    public override string ToString()
    //    {
    //        return IdFolder.ToString() + " : " + Name.ToString() + " : " + Comments.ToString();
    //    }
    //}

    //public class Archiver
    //{
    //    public int IdArchiver { get; set; }
    //    public string Name { get; set; }

    //    public string DemoMethod(string param)
    //    {
    //        return "XXX --- YYY --- ZZZ: " + param;
    //    }

    //}


}

