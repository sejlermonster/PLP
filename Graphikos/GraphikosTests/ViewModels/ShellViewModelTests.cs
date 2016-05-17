using System.Collections.Generic;
using System.Drawing;
using System.Windows.Interop;
using Graphikos.Models;
using Graphikos.Scheme;
using Graphikos.ViewModels;
using IronScheme.Runtime;
using Moq;
using Shouldly;
using Xunit;

namespace GraphikosTests.ViewModels
{
    public class ShellViewModelTests
    {
        private readonly ShellViewModel _shellViewModel;

        public ShellViewModelTests()
        {
            var mock = new Mock<ISchemeHandler>();
            mock.Setup(m => m.CallSchemeFunc(It.IsAny<string>())).Returns(new Cons(new object()));
            _shellViewModel = new ShellViewModel(mock.Object);
        }

        [Fact]
        public void CanGetExpressionToEvaluate()
        {
            var testData = new List<string> {"line(11, 11, 2, 213)", "line(23, 23, 23, 23)"};
            var result = _shellViewModel.GetExpressionsToEvaluate("line(11, 11, 2, 213)\r\nline(23, 23, 23, 23)");
            result.ShouldBe(testData);
        }

        [Fact]
        public void CanEvaluate()
        {
            var schemeHandler = new Mock<ISchemeHandler>();
            schemeHandler.Setup(m => m.CallSchemeFunc(It.IsAny<string>()))
                .Returns(new Cons("1", new List<string> {"1", "2"}));
            var shellView = new ShellViewModel(schemeHandler.Object) {Input = "line(11, 11, 2, 213)"};
            shellView.Evaluate();

            schemeHandler.Verify(x => x.CallSchemeFunc(It.IsAny<string>()));
        }

        [Fact]
        public void CanEvaluateHandleNull()
        {
            var schemeHandler = new Mock<ISchemeHandler>();
            schemeHandler.Setup(m => m.CallSchemeFunc(It.IsAny<string>())); //Returns null
            var shellView = new ShellViewModel(schemeHandler.Object) {Input = "line(11, 11, 2, 213)"};
            shellView.Evaluate();

            schemeHandler.Verify(x => x.CallSchemeFunc(It.IsAny<string>()));
        }

        [Fact]
        public void CanConvertBitmapToBitmapSource()
        {
            var testBitmap = new Bitmap(400, 400);
            var result = _shellViewModel.BitmapToBitmapSource(testBitmap);
            result.ShouldBeOfType<InteropBitmap>();
            result.Height.ShouldBe(400);
            result.Width.ShouldBe(400);
        }


        [Fact]
        public void SetPixelsThrowsIfPixelsArentInRangeOf1To400()
        {
            var listOfCoordinates = new List<object> {401, 401, 401, 401, 401, 401};
            _shellViewModel.SetPixels(listOfCoordinates, GraphikosColors.Black);
            _shellViewModel.Error.ShouldNotBeNullOrEmpty();
        }

        [Fact]
        public void CanGenerateEvaluationStringWithFilter()
        {
            var result = _shellViewModel.GenerateEvaluationString("(bounding-box 0 0 5 5)\r\n(line 0 0 5 5)");
            result.ShouldBe("(bounding-box 0 0 5 5)\r\n(filter 0 0 5 5 (line 0 0 5 5))\r\n");
        }

        [Fact]
        public void CanGenerateEvaluationStringWithOutFilter()
        {
            var result = _shellViewModel.GenerateEvaluationString("(bounding-box 0 0 5 5)\r\n");
            result.ShouldBe("(bounding-box 0 0 5 5)\r\n");
        }

        [Fact]
        public void CanGenerateEvaluationStringWithOutFBounding_Box()
        {
            var result = _shellViewModel.GenerateEvaluationString("(line 0 0 5 5)\r\n");
            result.ShouldBe("(line 0 0 5 5)\r\n");
        }
    }
}
