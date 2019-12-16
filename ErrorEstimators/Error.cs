using System;
using System.Collections.Generic;
using System.Linq;

namespace MonteCarloError
{
    public class Error
    {
        internal readonly string Name = "";

        internal List<double> Results = new List<double>();

        internal readonly double A;
        internal readonly double ErrorA;
        internal readonly double B;
        internal readonly double ErrorB;

        internal double X;
        internal double Mean;
        internal double SD;

        public bool UseAssumedTrueMean = false;

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
        public void CalculateError(double x, int iterations = 10000, bool output = true)
        {
            Results = new List<double>();

            X = x;

            Run(x, iterations);

            CalcError();

            if (output) Output();
        }

        internal virtual void Run(double x, int iterations = 10000)
        {

        }

        internal void Output()
        {
            PrintError();
        }

        internal virtual double GetTrueMean()
        {
            var dist = Distribution.Selected;
            Distribution.SetDefaultDistribution(DistributionType.Zero);

            Run(X, 1);

            Distribution.SetDefaultDistribution(dist);

            return Results.Last();
        }

        internal virtual void CalcError()
        {
            Mean = 0;
            foreach (var v in Results) Mean += v;

            Mean /= Results.Count;

            if (UseAssumedTrueMean) { Mean = GetTrueMean(); Results.RemoveAt(Results.Count - 1); }

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

        public void GetAllResults()
        {
            foreach (var r in Results) Console.WriteLine(r);
        }
    }
}
