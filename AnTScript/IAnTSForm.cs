using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnTScript
{
    public interface IAnTSForm
    {
        void Save();
        void Exit();
        void LockEdition(bool look);
    }
}
