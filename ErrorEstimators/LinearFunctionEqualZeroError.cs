using System;
namespace MonteCarloError
{
    public class LinearFunctionEqualZeroError : LinearError
    {
        /// <summary>
        /// Error simulator constructor 
        /// </summary>
        /// <param name="name">Name of data</param>
        /// <param name="a">Slope</param>
        /// <param name="da">Slope error</param>
        /// <param name="b">Intercept</param>
        /// <param name="db">Intercept error</param>
        public LinearFunctionEqualZeroError(string name, double a, double da, double b, double db) : base(name, a, da, b, db)
        {
        }

        internal override void Run(double x, int iterations = 10000)
        {
            for (int i = 0; i < iterations; i++)
            {
                var a = Distribution.Default(A, ErrorA);
                var b = Distribution.Default(B, ErrorB);

                var v = -b / a;

                Results.Add(v);
            }
        }
    }
}
