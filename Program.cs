using System;

namespace MonteCarloError
{
    class Program
    {
        static readonly int Iterations = 500000;

        static void Main(string[] args)
        {
            //Distribution.SetDefaultDistribution(DistributionType.Normal);

            //// dG = ax + b, x = T relative to fit origin
            //var D2AP6_gibbs = new LinearError("D2AP1_gibbs", 0.053387464, 0.070680823, -38.6358375, 0.353404114);
            //D2AP6_gibbs.CalculateError(5, Iterations);

            //// Kd = a * exp(bx), where a = 1 +/- 0 and b = dG +/- SD
            //var D2AP6_kd = new ExponentialError("D2AP1_kd", 1.0, 0.0, D2AP6_gibbs.Mean, D2AP6_gibbs.SD);
            ////x = 1/(RT)
            //var x = 1.0 / (298.15 * 0.008314472);

            ////D2AP6_kd.CalculateError(x, Iterations);

            //for (int i = 0; i < 100000; i++)
            //{
            //    D2AP6_kd.CalculateError(x, 1, false);
            //    Console.WriteLine(D2AP6_kd.Mean);
            //}

            var denat = new TemperatureSolventDenaturationError("dG", -134.1, 0.6, 322.9, 0.06, -5.5, 0.04);
            denat.CalculateError(298.15);

            for (int i = 0; i < 50000; i++)
            {
                denat.CalculateError(298.15, 1, false);
                Console.WriteLine(denat.Mean);
            }
        }
    }
}
