using System;
using IronScheme;
using IronScheme.Runtime;

namespace Graphikos.Scheme
{
    public class SchemeHandler : ISchemeHandler
    {
        //private string _schemefilePath = @"../../SchemeFiles/HelloWorld.ss";
        private string _schemefilePath = @"../../SchemeFiles/Scheme.ss";


        public object Evaluate(string input)
        {
            return input.Eval<Cons>();
        }

        public Cons CallSchemeFunc(string funcName)
        {
            if (funcName == null)
                throw new ArgumentNullException(nameof(funcName));

            try
            {
                System.IO.File.ReadAllText(_schemefilePath).Eval();
                return funcName.Eval<Cons>();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in Evaluation");
                return null;
            }
        }
    }
}
