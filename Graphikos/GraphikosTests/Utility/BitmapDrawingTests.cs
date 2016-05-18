using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Interop;
using Graphikos.Utility;
using Shouldly;
using Xunit;

namespace GraphikosTests.Utility
{
    public class BitmapDrawingTests
    {
        private readonly BitmapDrawing _bitmapDrawing;

        public BitmapDrawingTests()
        {
            _bitmapDrawing = new BitmapDrawing();
        }
        [Fact]
        public void CanConvertBitmapToBitmapSource()
        {
            var testBitmap = new Bitmap(400, 400);
            var result = _bitmapDrawing.BitmapToBitmapSource(testBitmap);
            result.ShouldBeOfType<InteropBitmap>();
            result.Height.ShouldBe(400);
            result.Width.ShouldBe(400);
        }


        [Fact]
        public void CanDrawPixels()
        {
            var listOfCoordinates = new List<object> { 1, 1, 2, 2, 3, 3 };
            var result = _bitmapDrawing.DrawPixels(listOfCoordinates, Color.Black, new Bitmap(400, 400));
            var bitmapComp = Ext.LoadBytes("CanDrawPixels.bmp");
            result.GetBytes().SequenceEqual(bitmapComp).ShouldBe(true);

        }

        [Fact]
        public void CanDrawText()
        {
            var listOfCoordinates = new List<object> { 1, 1, 2, 2, 3, 3 };
            var result = _bitmapDrawing.DrawText(listOfCoordinates, Color.Black, new Bitmap(400, 400));
            var bitmapComp = Ext.LoadBytes("CanDrawText.bmp");
            result.GetBytes().SequenceEqual(bitmapComp).ShouldBe(true);
        }

        [Fact]
        public void CanDrawTextWithLessThanThreeCoordinates()
        {
            var listOfCoordinates = new List<object> { 1, 1, 2, 2 };
            var result = _bitmapDrawing.DrawText(listOfCoordinates, Color.Black, new Bitmap(400, 400));
            var bitmapComp = Ext.LoadBytes("CanDrawTextWithLessThanThreeCoordinates.bmp");
            result.GetBytes().SequenceEqual(bitmapComp).ShouldBe(true);
        }

        [Fact]
        public void ThrowWhenPixelsIsDrawnOutsideBitmap()
        {
            var listOfCoordinates = new List<object> { 0, 0, 2, 2, 401, 401 };
            Assert.Throws<ArgumentOutOfRangeException>(() => _bitmapDrawing.DrawPixels(listOfCoordinates, Color.Black, new Bitmap(400, 400)));
        }
    }
}
