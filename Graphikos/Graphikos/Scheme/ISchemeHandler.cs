namespace Graphikos.Scheme
{
    public interface ISchemeHandler
    {
        object Evaluate(string input);
        object HelloWorld();
        object CallSchemeFunc(string funcName);
    }
}