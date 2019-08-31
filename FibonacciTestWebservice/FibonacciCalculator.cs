using System.Numerics;

namespace FibonacciTestWebservice
{
    public static class FibonacciCalculator
    {
        //https://www.nayuki.io/page/fast-fibonacci-algorithms
        private static BigInteger FastDoublingFibonacci(BigInteger n)
        {
            if (n.Equals(0))
                return BigInteger.Zero;
            if (n <= 2) return BigInteger.One;
            var k = n / 2;
            var a = FastDoublingFibonacci(k + 1);
            var b = FastDoublingFibonacci(k);
            return n % 2 == 1
                ? a * a + b * b
                : b * (2 * a - b);
        }

        public static BigInteger Calculate(long n)
        {
            return FastDoublingFibonacci(new BigInteger(n));
        }
    }
}