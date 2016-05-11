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
using IronScheme.Runtime;

namespace Graphikos.ViewModels
{
    public class ShellViewModel : PropertyChangedBase
    {
        private readonly ISchemeHandler _schemeHandler;
        private string _input = "";
        private string _someMessage = "";
        public ObservableCollection<Point> Points { get; set; }

        public ShellViewModel(ISchemeHandler schemeHandler)
        {
            if (schemeHandler == null)
                throw new ArgumentNullException(nameof(schemeHandler));

            _schemeHandler = schemeHandler;
            Points = new ObservableCollection<Point>();
        }

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

        public void Evaluate()
        {
            Points.Clear();
            foreach (var expressionToEvaluate in GetExpressionsToEvaluate(_input))
            {
                var result = _schemeHandler.CallSchemeFunc(expressionToEvaluate);
                Console.WriteLine("result returned");
                if (result == null)
                    return;
                var enumerable = result.Select(Convert.ToDouble);
                var listOfCoordinates = enumerable.ToList();
                Console.WriteLine("Algorithm started");
                AddCoordinatesToCanvas(listOfCoordinates);
            }
            
        }

        public void AddCoordinatesToCanvas(IReadOnlyCollection<double> listOfCoordinates)
        {

            for (var i = 0; i + 3 < listOfCoordinates.Count; i++)
            {
                var point = new Point
                {
                    X = listOfCoordinates.ElementAt(i),
                    Y = listOfCoordinates.ElementAt(i + 1),
                    Color = new SolidColorBrush(Colors.Black)
                };
                i++;
                Points.Add(point);
            }
                Console.WriteLine("Algorithm ended");
        }
        public IEnumerable<string> GetExpressionsToEvaluate(string input)
        {
            return Regex.Split(input, "\r\n").Where(x => !string.IsNullOrWhiteSpace(x));
        }
    }
}
