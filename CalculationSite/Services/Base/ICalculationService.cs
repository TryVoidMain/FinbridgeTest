namespace CalculationSite.Services
{
    public interface ICalculationService
    {
        Task<int> Calculate(int[] values);
    }
}
