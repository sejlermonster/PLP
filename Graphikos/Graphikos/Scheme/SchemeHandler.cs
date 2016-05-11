using System;
using IronScheme;
using IronScheme.Runtime;

namespace Graphikos.Scheme
{
    public class SchemeHandler : ISchemeHandler
    {
        //private string _schemefilePath = @"../../SchemeFiles/HelloWorld.ss";
        private readonly string _schemefilePath;

        public SchemeHandler(string schemeFilePath)
        {
            _schemefilePath = string.IsNullOrEmpty(schemeFilePath) ? @"../../SchemeFiles/Scheme.ss" : schemeFilePath;
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
