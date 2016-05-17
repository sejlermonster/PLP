using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
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
        private BitmapSource _imageSource;
        private string _input;
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
                if (!(listOfCoordinates.Last() is int))
                    DrawText(listOfCoordinates);

                SetPixels(listOfCoordinates, color);
            }
        }

        private void DrawText(List<object> listOfCoordinates)
        {
            if (listOfCoordinates.Count < 3)
                return;

            using (Graphics g = Graphics.FromImage(_operableBitMap))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                StringFormat format = new StringFormat()
                                      {
                                          Alignment = StringAlignment.Center,
                                          LineAlignment = StringAlignment.Center,
                                      };
                PointF drawPoint = new PointF(Convert.ToInt32(listOfCoordinates[0]), _operableBitMap.Height - Convert.ToInt32(listOfCoordinates[1]));
                g.DrawString(listOfCoordinates.Last().ToString(), new Font("Tahoma", 12), Brushes.Black, drawPoint, format);
            }
            ImageSource = BitmapToBitmapSource(_operableBitMap);
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

        private IEnumerable<string> GetExpressionsToEvaluate(string input)
        {
            return Regex.Split(input, "\r\n").Where(x => !string.IsNullOrWhiteSpace(x));
        }

        private void SetPixels(IReadOnlyCollection<object> listOfCoordinates, GraphikosColors color)
        {
            try
            {
                for (var i = 0; i + 3 < listOfCoordinates.Count; i++)
                {
                    _operableBitMap.SetPixel((int)listOfCoordinates.ElementAt(i),
                                             _operableBitMap.Height - (int)listOfCoordinates.ElementAt(i + 1),
                                             ColorTranslator.FromHtml(EnumDescriptions.GetEnumDescription(color)));
                    i++;
                }
            }
            catch (Exception)
            {
                Error = "Please use pixel in the range 1-400";
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
