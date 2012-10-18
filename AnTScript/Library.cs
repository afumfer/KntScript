using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace AnTScript
{
    public class Library
    {

        public int RandomInt(int minValue, int maxValue)
        {
            Random random = new Random();
            return random.Next(minValue, maxValue);
        }

        public int RandomInt()
        {
            Random random = new Random();
            return random.Next();
        }

        public DateTime DateTimeNow()
        {
            return DateTime.Now;
        }

        public DateTime DateTimeToday()
        {
            return DateTime.Today;
        }

        public Guid NewGuid()
        {
            Guid a = Guid.NewGuid();
            return a;
        }

        public List<object> NewCollectionObjects()
        {
            return new List<object>();            
        }

    }
}
