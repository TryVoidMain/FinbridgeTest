namespace CalculationSite.Services
{
    public class CalculationService : ICalculationService
    {
        private readonly IConfiguration _configuration;
        public CalculationService(IConfiguration configuration) 
        {
            _configuration = configuration;
        }

        public void CalculatePow()
        {

        }
    }
}
