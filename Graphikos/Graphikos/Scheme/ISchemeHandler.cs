using IronScheme.Runtime;

namespace Graphikos.Scheme
{
    public interface ISchemeHandler
    {
        Cons CallSchemeFunc(string funcName);
    }
}