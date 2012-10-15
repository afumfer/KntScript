using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnTScript
{
    public interface IInOutDevice
    {
        void Print(string str);
        bool ReadVars(List<ReadVarItem> readVarItmes);
        void Clear();
    }
}
