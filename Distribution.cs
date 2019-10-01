using System;

namespace MonteCarloError
{
    class Distribution
    {
        static readonly Random rand = new Random();

        public static DistributionType Selected { get; set; } = DistributionType.Normal;

        public static double Default(double mean, double stdDev)
        {
            return Selected switch
            {
                DistributionType.Constant => Constant(mean, stdDev),
                DistributionType.Normal => Normal(mean, stdDev),
                _ => mean,
            };
        }

        public static double Normal(double mean, double stdDev)
        {
            double u1 = 1.0 - rand.NextDouble(); //uniform(0,1] random doubles
            double u2 = 1.0 - rand.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
            double randNormal = mean + stdDev * randStdNormal; //random normal(mean,stdDev^2)

            return randNormal;
        }

        public static double Constant(double mean, double stdDev)
        {
            double u = 1 - rand.NextDouble(); //uniform(0,1] random double
            double randStd = 2 * (0.5 - u); //uniform(-1,1] random double
            double randConstant = mean + stdDev * randStd; //random constant within mean +/- SD

            return randConstant;
        }
    }

    enum DistributionType
    {
        Constant,
        Normal
    }
}
