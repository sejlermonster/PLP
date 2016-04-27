using System;
using IronScheme;

namespace Graphikos.Scheme
{
    public class SchemeHandler : ISchemeHandler
    {
        private string _schemefilePath = @"../../SchemeFiles/HelloWorld.ss";
        //private string _schemefilePath = @"../../SchemeFiles/Scheme.ss";


        public object Evaluate(string input)
        {
            return input.Eval();
        }

        public object CallSchemeFunc(string funcName)
        {
            try
            {
                System.IO.File.ReadAllText(_schemefilePath).Eval();
                return funcName.Eval();
            }
            catch (Exception e)
            {
                return "Error in Evaluation";
            }

        }
    }
}
