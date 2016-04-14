using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphikos.ViewModels
{
   
    public class ShellViewModel
    {
        public string SomeMessage { get; set; }
        public ShellViewModel()
        {
            SomeMessage = "hello world";
        }
    }
}
