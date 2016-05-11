using System.Collections.Generic;
using System.Linq;
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
        private readonly ShellViewModel shellViewModel;

        public ShellViewModelTests()
        {
            var mock = new Mock<ISchemeHandler>();
            mock.Setup(m => m.CallSchemeFunc(It.IsAny<string>())).Returns(new Cons(new object()));
            shellViewModel = new ShellViewModel(mock.Object);
        }

        [Fact]
        public void CanGetExpressionToEvaluate()
        {
            var testData = new List<string>() { "line(11, 11, 2, 213)", "line(23, 23, 23, 23)" };
            var result = shellViewModel.GetExpressionsToEvaluate("line(11, 11, 2, 213)\r\nline(23, 23, 23, 23)");
            result.ShouldBe(testData);
        }

        [Fact]
        public void CanAddCoordinatesToCanvas()
        {
            var testData = new List<double>() {2, 5, 3, 5, 4, 5, 5,5};
            shellViewModel.AddCoordinatesToCanvas(testData);
            shellViewModel.Points.Count.ShouldBe(3);
            shellViewModel.Points.Count(x => x.X == 2 && x.Y == 5).ShouldBe(1);
            shellViewModel.Points.Count(x => x.X == 3 && x.Y == 5).ShouldBe(1);
            shellViewModel.Points.Count(x => x.X == 4 && x.Y == 5).ShouldBe(1);
        }

        [Fact]
        public void CanEvaluate()
        {
            var schemeHandler = new Mock<ISchemeHandler>();
            schemeHandler.Setup(m => m.CallSchemeFunc(It.IsAny<string>())).Returns(new Cons("1", new List<string>() {"1", "2"}));
            var shellView = new ShellViewModel(schemeHandler.Object) {Input = "line(11, 11, 2, 213)"};
            shellView.Evaluate();

            schemeHandler.Verify(x => x.CallSchemeFunc(It.IsAny<string>()));
        }

        [Fact]
        public void CanEvaluateHandleNull()
        {
            var schemeHandler = new Mock<ISchemeHandler>();
            schemeHandler.Setup(m => m.CallSchemeFunc(It.IsAny<string>())); //Returns null
            var shellView = new ShellViewModel(schemeHandler.Object) { Input = "line(11, 11, 2, 213)" };
            shellView.Evaluate();

            schemeHandler.Verify(x => x.CallSchemeFunc(It.IsAny<string>()));
        }
    }
}
