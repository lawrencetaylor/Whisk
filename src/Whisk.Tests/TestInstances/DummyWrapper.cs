namespace Whisk.Tests
{
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

        public void SayMeWithStringArgVoid(string me)
        {
            _dummy.SayMeWithStringArgVoid(me);
        }

        public void SayMeWithComplexArgVoid(ComplexType me)
        {
            _dummy.SayMeWithComplexArgVoid(me);
        }

        public void SayMeWithTwoArgumentsVoid(string a1, string a2)
        {
            _dummy.SayMeWithTwoArgumentsVoid(a1,a2);
        }

        public void SayMeWithMixedPrimitiveAndComplexArgumentsVoid(string a1, ComplexType a2, int a3)
        {
            _dummy.SayMeWithMixedPrimitiveAndComplexArgumentsVoid(a1, a2, a3);
        }
    }
}