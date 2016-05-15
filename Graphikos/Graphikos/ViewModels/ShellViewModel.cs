using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using Graphikos.Models;
using Graphikos.Scheme;

namespace Graphikos.ViewModels
{
    public class ShellViewModel : PropertyChangedBase
    {
        private readonly ISchemeHandler _schemeHandler;
        private string _error;
        private string _input;
        private BitmapSource _imageSource;
        private Bitmap _operableBitMap;

        public string Error
        {
            get { return _error; }
            set
            {
                _error = value;
                NotifyOfPropertyChange();
            }
        }

        public BitmapSource ImageSource
        {
            get { return _imageSource; }
            set
            {
                _imageSource = value;
                NotifyOfPropertyChange(() => ImageSource);
            }
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

        public ShellViewModel(ISchemeHandler schemeHandler)
        {
            if (schemeHandler == null)
                throw new ArgumentNullException(nameof(schemeHandler));
            _schemeHandler = schemeHandler;
            _input = "";
        }

        public void Evaluate()
        {
            _operableBitMap = new Bitmap(400, 400);
            var evaluationString = GenerateEvaluationString(_input);

            foreach (var result in GetExpressionsToEvaluate(evaluationString).Select(expressionToEvaluate => _schemeHandler.CallSchemeFunc(expressionToEvaluate)))
            {
                Error = "";
                if (result == null)
                {
                    Error = "Error in evaluation";
                    return;
                }

                var enumerable = result.Select(x => x).ToList();
                var listOfCoordinates = enumerable.ToList();

                var color = GraphikosColors.Black;
                if (Enum.IsDefined(typeof(GraphikosColors), listOfCoordinates.Last()))
                {
                    Enum.TryParse(listOfCoordinates.Last().ToString(), out color);
                    listOfCoordinates.Remove(listOfCoordinates.Last());
                }
                SetPixels(listOfCoordinates, color);
            }
        }

        private string GenerateEvaluationString(string input)
        {
            var expressions = GetExpressionsToEvaluate(input);
            var evaluationString = "";
            foreach (var expression in expressions)
            {
                if (!expression.Contains("bounding-box"))
                    continue;
                var boundingBox = Regex.Split(expression, @"\s+");
                var boundingBoxCoordinates = new BoundingBox(Convert.ToInt32(boundingBox[1]),
                                                             Convert.ToInt32(boundingBox[2]),
                                                             Convert.ToInt32(boundingBox[3]),
                                                             Convert.ToInt32(boundingBox[4].Replace(")", "")));
                foreach (var exp in expressions)
                {
                    if (exp.Contains("bounding-box"))
                    {
                        evaluationString = "(bounding-box " +
                                           boundingBoxCoordinates.X1 + " " +
                                           boundingBoxCoordinates.Y1 + " " +
                                           boundingBoxCoordinates.X2 + " " +
                                           boundingBoxCoordinates.Y2 + ")\r\n";
                    }
                    else
                    {
                        evaluationString = evaluationString +
                                           "(filter "
                                           + boundingBoxCoordinates.X1 + " "
                                           + boundingBoxCoordinates.Y1 + " "
                                           + boundingBoxCoordinates.X2 + " "
                                           + boundingBoxCoordinates.Y2 + " " + exp + ")\r\n";
                    }
                }
            }
            return string.IsNullOrEmpty(evaluationString) ? input : evaluationString;
        }

        private IEnumerable<string> GetExpressionsToEvaluate(string input)
        {
            return Regex.Split(input, "\r\n").Where(x => !string.IsNullOrWhiteSpace(x));
        }

        private void SetPixels(IReadOnlyCollection<object> listOfCoordinates, GraphikosColors color)
        {
            for (var i = 0; i + 3 < listOfCoordinates.Count; i++)
            {
                _operableBitMap.SetPixel((int)listOfCoordinates.ElementAt(i), (int)listOfCoordinates.ElementAt(i + 1), ColorTranslator.FromHtml(EnumDescriptions.GetEnumDescription(color)));
                i++;
            }

            ImageSource = BitmapToBitmapSource(_operableBitMap);
        }

        private static BitmapSource BitmapToBitmapSource(Bitmap source)
        {
            return Imaging.CreateBitmapSourceFromHBitmap(
                                                         source.GetHbitmap(),
                                                         IntPtr.Zero,
                                                         Int32Rect.Empty,
                                                         BitmapSizeOptions.FromEmptyOptions());
        }
    }
}
