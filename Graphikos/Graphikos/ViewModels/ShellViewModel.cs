using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Media;
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
        public ObservableCollection<Point> Points { get; set; }
        public string Input
        {
            get { return _input; }
            set { _input = value; NotifyOfPropertyChange(() => Input); }
        }
        public string SomeMessage
        {
            get { return _someMessage; }
            set { _someMessage = value; NotifyOfPropertyChange(() => SomeMessage); }
        }

        public ShellViewModel(ISchemeHandler schemeHandler)
        {
            if (schemeHandler == null)
                throw new ArgumentNullException(nameof(schemeHandler));

            _schemeHandler = schemeHandler;
            Points = new ObservableCollection<Point>();
        }

        public void Evaluate(KeyEventArgs keyArgs)
        {
            Points.Clear();
            foreach (var expressionToEvaluate in Regex.Split(_input, "\r\n").Where(x => !string.IsNullOrWhiteSpace(x)))
            {
                var result = _schemeHandler.CallSchemeFunc(expressionToEvaluate);
                if (result == null)
                    return;
                var enumerable = result.Select(Convert.ToDouble);
                var listOfCoordinates = enumerable.ToList();

                for (var i = 0; i+3 < listOfCoordinates.Count; i++)
                {
                    var point = new Point
                    {
                        X1 = listOfCoordinates.ElementAt(i),
                        Y1 = listOfCoordinates.ElementAt(i + 1),
                        Color = new SolidColorBrush(Colors.Black)
                    };
                    i++;
                    Points.Add(point);
                    Console.WriteLine(i);
                }
            }
            
        }
    }
}
