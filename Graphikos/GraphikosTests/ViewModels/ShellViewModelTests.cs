using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Media.Imaging;
using Graphikos.Models;
using Graphikos.Scheme;
using Graphikos.Utility;
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
            var schemeHandlerMock = new Mock<ISchemeHandler>();
            var bitmapMock = new Mock<IBitmapDrawing>();
            schemeHandlerMock.Setup(m => m.CallSchemeFunc(It.IsAny<string>())).Returns(new Cons(new object()));
            _shellViewModel = new ShellViewModel(schemeHandlerMock.Object, bitmapMock.Object);
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
            var bitmapMock = new Mock<IBitmapDrawing>();
            schemeHandler.Setup(m => m.CallSchemeFunc(It.IsAny<string>())).Returns(new Cons("1", new List<string> {"1", "2"}));
            var shellView = new ShellViewModel(schemeHandler.Object, bitmapMock.Object) {Input = "line(11, 11, 2, 213)"};
            var result = shellView.Evaluate();
            result.Wait();

            schemeHandler.Verify(x => x.CallSchemeFunc(It.IsAny<string>()));
        }

        [Fact]
        public void CanEvaluateHandleException()
        {
            var schemeHandler = new Mock<ISchemeHandler>();
            var bitmapMock = new Mock<IBitmapDrawing>();
            schemeHandler.Setup(m => m.CallSchemeFunc(It.IsAny<string>())).Throws<Exception>();
            var shellView = new ShellViewModel(schemeHandler.Object, bitmapMock.Object) {Input = "line(11, 11, 2, 213)"};
            var reuslt=  shellView.Evaluate();
            reuslt.Wait();
            shellView.Error.ShouldNotBeNullOrEmpty();
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

        [Fact]
        public void CanDrawObjectOnBitmap()
        {
            var schemeHandler = new Mock<ISchemeHandler>();
            var bitmapMock = new Mock<IBitmapDrawing>();
            var bitmapSource = Ext.LoadBitMapSource("TestImage.bmp");
            bitmapMock.Setup(m => m.DrawPixels(It.IsAny<IReadOnlyCollection<object>>(),It.IsAny<Color>(), It.IsAny<Bitmap>())).Returns(bitmapSource);
            var shellView = new ShellViewModel(schemeHandler.Object, bitmapMock.Object);
            var listOfCoordinates = new List<object> { 1, 1, 2, 2, 3, 3 };

            var result = shellView.DrawObjectOnBitmap(listOfCoordinates, GraphikosColors.Black, false, new Bitmap(400, 400));
            result.Wait();

            shellView.ImageSource.GetBytes().SequenceEqual(bitmapSource.GetBytes()).ShouldBe(true);
        }

        [Fact]
        public void CanDrawTextOnBitmap()
        {
            var schemeHandler = new Mock<ISchemeHandler>();
            var bitmapMock = new Mock<IBitmapDrawing>();
            var bitmapSource = Ext.LoadBitMapSource("TestImage.bmp");
            bitmapMock.Setup(m => m.DrawText(It.IsAny<IReadOnlyCollection<object>>(), It.IsAny<Color>(), It.IsAny<Bitmap>())).Returns(bitmapSource);
            var shellView = new ShellViewModel(schemeHandler.Object, bitmapMock.Object);
            var listOfCoordinates = new List<object> { 1, 1, 2, 2, 3, 3, "Some Text" };

            var result = shellView.DrawObjectOnBitmap(listOfCoordinates, GraphikosColors.Black, false, new Bitmap(400, 400));
            result.Wait();

            shellView.ImageSource.GetBytes().SequenceEqual(bitmapSource.GetBytes()).ShouldBe(true);
        }

        [Fact]
        public void DrawObjectOnBitmapCanHandleException()
        {
            var schemeHandler = new Mock<ISchemeHandler>();
            var bitmapMock = new Mock<IBitmapDrawing>();
            bitmapMock.Setup(m => m.DrawPixels(It.IsAny<IReadOnlyCollection<object>>(), It.IsAny<Color>(), It.IsAny<Bitmap>())).Throws<Exception>();
            var shellView = new ShellViewModel(schemeHandler.Object, bitmapMock.Object);
            var listOfCoordinates = new List<object> { 0, 0, 2, 2, 401, 401};

            var result = shellView.DrawObjectOnBitmap(listOfCoordinates, GraphikosColors.Black, false, new Bitmap(400, 400));
            result.Wait();
            shellView.Error.ShouldNotBeNullOrEmpty();        
        }

        [Fact]
        public void CanDrawTextOnBitmapWithHighlighting()
        {
            var schemeHandler = new Mock<ISchemeHandler>();
            var bitmapMock = new Mock<IBitmapDrawing>();
            var bitmapSource = Ext.LoadBitMapSource("TestImage.bmp");
            bitmapMock.Setup(m => m.DrawText(It.IsAny<IReadOnlyCollection<object>>(), It.IsAny<Color>(), It.IsAny<Bitmap>())).Returns(bitmapSource);
            var shellView = new ShellViewModel(schemeHandler.Object, bitmapMock.Object);
            var listOfCoordinates = new List<object> { 1, 1, 2, 2, 3, 3, "Some Text" };

            var result = shellView.DrawObjectOnBitmap(listOfCoordinates, GraphikosColors.Black, true, new Bitmap(400, 400));
            result.Wait();

            shellView.ImageSource.GetBytes().SequenceEqual(bitmapSource.GetBytes()).ShouldBe(true);
        }

        [Fact]
        public void CanPixelsOnBitmapWithHighlighting()
        {
            var schemeHandler = new Mock<ISchemeHandler>();
            var bitmapMock = new Mock<IBitmapDrawing>();
            var bitmapSource = Ext.LoadBitMapSource("TestImage.bmp");
            bitmapMock.Setup(m => m.DrawPixels(It.IsAny<IReadOnlyCollection<object>>(), It.IsAny<Color>(), It.IsAny<Bitmap>())).Returns(bitmapSource);
            var shellView = new ShellViewModel(schemeHandler.Object, bitmapMock.Object);
            var listOfCoordinates = new List<object> { 1, 1, 2, 2, 3, 3};

            var result = shellView.DrawObjectOnBitmap(listOfCoordinates, GraphikosColors.Black, true, new Bitmap(400, 400));
            result.Wait();

            shellView.ImageSource.GetBytes().SequenceEqual(bitmapSource.GetBytes()).ShouldBe(true);
        }

        [Fact]
        public void CanHighlightObjects()
        {
            var bitmapSource = Ext.LoadBitMapSource("TestImage.bmp");
            var listOfCoordinates = new List<object> { 1, 1, 2, 2, 3, 3 };

            var result = _shellViewModel.HighlightObject(0, Color.Aqua, listOfCoordinates, new Bitmap(400, 400), Draw);
            result.Wait();

            _shellViewModel.ImageSource.GetBytes().SequenceEqual(bitmapSource.GetBytes()).ShouldBe(true);
        }

        [Fact]
        public void CanSelectColor()
        {
            var listOfCoordinates = new List<object> {"Red"};

            var result = _shellViewModel.ColorSelector(listOfCoordinates);

            result.ShouldBe(GraphikosColors.Red);
        }

        [Fact]
        public void SelectsDefaultColorIfNoColorIsInTheLastPostionOfTheListOfCoordinates()
        {
            var listOfCoordinates = new List<object> { 50 };

            var result = _shellViewModel.ColorSelector(listOfCoordinates);

            result.ShouldBe(GraphikosColors.Black);
        }

        private static BitmapSource Draw(IReadOnlyCollection<object> readOnlyCollection, Color color, Bitmap arg3)
        {
            return Ext.LoadBitMapSource("TestImage.bmp");
        }
    }
}
