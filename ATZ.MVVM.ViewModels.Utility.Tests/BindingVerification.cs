using ATZ.DependencyInjection;
using Ninject;
using NUnit.Framework;

namespace ATZ.MVVM.ViewModels.Utility.Tests
{
    public static class BindingVerification
    {
        public static void VerifyBinding<TService, TImplementation>()
        {
            var implementation = default(TService); // Relaxing the compiler
            Assert.DoesNotThrow(() => implementation = (TService)DependencyResolver.Instance.Get(typeof(TService)));
            Assert.AreEqual(typeof(TImplementation), implementation.GetType());
    }
}
}
