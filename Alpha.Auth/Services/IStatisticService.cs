namespace Alpha.Auth.Services
{
    public interface IStatisticService
    {
        void UpdatePageVisit(string url);

        void UpdateFunctionUsage(string url, string function, string subFunction, string site);

        void UpdateFunctionUsage(string function, string subFunction);
    }
}