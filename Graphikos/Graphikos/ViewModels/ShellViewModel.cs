using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Caliburn.Micro;
using Graphikos.Models;
using Graphikos.Scheme;

namespace Graphikos.ViewModels
{
    public class ShellViewModel : PropertyChangedBase
    {
        private readonly ISchemeHandler _schemeHandler;
        private string _input = "";
        private string _someMessage = "";
        public ObservableCollection<Line> Lines { get; set; }


        public ShellViewModel(ISchemeHandler schemeHandler)
        {
            _schemeHandler = schemeHandler;
            Lines = new ObservableCollection<Line>();
            var line1 = new Line {X1 = 0, X2 = 10, Y1 = 0, Y2 = 10 };
            var line2 = new Line {X1 = 10, X2 = 20, Y1 = 10, Y2 = 20};
            Lines.Add(line1);
            Lines.Add(line2);
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
            foreach (var expressionToEvaluate in Regex.Split(_input, "\r\n").Where(x => !string.IsNullOrWhiteSpace(x)))
            {
                SomeMessage = _schemeHandler.CallSchemeFunc(expressionToEvaluate).ToString();
            }


           
        }
    }
}
