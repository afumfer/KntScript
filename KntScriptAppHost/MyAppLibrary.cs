using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using KntScript;

namespace KntScriptAppHost
{
    public class MyLibrary: Library
    {        
        public List<DocumentDummy> ColecDocDemo()
        {
            List<DocumentDummy> lisRes = new List<DocumentDummy>();

            lisRes.Add(new DocumentDummy(1));
            lisRes.Add(new DocumentDummy(2));
            lisRes.Add(new DocumentDummy(3));
            lisRes.Add(new DocumentDummy(4));
            lisRes.Add(new DocumentDummy(5));
            return lisRes;
        }

        public float DemoSumNum(float x, float y)
        {
            return x + y;
        }

        public object TestNull()
        {
            return null;
        }

        public void TestMsg()
        {
            MessageBox.Show("TEST MyLibrary Method");
        }

        public static void TestStatic()
        {
            MessageBox.Show("Static");
        }

    }

}
