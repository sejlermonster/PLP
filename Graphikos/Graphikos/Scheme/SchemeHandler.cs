using IronScheme;

namespace Graphikos.Scheme
{
    public class SchemeHandler
    {
        public object Evaluate(string input)
        {
            return input.Eval();
        }
    }
}
