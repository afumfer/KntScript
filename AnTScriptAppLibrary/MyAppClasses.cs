using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using System.Reflection;

namespace AnTScriptAppLibrary
{
    public class Document
    {
        public float IdDocument { get; set; }
        public string Topic { get; set; }
        public string Description { get; set; }
        public DateTime CreationDateTiem { get; set; }
        public Folder Folder { get; set; }

        public Document(bool b)
        {
            IdDocument = 1;
            Topic = string.Empty;
            CreationDateTiem = DateTime.Now;
            Folder = new Folder();
            Folder.Name = "Constructor  bool";
            Description = "______ (bool)" + IdDocument.ToString();
        }

        public Document()
        {
            IdDocument = 1;
            Topic = string.Empty;
            CreationDateTiem = DateTime.Now;
            Folder = new Folder();
            Folder.Name = "Constructor";
            Description = "_____ () " + IdDocument.ToString();
        }


        public Document(int id)
        {
            IdDocument = id;
            Topic = string.Empty;
            CreationDateTiem = DateTime.Now;
            Folder = new Folder();
            Folder.Name = "Constructor id";
            Description = "________  (id) " + IdDocument.ToString();
        }

        public Document(Folder C)
        {
            IdDocument = 1;
            Topic = string.Empty;
            Description = string.Empty;
            CreationDateTiem = DateTime.Now;
            Folder = C;
            Description = "_______ (Folder)";
        }

        public override string ToString()
        {
            return IdDocument.ToString() + " : " + Topic.ToString() + " : " + Description.ToString();
        }

        public float TestMethodA(string param)
        {
            MessageBox.Show("param = " + param);
            return 9;
        }

        public void TestMethodB(object param)
        {
            Form a = new Form();
            a.Show();
        }

        public string TestMethodB(string param)
        {
            return ">> " + param;
        }
    }

    public class Folder
    {
        public int IdFolder { get; set; }
        public string Name { get; set; }
        public string Comments { get; set; }
        public Archiver Archiver { get; set; }

        public Folder()
        {
            IdFolder = 1;
            Name = "Folder name XXXXXX";
            Comments = string.Empty;
            Archiver = new Archiver();
            Archiver.Name = "Archiver Folder Name YYYYYYYYYY";
        }

        public override string ToString()
        {
            return IdFolder.ToString() + " : " + Name.ToString() + " : " + Comments.ToString();
        }
    }

    public class Archiver
    {
        public int IdArchiver { get; set; }
        public string Name { get; set; }

        public string DemoMethod(string param)
        {
            return "XXX --- YYY --- ZZZ: " + param;
        }

    }

}
