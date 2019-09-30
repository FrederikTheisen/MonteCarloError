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
        }
    }
}
