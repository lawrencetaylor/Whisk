namespace Whisk.Tests
{
    internal class ImplementationA : IInterfaceA
    {
        public void VoidMethod()
        {
            MethodHit = true;
        }

        public bool MethodHit { get; private set; }
    }
}