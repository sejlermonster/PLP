using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graphikos.Scheme;
using IronScheme;

namespace Graphikos.ViewModels
{
   
    public class ShellViewModel
    {
        private readonly ISchemeHandler _schemeHandler;
        public string SomeMessage { get; set; }
        public ShellViewModel(ISchemeHandler schemeHandler)
        {
            _schemeHandler = schemeHandler;
            SomeMessage = _schemeHandler.Evaluate("(+ 1 2 3)").ToString();
           
        }
    }
}
