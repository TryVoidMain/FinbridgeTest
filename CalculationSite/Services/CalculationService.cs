namespace CalculationSite.Services
{
    public class CalculationService : ICalculationService
    {
        private bool useDelay;
        private int minDelay;
        private int maxDelay;
        private Random rnd;
        public CalculationService(IConfiguration configuration)
        {
            InitVariables(configuration);

            if (useDelay)
                rnd = new Random();
        }

        public Task<int> Calculate(int[] values)
        {
            return Task.Run(() =>
            {
                int result = 0;

                for (int i = 0; i < values.Length; i++)
                    result += CountPow(values[i]);

                return result;
            });
        }

        private int CountPow(int value)
        {
            if (useDelay)
                Thread.Sleep(rnd.Next(minDelay, maxDelay));

            return (int)Math.Pow(value, 2);
        }

        private void InitVariables(IConfiguration configuration)
        {
            useDelay = configuration.GetValue<bool>("UseDelay");

            if (useDelay)
            {
                minDelay = configuration.GetValue<int>("MinCalculationDelay");
                maxDelay = configuration.GetValue<int>("MaxCalculationDelay");
            }
        }
    }
}
