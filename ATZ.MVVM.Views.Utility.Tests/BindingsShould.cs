using ATZ.DependencyInjection.System;
using ATZ.MVVM.ViewModels.Utility.Tests;
using NUnit.Framework;

namespace ATZ.MVVM.Views.Utility.Tests
{
    [TestFixture]
    public class BindingsShould
    {
        [Test]
        public void RegisterIDebugBindingBecauseWeUseIt()
        {
            Bindings.Initialize();
            BindingVerification.VerifyBinding<IDebug, SystemDebug>();
        }
    }
}
