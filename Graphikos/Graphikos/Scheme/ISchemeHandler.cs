using IronScheme.Runtime;

namespace Graphikos.Scheme
{
    public interface ISchemeHandler
    {
        object Evaluate(string input);
        Cons CallSchemeFunc(string funcName);
    }
}