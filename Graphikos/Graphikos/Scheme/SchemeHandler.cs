using System;
using IronScheme;
using IronScheme.Runtime;

namespace Graphikos.Scheme
{
    public class SchemeHandler : ISchemeHandler
    {
        //private string _schemefilePath = @"../../SchemeFiles/HelloWorld.ss";
        private string _schemefilePath = @"../../SchemeFiles/Scheme.ss";

        public SchemeHandler(string schemeFilePath)
        {
            _schemefilePath = schemeFilePath;
        }

        public SchemeHandler()
        {}

        public Cons CallSchemeFunc(string funcName)
        {
            if (funcName == null)
                throw new ArgumentNullException(nameof(funcName));

            try
            {
                System.IO.File.ReadAllText(_schemefilePath).Eval();
                return funcName.Eval<Cons>();
            }
            catch (Exception)
            {
                Console.WriteLine("Error in Evaluation");
                return null;
            }
        }
    }
}
