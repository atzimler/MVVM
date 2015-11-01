using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Views.Utility.Interfaces
{
    public interface IView<VM>
    {
        void BindModel(VM vm);
        void UnbindModel();
    }
}
