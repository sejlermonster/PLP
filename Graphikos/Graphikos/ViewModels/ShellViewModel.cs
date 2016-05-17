using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using Graphikos.Models;
using Graphikos.Scheme;
using Graphikos.Utility;
using IronScheme.Runtime;

namespace Graphikos.ViewModels
{
    public class ShellViewModel : PropertyChangedBase
    {
        private readonly IBitmapDrawing _bitmapDrawing;
        private readonly ISchemeHandler _schemeHandler;
        private string _error;
        private BitmapSource _imageSource;
        private string _input;

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

        public ShellViewModel(ISchemeHandler schemeHandler, IBitmapDrawing bitmapDrawing)
        {
            if (schemeHandler == null)
                throw new ArgumentNullException(nameof(schemeHandler));
            if (bitmapDrawing == null)
                throw new ArgumentNullException(nameof(bitmapDrawing));

            _schemeHandler = schemeHandler;
            _bitmapDrawing = bitmapDrawing;
            _input = "";
        }

        //Is called for Evaluation
        public async Task Evaluate()
        {
            Error = "";
            var bitmap = new Bitmap(400, 400);
            var evaluationString = GenerateEvaluationString(_input);
            var expressions = GetExpressionsToEvaluate(evaluationString);
            foreach (var expression in expressions)
            {
                Cons result;
                try
                {
                    result = _schemeHandler.CallSchemeFunc(expression);
                }
                catch (Exception)
                {
                    Error = "Error in evaluation";
                    return;
                }

                var listOfCoordinates = result.Select(x => x).ToList();
                var color = ColorSelector(listOfCoordinates);

                var shouldHighlight = expression == expressions.Last();
                await DrawObjectOnBitmap(listOfCoordinates, color, shouldHighlight, bitmap);
            }
        }

        //Selects a color if it is the last element in the list
        //Else it uses the default color black
        private GraphikosColors ColorSelector(List<object> listOfCoordinates)
        {
            var color = GraphikosColors.Black;
            if (Enum.IsDefined(typeof(GraphikosColors), listOfCoordinates.Last()))
            {
                Enum.TryParse(listOfCoordinates.Last().ToString(), out color);
                listOfCoordinates.Remove(listOfCoordinates.Last());
            }
            return color;
        }


        private async Task DrawObjectOnBitmap(IReadOnlyCollection<object> listOfCoordinates, GraphikosColors color, bool shouldHighlight, Bitmap bitmap)
        {
            var drawColor = ColorTranslator.FromHtml(EnumDescriptions.GetEnumDescription(color));
            try
            {
                //Then we draw it as text
                if (!(listOfCoordinates.Last() is int))
                {
                    if (shouldHighlight)
                        await HighlightObject(300, drawColor, listOfCoordinates, bitmap, _bitmapDrawing.DrawText);
                    else
                        ImageSource = _bitmapDrawing.DrawText(listOfCoordinates, drawColor, bitmap);
                    return;
                }

                //Else we draw the object
                if (shouldHighlight)
                    await HighlightObject(300, drawColor, listOfCoordinates, bitmap, _bitmapDrawing.DrawPixels);
                else
                    ImageSource = _bitmapDrawing.DrawPixels(listOfCoordinates, drawColor, bitmap);
            }
            catch (Exception)
            {
                Error = "Please use pixel in the range 1-400";
            }
        }

        //Generates a string for scheme evaluation. THis adds a filter function to the evaluation if a bounding-box is the first element.
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
                        evaluationString = exp + "\r\n";
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

        //Splits the inputs
        private IEnumerable<string> GetExpressionsToEvaluate(string input)
        {
            return Regex.Split(input, "\r\n").Where(x => !string.IsNullOrWhiteSpace(x));
        }

        //Is used to highlight the newest object.
        private async Task HighlightObject(int ms, Color color, IReadOnlyCollection<object> listOfCoordinates, Bitmap bitmap, Func<IReadOnlyCollection<object>, Color, Bitmap, BitmapSource> draw)
        {
            for (var j = 0; j < 5; j++)
            {
                var drawColor = j % 2 == 0 ? color : Color.White;
                ImageSource = draw(listOfCoordinates, drawColor, bitmap);
                await Task.Delay(ms);
            }
        }
    }
}
