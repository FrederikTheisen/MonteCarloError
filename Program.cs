using System;
using System.Collections.Generic;

namespace MonteCarloError
{
    class Program
    {
        static readonly int Iterations = 1000000;

        static void Main(string[] args)
        {
            var D2AP1_enthalpy = new LinearError("D2AP1_enthalpy", -1.29914285714286, 0.0480691678517448, -31.7071428571429, 0.240345839258724);
            D2AP1_enthalpy.CalculateError(5, Iterations);

            var D2AP1_entropy = new LinearError("D2AP1_entropy", 1.35253032142857, 0.0581747388859007, -6.92869464285708, 0.290873694429504);
            D2AP1_entropy.CalculateError(5, Iterations);

            var D2AP1_gibbs = new LinearError("D2AP1_gibbs", 0.0533874642857139, 0.0203693520168282, -38.6358375, 0.101846760084141);
            D2AP1_gibbs.CalculateError(5, Iterations);

            var D2AP1_kd = new ExponentialError("D2AP1_kd", 1, 0, D2AP1_gibbs.Mean, D2AP1_gibbs.SD);
            var x = 1 / (298.15 * 0.008314472);

            D2AP1_kd.CalculateError(x);
            D2AP1_kd.CalculateError(x);
            D2AP1_kd.CalculateError(x);
            D2AP1_kd.CalculateError(x);

            //for (int i = -10; i <= 10; i++) mc.Run(1000, i);
        }
    }

    class Distribution
    {
        static readonly Random rand = new Random();

        public static double Normal(double mean, double stdDev)
        {
            double u1 = 1.0 - rand.NextDouble(); //uniform(0,1] random doubles
            double u2 = 1.0 - rand.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
            double randNormal = mean + stdDev * randStdNormal; //random normal(mean,stdDev^2)

            return randNormal;
        }
    }

    internal class Error
    {
        internal readonly string Name = "";

        internal List<double> Results = new List<double>();

        internal readonly double A;
        internal readonly double ErrorA;
        internal readonly double B;
        internal readonly double ErrorB;

        double X;
        internal double Mean;
        internal double SD;

        public Error(string name, double a, double da, double b, double db)
        {
            Name = name;

            A = a;
            ErrorA = da;

            B = b;
            ErrorB = db;
        }

        /// <summary>
        /// Run the Monte Carlo error estimator
        /// </summary>
        /// <param name="iterations">Number of samples used to calculate the error</param>
        /// <param name="x">X value relative to the origin of the fit</param>
        public void CalculateError(double x, int iterations = 10000)
        {
            Results = new List<double>();

            X = x;

            Run(x, iterations);

            Output();
        }

        internal virtual void Run(double x, int iterations = 10000)
        {
            
        }

        void Output()
        {
            CalcError();

            PrintError();
        }

        void CalcError()
        {
            Mean = 0;
            foreach (var v in Results) Mean += v;

            Mean /= Results.Count;

            double SquareDeviation = 0;
            foreach (var v in Results) SquareDeviation += ((v - Mean) * (v - Mean));

            var Var = SquareDeviation / (Results.Count - 1);
            SD = (float)Math.Sqrt(Var);
        }

        internal void PrintError()
        {
            if (Math.Abs(Mean) < 0.000001) Console.WriteLine(Name + " @ " + X.ToString("#####0.00") + "\t" + (Mean * 1000000000).ToString("#####0.00") + " +/- " + (SD * 1000000000).ToString("#####0.00") + " [x10^9]");
            else if (Math.Abs(Mean) < 0.001) Console.WriteLine(Name + " @ " + X.ToString("#####0.00") + "\t" + (Mean * 1000000).ToString("#####0.00") + " +/- " + (SD * 1000000).ToString("#####0.00") + " [x10^6]");
            else if (Math.Abs(Mean) < 1) Console.WriteLine(Name + " @ " + X.ToString("#####0.00") + "\t" + (Mean * 1000).ToString("#####0.00") + " +/- " + (SD * 1000).ToString("#####0.00") + " [x10^3]");
            else Console.WriteLine(Name + " @ " + X.ToString("#####0.00") + "\t" + Mean.ToString("#####0.00") + " +/- " + SD.ToString("#####0.00"));
        }
    }

    /// <summary>
    /// Simple linear correlation error estimator. Make sure that the origin of the fit is the mean of the data and that the x value is relative to that mean.
    /// </summary>
    class LinearError : Error
    {
        /// <summary>
        /// Error simulator constructor 
        /// </summary>
        /// <param name="name">Name of data</param>
        /// <param name="a">Slope</param>
        /// <param name="da">Slope error</param>
        /// <param name="b">Intercept</param>
        /// <param name="db">Intercept error</param>
        public LinearError(string name, double a, double da, double b, double db) : base(name, a, da, b, db)
        {

        }

        internal override void Run(double x, int iterations = 10000)
        {
            for (int i = 0; i < iterations; i++)
            {
                var a = Distribution.Normal(A, ErrorA);
                var b = Distribution.Normal(B, ErrorB);

                var y = a * x + b;

                Results.Add(y);
            }
        }
    }

    /// <summary>
    /// Simple polynomial error propagation tool
    /// </summary>
    class PolynomialError : Error
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
                var a = Distribution.Normal(A, ErrorA);
                var b = Distribution.Normal(B, ErrorB);
                var c = Distribution.Normal(C, ErrorC);

                var y = a * x * x + b * x + c;

                Results.Add(y);
            }
        }
    }

    /// <summary>
    /// Exponential error propagation tool. Form is y = a exp(bx)
    /// </summary>
    class ExponentialError : Error
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
