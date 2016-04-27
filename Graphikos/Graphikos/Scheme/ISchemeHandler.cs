namespace Graphikos.Scheme
{
    public interface ISchemeHandler
    {
        object Evaluate(string input);
        object CallSchemeFunc(string funcName);
    }
}