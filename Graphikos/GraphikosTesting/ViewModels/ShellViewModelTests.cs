using System;
using System.Collections.Generic;
using Graphikos.Scheme;
using Graphikos.ViewModels;
using IronScheme.Runtime;
using Moq;
using Xunit;

namespace GraphikosTesting.ViewModels
{
    class ShellViewModelTests
    {
        private ShellViewModel shellViewModel;

        public ShellViewModelTests()
        {
            var mock = new Mock<ISchemeHandler>();
            mock.Setup(m => m.CallSchemeFunc(It.IsAny<string>())).Returns(new Cons(new object()));
            this.shellViewModel = new ShellViewModel(mock.Object);
        }

        [Fact]
        public void CanGetExpressionToEvaluate()
        {
            var testData = new List<string>() {"foo", "boo"};
            var foo = testData.ToString();
            shellViewModel.GetExpressionsToEvaluate("line(11, 11, 2, 213)\r\nline(23, 23, 23, 23)");
        }

        [Fact]
        public void CanEvaluate()
        {
            shellViewModel.Input = "SomeExpressionToHandle";
        }
    }
}
