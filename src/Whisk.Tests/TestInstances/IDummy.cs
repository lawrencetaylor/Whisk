namespace Whisk.Tests
{
    public interface IDummy
    {
        ComplexType SayMe();
        ComplexType SayMeWithComplexArgComplexRet(ComplexType me);
        ComplexType SayMeWithStringArgComplexRet(string me);
        string SayMeWithStringArgStringRet(string me);
        string SayMeWithComplexArgStringRet(ComplexType me);
        string SayMeWithTwoArguments(string a1, string a2);
        string SayMeWithMixedPrimitiveAndComplexArguments(string a1, ComplexType a2, int a3);
        void SayMeWithStringArgVoid(string me);
        void SayMeWithComplexArgVoid(ComplexType me);
        void SayMeWithTwoArgumentsVoid(string a1, string a2);
        void SayMeWithMixedPrimitiveAndComplexArgumentsVoid(string a1, ComplexType a2, int a3);

    }
}