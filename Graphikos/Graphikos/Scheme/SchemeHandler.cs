using IronScheme;

namespace Graphikos.Scheme
{
    public class SchemeHandler : ISchemeHandler
    {
        public object Evaluate(string input)
        {
            return input.Eval();
        }
    }
}
