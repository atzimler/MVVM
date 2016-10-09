using System;

namespace ATZ.MVVM.ViewModels.Utility
{
    public interface IVerifiable
    {
        bool IsValid { get; set; }
        event EventHandler IsValidChanged;
        void UpdateValidity(object sender, EventArgs e);
    }
}
