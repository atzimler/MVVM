using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATZ.MVVM.ViewModels.Utility
{
    public interface IVerifiable
    {
        bool IsValid { get; set; }
        event EventHandler IsValidChanged;
        void UpdateValidity(object sender, EventArgs e);
    }
}
