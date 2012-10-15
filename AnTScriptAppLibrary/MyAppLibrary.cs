using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnTScriptAppLibrary
{
    public class MyAppLibrary
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
    }
}
