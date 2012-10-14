using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace AnTScript
{
    public class Library
    {
        public float DemoSumNum(float x, float y)
        {
            return x + y;
        }

        public int RandomInt()
        {
            Random random = new Random();
            return random.Next();
        }

        public List<_Document> ColecNode()
        {
            List<_Document> lisRes = new List<_Document>();

            lisRes.Add(new _Document(1));
            lisRes.Add(new _Document(2));
            lisRes.Add(new _Document(3));
            lisRes.Add(new _Document(4));
            lisRes.Add(new _Document(5));

            return lisRes;
        }
    }
}
