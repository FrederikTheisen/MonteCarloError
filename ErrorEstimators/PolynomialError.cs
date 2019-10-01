namespace MonteCarloError
{
    /// <summary>
    /// Simple polynomial error propagation tool
    /// </summary>
    public class PolynomialError : Error
    {
        readonly double C;
        readonly double ErrorC;

        /// <summary>
        /// Error simulator constructor 
        /// </summary>
        /// <param name="name">Name of data</param>
        /// <param name="a">x^2 ceoffeicient</param>
        /// <param name="da">x^2 coefficient error</param>
        /// <param name="b">x coefficient</param>
        /// <param name="db">x coefficient error</param>
        /// <param name="c">Intercept</param>
        /// <param name="dc">Intercept error</param>
        public PolynomialError(string name, double a, double da, double b, double db, double c, double dc) : base(name, a, da, b, db)
        {
            C = c;
            ErrorC = dc;
        }

        internal override void Run(double x, int iterations = 30000)
        {
            for (int i = 0; i < iterations; i++)
            {
                var a = Distribution.Default(A, ErrorA);
                var b = Distribution.Default(B, ErrorB);
                var c = Distribution.Default(C, ErrorC);

                var y = a * x * x + b * x + c;

                Results.Add(y);
            }
        }
    }
}
