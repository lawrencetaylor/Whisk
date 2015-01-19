namespace Whisk.Tests
{
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

        public void SayMeWithStringArgVoid(string me)
        {
            
        }

        public void SayMeWithComplexArgVoid(ComplexType me)
        {
           
        }

        public void SayMeWithTwoArgumentsVoid(string a1, string a2)
        {
           
        }

        public void SayMeWithMixedPrimitiveAndComplexArgumentsVoid(string a1, ComplexType a2, int a3)
        {
           
        }
    }
}