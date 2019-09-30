using System;

namespace MonteCarloError
{
    /// <summary>
    /// Exponential error propagation tool. Form is y = a exp(bx)
    /// </summary>
    public class ExponentialError : Error
    {
        public ExponentialError(string name, double a, double da, double b, double db) : base(name, a, da, b, db)
        {

        }

        internal override void Run(double x, int iterations = 10000)
        {
            for (int i = 0; i < iterations; i++)
            {
                var a = Distribution.Normal(A, ErrorA);
                var b = Distribution.Normal(B, ErrorB);

                var y = a * Math.Exp(b * x);

                Results.Add(y);
            }
        }
    }
}
