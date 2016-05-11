using System;
using System.Collections.Generic;
using Graphikos.Scheme;
using Graphikos.ViewModels;
using Moq;
using IronScheme.Runtime;

namespace GraphikosTesting.ViewModels
{
    class ShellViewModelTests
    {
        private ShellViewModel shellViewModel;

        public ShellViewModelTests()
        {
            var mock = new Mock<ISchemeHandler>();
            mock.Setup(m => m.CallSchemeFunc(It.IsAny<String>())).Returns();
            this.shellViewModel = new ShellViewModel(mock.Object);
        }
    }
}
