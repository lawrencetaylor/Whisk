using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Whisk.Tests
{
    [TestClass]
    public class MixinTests
    {
        [TestMethod]
        public void DegenerateMixin()
        {
            var implementationA = new MixinTestClasses.ImplementationA();
            var mixin = new MixinBuilder<MixinTestClasses.IInterfaceA>(implementationA).Create();

            Assert.IsFalse(implementationA.MethodHit);

            mixin.VoidMethod();

            Assert.IsTrue(implementationA.MethodHit);

        }

        [TestMethod]
        public void SimpleMixinWithTwoInterfaces()
        {
            var implementationA = new MixinTestClasses.ImplementationA();
            var implementationB = new MixinTestClasses.ImplementationB();
            var mixin = new MixinBuilder<MixinTestClasses.IInterfaceA>(implementationA)
                 .AddMixin<MixinTestClasses.IInterfaceB>(implementationB)
                .Create<MixinTestClasses.IInterfaceC>();

            Assert.IsFalse(implementationA.MethodHit);

            mixin.VoidMethod();

            Assert.IsTrue(implementationA.MethodHit);

            Assert.AreEqual("Hello ABC", mixin.Cast<MixinTestClasses.IInterfaceB>().GreetMe("ABC"));

        }

        [TestMethod]
        public void UsingWithIDisposableMixin()
        {
            var disposable = new MixinTestClasses.Disposable();

            var interfaceA = new MixinBuilder<MixinTestClasses.IInterfaceA>(new MixinTestClasses.ImplementationA())
                .AddMixin<IDisposable>(disposable)
                .Create();

            Assert.IsFalse(disposable.IsDisposed);

            using (interfaceA as IDisposable)
            {
                interfaceA.VoidMethod();
            }

            Assert.IsTrue(disposable.IsDisposed);
        }

        [TestMethod]
        public void TestInterfaceWithVariousSignatures()
        {
            var implementationA = new MixinTestClasses.Dummy();
            var mixin = new MixinBuilder<MixinTestClasses.IDummy>(implementationA).Create();
            Assert.AreEqual("You", mixin.SayMe().Name);
            Assert.AreEqual("You", mixin.SayMeWithStringArgStringRet("me"));
            Assert.AreEqual("You", mixin.SayMeWithComplexArgStringRet(new MixinTestClasses.ComplexType()));
            Assert.AreEqual("You", mixin.SayMeWithComplexArgComplexRet(new MixinTestClasses.ComplexType()).Name);
            Assert.AreEqual("You", mixin.SayMeWithComplexArgStringRet(new MixinTestClasses.ComplexType()));
            Assert.AreEqual("You", mixin.SayMeWithTwoArguments("a", "b"));
            Assert.AreEqual("You", mixin.SayMeWithMixedPrimitiveAndComplexArguments("ad", new MixinTestClasses.ComplexType(), 2));
        }

        public class MixinTestClasses
        {
            internal class ImplementationA : IInterfaceA
            {
                public void VoidMethod()
                {
                    MethodHit = true;
                }

                public bool MethodHit { get; private set; }
            }

            internal class ImplementationB : IInterfaceB
            {
                public string GreetMe(string name)
                {
                    return "Hello " + name;
                }
            }

            public interface IInterfaceA
            {
                void VoidMethod();
            }

            public interface IInterfaceB
            {
                string GreetMe(string name);
            }

            public interface IInterfaceC : IInterfaceA, IInterfaceB
            {

            }

            internal class Disposable : IDisposable
            {
                public void Dispose()
                {
                    IsDisposed = true;
                }

                public bool IsDisposed { get; private set; }
            }

            public interface IDummy
            {
                ComplexType SayMe();
                ComplexType SayMeWithComplexArgComplexRet(ComplexType me);
                ComplexType SayMeWithStringArgComplexRet(string me);
                string SayMeWithStringArgStringRet(string me);
                string SayMeWithComplexArgStringRet(ComplexType me);
                string SayMeWithTwoArguments(string a1, string a2);
                string SayMeWithMixedPrimitiveAndComplexArguments(string a1, ComplexType a2, int a3);
            }

            public class Dummy : IDummy
            {
                public ComplexType SayMe()
                {
                    return new ComplexType() { Name = "You" };
                }

                public ComplexType SayMeWithComplexArgComplexRet(ComplexType me)
                {
                    return new ComplexType() { Name = "You" };
                }

                public ComplexType SayMeWithStringArgComplexRet(string me)
                {
                    return new ComplexType() { Name = "You" };
                }

                public string SayMeWithStringArgStringRet(string me)
                {
                    return "You";
                }

                public string SayMeWithComplexArgStringRet(ComplexType me)
                {
                    return "You";
                }

                public string SayMeWithTwoArguments(string a1, string a2)
                {
                    return "You";
                }

                public string SayMeWithMixedPrimitiveAndComplexArguments(string a1, ComplexType a2, int a3)
                {
                    return "You";
                }
            }

            public class ComplexType
            {
                public string Name { get; set; }
            }

            class DummyWrapper : IDummy
            {
                private readonly IDummy _dummy;

                public DummyWrapper(IDummy dummy)
                {
                    _dummy = dummy;
                }


                public ComplexType SayMe()
                {
                    return _dummy.SayMe();
                }

                public ComplexType SayMeWithComplexArgComplexRet(ComplexType me)
                {
                    return _dummy.SayMeWithComplexArgComplexRet(me);
                }

                public ComplexType SayMeWithStringArgComplexRet(string me)
                {
                    return _dummy.SayMeWithStringArgComplexRet(me);
                }

                public string SayMeWithStringArgStringRet(string me)
                {
                    return _dummy.SayMeWithStringArgStringRet(me);
                }

                public string SayMeWithComplexArgStringRet(ComplexType me)
                {
                    return _dummy.SayMeWithComplexArgStringRet(me);
                }

                public string SayMeWithTwoArguments(string a1, string a2)
                {
                    return _dummy.SayMeWithTwoArguments(a1, a2);
                }

                public string SayMeWithMixedPrimitiveAndComplexArguments(string a1, ComplexType a2, int a3)
                {
                    return _dummy.SayMeWithMixedPrimitiveAndComplexArguments(a1, a2, a3);
                }
            }
        }
    }

    public static class ObjectExtensions
    {
        public static T Cast<T>(this object obj)
        {
            return (T)obj;
        }
    }
}
