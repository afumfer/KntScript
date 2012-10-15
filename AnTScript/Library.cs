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
    }
}
