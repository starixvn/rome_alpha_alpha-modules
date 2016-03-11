namespace Alpha.Auth.Services
{
    public interface IStatisticService
    {
        void UpdateFunctionUsage(string url, string function, string subFunction, string site);
    }
}