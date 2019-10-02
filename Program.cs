using System;

namespace MonteCarloError
{
    class Program
    {
        static readonly int Iterations = 500000;

        static void Main(string[] args)
        {
            Distribution.SetDefaultDistribution(DistributionType.Normal);

            // dG = ax + b
            var D2AP6_gibbs = new LinearError("D2AP1_gibbs", 0.056639137, 0.179758227, -43.13963653, 0.898700575);
            D2AP6_gibbs.CalculateError(4.959561429, Iterations);

            // Kd = a * exp(bx), where a = 1 +/- 0 and b = dG +/- SD
            var D2AP6_kd = new ExponentialError("D2AP1_kd", 1.0, 0.0, D2AP6_gibbs.Mean, D2AP6_gibbs.SD);
            //x = 1/(RT)
            var x = 1.0 / (298.15 * 0.008314472);

            D2AP6_kd.CalculateError(x, Iterations);
        }
    }
}
