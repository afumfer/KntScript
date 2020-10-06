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
        
        public List<Document> ColecNode()
        {
            List<Document> lisRes = new List<Document>();

            lisRes.Add(new Document(1));
            lisRes.Add(new Document(2));
            lisRes.Add(new Document(3));
            lisRes.Add(new Document(4));
            lisRes.Add(new Document(5));
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
            MessageBox.Show("XXXXXXXX-----XXXXXX");
        }

        public static void TestStatic()
        {
            MessageBox.Show("Static");
        }

    }

}
