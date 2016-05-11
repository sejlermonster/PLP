using System;
using Graphikos.Scheme;
using Shouldly;
using Xunit;

namespace GraphikosTests.Scheme
{
    public class SchemeHandlerTests
    {
        private readonly SchemeHandler _schemehandler;

        public SchemeHandlerTests()
        {
            _schemehandler = new SchemeHandler(@"../../TestFiles/TestFile.ss");
        }

        [Fact]
        public void CanCallChemeFunc()
        {
            var result = _schemehandler.CallSchemeFunc("testFunc");
            result.car.ShouldBe(1000);
        }

        [Fact]
        public void ThrowsWhenParameterIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _schemehandler.CallSchemeFunc(null));
        }

        [Fact]
        public void RetunsNullWhenErrorInEvaluationHappens()
        {
            var result = _schemehandler.CallSchemeFunc("SomeFuncNameWhichDoesNotExist");
            result.ShouldBe(null);
        }
    }
}
