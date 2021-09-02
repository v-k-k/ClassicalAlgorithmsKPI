using System;
using System.Linq;
using System.Numerics;

namespace ClassicalAlgorithmsKPI.Multiplication
{
    public static class Karatsuba
    {
        private static string __one = "1";
        private static char __zero = '0';

        public static BigInteger MultTwoNums(string X, string Y)
        {
            string[] arr = { X, Y };
            var n = arr.Max(val => val.Length);

            var xMid = X.Length / 2;
            var a = BigInteger.Parse(X.Substring(0, xMid));
            var b = BigInteger.Parse(X.Substring(xMid, X.Length - xMid));

            var yMid = Y.Length / 2;
            var c = BigInteger.Parse(Y.Substring(0, yMid));
            var d = BigInteger.Parse(Y.Substring(yMid, Y.Length - yMid));

            var aC = a * c;
            var bD = b * d;
            var aPlusBcPlusD = (a + b) * (c + d);
            var powN = __one + new String(__zero, n);
            var powNdiv2 = __one + new String(__zero, n / 2);
            var aCpow = BigInteger.Parse(powN) * aC;
            var optimized = BigInteger.Parse(powNdiv2) * (aPlusBcPlusD - aC - bD);

            return aCpow + optimized + bD;
        }
    }
}
