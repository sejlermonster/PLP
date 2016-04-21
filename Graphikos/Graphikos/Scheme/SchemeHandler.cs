using IronScheme;

namespace Graphikos.Scheme
{
    public class SchemeHandler : ISchemeHandler
    {
        private string _schemefilePath = @"../../SchemeFiles/HelloWorld.ss";

        public object Evaluate(string input)
        {
            return input.Eval();
        }

        public object HelloWorld()
        {
            System.IO.File.ReadAllText(_schemefilePath).Eval();
            return "foo".Eval();
        }

        public object CallSchemeFunc(string funcName)
        {
            System.IO.File.ReadAllText(_schemefilePath).Eval();
            return funcName.Eval();
        }
    }
}
