namespace CalculationSite.Services
{
    public class CalculationService : ICalculationService
    {
        private readonly IConfiguration _configuration;
        public CalculationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public int Calculate(int[] values)
        {
            int result = 0;

            for (int i = 0; i < values.Length; i++)
                result += (int)Math.Pow(values[i], 2);

            return result;
        }
    }
}
