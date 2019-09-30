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
            D2AP1_enthalpy.Run(5, Iterations);

            var D2AP1_entropy = new LinearError("D2AP1_entropy", 1.35253032142857, 0.0581747388859007, -6.92869464285708, 0.290873694429504);
            D2AP1_entropy.Run(5, Iterations);

            var D2AP1_gibbs = new LinearError("D2AP1_gibbs", 0.0533874642857139, 0.0203693520168282, -38.6358375, 0.101846760084141);
            D2AP1_gibbs.Run(5, Iterations);

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

    /// <summary>
    /// Simple linear correlation error estimator. Make sure that the origin of the fit is the mean of the data and that the x value is relative to that mean.
    /// </summary>
    class LinearError
    {
        public string Name = "";

        public List<double> Results = new List<double>();

        double A;
        double ErrorA;
        double B;
        double ErrorB;

        public double Mean;
        public double SD;

        public LinearError(string name, double a, double da, double b, double db)
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
        public void Run(double x, int iterations = 10000)
        {
            Results = new List<double>();

            for (int i = 0; i < iterations; i++)
            {
                var a = Distribution.Normal(A, ErrorA);
                var b = Distribution.Normal(B, ErrorB);

                var y = a * x + b;

                Results.Add(y);
            }

            Mean = 0;
            foreach (var v in Results) Mean += v;

            Mean /= Results.Count;

            double SquareDeviation = 0;
            foreach (var v in Results) SquareDeviation += ((v - Mean) * (v - Mean));

            var Var = SquareDeviation / (Results.Count - 1);
            SD = (float)Math.Sqrt(Var);
            
            PrintError();
        }

        public void PrintError()
        {
            Console.WriteLine(Name + "\t" + Mean.ToString() + " +/- " + SD.ToString());
        }
    }
}
