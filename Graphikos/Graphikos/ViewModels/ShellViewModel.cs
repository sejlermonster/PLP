using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using Graphikos.Scheme;
using IronScheme;

namespace Graphikos.ViewModels
{
   
    public class ShellViewModel : PropertyChangedBase
    {
        private readonly ISchemeHandler _schemeHandler;
        private string _input = "";
        private string _someMessage = "";

        public ShellViewModel(ISchemeHandler schemeHandler)
        {
            _schemeHandler = schemeHandler;
        }

        public string Input
        {
            get { return _input; }
            set
            {
                _input = value;
                NotifyOfPropertyChange(() => Input);
            }
        }

        public string SomeMessage
        {
            get { return _someMessage; }
            set
            {
                _someMessage = value;
                NotifyOfPropertyChange(() => SomeMessage);
            }
        }

        public void Evaluate(KeyEventArgs keyArgs)
        {
            if (keyArgs.Key == Key.Enter)
            {
                SomeMessage = _schemeHandler.CallSchemeFunc(_input).ToString();
            }
        }
    }
}
