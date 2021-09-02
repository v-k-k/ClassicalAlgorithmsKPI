using ClassicalAlgorithmsKPI.Multiplication;
using NUnit.Framework;
using System.Numerics;

namespace Tests
{
    [TestFixture]
    public class TestKaratsuba
    {
        [Test(Author = "Me", Description = "Test for Karatsuba multiplication algorithm"), Order(1)]
        public void TestPrometheusValues()
        {
            var x = "1685287499328328297814655639278583667919355849391453456921116729";
            var y = "7114192848577754587969744626558571536728983167954552999895348492";
            var _res = "11989460275519080564894036768322865785999566885539505969749975204962718118914971586072960191064507745920086993438529097266122668";

            var calculated = Karatsuba.MultTwoNums(x, y);

            BigInteger RES = BigInteger.Parse(_res);
            Assert.AreEqual(0, BigInteger.Compare(RES, calculated));

            var recalculated = BigInteger.Multiply(BigInteger.Parse(x), BigInteger.Parse(y));
            Assert.AreEqual(0, BigInteger.Compare(recalculated, calculated));
        }
    }
}
