using Xunit;
using Prime.Services;

namespace Prime.UnitTests.Services
{
    public class PrimeService_IsPrimeShould
    {
        [Fact]
        public void IsPrime_InputIs1_ReturnFalse()
        {
            var primeService = new PrimeService();
            bool result = primeService.IsPrime(1);

            Assert.False(result, "1 should not be prime");
        }

        [Fact]
        public void Test_WillPass_Example1()
        {
            Assert.False(false, "2 should pass");
        }

        [Fact]
        public void Test_WillFail_Example2()
        {
            Assert.False(true, "3 should fail");
        }
    }
}
