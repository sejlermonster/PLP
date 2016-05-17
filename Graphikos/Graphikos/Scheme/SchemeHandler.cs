using System;
using IronScheme;
using IronScheme.Runtime;

namespace Graphikos.Scheme
{
    public class SchemeHandler : ISchemeHandler
    {
        private readonly string _schemefilePath;

        public SchemeHandler(string schemeFilePath)
        {
            _schemefilePath = string.IsNullOrEmpty(schemeFilePath) ? @"../../SchemeFiles/Scheme.ss" : schemeFilePath;
            System.IO.File.ReadAllText(_schemefilePath).Eval();
        }

        public Cons CallSchemeFunc(string funcName)
        {
            if (funcName == null)
                throw new ArgumentNullException(nameof(funcName));

            return funcName.Eval<Cons>();
        }
    }
}
