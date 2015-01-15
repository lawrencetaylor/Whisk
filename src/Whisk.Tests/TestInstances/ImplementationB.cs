namespace Whisk.Tests
{
    internal class ImplementationB : IInterfaceB
    {
        public string GreetMe(string name)
        {
            return "Hello " + name;
        }
    }
}