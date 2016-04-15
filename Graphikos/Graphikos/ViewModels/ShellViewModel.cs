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
        private SchemeHandler schemeHandler;
        public string SomeMessage { get; set; }
        public ShellViewModel()
        {
            schemeHandler = new SchemeHandler();
            SomeMessage = schemeHandler.Evaluate("(+ 1 2 3)").ToString();
           
        }
    }
}
