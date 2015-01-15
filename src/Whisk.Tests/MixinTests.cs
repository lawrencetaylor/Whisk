using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mixins;

namespace Whisk.Tests
{
    [TestClass]
    public class MixinTests
    {
        [TestMethod]
        public void DegenerateMixin()
        {
            var implementationA = new ImplementationA();
            var mixinFactory = MixinBuilder.CreateFactory<IInterfaceA, IInterfaceA>();

            IInterfaceA mixin = mixinFactory(implementationA);

            Assert.IsFalse(implementationA.MethodHit);

            mixin.VoidMethod();

            Assert.IsTrue(implementationA.MethodHit);

        }

        [TestMethod]
        public void SimpleMixinWithTwoInterfaces()
        {
            var implementationA = new ImplementationA();
            var implementationB = new ImplementationB();
            var mixinFactory = MixinBuilder.CreateFactory<IInterfaceA, IInterfaceB, IInterfaceC>();
            var mixin = mixinFactory(implementationA, implementationB);

            Assert.IsFalse(implementationA.MethodHit);

            mixin.VoidMethod();

            Assert.IsTrue(implementationA.MethodHit);

            Assert.AreEqual("Hello ABC", mixin.Cast<IInterfaceB>().GreetMe("ABC"));

        }

        [TestMethod]
        public void UsingWithIDisposableMixin()
        {
            var disposable = new Disposable();

            var mixinFactory = MixinBuilder.CreateFactory<IInterfaceA, IDisposable, IInterfaceA>();
            var mixin = mixinFactory(new ImplementationA(), disposable);

            Assert.IsFalse(disposable.IsDisposed);

            using (mixin as IDisposable)
            {
                mixin.VoidMethod();
            }

            Assert.IsTrue(disposable.IsDisposed);
        }

        [TestMethod]
        public void TestInterfaceWithVariousSignatures()
        {
            var mixinFactory = MixinBuilder.CreateFactory<IDummy, IDummy>();
            var mixin = mixinFactory(new Dummy());

            Assert.AreEqual("You", mixin.SayMe().Name);
            Assert.AreEqual("You", mixin.SayMeWithStringArgStringRet("me"));
            Assert.AreEqual("You", mixin.SayMeWithComplexArgStringRet(new ComplexType()));
            Assert.AreEqual("You", mixin.SayMeWithComplexArgComplexRet(new ComplexType()).Name);
            Assert.AreEqual("You", mixin.SayMeWithComplexArgStringRet(new ComplexType()));
            Assert.AreEqual("You", mixin.SayMeWithTwoArguments("a", "b"));
            Assert.AreEqual("You", mixin.SayMeWithMixedPrimitiveAndComplexArguments("ad", new ComplexType(), 2));
        }
    }
}