﻿using System;

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
                var a = Distribution.Default(A, ErrorA);
                var b = Distribution.Default(B, ErrorB);

                var y = a * Math.Exp(b * x);

                Results.Add(y);
            }
        }

        //Mean calculation removed for exponential error method
        internal override void CalcError()
        {
            Mean = 0;
            foreach (var v in Results) Mean += v;

            Mean /= Results.Count;

            var SquareDeviation = 0.0;
            foreach (var v in Results) SquareDeviation += ((v - X) * (v - X)); //use assumed true mean

            var Var = SquareDeviation / (Results.Count - 1);
            SD = (float)Math.Sqrt(Var);
        }
    }
}
