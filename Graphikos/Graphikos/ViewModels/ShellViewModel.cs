using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media.Imaging;
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
        private BitmapSource _imageSource;
        private Bitmap _operableBitMap;


        public ShellViewModel(ISchemeHandler schemeHandler)
        {
            if (schemeHandler == null)
                throw new ArgumentNullException(nameof(schemeHandler));
            _schemeHandler = schemeHandler;
        }

        public BitmapSource ImageSource
        {
            get { return _imageSource; }
            set { _imageSource = value; NotifyOfPropertyChange(() => ImageSource); }
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
            _operableBitMap = new Bitmap(400, 400);
            foreach (var result in GetExpressionsToEvaluate(_input).Select(expressionToEvaluate => _schemeHandler.CallSchemeFunc(expressionToEvaluate))) {
                if (result == null)
                    return;

                var enumerable = result.Select(x => x);
                var listOfCoordinates = enumerable.ToList();

                GraphikosColors color = GraphikosColors.Black;
                if (Enum.IsDefined(typeof(GraphikosColors), listOfCoordinates.Last()))
                {
                    Enum.TryParse<GraphikosColors>(listOfCoordinates.Last().ToString(), out color);
                    listOfCoordinates.Remove(listOfCoordinates.Last());
                }

                SetPixels(listOfCoordinates, color);
            }
        }

        public IEnumerable<string> GetExpressionsToEvaluate(string input)
        {
            return Regex.Split(input, "\r\n").Where(x => !string.IsNullOrWhiteSpace(x));
        }

        public void SetPixels(IReadOnlyCollection<object> listOfCoordinates, GraphikosColors color)
        {
           for (var i = 0; i + 3 < listOfCoordinates.Count; i++)
            {
                _operableBitMap.SetPixel((int)listOfCoordinates.ElementAt(i), (int)listOfCoordinates.ElementAt(i + 1), ColorTranslator.FromHtml(EnumDescriptions.GetEnumDescription(color)));
                i++;
            }

            ImageSource = BitmapToBitmapSource(_operableBitMap);
        }

        public static BitmapSource BitmapToBitmapSource(Bitmap source)
        {
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                          source.GetHbitmap(),
                          IntPtr.Zero,
                          Int32Rect.Empty,
                          BitmapSizeOptions.FromEmptyOptions());
        }
    }
}
