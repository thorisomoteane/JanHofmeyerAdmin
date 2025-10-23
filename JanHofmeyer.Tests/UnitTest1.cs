using Xunit;

namespace JanHofmeyer.Tests
{
    public class SmokeTests
    {
        [Fact]
        public void AppStarts_PseudoTest()
        {
            // pseudo-test: we just assert true. This proves the test runner works.
            Assert.True(true);
        }

        [Fact]
        public void ExampleBusinessRule_PseudoTest()
        {
            // Another pseudo-test — you can replace this with real checks later.
            var expected = 42;
            var actual = 42; // pretend this came from your app
            Assert.Equal(expected, actual);
        }
    }
}