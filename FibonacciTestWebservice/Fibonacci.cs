using System.Numerics;

namespace FibonacciTestWebservice
{
    public class Fibonacci
    {
        //https://www.nayuki.io/page/fast-fibonacci-algorithms
        private static BigInteger FastDoublingFibonacci(BigInteger n)
        {
            if (n.Equals(0))
            {
                return BigInteger.Zero;
            }else if (n <= 2)
            {
                return BigInteger.One;
            }
            BigInteger k = n/2;
            BigInteger a = FastDoublingFibonacci(k + 1);
            BigInteger b = FastDoublingFibonacci(k);
            return (n % 2 == 1) 
                ? a * a + b * b 
                : b * (2 * a - b); 
        }
        
        public BigInteger Calculate(long n)
        {
            return FastDoublingFibonacci(new BigInteger(n));
        }
    }
}