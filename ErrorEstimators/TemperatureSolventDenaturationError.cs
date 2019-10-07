using System;
using System.Collections.Generic;

namespace MonteCarloError
{
    public class TemperatureSolventDenaturationError : Error
    {
        readonly double C;
        readonly double ErrorC;

        readonly double D;
        readonly double ErrorD;

        readonly double E;
        readonly double ErrorE;

        readonly double F;
        readonly double ErrorF;

        /// <summary>
        /// Error simulator constructor 
        /// </summary>
        /// <param name="name">Name of data</param>
        /// <param name="dH">dH</param>
        /// <param name="dHerror">dH error</param>
        /// <param name="Tm">Tm</param>
        /// <param name="Tmerror">Tm error</param>
        /// <param name="dCp">dCp</param>
        /// <param name="dCperror">dCp error</param>
        public TemperatureSolventDenaturationError(string name, double dH, double dHerror, double Tm, double Tmerror, double dCp, double dCperror) : base(name, dH, dHerror, Tm, Tmerror)
        {
            C = dCp;
            ErrorC = dCperror;
        }

        /// <summary>
        /// Error simulator constructor 
        /// </summary>
        /// <param name="name">Name of data</param>
        /// <param name="dH">dH</param>
        /// <param name="dHerror">dH error</param>
        /// <param name="Tm">Tm</param>
        /// <param name="Tmerror">Tm error</param>
        /// <param name="dCp">dCp</param>
        /// <param name="dCperror">dCp error</param>
        /// <param name="m0">Denaturant M0</param>
        /// <param name="m0error">Denaturant M0 error</param>
        /// <param name="m1">Denaturant M0</param>
        /// <param name="m1error">Denaturant M0 error</param>
        /// <param name="m2">Denaturant M0</param>
        /// <param name="m2error">Denaturant M0 error</param>
        public TemperatureSolventDenaturationError(string name, double dH, double dHerror, double Tm, double Tmerror, double dCp, double dCperror, double m0, double m0error, double m1, double m1error, double m2, double m2error) : base(name, dH, dHerror, Tm, Tmerror)
        {
            C = dCp;
            ErrorC = dCperror;

            D = m0;
            ErrorD = m0error;

            E = m1;
            ErrorE = m1error;

            F = m2;
            ErrorF = m2error;
        }

        internal override void Run(double T, int iterations = 30000)
        {
            for (int i = 0; i < iterations; i++)
            {
                var a = Distribution.Default(A, ErrorA);
                var b = Distribution.Default(B, ErrorB);
                var c = Distribution.Default(C, ErrorC);

                var y = a * (1 - (T / b)) + c * ((T - b) - T * Math.Log(T / b));

                Results.Add(y);
            }
        }

        public void CalculateError(double T, double X, int iterations = 30000, bool output = true)
        {
            Results = new List<double>();

            Run(T, X, iterations);

            CalcError();

            if (output) Output();
        }

        internal void Run(double T, double X, int iterations = 30000)
        { 
            for (int i = 0; i < iterations; i++)
            {
                var a = Distribution.Default(A, ErrorA);
                var b = Distribution.Default(B, ErrorB);
                var c = Distribution.Default(C, ErrorC);
                var d = Distribution.Default(D, ErrorD);
                var e = Distribution.Default(E, ErrorE);
                var f = Distribution.Default(F, ErrorF);

                var y = a * (1 - (T / b)) + c * ((T - b) - T * Math.Log(T / b));

                Results.Add(y);
            }
        }
    }
}
